using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Hardware.NCanBus;
using NLog;
using Utilities.Net;

namespace Hardware.DeviceInterface
{
    public class CabinetDeviceRx4 : ICabinetDevice // 上海检修段工具柜
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CanBus检测设备
        /// </summary>
        private static List<CanbusController> _canBusGroup;

        private static double _doorOperationTime = 0.2;
        private static int _doorKeepTime = 10;
        private static int _drawerLockTime = 30;
        private static int _drawerUnlock;

        /// <summary>
        /// 工具箱计数
        /// </summary>
        private static ushort _toolCount;
        private static ushort _reserveToolCount;

        //点位替换
        private static readonly Hashtable BrokenCheckerMapping = new Hashtable();
        private static readonly Hashtable BrokenOperatorMapping = new Hashtable();
        private static int _lightCtrlPoint = 6;
        private static int _ledGreenCtrlPoint = 7;
        private static int _ledYellowCtrlPoint = 8;
        private static int _ledRedCtrlPoint = 9;

        /// <summary>
        /// 抽屉计数
        /// </summary>
        private static ushort _drawerCount;
        /// <summary>
        /// 初始化状态
        /// </summary>
        private static readonly object LedLock = new object();

        //延时信息
        private static DateTime _lastDoorUnlock, _lastDoorLock;
        private static DateTime[] _lastDrawerUnlock;
        private static readonly object DelayLock = new object();
        //门控信息
        private static bool _isDoorEverOpened;
        private static bool _isDoorTimeoutClosed;

        private static readonly Hashtable ToolCheckerCache = new Hashtable();
        private static readonly Hashtable LedOperatorCache = new Hashtable();

        public class CanbusController
        {
            public CanbusMaster CanbusCtrl;

            /// <summary>
            /// 连接类型 0=TCPIP
            /// </summary>
            public int ControllerType;
            /// <summary>
            /// 连接地址
            /// </summary>
            public string NetAddr;
            /// <summary>
            /// TCP方式连接端口
            /// </summary>
            public int NetPort;
            /// <summary>
            /// COM方式连接端口
            /// </summary>
            public string ComPort;

            public List<CanBusNode> ToolCheckerNodeList;
            public List<CanBusNode> LedOperatorNodeList;

            public int CabinetCheckerNode = -1;
            public int CabinetOperatorNode = -1;


            /// <summary>
            /// TCPIP方式连接对象
            /// </summary>
            public TcpClient CanbusClient;

            public SerialPort CanbusPort;

            public bool IsConnected()
            {
                if (ControllerType == 0)
                {
                    return CanbusClient.Connected;
                }
                if (ControllerType == 1)
                {
                    if (CanbusPort != null) return CanbusPort.IsOpen;
                    return false;
                }
                return false;
            }
        }

        public class CanBusNode
        {
            /// <summary>
            /// Node Id
            /// </summary>
            public int NodeId;

            /// <summary>
            /// 下级设备容量
            /// </summary>
            public ushort ControllerCapacity;
            /// <summary>
            /// 设备起始编号
            /// </summary>
            public ushort ControllerStartIndex;
        }

        /// <summary>
        /// 工具状态列表
        /// </summary>
        private static readonly CabinetCallback.ToolStatus[] ToolStatusList = new CabinetCallback.ToolStatus[256];

