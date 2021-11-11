using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Hardware.NModbus4.Device;
using NLog;
using Utilities.Net;

// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable FunctionNeverReturns

namespace Hardware.DeviceInterface
{
    public class CabinetDeviceRx3 : ICabinetDevice
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 抽屉/柜门检测设备
        /// </summary>
        private static ModbusMaster _cabinetCheckerMaster;

        private static int _cabinetCheckerControllerType;
        private static string _cabinetCheckerSerialPort;
        private static SerialPort _cabinetCheckerSerial;
        private static TcpClient _cabinetCheckerClient;
        private static string _cabinetCheckerIp;
        private static int _cabinetCheckerPort;

        /// <summary>
        /// 工具柜操作设备
        /// </summary>
        private static ModbusMaster _cabinetOperatorMaster;

        private static int _cabinetOperatorControllerType;
        private static string _cabinetOperatorSerialPort;
        private static SerialPort _cabinetOperatorSerial;
        private static TcpClient _cabinetOperatorClient;
        private static string _cabinetOperatorIp;
        private static int _cabinetOperatorPort;

        private static bool _mixCabinetChecker;
        private static bool _mixCabinetOperator;

        /// <summary>
        /// 工具箱状态检测设备阵列
        /// </summary>
        private static List<ModbusController> _toolCheckerGroup;

        /// <summary>
        /// 工具箱LED操作设备阵列
        /// </summary> 
        private static List<ModbusController> _toolLedOperatorGroup;

        private static int _doorOperationTime = 2;

        private static int _doorKeepTime = 10;
        //private static int _drawerLockTime = 10;
        //private static int _drawerUnlock;

        /// <summary>
        /// 工具箱计数
        /// </summary>
        private static ushort _toolCount;

        private static ushort _reserveToolCount;

        //点位替换
        private static readonly Hashtable BrokenCheckerMapping = new Hashtable();
        private static readonly Hashtable BrokenOperatorMapping = new Hashtable();
        private static int _lightCtrlPoint = 13;
        private static int _ledYellowCtrlPoint = 14;
        private static int _ledRedCtrlPoint = 15;

        //private static ushort _drawerCount;

        /// <summary>
        /// 初始化状态
        /// </summary>
        private static readonly object LedLock = new object();

        //延时信息
        private static DateTime _lastDoorUnlock, _lastDoorLock;

        //private static DateTime[] _lastDrawerUnlock;
        private static readonly object DelayLock = new object();

        //门控信息
        private static bool _isDoorEverOpened;
        private static bool _isDoorTimeoutClosed;

        public class ModbusController
        {
            /// <summary>
            /// NModbus对象
            /// </summary>
            public ModbusMaster ModbusCtrl;

            /// <summary>
            /// 连接类型 0=TCPIP
            /// </summary>
            public int ControllerType;

            /// <summary>
            /// 连接地址
            /// </summary>
            public string ConnAddr;

            /// <summary>
            /// TCP方式连接端口
            /// </summary>
            public int NetPort;

            /// <summary>
            /// COM方式连接端口
            /// </summary>
            public string ComPort;

            /// <summary>
            /// 下级设备容量
            /// </summary>
            public ushort ControllerCapacity;

            /// <summary>
            /// 设备起始编号
            /// </summary>
            public ushort ControllerStartIndex;

            /// <summary>
            /// TCPIP方式连接对象
            /// </summary>
            public TcpClient ModbusClient;

            public SerialPort ModbusPort;

            public bool IsConnected()
            {
                if (ControllerType == 0)
                {
                    return ModbusClient.Connected;
                }

                if (ControllerType == 1)
                {
                    if (ModbusPort != null) return ModbusPort.IsOpen;
                    return false;
                }

                return false;
            }
        }

        /// <summary>
        /// 工具状态列表
        /// </summary>
        private static readonly CabinetCallback.ToolStatus[] ToolStatusList = new CabinetCallback.ToolStatus[256];

        /*
        private static readonly DrawerStatus[] DrawerStatusList = new DrawerStatus[64];
        public class DrawerStatus
        {
            /// <summary>
            /// 警告
            /// </summary>
            public int AlertStatus;

            public int OpenStatus;

            public DrawerStatus()
            {
                AlertStatus = 0;
                OpenStatus = 0;
            }
        }*/

        public void CanDoorOpen(bool open)
        {

        }

        public void EmulateToolTaken(int toolPosition)
        {
            CabinetCallback.OnToolTaken?.Invoke(toolPosition);
        }

        public void EmulateToolReturn(int toolPosition)
        {
            CabinetCallback.OnToolReturn?.Invoke(toolPosition);
        }

        public void SetDrawerUnlockTime(int unlockTime)
        {

        }

        public int AddCheckerMapping(int origin, int dest)
        {
            BrokenCheckerMapping[origin] = dest;
            return 1;
        }

        public int AddOperatorMapping(int origin, int dest)
        {
            BrokenOperatorMapping[origin] = dest;
            return 1;
        }

