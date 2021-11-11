using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AxRealSvrOcxTcpLib;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using NLog;
using Utilities.DbHelper;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormDeviceLoader : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private CellStyle _ctRed, _ctGreen;
        private bool _isPassed = true;

        public FormDeviceLoader()
        {
            InitializeComponent();
            CabinetCallback.OnInitDone = OnCabinetInitDone;
        }

        private void FormDeviceLoader_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            ConfigLoader.LoadConfig();
            InitGrid();
        }

        private void FormDeviceLoader_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AppConfig.CabinetName))
            {
                MessageBox.Show("请指定CabinetName");
                Application.Exit();
                return;
            }
            if (AppConfig.DeviceType == 0)
            {
                MessageBox.Show("请指定设备类型(DeviceType)");
                Application.Exit();
                return;
            }

            if (AppConfig.DisableUpdate == 0)
            {
                Thread updChkThread = new Thread(UpdateChk);
                updChkThread.IsBackground = true;
                updChkThread.Start(false);
            }

            //指纹仪/面部识别相关处理(IFace)
            cStatusGrid.AddItem(new object[] { "", "初始化指纹仪", "正在执行" });
            InitIFaceDevice();

            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            if (height == 1920 && width == 1080)
            {
            }

            ////Magta扳手设备初始化
            //if (AppConfig.MagtaPort >= 0 && height != 1920 && width != 1080)
            //{
            //    cStatusGrid.AddItem(new object[] { "", "初始化MAGTA扳手接收模块", "正在执行" });
            //    if (TorqueDevice.InitMagtaDevice(AppConfig.MagtaPort) <= 0)
            //    {
            //        _isPassed = false;
            //        UpdateStatus("初始化MAGTA扳手接收模块", "力矩设备连接失败", 2);
            //    }
            //    else
            //    {
            //        UpdateStatus("初始化MAGTA扳手接收模块", "成功", 1);
            //    }
            //}

            ////IR扳手设备初始化
            //if (AppConfig.IRUsing == 1 && AppConfig.IRPort >= 0 && height != 1920 && width != 1080)
            //{
            //    cStatusGrid.AddItem(new object[] { "", "初始化IR扳手1接收模块", "正在执行" });
            //    if (TorqueDevice.InitIRDevice(AppConfig.IRPort) <= 0)
            //    {
            //        _isPassed = false;
            //        UpdateStatus("初始化IR扳手1接收模块", "力矩设备连接失败", 2);
            //    }
            //    else
            //    {
            //        UpdateStatus("初始化IR扳手1接收模块", "成功", 1);
            //    }
            //}

            //if (AppConfig.IRUsing == 1 && AppConfig.IRPort2 >= 0)
            //{
            //    cStatusGrid.AddItem(new object[] { "", "初始化IR扳手2接收模块", "正在执行" });
            //    if (TorqueDevice.InitIRDevice(AppConfig.IRPort2) <= 0)
            //    {
            //        _isPassed = false;
            //        UpdateStatus("初始化IR扳手2接收模块", "力矩设备连接失败", 2);
            //    }
            //    else
            //    {
            //        UpdateStatus("初始化IR扳手2接收模块", "成功", 1);
            //    }
            //}

            ////千分尺设备初始化
            //if (AppConfig.MeasuringPort >= 0 && height != 1920 && width != 1080)
            //{
            //    cStatusGrid.AddItem(new object[] { "", "初始化千分尺接收模块", "正在执行" });
            //    if (MeasuringDevice.Init(AppConfig.MeasuringPort) <= 0)
            //    {
            //        _isPassed = false;
            //        UpdateStatus("初始化千分尺接收模块", "千分尺设备连接失败", 2);
            //    }
            //    else
            //    {
            //        UpdateStatus("初始化千分尺接收模块", "成功", 1);
            //    }
            //}

            //检查屏幕数量
            //cStatusGrid.AddItem(new object[] { "", "检查屏幕状态", "正在执行" });
            for (int i = 0; i < 4; i++)
            {
                if (Screen.AllScreens.Length > 1)
                {
                    break;
                }
                Thread.Sleep(1500);
            }
            if (Screen.AllScreens.Length <= 1)
            {
                //_isPassed = false;
                //UpdateStatus("检查屏幕状态", "未连接副屏", 2);
            }
            else
            {
                //UpdateStatus("检查屏幕状态", "已连接屏幕数量: " + Screen.AllScreens.Length, 1);
            }

            //初始化数据同步
            if (AppConfig.LocalMode == 0)
            {
                cStatusGrid.AddItem(new object[] { "", "初始化数据同步", "正在执行" });
                //启动数据同步
                bool isFailed = false;
                int updResult = BllSyncManager.RegisterSyncItem(BllSyncManager.SyncType.Upload, typeof(BorrowRecord),
                    typeof(ReturnRecord));
                if (updResult <= 0)
                {
                    isFailed = true;
                    _isPassed = false;
                    UpdateStatus("初始化数据同步", "初始化数据上传失败", 2);
                }
                updResult = BllSyncManager.RegisterSyncItem(BllSyncManager.SyncType.Download, typeof(UserInfo));
                if (updResult <= 0)
                {
                    isFailed = true;
                    _isPassed = false;
                    UpdateStatus("初始化数据同步", "初始化数据下载失败", 2);
                }
                BllSyncManager.StartSync();
                if (!isFailed) UpdateStatus("初始化数据同步", "成功", 1);
            }

            //智能柜设备初始化
            cStatusGrid.AddItem(new object[] { "", "初始化工具柜控制模块", "正在执行" });
            switch (AppConfig.DeviceType)
            {
                case 1:
                    DeviceLayer.CabinetDevice = new CabinetDeviceRx1();
                    break;
                case 2:
                    DeviceLayer.CabinetDevice = new CabinetDeviceRx2();
                    break;
                case 3:
                    DeviceLayer.CabinetDevice = new CabinetDeviceRx3();
                    break;
                case 4:
                    DeviceLayer.CabinetDevice = new CabinetDeviceRx4();
                    break;
                case 5:
                    DeviceLayer.CabinetDevice = new CabinetDeviceRx5();
                    break;
                case 6:
                    DeviceLayer.CabinetDevice = new CabinetDeviceRx6();
                    break;
                case 7:
                    DeviceLayer.CabinetDevice = new CabinetDeviceRx7();
                    break;
            }
            if (AppConfig.AppType == 0)
            {
                Env.DataType = "1";
            }
            else if (AppConfig.AppType == 1)
            {
                Env.DataType = "0";
            }
            else
            {
                Env.DataType = AppConfig.AppType.ToString();
            }
            InitCabinet();
        }

        private void InitGrid()
        {
            cStatusGrid.Cols.Count = 3;
            cStatusGrid.Rows.Count = 1;
            cStatusGrid.Cols[1].Caption = "项目";
            cStatusGrid.Cols[2].Caption = "状态";
            cStatusGrid.Cols[1].Width = (cStatusGrid.Width - 50) / 2;
            cStatusGrid.Cols[2].Width = (cStatusGrid.Width - 50) / 2;

            _ctRed = cStatusGrid.Styles.Add("CtRed");
            _ctRed.BackColor = Color.HotPink;
            _ctGreen = cStatusGrid.Styles.Add("CtGreen");
            _ctGreen.BackColor = Color.LightGreen;
        }

        private void UpdateChk(object needWait)
        {
            try
            {

                if ((bool)needWait) Thread.Sleep(6000);
                
                string serverVer = BllDataMgr.GetServerAppVer();
                if (!string.IsNullOrEmpty(serverVer))
                {
                    if (!Application.ProductVersion.Equals(serverVer))
                    {
                        //if (SysHelper.MessageBoxFG_OkCancel(@"检查到新版本，是否进行更新？", @"提示") == DialogResult.OK)
                        {
                            if (File.Exists("AppUpdater.exe"))
                            {
                                Process.Start("AppUpdater.exe");
                                Environment.Exit(0);
                            }
                            else if (File.Exists("CabinetMgr.Updater.exe"))
                            {
                                Process.Start("CabinetMgr.Updater.exe");
                                Environment.Exit(0);
                            }
                            else
                            {
                                MessageBox.Show("未找到更新程序，请重新安装");
                                Environment.Exit(0);
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private delegate void UpdateStatusDelegate(string itemName, string result, int color = 0);
        public void UpdateStatus(string itemName, string result, int color = 0)
        {
            try
            {
                if (cStatusGrid.InvokeRequired)
                {
                    UpdateStatusDelegate d = UpdateStatus;
                    cStatusGrid.Invoke(d, itemName, result, color);
                }
                else
                {
                    for (int i = 1; i < cStatusGrid.Rows.Count; i++)
                    {
                        if (cStatusGrid.Rows[i][1].ToString() == itemName)
                        {
                            cStatusGrid.Rows[i][2] = result;
                            if (color == 1) cStatusGrid.SetCellStyle(i, 2, _ctGreen);
                            else if (color == 2) cStatusGrid.SetCellStyle(i, 2, _ctRed);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void InitIFaceDevice()
        {
            if (AppConfig.IfaceAsyncCheck == 1)
            {
                FpZkIFace.Prepare();
                Thread initIfaceThread = new Thread(InitIFaceDeviceThd) { IsBackground = true };
                initIfaceThread.Start();
            }
            else
            {
                InitIFaceDeviceThd();
            }
            if (AppConfig.AutoUpdateFp == 1)
            {
                Thread fpThread = new Thread(RegisterFingerPrint) { IsBackground = true };
                fpThread.Start();
            }

        }

        private void InitIFaceDeviceThd()
        {
            if (AppConfig.IfaceType != "zoko")
            {
                if (FpZkIFace.CheckConnection(AppConfig.IfaceIp, AppConfig.IfacePort) > 0)
                {
                    if (FpZkIFace.Init(AppConfig.IfaceIp, AppConfig.IfacePort) > 0)
                    {
                        UpdateStatus("初始化指纹仪", "成功", 1);
                        return;
                    }
                }
            }
            else
            {
                if (FKDevice.Init(AppConfig.IfaceIp, AppConfig.IfacePort) > 0)
                {
                    UpdateStatus("初始化指纹仪", "成功", 1);
                    return;
                }
            }
           
            _isPassed = false;
            UpdateStatus("初始化指纹仪", "异常", 2);
        }

        private void RegisterFingerPrint()
        {
            int execCount = 120;
            while (true)
            {
                execCount++;
                
                if (execCount >= 120)
                {
                    execCount = 0;
                    if (AppConfig.IfaceType != "zoko")
                    {
                        if (!FpZkIFace.IsConnected())
                        {
                            FpZkIFace.Init(AppConfig.IfaceIp, AppConfig.IfacePort);
                        }
                    }
                    else
                    {
                        if (!FKDevice.IsConnected())
                        {
                            FKDevice.Init(AppConfig.IfaceIp, AppConfig.IfacePort);
                        }
                    }
                    try
                    {
                        string[] group = AppConfig.AllowedGroup.Replace('，', ',').Split(',');
                        IList<UserInfo> uiList = BllUserInfo.SearchUserByGroup(group);
                        if (AppConfig.IfaceType != "zoko")
                        {
                            FpZkIFace.SetDeviceInfo(uiList.ToList());
                            //FpZkIFace.ClearUser();
                            //if (SqlDataHelper.IsDataValid(uiList))
                            //{
                            //    List<FpZkIFace.IfaceUser> ifList = new List<FpZkIFace.IfaceUser>();
                            //    for (int i = 0; i < uiList.Count; i++)
                            //    {
                            //        UserInfo ui = (UserInfo)uiList[i];
                            //        FpZkIFace.IfaceUser ifUser = new FpZkIFace.IfaceUser(ui.FullName,
                            //            ui.TemplateUserId.ToString(), ui.LeftTemplateV10,
                            //            ui.RightTemplateV10);
                            //        ifList.Add(ifUser);
                            //    }
                            //    int result = FpZkIFace.UploadUserInfo(ifList);
                            //}
                            //IList<UserInfo> userList = BllUserInfo.SearchUserNotRegistered();
                            //if (SqlDataHelper.IsDataValid(userList))
                            //{
                            //    List<FpZkIFace.IfaceUser> ifList = new List<FpZkIFace.IfaceUser>();
                            //    for (int i = 0; i < userList.Count; i++)
                            //    {
                            //        UserInfo ui = userList[i];
                            //        FpZkIFace.IfaceUser ifUser = new FpZkIFace.IfaceUser(ui.FullName, ui.TemplateUserId.ToString(), ui.LeftTemplateV10,
                            //            ui.RightTemplateV10);
                            //        ifList.Add(ifUser);
                            //    }
                            //    int result = FpZkIFace.UploadUserInfo(ifList);
                            //    if (result > 0)
                            //    {
                            //        BllUserInfo.SetAsRegistered(userList);
                            //    }
                            //}
                        }
                        else
                        {
                            List<UInt32> lstFailed;
                            List<UInt32> lstSucceed;
                            FKDevice.SetDeviceInfo(uiList.ToList(), out lstFailed, out lstSucceed);
                            //if (SqlDataHelper.IsDataValid(uiList))
                            //{
                            //    UInt32 vEnrollNumber = 0;
                            //    int vPrivilege = 0;
                            //    int vnResultCode, enrollResult, nameResult;
                            //    byte[] mbytCurEnrollData = new byte[20080];
                            //    int vBackupNumber = 10;
                            //    int mnCurPassword = 0;
                            //    int vnEnableFlag = 0;
                            //    uint tmpEnrollNumber = 0;
                            //    int tmpBackupNumber = 0, tmpPrivilege = 0, tmpEnableFlag = 0;
                            //    List<int> lstExistEnrollNum = new List<int>();
                            //    List<UserInfo> failedInfos = new List<UserInfo>();
                            //    do
                            //    {
                            //        vnResultCode = FKAttendDLL.FK_GetAllUserID(FKAttendDLL.nCommHandleIndex, ref tmpEnrollNumber, ref tmpBackupNumber, ref tmpPrivilege, ref tmpEnableFlag);
                            //        if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
                            //        {
                            //            if (vnResultCode == (int)enumErrorCode.RUNERR_DATAARRAY_END)
                            //                vnResultCode = (int)enumErrorCode.RUN_SUCCESS;
                            //            break;

                            //        }
                            //        lstExistEnrollNum.Add(Convert.ToInt32(tmpEnrollNumber.ToString()));
                            //    }
                            //    while (true);
                            //    for (int i = 0; i < uiList.Count; i++)
                            //    {
                            //        UserInfo ui = (UserInfo)uiList[i];
                            //        if(lstExistEnrollNum.Contains(ui.TemplateUserId))
                            //            continue;
                            //        enrollResult = FKAttendDLL.FK_PutEnrollData(
                            //            FKAttendDLL.nCommHandleIndex,
                            //            Convert.ToUInt32(ui.TemplateUserId),
                            //            vBackupNumber,
                            //            vPrivilege,
                            //            mbytCurEnrollData,
                            //            mnCurPassword);
                            //        if (enrollResult != (int)enumErrorCode.RUN_SUCCESS)
                            //        {
                            //            failedInfos.Add(ui);
                            //        }
                            //        nameResult = FKAttendDLL.FK_SetUserName(FKAttendDLL.nCommHandleIndex, Convert.ToUInt32(ui.TemplateUserId), ui.UserName);
                            //        if (nameResult != (int)enumErrorCode.RUN_SUCCESS)
                            //        {
                            //            failedInfos.Add(ui);
                            //        }
                            //        //if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
                            //        //    lblMessage.Text = FKAttendDLL.ReturnResultPrint(vnResultCode);
                            //        //cmdSetEnrollData.Enabled = true;
                            //    }
                            //    vnResultCode = FKAttendDLL.FK_SaveEnrollData(FKAttendDLL.nCommHandleIndex);
                            //    FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
                            //    if (failedInfos.Count > 0 || vnResultCode < 0)
                            //    {
                            //        MessageBox.Show("人员下发出现错误");
                            //    }
                            //}
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        // ignored
                    }
                }
                Thread.Sleep(30000);
            }
            // ReSharper disable once FunctionNeverReturns
        }


        private void InitCabinet()
        {
            bool debugMode = AppConfig.DebugMode == 1;
            //配置Modbus模块信息
            Type configType = typeof(AppConfig);
            for (int i = 1; i <= 9; i++)
            {
                string ip = "";
                int port = 0, capacity = 0, startIndex = 0, nodeId = 0;
                //状态检测设备
                PropertyInfo property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}Ip");
                if (property != null)
                {
                    ip = (string)property.GetValue(null, null);
                }
                if (!string.IsNullOrEmpty(ip))
                {
                    property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}Port");
                    if (property != null)
                    {
                        port = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}Capacity");
                    if (property != null)
                    {
                        capacity = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}StartIndex");
                    if (property != null)
                    {
                        startIndex = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}NodeId");
                    if (property != null)
                    {
                        nodeId = (int)property.GetValue(null, null);
                    }
                    if (AppConfig.DeviceType < 4) DeviceLayer.CabinetDevice.AddToolCheckerDevice(ip, port, (ushort)capacity, (ushort)startIndex);
                    else DeviceLayer.CabinetDevice.AddToolCheckerDevice(ip, port, (ushort)nodeId, (ushort)capacity, (ushort)startIndex);
                }
                //灯控设备
                property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}Ip");
                if (property != null)
                {
                    ip = (string)property.GetValue(null, null);
                }
                if (!string.IsNullOrEmpty(ip))
                {
                    property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}Port");
                    if (property != null)
                    {
                        port = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}Capacity");
                    if (property != null)
                    {
                        capacity = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}StartIndex");
                    if (property != null)
                    {
                        startIndex = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}NodeId");
                    if (property != null)
                    {
                        nodeId = (int)property.GetValue(null, null);
                    }
                    if (AppConfig.DeviceType < 4) DeviceLayer.CabinetDevice.AddToolLedOperatorDevice(ip, port, (ushort)capacity, (ushort)startIndex);
                    else DeviceLayer.CabinetDevice.AddToolLedOperatorDevice(ip, port, (ushort)nodeId, (ushort)capacity, (ushort)startIndex);
                }
            }
            for (int i = 1; i <= 9; i++)
            {
                PropertyInfo mProperty = configType.GetProperty($"BrokenCheckerMapping{i}");
                if (mProperty != null)
                {
                    string mapping = (string)mProperty.GetValue(null, null);
                    if (!string.IsNullOrEmpty(mapping))
                    {
                        string[] data = mapping.Split(',');
                        int origin, dest;
                        if (int.TryParse(data[0], out origin) && int.TryParse(data[1], out dest))
                        {
                            DeviceLayer.CabinetDevice.AddCheckerMapping(origin - 1, dest - 1);
                        }
                    }
                }
                mProperty = configType.GetProperty($"BrokenOperatorMapping{i}");
                if (mProperty != null)
                {
                    string mapping = (string)mProperty.GetValue(null, null);
                    if (!string.IsNullOrEmpty(mapping))
                    {
                        string[] data = mapping.Split(',');
                        int origin, dest;
                        if (int.TryParse(data[0], out origin) && int.TryParse(data[1], out dest))
                        {
                            DeviceLayer.CabinetDevice.AddOperatorMapping(origin - 1, dest - 1);
                        }
                    }
                }
            }
            PropertyInfo spotProperty = configType.GetProperty($"SpotMapping");
            if (spotProperty != null)
            {
                string mapping = (string)spotProperty.GetValue(null, null);
                if (!string.IsNullOrEmpty(mapping))
                {
                    string[] data = mapping.Split('|');
                    int origin, dest;
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (int.TryParse(data[i].Split(',')[0], out dest) && int.TryParse(data[i].Split(',')[1], out origin))
                        {
                            DeviceLayer.CabinetDevice.AddCheckerMapping(origin - 1, dest - 1);
                        }
                    }

                }
            }
            if (AppConfig.ReserveToolCount < AppConfig.ToolCount) AppConfig.ReserveToolCount = AppConfig.ToolCount;

            //设置指示灯点位
            DeviceLayer.CabinetDevice.SetLedMapping(AppConfig.LightCtrlPoint, AppConfig.LedYellowCtrlPoint,
                AppConfig.LedRedCtrlPoint);

            //智能柜抽屉常开配置
            DeviceLayer.CabinetDevice.SetDrawerUnlock(AppConfig.DrawerUnlock);

            if (AppConfig.DrawerUnlockTime < 10) AppConfig.DrawerUnlockTime = 10;
            DeviceLayer.CabinetDevice.SetDrawerUnlockTime(AppConfig.DrawerUnlockTime);

            //初始化工具柜连接
            if (AppConfig.DeviceType < 4)
            {
                DeviceLayer.CabinetDevice.InitDevice((ushort) AppConfig.ToolCount, (ushort) AppConfig.ReserveToolCount,
                    (ushort) AppConfig.DrawerCount,
                    AppConfig.CabinetCheckerIp, AppConfig.CabinetOperatorIp, AppConfig.CabinetCheckerPort,
                    AppConfig.CabinetOperatorPort, debugMode);
            }
            else
            {
                DeviceLayer.CabinetDevice.InitDevice((ushort) AppConfig.ToolCount, (ushort) AppConfig.ReserveToolCount,
                    (ushort) AppConfig.DrawerCount,
                    AppConfig.CanBusIp, AppConfig.CanBusPort, 1, 1, debugMode);
            }

            //连接抽屉UHF扫描设备
            for (int i = 1; i <= AppConfig.DrawerCount; i++)
            {
                PropertyInfo property = configType.GetProperty("DrawerPort" + i);
                if (property != null)
                {
                    int port = (int)property.GetValue(null, null);
                    if (port != -1) UhfDevice.Init(port);
                }
            }
        }

        public void OnCabinetInitDone(string result)
        {
            try
            {
                Invoke(new OnCabinetInitDoneDelegate(OnCabinetInitDoneMtd), result);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private delegate void OnCabinetInitDoneDelegate(string result);
        public void OnCabinetInitDoneMtd(string result)
        {
            try
            {
                int status = 1;
                if (!result.Contains("成功"))
                {
                    _isPassed = false;
                    status = 2;
                }
                UpdateStatus("初始化工具柜控制模块", result, status);

                if (!_isPassed)
                {
                    using (FormMessageBox messageBox = new FormMessageBox("初始化过程出现错误，是否继续?", "提示", 1, 5000))
                    {
                        messageBox.ShowDialog();
                        if (messageBox.Result == 20)
                        {
                            Application.Exit();
                            return;
                        }
                    }
                }
                //Thread openLight = new Thread(OpenLight) { IsBackground = true };
                //if (string.IsNullOrEmpty(AppConfig.LightBright) || AppConfig.LightBright == "1")
                //{
                //    //openLight.Start();
                //}
                Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        //public void OpenLight()
        //{
        //    while (true)
        //    {
        //        DeviceLayer.CabinetDevice.OpenAllLight(true);
        //        Thread.Sleep(200);
        //    }

        //}
    }
}
