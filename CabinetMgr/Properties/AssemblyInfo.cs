using System.Reflection;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("CabinetMgr")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("CabinetMgr")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("bfe44583-6ef0-4a12-9dce-3f8be4110455")]

// 程序集的版本信息由下面四个值组成: 
//
//      主版本
//      次版本 
//      生成号
//      修订号
//
// 可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值，
// 方法是按如下所示使用“*”: 
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.17")]
[assembly: AssemblyFileVersion("1.0.0.17")]


/*  程序配置项说明，对应CabinetMgr.Config.AppConfig中对应字段，可在Config.ini文件中进行自定义
    程序部分配置项目使用反射方式加载，请勿按照VS灰色提示删除配置项

/// 程序名称，用于软件更新
AppName

/// 程序单位，用于软件更新
AppUnit

/// 程序类型，0=转向架，1=电机，2=轮驱
AppType

/// 设备类型，1=电机，2=转向架V1，3=轮驱V1
DeviceType

/// 自动更新WebService地址
UpdateServiceUrl

/// 用于屏蔽更新功能
DisableUpdate

/// 用于屏蔽远程协助功能
DisableWatcher

/// 调试模式开关
DebugMode

/// 本地演示模式开关
LocalMode

/// 本地数据库连接字符串
LocalDb

/// Magta扳手接收器COM端口号
MagtaPort

/// 千分尺接收器COM端口号
MeasuringPort

/// 外径千分尺数值范围MAX
MeasuringWjMax

/// 外径千分尺数值范围MIN
MeasuringWjMin

/// 中控指纹人脸识别设备IP
IfaceIp

/// 中控指纹人脸识别设备端口号
IfacePort

/// 中控指纹人脸识别设备异步加载开关，开启后可避免指纹机无响应拖慢系统启动的情况
/// (在低版本系统(Win7及以下)兼容问题，建议关闭)
IfaceAsyncCheck

/// 下班时间(下班后未归还设备将显示异常)
OffDutyTime

/// 工具柜ID(用于数据同步等，请保证各个工具柜此项不同)
CabinetName

/// 工具柜位置(保修时将上报此位置)
CabinetPosition

/// 用于柜门班组控制(可开启柜门的班组名称)
AllowedGroup

/// 用于柜门班组控制(可开启柜门的班组名称)
Drawer1Group

/// 用于柜门班组控制(可开启柜门的班组名称)
Drawer2Group

/// 用于柜门班组控制(可开启柜门的班组名称)
Drawer3Group

/// 用于柜门班组控制(可开启柜门的班组名称)
Drawer4Group

/// 副屏工具编号文字X坐标
ToolCodeX

/// 副屏工具编号文字Y坐标
ToolCodeY

/// 副屏工具状态文字X坐标
ToolStatusX

/// 副屏工具状态文字Y坐标
ToolStatusY

/// 报修界面工具编号文字X坐标
ReportToolCodeX

/// 报修界面工具编号文字Y坐标
ReportToolCodeY

/// 报修界面工具状态文字X坐标
ReportToolStatusX

/// 报修界面工具状态文字Y坐标
ReportToolStatusY

/// 工具数量
ToolCount

/// 保留工具数量(暂停使用，请保证数值与工具数量一致)
ReserveToolCount

/// 副屏每行显示工具个数
ToolCol

/// 报修界面每行显示工具个数
ReportToolCol

/// 同步功能WebService地址
SyncServiceUrl

/// 量值功能WebService地址
MeasureServiceUrl

/// 校验功能Url地址
ChecksumWebUrl

/// 柜体检测MODBUS模块IP (柜门状态等)
CabinetCheckerIp

/// 柜体检测MODBUS模块端口
CabinetCheckerPort

/// 柜体操作MODBUS模块IP (操作柜门开关、柜体LED照明、报警灯等)
CabinetOperatorIp

/// 柜体操作MODBUS模块端口
CabinetOperatorPort

/// 接近开关检测模块1 IP
ToolCheckerIp

/// 接近开关检测模块1 端口
ToolCheckerPort

/// 接近开关检测模块1 点数
ToolCheckerCapacity

/// 接近开关检测模块1 起始点
ToolCheckerStartIndex

/// 接近开关检测模块1 COM端口号
ToolCheckerCom

//ToolChecker2-9系列依次类推

/// LED控制MODBUS模块1 IP
ToolLedOperatorIp

/// LED控制MODBUS模块1 端口
ToolLedOperatorPort

/// LED控制MODBUS模块1 点数
ToolLedOperatorCapacity

/// LED控制MODBUS模块1 起始点
ToolLedOperatorStartIndex

//ToolLedOperator2-9系列依次类推

/// 抽屉数量
DrawerCount

/// 副屏每行抽屉个数
DrawerCol

/// 报修窗口每行抽屉个数
ReportDrawerCol

/// 抽屉不上锁
DrawerUnlock

/// 抽屉1 COM端口
DrawerPort1

/// 抽屉2 COM端口
DrawerPort2

/// 抽屉3 COM端口
DrawerPort3

/// 抽屉4 COM端口
DrawerPort4

/// 抽屉5 COM端口
DrawerPort5

/// 抽屉6 COM端口
DrawerPort6

// LED照明灯起始点位Modbus序号
LightCtrlPoint
// 黄色报警灯LED点位Modbus序号
LedYellowCtrlPoint
// 红色报警灯LED点位Modbus序号
LedRedCtrlPoint

// 损坏点位映射 如填写 1,105 即表示用105号点位代替1号点位功能
BrokenCheckerMapping1
BrokenCheckerMapping2
BrokenCheckerMapping3
BrokenCheckerMapping4
BrokenCheckerMapping5
BrokenCheckerMapping6

// 损坏点位映射 如填写 1,105 即表示用105号点位代替1号点位功能
BrokenOperatorMapping1
BrokenOperatorMapping2
BrokenOperatorMapping3
BrokenOperatorMapping4
BrokenOperatorMapping5
BrokenOperatorMapping6

*/