        /// <summary>
        /// 添加工具检查模块
        /// </summary>
        /// <returns></returns>
        public int AddToolCheckerDevice(string toolCheckerIp,
            int toolCheckerPort, ushort toolCheckerCapacity, ushort toolCheckerStartIndex)
        {
            if (_toolCheckerGroup == null)
            {
                _toolCheckerGroup = new List<ModbusController>();
            }

            if (!string.IsNullOrEmpty(toolCheckerIp))
            {
                ModbusController mc = new ModbusController
                {
                    ControllerType = 0,
                    ConnAddr = toolCheckerIp,
                    NetPort = toolCheckerPort,
                    ControllerCapacity = toolCheckerCapacity,
                    ControllerStartIndex = toolCheckerStartIndex
                };
                _toolCheckerGroup.Add(mc);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// 添加工具检查模块
        /// </summary>
        /// <returns></returns>
        public int AddToolCheckerComDevice(int comPort, ushort toolCheckerCapcity, ushort toolCheckerStartIndex)
        {
            if (_toolCheckerGroup == null)
            {
                _toolCheckerGroup = new List<ModbusController>();
            }

            if (comPort >= 0)
            {
                ModbusController mc = new ModbusController
                {
                    ControllerType = 1,
                    ComPort = "COM" + comPort,
                    ControllerCapacity = toolCheckerCapcity,
                    ControllerStartIndex = toolCheckerStartIndex,
                };
                _toolCheckerGroup.Add(mc);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// 添加LED控制模块
        /// </summary>
        /// <returns></returns>
        public int AddToolLedOperatorDevice(string toolLedOperatorIp,
            int toolLedOperatorPort, ushort toolLedOperatorCapacity, ushort toolLedOperatorStartIndex)
        {
            if (_toolLedOperatorGroup == null)
            {
                _toolLedOperatorGroup = new List<ModbusController>();
            }

            if (!string.IsNullOrEmpty(toolLedOperatorIp))
            {
                ModbusController mc = new ModbusController
                {
                    ControllerType = 0,
                    ConnAddr = toolLedOperatorIp,
                    NetPort = toolLedOperatorPort,
                    ControllerCapacity = toolLedOperatorCapacity,
                    ControllerStartIndex = toolLedOperatorStartIndex
                };
                _toolLedOperatorGroup.Add(mc);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="toolCount">工具箱计数</param>
        /// <param name="reserveToolCount">保留工具计数</param>
        /// <param name="drawerCount">抽屉计数</param>
        /// <param name="cabinetCheckerIp"></param>
        /// <param name="cabinetOperatorIp"></param>
        /// <param name="cabinetCheckerPort"></param>
        /// <param name="cabinetOperatorPort"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public int InitDevice(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string cabinetCheckerIp,
            string cabinetOperatorIp, int cabinetCheckerPort = 502, int cabinetOperatorPort = 502,
            bool debugMode = false)
        {
            _toolCount = toolCount;
            _reserveToolCount = reserveToolCount;
            //_drawerCount = drawerCount;

            _cabinetCheckerIp = cabinetCheckerIp;
            _cabinetCheckerPort = cabinetCheckerPort;
            _cabinetCheckerControllerType = 0;
            _cabinetOperatorIp = cabinetOperatorIp;
            _cabinetOperatorPort = cabinetOperatorPort;
            _cabinetOperatorControllerType = 0;

            /*
            _lastDrawerUnlock = new DateTime[_drawerCount];
            for (int i = 0; i < _drawerCount; i++)
            {
                _lastDrawerUnlock[i] = DateTime.MinValue;
            }*/

            for (int i = 0; i < _toolCount; i++)
            {
                ToolStatusList[i] = new CabinetCallback.ToolStatus();
            }
            /*
            for (int i = 0; i < _drawerCount; i++)
            {
                DrawerStatusList[i] = new DrawerStatus();
            }*/

            Thread initThread = new Thread(Init) { IsBackground = true };
            initThread.Start();
            return 1;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="toolCount">工具箱计数</param>
        /// <param name="drawerCount">抽屉计数</param>
        /// <param name="cabinetCheckerComPort"></param>
        /// <param name="cabinetOperatorComPort"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public int InitDevice(ushort toolCount, ushort drawerCount, int cabinetCheckerComPort,
            int cabinetOperatorComPort, bool debugMode = false)
        {
            _toolCount = toolCount;
            //_drawerCount = drawerCount;

            _cabinetCheckerSerialPort = "COM" + cabinetCheckerComPort;
            _cabinetCheckerControllerType = 1;
            _cabinetOperatorSerialPort = "COM" + cabinetOperatorComPort;
            _cabinetOperatorControllerType = 1;

            /*
            _lastDrawerUnlock = new DateTime[_drawerCount];
            for (int i = 0; i < _drawerCount; i++)
            {
                _lastDrawerUnlock[i] = DateTime.MinValue;
            }*/

            for (int i = 0; i < _toolCount; i++)
            {
                ToolStatusList[i] = new CabinetCallback.ToolStatus();
            }
            /*
            for (int i = 0; i < _drawerCount; i++)
            {
                DrawerStatusList[i] = new DrawerStatus();
            }*/

            Thread initThread = new Thread(Init) { IsBackground = true };
            initThread.Start();
            return 1;
        }

        public int InitDevicePure(ushort toolCount, ushort reserveToolCount, ushort drawerCount,
            string cabinetCheckerIp,
            string cabinetOperatorIp, int cabinetCheckerPort = 502, int cabinetOperatorPort = 502,
            bool debugMode = false)
        {
            _toolCount = toolCount;
            _reserveToolCount = reserveToolCount;
            //_drawerCount = drawerCount;

            _cabinetCheckerIp = cabinetCheckerIp;
            _cabinetCheckerPort = cabinetCheckerPort;
            _cabinetCheckerControllerType = 0;
            _cabinetOperatorIp = cabinetOperatorIp;
            _cabinetOperatorPort = cabinetOperatorPort;
            _cabinetOperatorControllerType = 0;

            /*
            _lastDrawerUnlock = new DateTime[_drawerCount];
            for (int i = 0; i < _drawerCount; i++)
            {
                _lastDrawerUnlock[i] = DateTime.MinValue;
            }*/

            for (int i = 0; i < _toolCount; i++)
            {
                ToolStatusList[i] = new CabinetCallback.ToolStatus();
            }
            /*
            for (int i = 0; i < _drawerCount; i++)
            {
                DrawerStatusList[i] = new DrawerStatus();
            }*/

            Thread initThread = new Thread(InitPure) { IsBackground = true };
            initThread.Start();
            return 1;
        }

        public void SetLedMapping(int lightPoint, int yellowPoint, int redPoint)
        {
            if (lightPoint > 0)
            {
                _lightCtrlPoint = lightPoint;
            }

            if (yellowPoint > 0)
            {
                _ledYellowCtrlPoint = yellowPoint;
            }

            if (redPoint > 0)
            {
                _ledRedCtrlPoint = redPoint;
            }
        }

        /// <summary>
        /// 异步初始化工具柜连接
        /// </summary>
        private void Init()
        {
            bool isSuccess = false;
            try
            {
                isSuccess = FixConnection(true);

                //启动时LED测试
                bool[] ledStatus = new bool[_toolCount];
                for (int i = 0; i < _toolCount; i++)
                {
                    ledStatus[i] = true;
                }

                SetToolLedStatus(ledStatus);
                Thread.Sleep(1000);
                for (int i = 0; i < _toolCount; i++)
                {
                    ledStatus[i] = false;
                }

                SetToolLedStatus(ledStatus);

                //抽屉初始状态
                /*
                if (_drawerUnlock != 0)
                {
                    for (int i = 0; i < _drawerCount; i++)
                    {
                        UnlockDrawer(i, true, false);
                    }
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                CabinetCallback.OnInitDone?.Invoke("工具柜连接失败");
            }

            //网络重连线程
            Thread linkMonnitorThread = new Thread(LinkMonitor) { IsBackground = true };
            linkMonnitorThread.Start();
            //LED控制线程
            Thread ledCtrlThread = new Thread(LedControlThread) { IsBackground = true };
            ledCtrlThread.Start();
            //工具检查线程
            Thread toolStatusThread = new Thread(ToolStatusMonitor) { IsBackground = true };
            toolStatusThread.Start();
            //锁控线程
            Thread lockSchedulerThread = new Thread(LockScheduler) { IsBackground = true };
            lockSchedulerThread.Start();

            for (int i = 0; i < _toolCount; i++)
            {
                ToolStatusList[i].Status = 0;
            }

            if (isSuccess)
            {
                CabinetCallback.OnInitDone?.Invoke("成功");
            }
            else
            {
                CabinetCallback.OnInitDone?.Invoke("模块连接失败");
            }
        }

        private void InitPure()
        {
            try
            {
                FixConnection();

                //启动时LED测试
                bool[] ledStatus = new bool[_toolCount];
                for (int i = 0; i < _toolCount; i++)
                {
                    ledStatus[i] = true;
                }

                SetToolLedStatus(ledStatus);
                Thread.Sleep(1000);
                for (int i = 0; i < _toolCount; i++)
                {
                    ledStatus[i] = false;
                }

                SetToolLedStatus(ledStatus);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                CabinetCallback.OnInitDone?.Invoke("工具柜连接失败");
            }

            for (int i = 0; i < _toolCount; i++)
            {
                ToolStatusList[i].Status = 0;
            }
        }

        public bool IsInitDone()
        {
            return true;
            //return _isInitDone;
        }

        /// <summary>
        /// 网络重连线程
        /// </summary>
        private static void LinkMonitor()
        {
            while (true)
            {
                try
                {
                    FixConnection();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                Thread.Sleep(2000);
            }
        }


        /// <summary>
        /// LED闪烁控制线程
        /// </summary>
        private void LedControlThread()
        {

            while (true)
            {
                try
                {
                    //开启工具LED
                    bool[] ledStatus = new bool[_toolCount];
                    lock (LedLock)
                    {
                        for (int i = 0; i < _toolCount; i++)
                        {
                            if (ToolStatusList[i].AlertStatus == 1 || ToolStatusList[i].WaitStatus == 1 ||
                                ToolStatusList[i].CheckStatus == 1)
                            {
                                ledStatus[i] = true;
                            }
                            else
                            {
                                ledStatus[i] = false;
                            }
                        }
                    }

                    SetToolLedStatus(ledStatus);
                    Thread.Sleep(300);
                    //关闭工具LED
                    lock (LedLock)
                    {
                        for (int i = 0; i < _toolCount; i++)
                        {
                            if (ToolStatusList[i].AlertStatus == 1)
                            {
                                ledStatus[i] = false;
                            }
                        }
                    }

                    SetToolLedStatus(ledStatus);
                    Thread.Sleep(200);
                    //超时检查
                    lock (LedLock)
                    {
                        for (int i = 0; i < _toolCount; i++)
                        {
                            if (ToolStatusList[i].AlertStatus == 1 && DateTime.Now > ToolStatusList[i].AlertExpire)
                            {
                                ToolStatusList[i].AlertStatus = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        private static bool FixConnection(bool fastCheck = false)
        {
            bool isSuccess = true;
            try
            {
                //检测模块
                for (int i = 0; i < _toolCheckerGroup.Count; i++)
                {
                    if (_toolCheckerGroup[i].ControllerType == 0)
                    {
                        if (_toolCheckerGroup[i].ModbusClient == null || !_toolCheckerGroup[i].ModbusClient.Connected)
                        {
                            TcpClientEx client = new TcpClientEx(_toolCheckerGroup[i].ConnAddr,
                                _toolCheckerGroup[i].NetPort, 8000);
                            _toolCheckerGroup[i].ModbusClient = client.Connect();
                            _toolCheckerGroup[i].ModbusClient.SendTimeout = 3000;
                            _toolCheckerGroup[i].ModbusClient.ReceiveTimeout = 3000;
                            _toolCheckerGroup[i].ModbusCtrl =
                                ModbusIpMaster.CreateIp(_toolCheckerGroup[i].ModbusClient);
                            _toolCheckerGroup[i].ModbusCtrl.Transport.ReadTimeout = 3000;
                            _toolCheckerGroup[i].ModbusCtrl.Transport.WriteTimeout = 3000;
                        }
                    }
                    else if (_toolCheckerGroup[i].ControllerType == 1)
                    {
                        if (_toolCheckerGroup[i].ModbusPort == null || !_toolCheckerGroup[i].ModbusPort.IsOpen)
                        {
                            _toolCheckerGroup[i].ModbusPort = new SerialPort(_toolCheckerGroup[i].ComPort, 9600,
                                Parity.None, 8, StopBits.One);
                            _toolCheckerGroup[i].ModbusPort.Open();
                            _toolCheckerGroup[i].ModbusCtrl =
                                ModbusSerialMaster.CreateRtu(_toolCheckerGroup[i].ModbusPort);
                            _toolCheckerGroup[i].ModbusCtrl.Transport.ReadTimeout = 3000;
                            _toolCheckerGroup[i].ModbusCtrl.Transport.WriteTimeout = 3000;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.Error(ex);
                if (fastCheck) return false;
            }

            try
            {
                //灯控模块
                for (int i = 0; i < _toolLedOperatorGroup.Count; i++)
                {
                    if (_toolLedOperatorGroup[i].ControllerType == 0)
                    {
                        if (_toolLedOperatorGroup[i].ModbusClient == null ||
                            !_toolLedOperatorGroup[i].ModbusClient.Connected)
                        {
                            TcpClientEx client = new TcpClientEx(_toolLedOperatorGroup[i].ConnAddr,
                                _toolLedOperatorGroup[i].NetPort, 8000);
                            _toolLedOperatorGroup[i].ModbusClient = client.Connect();
                            _toolLedOperatorGroup[i].ModbusClient.SendTimeout = 3000;
                            _toolLedOperatorGroup[i].ModbusClient.ReceiveTimeout = 3000;
                            _toolLedOperatorGroup[i].ModbusCtrl =
                                ModbusIpMaster.CreateIp(_toolLedOperatorGroup[i].ModbusClient);
                            _toolLedOperatorGroup[i].ModbusCtrl.Transport.ReadTimeout = 3000;
                            _toolLedOperatorGroup[i].ModbusCtrl.Transport.WriteTimeout = 3000;
                        }
                    }
                    else if (_toolLedOperatorGroup[i].ControllerType == 1)
                    {
                        if (_toolLedOperatorGroup[i].ModbusPort == null || !_toolLedOperatorGroup[i].ModbusPort.IsOpen)
                        {
                            _toolLedOperatorGroup[i].ModbusPort = new SerialPort(_toolLedOperatorGroup[i].ComPort, 9600,
                                Parity.None, 8, StopBits.One);
                            _toolLedOperatorGroup[i].ModbusPort.Open();
                            _toolLedOperatorGroup[i].ModbusCtrl =
                                ModbusSerialMaster.CreateRtu(_toolLedOperatorGroup[i].ModbusPort);
                            _toolLedOperatorGroup[i].ModbusCtrl.Transport.ReadTimeout = 3000;
                            _toolLedOperatorGroup[i].ModbusCtrl.Transport.WriteTimeout = 3000;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.Error(ex);
                if (fastCheck) return false;
            }

            try
            {
                //重用判定
                for (int i = 0; i < _toolCheckerGroup.Count; i++)
                {
                    if (_toolCheckerGroup[i].ControllerType == 0)
                    {
                        if (_cabinetCheckerIp == _toolCheckerGroup[i].ConnAddr &&
                            _cabinetCheckerPort == _toolCheckerGroup[i].NetPort)
                        {
                            _cabinetCheckerMaster = _toolCheckerGroup[i].ModbusCtrl;
                            _cabinetCheckerClient = _toolCheckerGroup[i].ModbusClient;
                            _mixCabinetChecker = true;
                        }
                    }
                    else if (_toolCheckerGroup[i].ControllerType == 1)
                    {
                        if (_cabinetCheckerSerialPort == _toolCheckerGroup[i].ComPort)
                        {
                            _cabinetCheckerMaster = _toolCheckerGroup[i].ModbusCtrl;
                            _cabinetCheckerSerial = _toolCheckerGroup[i].ModbusPort;
                            _mixCabinetChecker = true;
                        }
                    }
                }

                for (int i = 0; i < _toolLedOperatorGroup.Count; i++)
                {
                    if (_toolLedOperatorGroup[i].ControllerType == 0)
                    {
                        if (_cabinetOperatorIp == _toolLedOperatorGroup[i].ConnAddr &&
                            _cabinetOperatorPort == _toolLedOperatorGroup[i].NetPort)
                        {
                            _cabinetOperatorMaster = _toolLedOperatorGroup[i].ModbusCtrl;
                            _cabinetOperatorClient = _toolLedOperatorGroup[i].ModbusClient;
                            _mixCabinetOperator = true;
                        }
                    }
                    else if (_toolLedOperatorGroup[i].ControllerType == 1)
                    {
                        if (_cabinetOperatorSerialPort == _toolLedOperatorGroup[i].ComPort)
                        {
                            _cabinetOperatorMaster = _toolLedOperatorGroup[i].ModbusCtrl;
                            _cabinetOperatorSerial = _toolLedOperatorGroup[i].ModbusPort;
                            _mixCabinetOperator = true;
                        }
                    }
                }

                if (!_mixCabinetOperator)
                {
                    if (_cabinetOperatorIp == _cabinetCheckerIp)
                    {
                        _cabinetOperatorMaster = _cabinetCheckerMaster;
                        _mixCabinetOperator = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.Error(ex);
                if (fastCheck) return false;
            }

            try
            {
                //非重用连接
                if (!_mixCabinetChecker)
                {
                    if (_cabinetCheckerControllerType == 0)
                    {
                        TcpClientEx client = new TcpClientEx(_cabinetCheckerIp, _cabinetCheckerPort, 8000);
                        _cabinetCheckerClient = client.Connect();
                        _cabinetCheckerClient.SendTimeout = 3000;
                        _cabinetCheckerClient.ReceiveTimeout = 3000;
                        _cabinetCheckerMaster = ModbusIpMaster.CreateIp(_cabinetCheckerClient);
                        _cabinetCheckerMaster.Transport.ReadTimeout = 3000;
                        _cabinetCheckerMaster.Transport.WriteTimeout = 3000;
                    }
                    else if (_cabinetCheckerControllerType == 1)
                    {
                        _cabinetCheckerSerial = new SerialPort(_cabinetCheckerSerialPort, 9600,
                            Parity.None, 8, StopBits.One);
                        _cabinetCheckerMaster = ModbusSerialMaster.CreateRtu(_cabinetCheckerSerial);
                    }
                }

                if (!_mixCabinetOperator)
                {
                    if (_cabinetOperatorControllerType == 0)
                    {
                        TcpClientEx client = new TcpClientEx(_cabinetOperatorIp, _cabinetOperatorPort, 8000);
                        _cabinetOperatorClient = client.Connect();
                        _cabinetOperatorClient.SendTimeout = 3000;
                        _cabinetOperatorClient.ReceiveTimeout = 3000;
                        _cabinetOperatorMaster = ModbusIpMaster.CreateIp(_cabinetOperatorClient);
                        _cabinetOperatorMaster.Transport.ReadTimeout = 3000;
                        _cabinetOperatorMaster.Transport.WriteTimeout = 3000;
                    }
                    else if (_cabinetOperatorControllerType == 1)
                    {
                        _cabinetOperatorSerial = new SerialPort(_cabinetOperatorSerialPort, 9600,
                            Parity.None, 8, StopBits.One);
                        _cabinetOperatorMaster = ModbusSerialMaster.CreateRtu(_cabinetOperatorSerial);
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.Error(ex);
                if (fastCheck) return false;
            }

            return isSuccess;
        }

        /// <summary>
        /// 工具状态检索线程
        /// </summary>
        private void ToolStatusMonitor()
        {

            int curDoorStatus = 0;
            while (true)
            {
                try
                {
                    bool[] status = IsToolExist();
                    if (status != null)
                    {
                        for (int i = 0; i < _toolCount; i++)
                        {
                            if (ToolStatusList[i].Status == 0)
                            {
                                if (status[i])
                                {
                                    Thread retnThd = new Thread(RetnThd) { IsBackground = true };
                                    retnThd.Start(i);
                                }
                            }
                            else if (ToolStatusList[i].Status == 1)
                            {
                                if (!status[i])
                                {
                                    Thread takenThd = new Thread(TakenThd) { IsBackground = true };
                                    takenThd.Start(i);
                                }
                            }

                            //更新状态信息
                            ToolStatusList[i].Status = status[i] ? 1 : 0;
                        }
                    }

                    int doorStatus = IsDoorClosed();
                    if (doorStatus >= 0)
                    {
                        //门控
                        if (curDoorStatus == 0 && doorStatus == 1)
                        {
                            CabinetCallback.OnCabinetClosed?.Invoke();
                        }

                        curDoorStatus = doorStatus;
                        //灯控
                        if (doorStatus == 1)
                        {
                            if ((DateTime.Now - _lastDoorUnlock).TotalSeconds >= _doorKeepTime)
                            {
                                //关闭照明灯
                                OpenAllLight(false);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                Thread.Sleep(1000);
            }
        }

        private void RetnThd(object toolNo)
        {
            //工具归还
            CabinetCallback.OnToolReturn?.Invoke((int)toolNo);
        }

        private void TakenThd(object toolNo)
        {
            //工具借取
            CabinetCallback.OnToolTaken?.Invoke((int)toolNo);
        }

        private void LockScheduler()
        {
            while (true)
            {
                int doorCloseStatus = IsDoorClosed();
                try
                {
                    //门解锁控制
                    bool result = false;
                    lock (DelayLock)
                    {
                        //延时期过后停止解锁过程
                        if ((DateTime.Now - _lastDoorUnlock).TotalSeconds >= _doorOperationTime)
                        {
                            result = true;
                        }
                    }

                    if (result)
                    {
                        //终止解锁信号
                        UnlockDoor(false, false);
                    }
                    else
                    {
                        //保持解锁信号
                        UnlockDoor(true, false);
                    }

                    //如正在进行解锁则跳过上锁判断
                    if (result)
                    {
                        result = false;
                        //门锁定控制
                        lock (DelayLock)
                        {
                            //延时期过后停止上锁过程
                            if ((DateTime.Now - _lastDoorLock).TotalSeconds >= _doorOperationTime)
                            {
                                result = true;
                            }
                        }

                        if (result)
                        {
                            //终止锁定信号
                            LockDoor(false, false);
                        }
                        else
                        {
                            //保持锁定信号
                            LockDoor(true, false);
                        }
                    }

                    //门开时启动磁铁否则关闭
                    /*
                    if (doorCloseStatus == 0)
                    {
                        _isDoorEverOpened = true;
                        EnableDoorMagnet(true);
                    }
                    else
                    {
                        EnableDoorMagnet(false);
                    }*/
                    //门超时自动锁定
                    if (doorCloseStatus == 1)
                    {
                        if (!_isDoorEverOpened)
                        {
                            if (!_isDoorTimeoutClosed)
                            {
                                bool needLock = false;
                                lock (DelayLock)
                                {
                                    if ((DateTime.Now - _lastDoorUnlock).TotalSeconds >=
                                        _doorKeepTime)
                                    {
                                        needLock = true;
                                    }
                                }

                                if (needLock)
                                {
                                    LockDoor(true);
                                    _isDoorTimeoutClosed = true;
                                }
                            }
                        }
                        else
                        {
                            LockDoor(true);
                            OpenAllLight(false);
                            _isDoorEverOpened = false;
                        }
                    }
                    else
                    {
                        _isDoorEverOpened = true;
                    }

                    //抽屉锁定控制
                    /*
                    if (_drawerUnlock == 0)
                    {
                        for (int i = 0; i < _drawerCount; i++)
                        {
                            result = false;
                            lock (DelayLock)
                            {
                                //延时期过后停止解锁过程
                                if ((DateTime.Now - _lastDrawerUnlock[i]).TotalSeconds >= _drawerLockTime)
                                {
                                    result = true;
                                }
                            }

                            if (result)
                            {
                                //锁定抽屉
                                UnlockDrawer(i, false);
                            }
                            else
                            {
                                //保持解锁信号
                                UnlockDrawer(i, true, false);
                            }
                        }
                    }*/
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                Thread.Sleep(200);
            }
        }

        #region CabinetCheckerMaster

        /// <summary>
        /// 检索柜门状态(禁止主线程运行)
        /// </summary>
        /// <returns></returns>
        private int IsDoorClosed()
        {
            if (_cabinetCheckerMaster == null) return -1;
            try
            {
                if (!_cabinetCheckerClient.Connected) return -1;
                bool[] result = _cabinetCheckerMaster.ReadInputs(1, 0, 4);
                if (result[0] && result[1] && result[2] && result[3]) return 1;
                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        #endregion

        #region CabinetOperatorMaster
        public int UnlockDrawer(bool[] status, bool updateSignal = true)
        {
            /*
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                if (updateSignal)
                {
                    lock (DelayLock)
                    {
                        //记录解锁时间
                        for (int i = 0; i < _drawerCount; i++)
                        {
                            if (status[i])
                            {
                                _lastDrawerUnlock[i] = DateTime.Now;
                            }
                            else
                            {
                                _lastDrawerUnlock[i] = DateTime.MinValue;
                            }
                        }
                    }
                }
                //解锁抽屉
                _cabinetOperatorMaster.WriteMultipleCoils(1, 0, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }*/
            return -1;
        }

        public int UnlockDrawer(int drawerNo, bool status, bool updateSignal = true)
        {
            /*
                if (_cabinetOperatorMaster == null) return -1;
                try
                {
                    if (!_cabinetOperatorClient.Connected) return -1;
                    if (updateSignal)
                    {
                        lock (DelayLock)
                        {
                            if (status)
                            {
                                //记录解锁时间
                                _lastDrawerUnlock[drawerNo] = DateTime.Now;
                            }
                            else
                            {
                                _lastDrawerUnlock[drawerNo] = DateTime.MinValue;
                            }
                        }
                    }
                    //解锁抽屉
                    _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)drawerNo, status);
                    return 1;
                }
                catch (Exception)
                {
                    return -10;
                }*/
            return -1;
        }

        void ICabinetDevice.EmulateToolReturn(int toolPosition)
        {
            EmulateToolReturn(toolPosition);
        }


        public int UnlockDoor(bool status, bool updateSignal = true)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                if (updateSignal)
                {
                    lock (DelayLock)
                    {
                        if (status)
                        {
                            //记录解锁时间
                            _lastDoorUnlock = DateTime.Now;
                        }
                        else
                        {
                            _lastDoorUnlock = DateTime.MinValue;
                        }
                    }
                }
                if (status) _isDoorTimeoutClosed = false;
                //柜门操作
                for (int i = 0; i <= 1; i++)
                {
                    if (status)
                    {
                        _cabinetOperatorMaster.WriteMultipleCoils(1, (ushort)(i * 2),
                            new[] { false, true });
                    }
                    else
                    {
                        _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)(1 + i * 2), false);
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int LockDoor(bool status, bool updateSignal = true)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                if (updateSignal)
                {
                    lock (DelayLock)
                    {
                        if (status)
                        {
                            //记录解锁时间
                            _lastDoorLock = DateTime.Now;
                        }
                        else
                        {
                            _lastDoorLock = DateTime.MinValue;
                        }
                    }
                }
                //柜门操作
                for (int i = 0; i <= 1; i++)
                {
                    if (status)
                    {
                        _cabinetOperatorMaster.WriteMultipleCoils(1, (ushort)(i * 2),
                            new[] { true, false });
                    }
                    else
                    {
                        _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)(i * 2), false);
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public bool[] IsDoorUnlocking()
        {
            if (_cabinetOperatorMaster == null) return null;
            try
            {
                if (!_cabinetOperatorClient.Connected) return null;
                return _cabinetOperatorMaster.ReadCoils(1, 0, 1);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
        private int EnableDoorMagnet(bool status)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                //启动磁铁
                _cabinetOperatorMaster.WriteSingleCoil(1, 12, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        private int IsMagnetEnabled()
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                bool[] result = _cabinetOperatorMaster.ReadCoils(1, 0, 1);
                if (result[0]) return 1;
                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }*/

        public int OpenLight(int lightNo, bool status)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)(_lightCtrlPoint + lightNo), status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenAllLight(bool status)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)_lightCtrlPoint, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }


        public int OpenLedGreen(bool status)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                //_cabinetOperatorMaster.WriteSingleCoil(1, 12, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenLedYellow(bool status)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)_ledYellowCtrlPoint, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenLedRed(bool status)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)_ledRedCtrlPoint, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenAlarm(bool status)
        {
            if (_cabinetOperatorMaster == null) return -1;
            try
            {
                if (!_cabinetOperatorClient.Connected) return -1;
                _cabinetOperatorMaster.WriteSingleCoil(1, (ushort)_ledRedCtrlPoint, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }
        #endregion

        #region ToolCheckerMaster
        /// <summary>
        /// 检索工具状态(禁止主线程运行)
        /// </summary>
        /// <returns></returns>
        private static bool[] IsToolExist()
        {
            if (_toolCheckerGroup == null) return null;
            try
            {
                int fullCount = 0;
                bool[] fullresult = new bool[_reserveToolCount];
                for (int i = 0; i < _toolCheckerGroup.Count; i++)
                {
                    if (!_toolCheckerGroup[i].IsConnected()) return null;
                    int readCount = _reserveToolCount - fullCount >=
                    _toolCheckerGroup[i].ControllerCapacity - _toolCheckerGroup[i].ControllerStartIndex
                    ? _toolCheckerGroup[i].ControllerCapacity - _toolCheckerGroup[i].ControllerStartIndex :
                        _reserveToolCount - fullCount;
                    bool[] curResult;
                    int retryCount = 0;
                    //针对不稳定设备优化读取
                    while (true)
                    {
                        try
                        {
                            if (!_toolCheckerGroup[i].IsConnected())
                            {
                                Thread.Sleep(100);
                                continue;
                            }
                            if (retryCount > 5)
                            {
                                if (_toolCheckerGroup[i].ControllerType == 0)
                                {
                                    _toolCheckerGroup[i].ModbusClient.Close();
                                }
                                else if (_toolCheckerGroup[i].ControllerType == 1)
                                {
                                    _toolCheckerGroup[i].ModbusPort.Close();
                                }
                                retryCount = 0;
                                continue;
                            }
                            curResult = _toolCheckerGroup[i].ModbusCtrl.ReadInputs(1, _toolCheckerGroup[i].ControllerStartIndex, (ushort)readCount);
                            break;
                        }
                        catch (Exception)
                        {
                            retryCount++;
                            Thread.Sleep(100);
                        }
                    }
                    curResult.CopyTo(fullresult, fullCount);
                    fullCount += readCount;
                    if (fullCount == _toolCount) break;
                }
                //映射修正
                foreach (DictionaryEntry de in BrokenCheckerMapping)
                {
                    int origin = (int)de.Key;
                    int dest = (int)de.Value;
                    fullresult[origin] = fullresult[dest];
                }
                bool[] result = new bool[_toolCount];
                for (int i = 0; i < _toolCount; i++)
                {
                    result[i] = fullresult[i];
                }
                //返回结果
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CabinetCallback.ToolStatus[] GetToolStatus()
        {
            return ToolStatusList;
        }
        #endregion

        #region ToolLedOperatorMaster
        /// <summary>
        /// 批量设置工具LED状态
        /// </summary>
        /// <param name="status"></param>
        public int SetToolLedStatus(bool[] status)
        {
            if (_toolLedOperatorGroup == null) return -1;
            try
            {
                bool[] fullStatus = new bool[_reserveToolCount];
                status.CopyTo(fullStatus, 0);
                foreach (DictionaryEntry de in BrokenOperatorMapping)
                {
                    int origin = (int)de.Key;
                    int dest = (int)de.Value;
                    fullStatus[dest] = fullStatus[origin];
                }
                int outputCount = 0, fullCount = fullStatus.Length;
                for (int i = 0; i < _toolLedOperatorGroup.Count; i++)
                {
                    int writeCount = _reserveToolCount - outputCount >=
                                     _toolLedOperatorGroup[i].ControllerCapacity - _toolLedOperatorGroup[i].ControllerStartIndex
                        ? _toolLedOperatorGroup[i].ControllerCapacity - _toolLedOperatorGroup[i].ControllerStartIndex :
                        _reserveToolCount - outputCount;
                    bool[] opArray = fullStatus.Skip(outputCount).Take(writeCount).ToArray();
                    _toolLedOperatorGroup[i].ModbusCtrl.WriteMultipleCoils(1, _toolLedOperatorGroup[i].ControllerStartIndex, opArray);
                    outputCount += writeCount;
                    if (outputCount == fullCount) break;
                }
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public void SetDrawerUnlock(int status)
        {
            //_drawerUnlock = status;
        }

        /// <summary>
        /// 尚未修正
        /// </summary>
        /// <returns></returns>
        public bool[] GetToolLedStatus()
        {
            if (_toolLedOperatorGroup == null) return null;
            try
            {
                int fullCount = 0;
                bool[] result = new bool[_toolCount];
                for (int i = 0; i < _toolLedOperatorGroup.Count; i++)
                {
                    if (!_toolLedOperatorGroup[i].IsConnected()) return null;
                    int readCount = _toolCount - fullCount >= _toolLedOperatorGroup[i].ControllerCapacity
                        ? _toolLedOperatorGroup[i].ControllerCapacity : _toolCount - fullCount;
                    bool[] curResult = _toolLedOperatorGroup[i].ModbusCtrl.ReadInputs(1, 0, (ushort)readCount);
                    curResult.CopyTo(result, fullCount);
                    fullCount += readCount;
                    if (fullCount == _toolCount) break;
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 工具警告指示
        /// </summary>
        /// <param name="ledNo"></param>
        /// <param name="expireTime"></param>
        public void StartToolAlert(int ledNo, DateTime expireTime)
        {
            lock (LedLock)
            {
                ToolStatusList[ledNo].AlertStatus = 1;
                ToolStatusList[ledNo].AlertExpire = expireTime;
            }
        }

        public void EndToolAlert(int ledNo)
        {
            lock (LedLock)
            {
                ToolStatusList[ledNo].AlertStatus = 0;
            }
        }

        /// <summary>
        /// 抽屉警告指示
        /// </summary>
        /// <param name="ledNo"></param>
        /// <param name="enableAlert"></param>

        public void SetDrawerAlertStatus(int ledNo, bool enableAlert)
        {
            /*
            lock (LedLock)
            {
                DrawerStatusList[ledNo].AlertStatus = enableAlert ? 1 : 0;
            }*/
        }

        public void SetDrawerLedOpenStatus(int ledNo, bool isOpened)
        {
            /*
            if (isOpened)
            {
                DrawerStatusList[ledNo].OpenStatus = 1;
            }
            else
            {
                DrawerStatusList[ledNo].OpenStatus = 0;
            }*/
        }

        public void SetToolLedWaitStatus(int ledNo, bool isWaiting)
        {
            lock (LedLock)
            {
                if (isWaiting)
                {
                    ToolStatusList[ledNo].WaitStatus = 1;
                }
                else
                {
                    ToolStatusList[ledNo].WaitStatus = 0;
                }
            }
        }

        public void SetToolLedWaiting(List<int> ledNo)
        {
            lock (LedLock)
            {
                for (int i = 0; i < _toolCount; i++)
                {
                    if (ledNo != null && ledNo.Contains(i))
                    {
                        ToolStatusList[i].WaitStatus = 1;
                    }
                    else
                    {
                        ToolStatusList[i].WaitStatus = 0;
                    }
                }
            }
        }

        public void SetToolLedRepairStatus(int ledNo, bool isRepairing)
        {
            lock (LedLock)
            {
                if (ledNo >= _toolCount) return;
                ToolStatusList[ledNo].RepairStatus = isRepairing ? 1 : 0;
            }
        }

        public void SetToolLedCheckStatus(int ledNo, bool needCheck)
        {
            lock (LedLock)
            {
                if (ledNo >= _toolCount) return;
                ToolStatusList[ledNo].CheckStatus = needCheck ? 1 : 0;
            }
        }

        public void ResetToolLed()
        {
            bool[] status = new bool[_toolCount];
            lock (LedLock)
            {
                for (int i = 0; i < _toolCount; i++)
                {
                    ToolStatusList[i].AlertStatus = 0;
                    ToolStatusList[i].WaitStatus = 0;
                    ToolStatusList[i].RepairStatus = 0;
                    status[i] = false;
                }
            }
            //立即生效
            SetToolLedStatus(status);
        }

        public int AddToolCheckerDevice(string toolCheckerIp, int toolCheckerPort, ushort nodeId, ushort toolCheckerCapacity,
            ushort toolCheckerStartIndex)
        {
            throw new NotImplementedException();
        }

        public int AddToolCheckerComDevice(int comPort, ushort nodeId, ushort toolCheckerCapcity, ushort toolCheckerStartIndex)
        {
            throw new NotImplementedException();
        }

        public int AddToolLedOperatorDevice(string toolLedOperatorIp, int toolLedOperatorPort, ushort nodeId,
            ushort toolLedOperatorCapacity, ushort toolLedOperatorStartIndex)
        {
            throw new NotImplementedException();
        }

        public int AddToolLedOperatorDevice(int comPort, ushort nodeId, ushort toolLedOperatorCapacity,
            ushort toolLedOperatorStartIndex)
        {
            throw new NotImplementedException();
        }

        public int InitDevice(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string canBusIp, int canBusPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false)
        {
            throw new NotImplementedException();
        }

        public int InitDevice(ushort toolCount, ushort drawerCount, int canBusComPort, int cabinetCheckerNode = 1,
            int cabinetOperatorNode = 1, bool debugMode = false)
        {
            throw new NotImplementedException();
        }

        public int InitDevicePure(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string canBusIp, int canBusPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
