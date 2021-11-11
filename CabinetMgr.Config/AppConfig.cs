// ReSharper disable RedundantToStringCall

using System.Globalization;
using Utilities.FileHelper;

namespace CabinetMgr.Config
{
    public class AppConfig
    {
        /// <summary>
        /// 程序名称，用于软件更新
        /// </summary>
        public static string AppName
        {
            get => _appName;
            set
            {
                _appName = value;
                IniHelper.Write("AppConfig", "AppName", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 程序ID，用于软件更新
        /// </summary>
        public static string AppID
        {
            get => _appID;
            set
            {
                _appName = value;
                IniHelper.Write("AppConfig", "AppID", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用户ID，用于软件更新
        /// </summary>
        public static string UserID
        {
            get => _userID;
            set
            {
                _appName = value;
                IniHelper.Write("AppConfig", "UserID", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 程序单位，用于软件更新
        /// </summary>
        public static string AppUnit
        {
            get => _appUnit;
            set
            {
                _appUnit = value;
                IniHelper.Write("AppConfig", "AppUnit", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 程序类型，0=转向架，1=电机
        /// </summary>
        public static int AppType
        {
            get => _appType;
            set
            {
                _appType = value;
                IniHelper.Write("AppConfig", "AppType", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 设备类型，0=未配置，1=电机，2=转向机V1，3=转向架V2
        /// </summary>
        public static int DeviceType
        {
            get => _deviceType;
            set
            {
                _deviceType = value;
                IniHelper.Write("AppConfig", "DeviceType", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 自动更新WebService地址
        /// </summary>
        public static string UpdateServiceUrl
        {
            get => _updateServiceUrl;
            set
            {
                _updateServiceUrl = value;
                IniHelper.Write("AppConfig", "UpdateServiceUrl", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 工具信息WebService地址
        /// </summary>
        public static string ToolServiceUrl
        {
            get => _toolServiceUrl;
            set
            {
                _toolServiceUrl = value;
                IniHelper.Write("AppConfig", "ToolServiceUrl", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用于屏蔽更新功能
        /// </summary>
        public static int DisableUpdate
        {
            get => _disableUpdate;
            set
            {
                _disableUpdate = value;
                IniHelper.Write("AppConfig", "DisableUpdate", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用于屏蔽协助功能
        /// </summary>
        public static int DisableWatcher
        {
            get => _disableWatcher;
            set
            {
                _disableWatcher = value;
                IniHelper.Write("AppConfig", "DisableWatcher", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 调试模式开关
        /// </summary>
        public static int DebugMode
        {
            get => _debugMode;
            set
            {
                _debugMode = value;
                IniHelper.Write("AppConfig", "DebugMode", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 本地演示模式开关
        /// </summary>
        public static int LocalMode
        {
            get => _localMode;
            set
            {
                _localMode = value;
                IniHelper.Write("AppConfig", "LocalMode", value.ToString(), Env.ConfigPath);
            }
        }

        public static int RecvLog
        {
            get => _recvLog;
            set
            {
                _recvLog = value;
                IniHelper.Write("AppConfig", "RecvLog", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 本地数据库连接字符串
        /// </summary>
        public static string LocalDb
        {
            get => _localDb;
            set => _localDb = value;
        }

        /// <summary>
        /// Magta扳手接收器COM端口号
        /// </summary>
        public static int MagtaPort
        {
            get => _magtaPort;
            set
            {
                _magtaPort = value;
                IniHelper.Write("AppConfig", "MagtaPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// IR扳手接收器COM端口号
        /// </summary>
        public static int IRPort
        {
            get => _iRPort;
            set
            {
                _iRPort = value;
                IniHelper.Write("AppConfig", "IRPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// IR扳手接收器COM端口号
        /// </summary>
        public static int IRPort2
        {
            get => _iRPort2;
            set
            {
                _iRPort2 = value;
                IniHelper.Write("AppConfig", "IRPort2", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// IR扳手是否适用
        /// </summary>
        public static int IRUsing
        {
            get => _iRUsing;
            set
            {
                _iRUsing = value;
                IniHelper.Write("AppConfig", "IRUsing", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 千分尺接收器COM端口号
        /// </summary>
        public static int MeasuringPort
        {
            get => _measuringPort;
            set
            {
                _measuringPort = value;
                IniHelper.Write("AppConfig", "MeasuringPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 外径千分尺数值范围MAX
        /// </summary>
        public static float MeasuringWjMax
        {
            get => _measuringWjMax;
            set
            {
                _measuringWjMax = value;
                IniHelper.Write("AppConfig", "MeasuringWjMax", value.ToString(CultureInfo.InvariantCulture), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 外径千分尺数值范围MIN
        /// </summary>
        public static float MeasuringWjMin
        {
            get => _measuringWjMin;
            set
            {
                _measuringWjMin = value;
                IniHelper.Write("AppConfig", "MeasuringWjMin", value.ToString(CultureInfo.InvariantCulture), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 考勤机类型
        /// </summary>
        public static string IfaceType
        {
            get => _ifacetype;
            set
            {
                _ifacetype = value;
                IniHelper.Write("AppConfig", "IfaceType", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 中控指纹人脸识别设备IP
        /// </summary>
        public static string IfaceIp
        {
            get => _ifaceIp;
            set
            {
                _ifaceIp = value;
                IniHelper.Write("AppConfig", "IfaceIp", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 中控指纹人脸识别设备端口号
        /// </summary>
        public static int IfacePort
        {
            get => _ifacePort;
            set
            {
                _ifacePort = value;
                IniHelper.Write("AppConfig", "IfacePort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 中控指纹人脸识别设备异步加载开关(存在低版本系统兼容问题，建议关闭)
        /// </summary>
        public static int IfaceAsyncCheck
        {
            get => _ifaceAsyncCheck;
            set
            {
                _ifaceAsyncCheck = value;
                IniHelper.Write("AppConfig", "IfaceAsyncCheck", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 下班时间(下班后未归还设备将显示异常)
        /// </summary>
        public static string OffDutyTime
        {
            get => _offDutyTime;
            set
            {
                _offDutyTime = value;
                IniHelper.Write("AppConfig", "OffDutyTime", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 工具柜ID(用于数据同步等，请保证各个工具柜此项不同)
        /// </summary>
        public static string CabinetName
        {
            get => _cabinetName;
            set
            {
                _cabinetName = value;
                IniHelper.Write("AppConfig", "CabinetName", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 工具柜位置(保修时将上报此位置)
        /// </summary>
        public static string CabinetPosition
        {
            get => _cabinetPosition;
            set
            {
                _cabinetPosition = value;
                IniHelper.Write("AppConfig", "CabinetPosition", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用于柜门班组控制
        /// </summary>
        public static string AllowedGroup
        {
            get => _allowedGroup;
            set
            {
                _allowedGroup = value;
                IniHelper.Write("AppConfig", "AllowedGroup", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用于柜门班组控制
        /// </summary>
        public static string Drawer1Group
        {
            get => _drawer1Group;
            set
            {
                _drawer1Group = value;
                IniHelper.Write("AppConfig", "Drawer1Group", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用于柜门班组控制
        /// </summary>
        public static string Drawer2Group
        {
            get => _drawer2Group;
            set
            {
                _drawer2Group = value;
                IniHelper.Write("AppConfig", "Drawer2Group", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用于柜门班组控制
        /// </summary>
        public static string Drawer3Group
        {
            get => _drawer3Group;
            set
            {
                _drawer3Group = value;
                IniHelper.Write("AppConfig", "Drawer3Group", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 用于柜门班组控制
        /// </summary>
        public static string Drawer4Group
        {
            get => _drawer4Group;
            set
            {
                _drawer4Group = value;
                IniHelper.Write("AppConfig", "Drawer4Group", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 副屏工具编号X坐标
        /// </summary>
        public static int ToolCodeX
        {
            get => _toolCodeX;
            set
            {
                _toolCodeX = value;
                IniHelper.Write("AppConfig", "ToolCodeX", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 副屏工具编号Y坐标
        /// </summary>
        public static int ToolCodeY
        {
            get => _toolCodeY;
            set
            {
                _toolCodeY = value;
                IniHelper.Write("AppConfig", "ToolCodeY", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 副屏工具状态X坐标
        /// </summary>
        public static int ToolStatusX
        {
            get => _toolStatusX;
            set
            {
                _toolStatusX = value;
                IniHelper.Write("AppConfig", "ToolStatusX", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 副屏工具状态Y坐标
        /// </summary>
        public static int ToolStatusY
        {
            get => _toolStatusY;
            set
            {
                _toolStatusY = value;
                IniHelper.Write("AppConfig", "ToolStatusY", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 报修界面工具编号X坐标
        /// </summary>
        public static int ReportToolCodeX
        {
            get => _reportToolCodeX;
            set
            {
                _reportToolCodeX = value;
                IniHelper.Write("AppConfig", "ReportToolCodeX", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 报修界面工具编号Y坐标
        /// </summary>
        public static int ReportToolCodeY
        {
            get => _reportToolCodeY;
            set
            {
                _reportToolCodeY = value;
                IniHelper.Write("AppConfig", "ReportToolCodeY", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 报修界面工具状态X坐标
        /// </summary>
        public static int ReportToolStatusX
        {
            get => _reportToolStatusX;
            set
            {
                _reportToolStatusX = value;
                IniHelper.Write("AppConfig", "ReportToolStatusX", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 报修界面工具状态Y坐标
        /// </summary>
        public static int ReportToolStatusY
        {
            get => _reportToolStatusY;
            set
            {
                _reportToolStatusY = value;
                IniHelper.Write("AppConfig", "ReportToolStatusY", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 工具数量
        /// </summary>
        public static int ToolCount
        {
            get => _toolCount;
            set
            {
                _toolCount = value;
                IniHelper.Write("AppConfig", "ToolCount", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 保留工具数量
        /// </summary>
        public static int ReserveToolCount
        {
            get => _reserveToolCount;
            set
            {
                _reserveToolCount = value;
                IniHelper.Write("AppConfig", "ReserveToolCount", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 副屏每行显示工具个数
        /// </summary>
        public static int ToolCol
        {
            get => _toolCol;
            set
            {
                _toolCol = value;
                IniHelper.Write("AppConfig", "ToolCol", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 报修界面每行显示工具个数
        /// </summary>
        public static int ReportToolCol
        {
            get => _reportToolCol;
            set
            {
                _reportToolCol = value;
                IniHelper.Write("AppConfig", "ReportToolCol", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 同步功能WebService地址
        /// </summary>
        public static string SyncServiceUrl
        {
            get => _syncServiceUrl;
            set
            {
                _syncServiceUrl = value;
                IniHelper.Write("AppConfig", "SyncServiceUrl", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 量值功能WebService地址
        /// </summary>
        public static string MeasureServiceUrl
        {
            get => _measureServiceUrl;
            set
            {
                _measureServiceUrl = value;
                IniHelper.Write("AppConfig", "MeasureServiceUrl", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 校验功能Url地址
        /// </summary>
        public static string ChecksumWebUrl
        {
            get => _checksumWebUrl;
            set
            {
                _checksumWebUrl = value;
                IniHelper.Write("AppConfig", "ChecksumWebUrl", value.ToString(), Env.ConfigPath);
            }
        }

        public static string CanBusIp
        {
            get => _canBusIp;
            set
            {
                _canBusIp = value;
                IniHelper.Write("AppConfig", "CanBusIp", value.ToString(), Env.ConfigPath);
            }
        }

        public static int CanBusPort
        {
            get => _canBusPort;
            set
            {
                _canBusPort = value;
                IniHelper.Write("AppConfig", "CanBusPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 柜体检测MODBUS模块IP
        /// </summary>
        public static string CabinetCheckerIp
        {
            get => _cabinetCheckerIp;
            set
            {
                _cabinetCheckerIp = value;
                IniHelper.Write("AppConfig", "CabinetCheckerIp", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 柜体检测MODBUS模块端口
        /// </summary>
        public static int CabinetCheckerPort
        {
            get => _cabinetCheckerPort;
            set
            {
                _cabinetCheckerPort = value;
                IniHelper.Write("AppConfig", "CabinetCheckerPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 柜体操作MODBUS模块IP
        /// </summary>
        public static string CabinetOperatorIp
        {
            get => _cabinetOperatorIp;
            set
            {
                _cabinetOperatorIp = value;
                IniHelper.Write("AppConfig", "CabinetOperatorIp", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 柜体操作MODBUS模块端口
        /// </summary>
        public static int CabinetOperatorPort
        {
            get => _cabinetOperatorPort;
            set
            {
                _cabinetOperatorPort = value;
                IniHelper.Write("AppConfig", "CabinetOperatorPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块1 IP
        /// </summary>
        public static string ToolCheckerIp
        {
            get => _toolCheckerIp;
            set
            {
                _toolCheckerIp = value;
                IniHelper.Write("AppConfig", "ToolCheckerIp", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块1 端口
        /// </summary>
        public static int ToolCheckerPort
        {
            get => _toolCheckerPort;
            set
            {
                _toolCheckerPort = value;
                IniHelper.Write("AppConfig", "ToolCheckerPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块1 点数
        /// </summary>
        public static int ToolCheckerCapacity
        {
            get => _toolCheckerCapacity;
            set
            {
                _toolCheckerCapacity = value;
                IniHelper.Write("AppConfig", "ToolCheckerCapacity", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块1 起始点
        /// </summary>
        public static int ToolCheckerStartIndex
        {
            get => _toolCheckerStartIndex;
            set
            {
                _toolCheckerStartIndex = value;
                IniHelper.Write("AppConfig", "ToolCheckerStartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块1 COM端口号
        /// </summary>
        public static int ToolCheckerCom
        {
            get => _toolCheckerCom;
            set
            {
                _toolCheckerCom = value;
                IniHelper.Write("AppConfig", "ToolCheckerCom", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolCheckerNodeId
        {
            get => _toolCheckerNodeId;
            set
            {
                _toolCheckerNodeId = value;
                IniHelper.Write("AppConfig", "ToolCheckerNodeId", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块2 IP
        /// </summary>
        public static string ToolChecker2Ip
        {
            get => _toolChecker2Ip;
            set
            {
                _toolChecker2Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker2Ip", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块2 端口
        /// </summary>
        public static int ToolChecker2Port
        {
            get => _toolChecker2Port;
            set
            {
                _toolChecker2Port = value;
                IniHelper.Write("AppConfig", "ToolChecker2Port", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块2 点数
        /// </summary>
        public static int ToolChecker2Capacity
        {
            get => _toolChecker2Capacity;
            set
            {
                _toolChecker2Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker2Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块2 起始点
        /// </summary>
        public static int ToolChecker2StartIndex
        {
            get => _toolChecker2StartIndex;
            set
            {
                _toolChecker2StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker2StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 接近开关检测模块2 COM端口号
        /// </summary>
        public static int ToolChecker2Com
        {
            get => _toolChecker2Com;
            set
            {
                _toolChecker2Com = value;
                IniHelper.Write("AppConfig", "ToolChecker2Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker2NodeId
        {
            get => _toolChecker2NodeId;
            set
            {
                _toolChecker2NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker2NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolChecker3Ip
        {
            get => _toolChecker3Ip;
            set
            {
                _toolChecker3Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker3Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker3Port
        {
            get => _toolChecker3Port;
            set
            {
                _toolChecker3Port = value;
                IniHelper.Write("AppConfig", "ToolChecker3Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker3Capacity
        {
            get => _toolChecker3Capacity;
            set
            {
                _toolChecker3Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker3Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker3StartIndex
        {
            get => _toolChecker3StartIndex;
            set
            {
                _toolChecker3StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker3StartIndex", value.ToString(), Env.ConfigPath);
            }
        }


        public static int ToolChecker3Com
        {
            get => _toolChecker3Com;
            set
            {
                _toolChecker3Com = value;
                IniHelper.Write("AppConfig", "ToolChecker3Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker3NodeId
        {
            get => _toolChecker3NodeId;
            set
            {
                _toolChecker3NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker3NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolChecker4Ip
        {
            get => _toolChecker4Ip;
            set
            {
                _toolChecker4Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker4Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker4Port
        {
            get => _toolChecker4Port;
            set
            {
                _toolChecker4Port = value;
                IniHelper.Write("AppConfig", "ToolChecker4Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker4Capacity
        {
            get => _toolChecker4Capacity;
            set
            {
                _toolChecker4Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker4Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker4StartIndex
        {
            get => _toolChecker4StartIndex;
            set
            {
                _toolChecker4StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker4StartIndex", value.ToString(), Env.ConfigPath);
            }
        }


        public static int ToolChecker4Com
        {
            get => _toolChecker4Com;
            set
            {
                _toolChecker4Com = value;
                IniHelper.Write("AppConfig", "ToolChecker4Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker4NodeId
        {
            get => _toolChecker4NodeId;
            set
            {
                _toolChecker4NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker4NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolChecker5Ip
        {
            get => _toolChecker5Ip;
            set
            {
                _toolChecker5Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker5Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker5Port
        {
            get => _toolChecker5Port;
            set
            {
                _toolChecker5Port = value;
                IniHelper.Write("AppConfig", "ToolChecker5Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker5Capacity
        {
            get => _toolChecker5Capacity;
            set
            {
                _toolChecker5Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker5Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker5StartIndex
        {
            get => _toolChecker5StartIndex;
            set
            {
                _toolChecker5StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker5StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker5Com
        {
            get => _toolChecker5Com;
            set
            {
                _toolChecker5Com = value;
                IniHelper.Write("AppConfig", "ToolChecker5Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker5NodeId
        {
            get => _toolChecker5NodeId;
            set
            {
                _toolChecker5NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker5NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolChecker6Ip
        {
            get => _toolChecker6Ip;
            set
            {
                _toolChecker6Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker6Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker6Port
        {
            get => _toolChecker6Port;
            set
            {
                _toolChecker6Port = value;
                IniHelper.Write("AppConfig", "ToolChecker6Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker6Capacity
        {
            get => _toolChecker6Capacity;
            set
            {
                _toolChecker6Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker6Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker6StartIndex
        {
            get => _toolChecker6StartIndex;
            set
            {
                _toolChecker6StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker6StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker6Com
        {
            get => _toolChecker6Com;
            set
            {
                _toolChecker6Com = value;
                IniHelper.Write("AppConfig", "ToolChecker6Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker6NodeId
        {
            get => _toolChecker6NodeId;
            set
            {
                _toolChecker6NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker6NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolChecker7Ip
        {
            get => _toolChecker7Ip;
            set
            {
                _toolChecker7Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker7Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker7Port
        {
            get => _toolChecker7Port;
            set
            {
                _toolChecker7Port = value;
                IniHelper.Write("AppConfig", "ToolChecker7Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker7Capacity
        {
            get => _toolChecker7Capacity;
            set
            {
                _toolChecker7Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker7Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker7StartIndex
        {
            get => _toolChecker7StartIndex;
            set
            {
                _toolChecker7StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker7StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker7Com
        {
            get => _toolChecker7Com;
            set
            {
                _toolChecker7Com = value;
                IniHelper.Write("AppConfig", "ToolChecker7Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker7NodeId
        {
            get => _toolChecker7NodeId;
            set
            {
                _toolChecker7NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker7NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolChecker8Ip
        {
            get => _toolChecker8Ip;
            set
            {
                _toolChecker8Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker8Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker8Port
        {
            get => _toolChecker8Port;
            set
            {
                _toolChecker8Port = value;
                IniHelper.Write("AppConfig", "ToolChecker8Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker8Capacity
        {
            get => _toolChecker8Capacity;
            set
            {
                _toolChecker8Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker8Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker8StartIndex
        {
            get => _toolChecker8StartIndex;
            set
            {
                _toolChecker8StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker8StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker8Com
        {
            get => _toolChecker8Com;
            set
            {
                _toolChecker8Com = value;
                IniHelper.Write("AppConfig", "ToolChecker8Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker8NodeId
        {
            get => _toolChecker8NodeId;
            set
            {
                _toolChecker8NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker8NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolChecker9Ip
        {
            get => _toolChecker9Ip;
            set
            {
                _toolChecker9Ip = value;
                IniHelper.Write("AppConfig", "ToolChecker9Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker9Port
        {
            get => _toolChecker9Port;
            set
            {
                _toolChecker9Port = value;
                IniHelper.Write("AppConfig", "ToolChecker9Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker9Capacity
        {
            get => _toolChecker9Capacity;
            set
            {
                _toolChecker9Capacity = value;
                IniHelper.Write("AppConfig", "ToolChecker9Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker9StartIndex
        {
            get => _toolChecker9StartIndex;
            set
            {
                _toolChecker9StartIndex = value;
                IniHelper.Write("AppConfig", "ToolChecker9StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker9Com
        {
            get => _toolChecker9Com;
            set
            {
                _toolChecker9Com = value;
                IniHelper.Write("AppConfig", "ToolChecker9Com", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolChecker9NodeId
        {
            get => _toolChecker9NodeId;
            set
            {
                _toolChecker9NodeId = value;
                IniHelper.Write("AppConfig", "ToolChecker9NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块1 IP
        /// </summary>
        public static string ToolLedOperatorIp
        {
            get => _toolLedOperatorIp;
            set
            {
                _toolLedOperatorIp = value;
                IniHelper.Write("AppConfig", "ToolLedOperatorIp", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块1 端口
        /// </summary>
        public static int ToolLedOperatorPort
        {
            get => _toolLedOperatorPort;
            set
            {
                _toolLedOperatorPort = value;
                IniHelper.Write("AppConfig", "ToolLedOperatorPort", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块1 点数
        /// </summary>
        public static int ToolLedOperatorCapacity
        {
            get => _toolLedOperatorCapacity;
            set
            {
                _toolLedOperatorCapacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperatorCapacity", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块1 起始点
        /// </summary>
        public static int ToolLedOperatorStartIndex
        {
            get => _toolLedOperatorStartIndex;
            set
            {
                _toolLedOperatorStartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperatorStartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperatorNodeId
        {
            get => _toolLedOperatorNodeId;
            set
            {
                _toolLedOperatorNodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperatorNodeId", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块2 IP
        /// </summary>
        public static string ToolLedOperator2Ip
        {
            get => _toolLedOperator2Ip;
            set
            {
                _toolLedOperator2Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator2Ip", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块2 端口
        /// </summary>
        public static int ToolLedOperator2Port
        {
            get => _toolLedOperator2Port;
            set
            {
                _toolLedOperator2Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator2Port", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块2 点数
        /// </summary>
        public static int ToolLedOperator2Capacity
        {
            get => _toolLedOperator2Capacity;
            set
            {
                _toolLedOperator2Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator2Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// LED控制MODBUS模块2 起始点
        /// </summary>
        public static int ToolLedOperator2StartIndex
        {
            get => _toolLedOperator2StartIndex;
            set
            {
                _toolLedOperator2StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator2StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator2NodeId
        {
            get => _toolLedOperator2NodeId;
            set
            {
                _toolLedOperator2NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator2NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolLedOperator3Ip
        {
            get => _toolLedOperator3Ip;
            set
            {
                _toolLedOperator3Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator3Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator3Port
        {
            get => _toolLedOperator3Port;
            set
            {
                _toolLedOperator3Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator3Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator3Capacity
        {
            get => _toolLedOperator3Capacity;
            set
            {
                _toolLedOperator3Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator3Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator3StartIndex
        {
            get => _toolLedOperator3StartIndex;
            set
            {
                _toolLedOperator3StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator3StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator3NodeId
        {
            get => _toolLedOperator3NodeId;
            set
            {
                _toolLedOperator3NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator3NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolLedOperator4Ip
        {
            get => _toolLedOperator4Ip;
            set
            {
                _toolLedOperator4Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator4Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator4Port
        {
            get => _toolLedOperator4Port;
            set
            {
                _toolLedOperator4Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator4Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator4Capacity
        {
            get => _toolLedOperator4Capacity;
            set
            {
                _toolLedOperator4Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator4Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator4StartIndex
        {
            get => _toolLedOperator4StartIndex;
            set
            {
                _toolLedOperator4StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator4StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator4NodeId
        {
            get => _toolLedOperator4NodeId;
            set
            {
                _toolLedOperator4NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator4NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolLedOperator5Ip
        {
            get => _toolLedOperator5Ip;
            set
            {
                _toolLedOperator5Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator5Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator5Port
        {
            get => _toolLedOperator5Port;
            set
            {
                _toolLedOperator5Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator5Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator5Capacity
        {
            get => _toolLedOperator5Capacity;
            set
            {
                _toolLedOperator5Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator5Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator5StartIndex
        {
            get => _toolLedOperator5StartIndex;
            set
            {
                _toolLedOperator5StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator5StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator5NodeId
        {
            get => _toolLedOperator5NodeId;
            set
            {
                _toolLedOperator5NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator5NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolLedOperator6Ip
        {
            get => _toolLedOperator6Ip;
            set
            {
                _toolLedOperator6Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator6Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator6Port
        {
            get => _toolLedOperator6Port;
            set
            {
                _toolLedOperator6Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator6Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator6Capacity
        {
            get => _toolLedOperator6Capacity;
            set
            {
                _toolLedOperator6Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator6Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator6StartIndex
        {
            get => _toolLedOperator6StartIndex;
            set
            {
                _toolLedOperator6StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator6StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator6NodeId
        {
            get => _toolLedOperator6NodeId;
            set
            {
                _toolLedOperator6NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator6NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolLedOperator7Ip
        {
            get => _toolLedOperator7Ip;
            set
            {
                _toolLedOperator7Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator7Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator7Port
        {
            get => _toolLedOperator7Port;
            set
            {
                _toolLedOperator7Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator7Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator7Capacity
        {
            get => _toolLedOperator7Capacity;
            set
            {
                _toolLedOperator7Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator7Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator7StartIndex
        {
            get => _toolLedOperator7StartIndex;
            set
            {
                _toolLedOperator7StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator7StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator7NodeId
        {
            get => _toolLedOperator7NodeId;
            set
            {
                _toolLedOperator7NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator7NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolLedOperator8Ip
        {
            get => _toolLedOperator8Ip;
            set
            {
                _toolLedOperator8Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator8Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator8Port
        {
            get => _toolLedOperator8Port;
            set
            {
                _toolLedOperator8Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator8Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator8Capacity
        {
            get => _toolLedOperator8Capacity;
            set
            {
                _toolLedOperator8Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator8Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator8StartIndex
        {
            get => _toolLedOperator8StartIndex;
            set
            {
                _toolLedOperator8StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator8StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator8NodeId
        {
            get => _toolLedOperator8NodeId;
            set
            {
                _toolLedOperator8NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator8NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        public static string ToolLedOperator9Ip
        {
            get => _toolLedOperator9Ip;
            set
            {
                _toolLedOperator9Ip = value;
                IniHelper.Write("AppConfig", "ToolLedOperator9Ip", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator9Port
        {
            get => _toolLedOperator9Port;
            set
            {
                _toolLedOperator9Port = value;
                IniHelper.Write("AppConfig", "ToolLedOperator9Port", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator9Capacity
        {
            get => _toolLedOperator9Capacity;
            set
            {
                _toolLedOperator9Capacity = value;
                IniHelper.Write("AppConfig", "ToolLedOperator9Capacity", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator9StartIndex
        {
            get => _toolLedOperator9StartIndex;
            set
            {
                _toolLedOperator9StartIndex = value;
                IniHelper.Write("AppConfig", "ToolLedOperator9StartIndex", value.ToString(), Env.ConfigPath);
            }
        }

        public static int ToolLedOperator9NodeId
        {
            get => _toolLedOperator9NodeId;
            set
            {
                _toolLedOperator9NodeId = value;
                IniHelper.Write("AppConfig", "ToolLedOperator9NodeId", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉数量
        /// </summary>
        public static int DrawerCount
        {
            get => _drawerCount;
            set
            {
                _drawerCount = value;
                IniHelper.Write("AppConfig", "DrawerCount", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 副屏每行抽屉个数
        /// </summary>
        public static int DrawerCol
        {
            get => _drawerCol;
            set
            {
                _drawerCol = value;
                IniHelper.Write("AppConfig", "DrawerCol", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 报修窗口每行抽屉个数
        /// </summary>
        public static int ReportDrawerCol
        {
            get => _reportDrawerCol;
            set
            {
                _reportDrawerCol = value;
                IniHelper.Write("AppConfig", "ReportDrawerCol", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉不上锁
        /// </summary>
        public static int DrawerUnlock
        {
            get => _drawerUnlock;
            set
            {
                _drawerUnlock = value;
                IniHelper.Write("AppConfig", "DrawerUnlock", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉解锁时间
        /// </summary>
        public static int DrawerUnlockTime
        {
            get => _drawerUnlockTime;
            set
            {
                _drawerUnlockTime = value;
                IniHelper.Write("AppConfig", "DrawerUnlockTime", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉1 COM端口
        /// </summary>
        public static int DrawerPort1
        {
            get => _drawerPort1;
            set
            {
                _drawerPort1 = value;
                IniHelper.Write("AppConfig", "DrawerPort1", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉2 COM端口
        /// </summary>
        public static int DrawerPort2
        {
            get => _drawerPort2;
            set
            {
                _drawerPort2 = value;
                IniHelper.Write("AppConfig", "DrawerPort2", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉3 COM端口
        /// </summary>
        public static int DrawerPort3
        {
            get => _drawerPort3;
            set
            {
                _drawerPort3 = value;
                IniHelper.Write("AppConfig", "DrawerPort3", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉4 COM端口
        /// </summary>
        public static int DrawerPort4
        {
            get => _drawerPort4;
            set
            {
                _drawerPort4 = value;
                IniHelper.Write("AppConfig", "DrawerPort4", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉5 COM端口
        /// </summary>
        public static int DrawerPort5
        {
            get => _drawerPort5;
            set
            {
                _drawerPort5 = value;
                IniHelper.Write("AppConfig", "DrawerPort5", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 抽屉6 COM端口
        /// </summary>
        public static int DrawerPort6
        {
            get => _drawerPort6;
            set
            {
                _drawerPort6 = value;
                IniHelper.Write("AppConfig", "DrawerPort6", value.ToString(), Env.ConfigPath);
            }
        }

        public static int LightCtrlPoint
        {
            get => _lightCtrlPoint;
            set
            {
                _lightCtrlPoint = value;
                IniHelper.Write("AppConfig", "LightCtrlPoint", value.ToString(), Env.ConfigPath);
            }
        }

        public static int LedYellowCtrlPoint
        {
            get => _ledYellowCtrlPoint;
            set
            {
                _ledYellowCtrlPoint = value;
                IniHelper.Write("AppConfig", "LedYellowCtrlPoint", value.ToString(), Env.ConfigPath);
            }
        }

        public static int LedRedCtrlPoint
        {
            get => _ledRedCtrlPoint;
            set
            {
                _ledRedCtrlPoint = value;
                IniHelper.Write("AppConfig", "LedRedCtrlPoint", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenCheckerMapping1
        {
            get => _brokenCheckerMapping1;
            set
            {
                _brokenCheckerMapping1 = value;
                IniHelper.Write("AppConfig", "BrokenCheckerMapping1", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenCheckerMapping2
        {
            get => _brokenCheckerMapping2;
            set
            {
                _brokenCheckerMapping2 = value;
                IniHelper.Write("AppConfig", "BrokenCheckerMapping2", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenCheckerMapping3
        {
            get => _brokenCheckerMapping3;
            set
            {
                _brokenCheckerMapping3 = value;
                IniHelper.Write("AppConfig", "BrokenCheckerMapping3", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenCheckerMapping4
        {
            get => _brokenCheckerMapping4;
            set
            {
                _brokenCheckerMapping4 = value;
                IniHelper.Write("AppConfig", "BrokenCheckerMapping4", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenCheckerMapping5
        {
            get => _brokenCheckerMapping5;
            set
            {
                _brokenCheckerMapping5 = value;
                IniHelper.Write("AppConfig", "BrokenCheckerMapping5", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenCheckerMapping6
        {
            get => _brokenCheckerMapping6;
            set
            {
                _brokenCheckerMapping6 = value;
                IniHelper.Write("AppConfig", "BrokenCheckerMapping6", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenOperatorMapping1
        {
            get => _brokenOperatorMapping1;
            set
            {
                _brokenOperatorMapping1 = value;
                IniHelper.Write("AppConfig", "BrokenOperatorMapping1", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenOperatorMapping2
        {
            get => _brokenOperatorMapping2;
            set
            {
                _brokenOperatorMapping2 = value;
                IniHelper.Write("AppConfig", "BrokenOperatorMapping2", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenOperatorMapping3
        {
            get => _brokenOperatorMapping3;
            set
            {
                _brokenOperatorMapping3 = value;
                IniHelper.Write("AppConfig", "BrokenOperatorMapping3", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenOperatorMapping4
        {
            get => _brokenOperatorMapping4;
            set
            {
                _brokenOperatorMapping4 = value;
                IniHelper.Write("AppConfig", "BrokenOperatorMapping4", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenOperatorMapping5
        {
            get => _brokenOperatorMapping5;
            set
            {
                _brokenOperatorMapping5 = value;
                IniHelper.Write("AppConfig", "BrokenOperatorMapping5", value.ToString(), Env.ConfigPath);
            }
        }

        public static string BrokenOperatorMapping6
        {
            get => _brokenOperatorMapping6;
            set
            {
                _brokenOperatorMapping6 = value;
                IniHelper.Write("AppConfig", "BrokenOperatorMapping6", value.ToString(), Env.ConfigPath);
            }
        }

        public static string LightBright
        {
            get => _lightBright;
            set
            {
                _lightBright = value;
                IniHelper.Write("AppConfig", "LightBright", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 点位错位，调整
        /// </summary>
        public static string SpotMapping
        {
            get => _spotMapping;
            set
            {
                _spotMapping = value;
                IniHelper.Write("AppConfig", "SpotMapping", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 是否自动更新指纹
        /// </summary>
        public static int AutoUpdateFp
        {
            get => _autoUpdateFp;
            set
            {
                _autoUpdateFp = value;
                IniHelper.Write("AppConfig", "AutoUpdateFp", value.ToString(), Env.ConfigPath);
            }
        }

        /// <summary>
        /// 换行位置
        /// </summary>
        public static string AddRowPoint
        {
            get => _addRowPoint;
            set
            {
                _addRowPoint = value;
                IniHelper.Write("AppConfig", "AddRowPoint", value.ToString(), Env.ConfigPath);
            }
        }

        private static string _appName = "智能柜管理R2";
        private static string _appID = "1083CD20-589A-4A2B-A855-E67443217202";
        private static string _userID = "697D02F5-A5BB-4AF3-9BF2-F4F63531907A";
        private static string _appUnit = "新联铁";

        private static string _updateServiceUrl = "http://193.168.0.249:8080/UpdateService/UpdateService.asmx";
        private static string _toolServiceUrl = "http://193.168.0.249:8080/ToolService/ToolService.asmx";

        private static int _disableUpdate;
        private static int _disableWatcher;

        private static int _debugMode;
        private static int _localMode;
        private static int _recvLog;

        private static int _appType = 0;
        private static int _deviceType = 0;

        private static string _localDb = "vHQUCoDfttB+VYYXBlOVZtbhoiVGZ86xX1YRJ2bED4hh3KDy/+KN5oWNSaKvAs1Kv1fcBvqnqyF6nK3Amz2EvCh3UHV50dRQ+z13FgLnd4o=";

        private static int _magtaPort = 7;
        private static int _iRPort = 8;
        private static int _iRPort2 = 4;
        private static int _iRUsing = 0;

        private static int _measuringPort = 6;
        private static float _measuringWjMax = 2;
        private static float _measuringWjMin = 1;

        private static string _ifacetype = "";
        private static string _ifaceIp = "192.168.1.201";
        private static int _ifacePort = 4370;
        private static int _ifaceAsyncCheck;

        private static string _offDutyTime = "24:20";

        private static string _cabinetName = "";
        private static string _cabinetPosition = "";

        private static string _allowedGroup = "";
        private static string _drawer1Group = "";
        private static string _drawer2Group = "";
        private static string _drawer3Group = "";
        private static string _drawer4Group = "";

        private static int _toolCodeX = 20, _toolCodeY = 8;
        private static int _toolStatusX = 16, _toolStatusY = 68;

        private static int _reportToolCodeX = 14, _reportToolCodeY = 2;
        private static int _reportToolStatusX = 7, _reportToolStatusY = 54;

        private static int _toolCount = 16;
        private static int _reserveToolCount = 0;
        private static int _toolCol = 16;
        private static int _reportToolCol = 16;

        private static string _syncServiceUrl = "http://193.168.0.249:8080/CabinetService/SyncService.asmx";

        private static string _measureServiceUrl = "";

        private static string _checksumWebUrl = "";

        private static string _canBusIp = "";
        private static int _canBusPort = 502;

        private static string _cabinetCheckerIp = "";
        private static int _cabinetCheckerPort = 502;
        private static string _cabinetOperatorIp = "";
        private static int _cabinetOperatorPort = 502;

        private static string _toolCheckerIp = "";
        private static int _toolCheckerPort = 502;
        private static int _toolCheckerCapacity = 16;
        private static int _toolCheckerStartIndex;
        private static int _toolCheckerCom = -1;
        private static int _toolCheckerNodeId = -1;

        private static string _toolChecker2Ip = "";
        private static int _toolChecker2Port = 502;
        private static int _toolChecker2Capacity = 24;
        private static int _toolChecker2StartIndex;
        private static int _toolChecker2Com = -1;
        private static int _toolChecker2NodeId = -1;

        private static string _toolChecker3Ip = "";
        private static int _toolChecker3Port = 502;
        private static int _toolChecker3Capacity = 24;
        private static int _toolChecker3StartIndex;
        private static int _toolChecker3Com = -1;
        private static int _toolChecker3NodeId = -1;

        private static string _toolChecker4Ip = "";
        private static int _toolChecker4Port = 502;
        private static int _toolChecker4Capacity = 16;
        private static int _toolChecker4StartIndex;
        private static int _toolChecker4Com = -1;
        private static int _toolChecker4NodeId = -1;

        private static string _toolChecker5Ip = "";
        private static int _toolChecker5Port = 502;
        private static int _toolChecker5Capacity = 16;
        private static int _toolChecker5StartIndex;
        private static int _toolChecker5Com = -1;
        private static int _toolChecker5NodeId = -1;

        private static string _toolChecker6Ip = "";
        private static int _toolChecker6Port = 502;
        private static int _toolChecker6Capacity = 16;
        private static int _toolChecker6StartIndex;
        private static int _toolChecker6Com = -1;
        private static int _toolChecker6NodeId = -1;

        private static string _toolChecker7Ip = "";
        private static int _toolChecker7Port = 502;
        private static int _toolChecker7Capacity = 16;
        private static int _toolChecker7StartIndex;
        private static int _toolChecker7Com = -1;
        private static int _toolChecker7NodeId = -1;

        private static string _toolChecker8Ip = "";
        private static int _toolChecker8Port = 502;
        private static int _toolChecker8Capacity = 16;
        private static int _toolChecker8StartIndex;
        private static int _toolChecker8Com = -1;
        private static int _toolChecker8NodeId = -1;

        private static string _toolChecker9Ip = "";
        private static int _toolChecker9Port = 502;
        private static int _toolChecker9Capacity = 16;
        private static int _toolChecker9StartIndex;
        private static int _toolChecker9Com = -1;
        private static int _toolChecker9NodeId = -1;

        private static string _toolLedOperatorIp = "";
        private static int _toolLedOperatorPort = 502;
        private static int _toolLedOperatorCapacity = 16;
        private static int _toolLedOperatorStartIndex;
        private static int _toolLedOperatorNodeId = -1;

        private static string _toolLedOperator2Ip = "";
        private static int _toolLedOperator2Port = 502;
        private static int _toolLedOperator2Capacity = 16;
        private static int _toolLedOperator2StartIndex;
        private static int _toolLedOperator2NodeId = -1;

        private static string _toolLedOperator3Ip = "";
        private static int _toolLedOperator3Port = 502;
        private static int _toolLedOperator3Capacity = 16;
        private static int _toolLedOperator3StartIndex;
        private static int _toolLedOperator3NodeId = -1;

        private static string _toolLedOperator4Ip = "";
        private static int _toolLedOperator4Port = 502;
        private static int _toolLedOperator4Capacity = 16;
        private static int _toolLedOperator4StartIndex;
        private static int _toolLedOperator4NodeId = -1;

        private static string _toolLedOperator5Ip = "";
        private static int _toolLedOperator5Port = 502;
        private static int _toolLedOperator5Capacity = 16;
        private static int _toolLedOperator5StartIndex;
        private static int _toolLedOperator5NodeId = -1;

        private static string _toolLedOperator6Ip = "";
        private static int _toolLedOperator6Port = 502;
        private static int _toolLedOperator6Capacity = 16;
        private static int _toolLedOperator6StartIndex;
        private static int _toolLedOperator6NodeId = -1;

        private static string _toolLedOperator7Ip = "";
        private static int _toolLedOperator7Port = 502;
        private static int _toolLedOperator7Capacity = 16;
        private static int _toolLedOperator7StartIndex;
        private static int _toolLedOperator7NodeId = -1;

        private static string _toolLedOperator8Ip = "";
        private static int _toolLedOperator8Port = 502;
        private static int _toolLedOperator8Capacity = 16;
        private static int _toolLedOperator8StartIndex;
        private static int _toolLedOperator8NodeId = -1;

        private static string _toolLedOperator9Ip = "";
        private static int _toolLedOperator9Port = 502;
        private static int _toolLedOperator9Capacity = 16;
        private static int _toolLedOperator9StartIndex;
        private static int _toolLedOperator9NodeId = -1;

        private static int _drawerCount = 4;
        private static int _drawerCol = 2;
        private static int _reportDrawerCol = 4;
        private static int _drawerUnlock = 0;
        private static int _drawerUnlockTime = 30;

        private static int _drawerPort1 = -1;
        private static int _drawerPort2 = -1;
        private static int _drawerPort3 = -1;
        private static int _drawerPort4 = -1;
        private static int _drawerPort5 = -1;
        private static int _drawerPort6 = -1;

        private static int _lightCtrlPoint = -1;
        private static int _ledYellowCtrlPoint = -1;
        private static int _ledRedCtrlPoint = -1;

        private static string _brokenCheckerMapping1 = "";
        private static string _brokenCheckerMapping2 = "";
        private static string _brokenCheckerMapping3 = "";
        private static string _brokenCheckerMapping4 = "";
        private static string _brokenCheckerMapping5 = "";
        private static string _brokenCheckerMapping6 = "";

        private static string _brokenOperatorMapping1 = "";
        private static string _brokenOperatorMapping2 = "";
        private static string _brokenOperatorMapping3 = "";
        private static string _brokenOperatorMapping4 = "";
        private static string _brokenOperatorMapping5 = "";
        private static string _brokenOperatorMapping6 = "";

        private static string _lightBright = "";
        private static string _spotMapping = "";
        private static int _autoUpdateFp = 1;
        private static string _addRowPoint = "";
    }
}