        /// <summary>
        /// 工具状态列表
        /// </summary>
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
        }

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
            _drawerLockTime = unlockTime;
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
            int toolCheckerPort, ushort nodeId, ushort toolCheckerCapacity, ushort toolCheckerStartIndex)
        {
            if (!string.IsNullOrEmpty(toolCheckerIp))
            {
                if (ToolCheckerCache[toolCheckerIp] == null) ToolCheckerCache[toolCheckerIp] = new List<CanBusNode>();
                CanBusNode nodeInfo = new CanBusNode
                {
                    NodeId = nodeId,
                    ControllerCapacity = toolCheckerCapacity,
                    ControllerStartIndex = toolCheckerStartIndex
                };
                ((List<CanBusNode>)ToolCheckerCache[toolCheckerIp]).Add(nodeInfo);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 添加工具检查模块
        /// </summary>
        /// <returns></returns>
        public int AddToolCheckerComDevice(int comPort, ushort nodeId, ushort toolCheckerCapcity, ushort toolCheckerStartIndex)
        {
            if (comPort >= 0)
            {
                if (ToolCheckerCache[comPort] == null) ToolCheckerCache[comPort] = new List<CanBusNode>();
                CanBusNode mc = new CanBusNode
                {
                    NodeId = nodeId,
                    ControllerCapacity = toolCheckerCapcity,
                    ControllerStartIndex = toolCheckerStartIndex,
                };
                ((List<CanBusNode>)ToolCheckerCache[comPort]).Add(mc);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 添加LED控制模块
        /// </summary>
        /// <returns></returns>
        public int AddToolLedOperatorDevice(string toolLedOperatorIp,
            int toolLedOperatorPort, ushort nodeId, ushort toolLedOperatorCapacity, ushort toolLedOperatorStartIndex)
        {
            if (!string.IsNullOrEmpty(toolLedOperatorIp))
            {
                if (LedOperatorCache[toolLedOperatorIp] == null) LedOperatorCache[toolLedOperatorIp] = new List<CanBusNode>();
                CanBusNode nodeInfo = new CanBusNode
                {
                    NodeId = nodeId,
                    ControllerCapacity = toolLedOperatorCapacity,
                    ControllerStartIndex = toolLedOperatorStartIndex
                };
                ((List<CanBusNode>)LedOperatorCache[toolLedOperatorIp]).Add(nodeInfo);
                return 1;
            }
            return 0;
        }

        public int AddToolLedOperatorDevice(int comPort, ushort nodeId, ushort toolLedOperatorCapacity, ushort toolLedOperatorStartIndex)
        {
            if (comPort >= 0)
            {
                if (LedOperatorCache[comPort] == null) LedOperatorCache[comPort] = new List<CanBusNode>();
                CanBusNode mc = new CanBusNode
                {
                    NodeId = nodeId,
                    ControllerCapacity = toolLedOperatorCapacity,
                    ControllerStartIndex = toolLedOperatorStartIndex
                };
                ((List<CanBusNode>)LedOperatorCache[comPort]).Add(mc);
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
        /// <param name="cabinetOperatorNode"></param>
        /// <param name="debugMode"></param>
        /// <param name="canBusIp"></param>
        /// <param name="canBusPort"></param>
        /// <param name="cabinetCheckerNode"></param>
        /// <returns></returns>
        public int InitDevice(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string canBusIp, int canBusPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false)
        {
            _toolCount = toolCount;
            _reserveToolCount = reserveToolCount;
            _drawerCount = drawerCount;

            _canBusGroup = new List<CanbusController>();
            CanbusController mainController = new CanbusController
            {
                NetAddr = canBusIp,
                NetPort = canBusPort,
                CabinetCheckerNode = cabinetCheckerNode,
                CabinetOperatorNode = cabinetOperatorNode,
                ControllerType = 0
            };
            if (ToolCheckerCache[canBusIp] != null)
            {
                mainController.ToolCheckerNodeList = (List<CanBusNode>)ToolCheckerCache[canBusIp];
            }
            if (LedOperatorCache[canBusIp] != null)
            {
                mainController.LedOperatorNodeList = (List<CanBusNode>)LedOperatorCache[canBusIp];
            }
            _canBusGroup.Add(mainController);

            _lastDrawerUnlock = new DateTime[_drawerCount];
            for (int i = 0; i < _drawerCount; i++)
            {
                _lastDrawerUnlock[i] = DateTime.MinValue;
            }

            for (int i = 0; i < _toolCount; i++)
            {
                ToolStatusList[i] = new CabinetCallback.ToolStatus();
            }
            for (int i = 0; i < _drawerCount; i++)
            {
                DrawerStatusList[i] = new DrawerStatus();
            }

            Thread initThread = new Thread(Init) { IsBackground = true };
            initThread.Start();
            return 1;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="toolCount">工具箱计数</param>
        /// <param name="drawerCount">抽屉计数</param>
        /// <param name="canBusComPort"></param>
        /// <param name="cabinetCheckerNode"></param>
        /// <param name="cabinetOperatorNode"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public int InitDevice(ushort toolCount, ushort drawerCount, int canBusComPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false)
        {
            //_toolCount = toolCount;
            //_drawerCount = drawerCount;

            //_canBusGroup = new List<CanbusController>();
            //CanbusController mainController = new CanbusController
            //{
            //    ComPort = "COM" + canBusComPort,
            //    CabinetCheckerNode = cabinetCheckerNode,
            //    CabinetOperatorNode = cabinetOperatorNode,
            //    ControllerType = 1
            //};
            //if (ToolCheckerCache[canBusComPort] != null)
            //{
            //    mainController.ToolCheckerNodeList = (List<CanBusNode>)ToolCheckerCache[canBusComPort];
            //}
            //if (LedOperatorCache[canBusComPort] != null)
            //{
            //    mainController.LedOperatorNodeList = (List<CanBusNode>)LedOperatorCache[canBusComPort];
            //}
            //_canBusGroup.Add(mainController);

            //_lastDrawerUnlock = new DateTime[_drawerCount];
            //for (int i = 0; i < _drawerCount; i++)
            //{
            //    _lastDrawerUnlock[i] = DateTime.MinValue;
            //}

            //for (int i = 0; i < _toolCount; i++)
            //{
            //    ToolStatusList[i] = new CabinetCallback.ToolStatus();
            //}
            //for (int i = 0; i < _drawerCount; i++)
            //{
            //    DrawerStatusList[i] = new DrawerStatus();
            //}

            //Thread initThread = new Thread(Init) { IsBackground = true };
            //initThread.Start();
            return 1;
        }

        public int InitDevicePure(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string canBusIp, int canBusPort,
            int cabinetCheckerNode = 1, int cabinetOperatorNode = 1, bool debugMode = false)
        {
            //_toolCount = toolCount;
            //_reserveToolCount = reserveToolCount;
            //_drawerCount = drawerCount;

            //_canBusGroup = new List<CanbusController>();
            //CanbusController mainController = new CanbusController
            //{
            //    NetAddr = canBusIp,
            //    NetPort = canBusPort,
            //    CabinetCheckerNode = cabinetCheckerNode,
            //    CabinetOperatorNode = cabinetOperatorNode,
            //    ControllerType = 0
            //};
            //if (ToolCheckerCache[canBusIp] != null)
            //{
            //    mainController.ToolCheckerNodeList = (List<CanBusNode>)ToolCheckerCache[canBusIp];
            //}
            //if (LedOperatorCache[canBusIp] != null)
            //{
            //    mainController.LedOperatorNodeList = (List<CanBusNode>)LedOperatorCache[canBusIp];
            //}
            //_canBusGroup.Add(mainController);

            //_lastDrawerUnlock = new DateTime[_drawerCount];
            //for (int i = 0; i < _drawerCount; i++)
            //{
            //    _lastDrawerUnlock[i] = DateTime.MinValue;
            //}

            //for (int i = 0; i < _toolCount; i++)
            //{
            //    ToolStatusList[i] = new CabinetCallback.ToolStatus();
            //}
            //for (int i = 0; i < _drawerCount; i++)
            //{
            //    DrawerStatusList[i] = new DrawerStatus();
            //}

            //Thread initThread = new Thread(InitPure) { IsBackground = true };
            //initThread.Start();
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
                if (_drawerUnlock != 0)
                {
                    for (int i = 0; i < _drawerCount; i++)
                    {
                        UnlockDrawer(i, true, false);
                    }
                }
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
                for (int i = 0; i < _canBusGroup.Count; i++)
                {
                    if (_canBusGroup[i].ControllerType == 0)
                    {
                        if (_canBusGroup[i].CanbusClient == null || !_canBusGroup[i].CanbusClient.Connected)
                        {
                            TcpClientEx client = new TcpClientEx(_canBusGroup[i].NetAddr, _canBusGroup[i].NetPort, 8000);
                            _canBusGroup[i].CanbusClient = client.Connect();
                            _canBusGroup[i].CanbusClient.SendTimeout = 3000;
                            _canBusGroup[i].CanbusClient.ReceiveTimeout = 3000;
                            _canBusGroup[i].CanbusCtrl =
                                CanbusIpMaster.CreateIp(_canBusGroup[i].CanbusClient);
                        }
                    }
                    else if (_canBusGroup[i].ControllerType == 1)
                    {
                        if (_canBusGroup[i].CanbusPort == null || !_canBusGroup[i].CanbusPort.IsOpen)
                        {
                            _canBusGroup[i].CanbusPort = new SerialPort(_canBusGroup[i].ComPort, 9600,
                                Parity.None, 8, StopBits.One);
                            _canBusGroup[i].CanbusPort.Open();
                            _canBusGroup[i].CanbusCtrl =
                                CanbusSerialMaster.CreateRtu(_canBusGroup[i].CanbusPort);
                        }
                    }
                }
                InitRtu(true);
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
                    //抽屉锁定控制
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
                    }
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
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                Hashtable infoBuffer = _canBusGroup[0].CanbusCtrl.GetStatusBuffer();
                bool[] statusBuffer = (bool[])infoBuffer[1];
                if (statusBuffer[0] && statusBuffer[1]) return 1;
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
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
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
                _canBusGroup[0].CanbusCtrl.WriteMultipleCoils(1, 2, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int UnlockDrawer(int drawerNo, bool status, bool updateSignal = true)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
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
                _canBusGroup[0].CanbusCtrl.WriteSingleCoil(1, 2 + drawerNo, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int UnlockDoor(bool status, bool updateSignal = true)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
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
                for (int i = 0; i < 4; i++)
                {
                    if (status)
                    {
                        _canBusGroup[0].CanbusCtrl.WriteMultipleCoils(1, 0, new[] { true, true });
                        //Thread.Sleep(2000);
                        //_canBusGroup[0].CanbusCtrl.WriteMultipleCoils(1, 0, new[] { false, false });
                    }
                    else
                    {
                        _canBusGroup[0].CanbusCtrl.WriteMultipleCoils(1, 0, new[] { false, false });
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                return -10;
            }
        }

        public int LockDoor(bool status, bool updateSignal = true)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
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
                for (int i = 0; i < 4; i++)
                {
                    if (status)
                    {
                        _canBusGroup[0].CanbusCtrl.WriteMultipleCoils(1, 0, new[] { false, false });
                    }
                    else
                    {
                        _canBusGroup[0].CanbusCtrl.WriteMultipleCoils(1, 0, new[] { true, true });
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
            throw new NotImplementedException();
        }

        public int OpenLight(int lightNo, bool status)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                _canBusGroup[0].CanbusCtrl.WriteSingleCoil(1, (ushort)(_lightCtrlPoint + lightNo), status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenAllLight(bool status)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                _canBusGroup[0].CanbusCtrl.WriteSingleCoil(1, (ushort)_lightCtrlPoint, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public static int InitRtu(bool status)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                _canBusGroup[0].CanbusCtrl.InitRtu(1);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }


        public int OpenLedGreen(bool status)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                //_canBusGroup[0].CanbusCtrl.WriteSingleCoil(1, 12, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenLedYellow(bool status)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                _canBusGroup[0].CanbusCtrl.WriteSingleCoil(1, (ushort)_ledYellowCtrlPoint, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenLedRed(bool status)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                _canBusGroup[0].CanbusCtrl.WriteSingleCoil(1, (ushort)_ledRedCtrlPoint, status);
                return 1;
            }
            catch (Exception)
            {
                return -10;
            }
        }

        public int OpenAlarm(bool status)
        {
            if (_canBusGroup == null) return -1;
            try
            {
                if (!_canBusGroup[0].CanbusCtrl.Connected) return -1;
                _canBusGroup[0].CanbusCtrl.WriteSingleCoil(1, (ushort)_ledRedCtrlPoint, status);
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
            if (_canBusGroup == null) return null;
            try
            {
                bool[] fullResult = new bool[_reserveToolCount];
                Hashtable infoBuffer = _canBusGroup[0].CanbusCtrl.GetStatusBuffer();
                List<bool> statusList = new List<bool>();
                for (int i = 0; i < _canBusGroup[0].ToolCheckerNodeList.Count; i++)
                {
                    int nodeId = _canBusGroup[0].ToolCheckerNodeList[i].NodeId;
                    if (infoBuffer[nodeId] == null) continue;
                    statusList.AddRange((bool[])infoBuffer[nodeId]);
                }
                for (int i = 0; i < statusList.Count; i++)
                {
                    fullResult[i] = statusList[i];
                }
                //映射修正
                foreach (DictionaryEntry de in BrokenCheckerMapping)
                {
                    int origin = (int)de.Key;
                    int dest = (int)de.Value;
                    fullResult[origin] = fullResult[dest];
                }
                bool[] result = new bool[_toolCount];
                for (int i = 0; i < _toolCount; i++)
                {
                    result[i] = fullResult[i];
                }
                //返回结果
                return result;
            }
            catch (Exception e)
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
            if (_canBusGroup == null) return -1;
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
                for (int i = 0; i < _canBusGroup.Count; i++)
                {
                    for (int j = 0; j < _canBusGroup[i].LedOperatorNodeList.Count; j++)
                    {
                        int writeCount = _reserveToolCount - outputCount >=
                                         _canBusGroup[i].LedOperatorNodeList[j].ControllerCapacity - _canBusGroup[i].LedOperatorNodeList[j].ControllerStartIndex
                            ? _canBusGroup[i].LedOperatorNodeList[j].ControllerCapacity - _canBusGroup[i].LedOperatorNodeList[j].ControllerStartIndex :
                            _reserveToolCount - outputCount;
                        bool[] opArray = fullStatus.Skip(outputCount).Take(writeCount).ToArray();
                        _canBusGroup[i].CanbusCtrl.WriteMultipleCoils(_canBusGroup[i].LedOperatorNodeList[j].NodeId, _canBusGroup[i].LedOperatorNodeList[j].ControllerStartIndex, opArray);
                        outputCount += writeCount;
                        if (outputCount == fullCount) break;
                    }
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
            _drawerUnlock = status;
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
            lock (LedLock)
            {
                DrawerStatusList[ledNo].AlertStatus = enableAlert ? 1 : 0;
            }
        }

        public void SetDrawerLedOpenStatus(int ledNo, bool isOpened)
        {
            if (isOpened)
            {
                DrawerStatusList[ledNo].OpenStatus = 1;
            }
            else
            {
                DrawerStatusList[ledNo].OpenStatus = 0;
            }
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
        #endregion

        //Not Valid For Can Device
        public bool[] GetToolLedStatus()
        {
            throw new NotImplementedException();
        }

        public int AddToolCheckerDevice(string toolCheckerIp, int toolCheckerPort, ushort toolCheckerCapacity,
            ushort toolCheckerStartIndex)
        {
            throw new NotImplementedException();
        }

        public int AddToolCheckerComDevice(int comPort, ushort toolCheckerCapcity, ushort toolCheckerStartIndex)
        {
            throw new NotImplementedException();
        }

        public int AddToolLedOperatorDevice(string toolLedOperatorIp, int toolLedOperatorPort, ushort toolLedOperatorCapacity,
            ushort toolLedOperatorStartIndex)
        {
            throw new NotImplementedException();
        }

        public int InitDevice(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string cabinetCheckerIp,
            string cabinetOperatorIp, int cabinetCheckerPort = 502, int cabinetOperatorPort = 502, bool debugMode = false)
        {
            throw new NotImplementedException();
        }

        public int InitDevice(ushort toolCount, ushort drawerCount, int cabinetCheckerComPort, int cabinetOperatorComPort,
            bool debugMode = false)
        {
            throw new NotImplementedException();
        }

        public int InitDevicePure(ushort toolCount, ushort reserveToolCount, ushort drawerCount, string cabinetCheckerIp,
            string cabinetOperatorIp, int cabinetCheckerPort = 502, int cabinetOperatorPort = 502, bool debugMode = false)
        {
            throw new NotImplementedException();
        }
    }
}
