using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxRealSvrOcxTcpLib;
using AxZKFPEngXControl;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using NLog;

namespace Hardware.DeviceInterface
{
    public class FKDevice
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //匹配引擎
        //重入锁
        private static readonly object CaptureLock = new object();

        //连续识别锁
        private static int _lastUserId = -1;
        private static DateTime _lastUserAccess;

        private static AxRealSvrOcxTcp AxRealSvrOcxTcp1;

        /// <summary>
        /// 初始化匹配引擎
        /// </summary>
        /// <returns></returns>
        public static int Init(string ip, int port)
        {
            //动态添加控件
            int vnMachineNumber = 1;
            string vstrTelNumber = "";
            int vnWaitDialTime = 3000;
            int vnLicense = 1261;
            string vpszIPAddress = ip;
            int vpszNetPort = port;
            int vpszNetPassword = 0;
            int vnTimeOut = 5000;
            int vnProtocolType;
            long vnResultCode = (long)enumErrorCode.RUNERR_UNKNOWNERROR;
            vnProtocolType = (int)enumProtocolType.PROTOCOL_TCPIP;
            try
            {
                FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectNet(vnMachineNumber, vpszIPAddress, vpszNetPort, vnTimeOut, vnProtocolType, vpszNetPassword, vnLicense);

                if (FKAttendDLL.nCommHandleIndex > 0)
                {
                    return FKAttendDLL.nCommHandleIndex;
                }
                else
                {
                    //vnResultCode = FKAttendDLL.nCommHandleIndex;
                    //MessageBox.Show(FKAttendDLL.ReturnResultPrint(vnResultCode), "error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return -1;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -1;
            }

        }

        public static int AddEvents(AxRealSvrOcxTcp axRealSvrOcxTcp)
        {
            AxRealSvrOcxTcp1 = axRealSvrOcxTcp;
            AxRealSvrOcxTcp1.OnReceiveGLogDataExtend += AxRealSvrOcxTcp1_OnReceiveGLogDataExtend;
            AxRealSvrOcxTcp1.OnReceiveGLogTextAndImage += AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage;
            AxRealSvrOcxTcp1.OnReceiveGLogTextOnDoorOpen += AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen;
            int retCode = AxRealSvrOcxTcp1.OpenNetwork(7005);
            if (retCode != (long)enumErrorCode.RUN_SUCCESS) return -1;
            return 1;
        }

        public static bool IsConnected()
        {
            if (AxRealSvrOcxTcp1 == null) return false;
            int vnResultCode;
            vnResultCode = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
            if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
            {
                return false;
            }
            return true;
        }

        public static int SetDeviceInfo(List<UserInfo> lst, out List<UInt32> lstFailed, out List<UInt32> lstSucceed)
        {
            lstFailed = new List<uint>();
            lstSucceed = new List<uint>();
            int vnResultCode = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
            if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
            {
                Logger.Info("FK_EnableDevice" + vnResultCode);
                return -100;
            }
            int vnResultCode1, vnResultCode2, vnResultCode3;
            vnResultCode = FKAttendDLL.FK_EmptyEnrollData(FKAttendDLL.nCommHandleIndex);
            for (int i = 0; i < lst.Count; i++)
            {
                UserInfo ui = lst[i];
                if (Char.IsLetter(ui.UserName.ToCharArray()[0])) continue;

                if (GetName(uint.Parse(ui.EnrollId)) == null)
                {
                    SetName(uint.Parse(ui.EnrollId), ui.FullName);
                }
                vnResultCode1 = 0; vnResultCode2 = 0; vnResultCode3 = 0;
                if (ui.LeftTemplate.Length > 0)
                {
                    vnResultCode1 = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, uint.Parse(ui.EnrollId), 0, 0, ui.LeftTemplate, 0);
                }
                if (ui.RightTemplate.Length > 0)
                {
                    vnResultCode2 = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, uint.Parse(ui.EnrollId), 1, 0, ui.RightTemplate, 0);
                }
                if (ui.FaceTemplate.Length > 0)
                {
                    vnResultCode3 = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, uint.Parse(ui.EnrollId), 12, 0, ui.FaceTemplate, 0);
                }
                if (vnResultCode1 < 0 || vnResultCode2 < 0 || vnResultCode3 < 0)
                {
                    lstFailed.Add(uint.Parse(ui.EnrollId));
                    Logger.Warn("FK_PutEnrollData" + ui.EnrollId);
                }
                else
                {
                    lstSucceed.Add(uint.Parse(ui.EnrollId));
                }
            }
            FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
            return vnResultCode;
        }

        public static string GetName(UInt32 enrollId)
        {
            string vName = "";
            var vnResultCode = FKAttendDLL.FK_GetUserName(FKAttendDLL.nCommHandleIndex, enrollId, ref vName);
            if (vnResultCode == (int)enumErrorCode.RUN_SUCCESS)
            {
                return vName;
            }
            return null;
        }

        public static int SetName(UInt32 enrollId, string name)
        {
            string vName = "";
            var vnResultCode = FKAttendDLL.FK_SetUserName(FKAttendDLL.nCommHandleIndex, enrollId, name);
            if (vnResultCode == (int)enumErrorCode.RUN_SUCCESS)
            {
                return 1;
            }
            return -100;
        }

        //public static int AddUser(List<UserInfo> lstUserInfo)
        //{
        //    //string vStrEnrollNumber;
        //    UInt32 vEnrollNumber = 0;
        //    int vPrivilege = 0;
        //    int vnResultCode;
        //    byte[] mbytCurEnrollData = new byte[20080];
        //    //Application.DoEvents();
        //    //vStrEnrollNumber = enrollNumber;
        //    //vEnrollNumber = FKAttendDLL.GetInt(vStrEnrollNumber);
        //    int vBackupNumber = 10;
        //    int mnCurPassword = 0;
        //    vnResultCode = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
        //    if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
        //    {
        //        //lblMessage.Text = FKAttendDLL.msErrorNoDevice;
        //        Logger.Error(new Exception(FKAttendDLL.msErrorNoDevice));
        //        return -1;
        //    }

        //    int vnEnableFlag = 0;
        //    uint tmpEnrollNumber = 0;
        //    int tmpBackupNumber = 0, tmpPrivilege = 0, tmpEnableFlag = 0;
        //    List<UInt32> lstExistEnrollNum = new List<UInt32>();
        //    do
        //    {
        //        vnResultCode = FKAttendDLL.FK_GetAllUserID(FKAttendDLL.nCommHandleIndex, ref tmpEnrollNumber, ref tmpBackupNumber, ref tmpPrivilege, ref tmpEnableFlag);
        //        if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
        //        {
        //            if (vnResultCode == (int)enumErrorCode.RUNERR_DATAARRAY_END)
        //                vnResultCode = (int)enumErrorCode.RUN_SUCCESS;
        //            break;

        //        }
        //        lstExistEnrollNum.Add(tmpEnrollNumber);
        //    }
        //    while (true);

        //    vnResultCode = FKAttendDLL.FK_PutEnrollData(
        //        FKAttendDLL.nCommHandleIndex,
        //        vEnrollNumber,
        //        vBackupNumber,
        //        vPrivilege,
        //        mbytCurEnrollData,
        //        mnCurPassword);

        //    if (vnResultCode == (int)enumErrorCode.RUN_SUCCESS)
        //    {
        //        Application.DoEvents();
        //        vnResultCode = FKAttendDLL.FK_SaveEnrollData(FKAttendDLL.nCommHandleIndex);
        //        if (vnResultCode == (int) enumErrorCode.RUN_SUCCESS)
        //            return 1;
        //    }

        //    //if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
        //    //    lblMessage.Text = FKAttendDLL.ReturnResultPrint(vnResultCode);

        //    FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
        //    //cmdSetEnrollData.Enabled = true;
        //}

        private static void AxRealSvrOcxTcp1_OnReceiveGLogDataExtend(object sender, AxRealSvrOcxTcpLib._DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEvent e)
        {
            if (FKCallBack.OnUserRecognised == null) return;
            //防止线程重入
            lock (CaptureLock)
            {
                int score = 0, matchCount = 0;
                //匹配指纹
                int userId = e.anSEnrollNumber;
                //去除连续扫描成功的多余请求
                if (userId != -1)
                {
                    if (userId == _lastUserId)
                    {
                        if ((DateTime.Now - _lastUserAccess).TotalSeconds <= 5)
                        {
                            FKCallBack.OnUserRecognised(userId, 1);
                            return;
                        }
                    }
                    _lastUserId = userId;
                    _lastUserAccess = DateTime.Now;
                }
                //识别成功，返回结果
                _lastUserId = userId;// % FpConfig.RegDivider;
                FKCallBack.OnUserRecognised(_lastUserId, 1);
            }
            //funcShowGeneralLogDataToGrid(e.anSEnrollNumber, e.anVerifyMode, e.anInOutMode, e.anLogDate, true, e.astrDeviceIP, e.anDevicePort, e.anDeviceID, e.astrSerialNo, e.astrRootIP);
        }

        private static void AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen(object sender, AxRealSvrOcxTcpLib._DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEvent e)
        {
            //String vstrLogId;
            //String vstrEnrollNumber;
            //String vstrDeviceID;
            //String vstrVerifyMode;
            //String vstrInOutMode;
            //String vstrDate;
            //String vDate;
            //String vstrResponse;
            //String vstrEmergency;
            //String vstrIsSupportStringID;
            //JObject jobjTest = JObject.Parse(e.astrLogText);

            //vstrLogId = jobjTest["log_id"].ToString();
            //vstrEnrollNumber = jobjTest["user_id"].ToString();
            //vstrDeviceID = jobjTest["fk_device_id"].ToString();
            //vstrVerifyMode = jobjTest["verify_mode"].ToString();
            //vstrInOutMode = jobjTest["io_mode"].ToString();
            //vstrDate = jobjTest["io_time"].ToString();


            //vDate = vstrDate.Substring(0, 4) + "-" +
            //        vstrDate.Substring(4, 2) + "-" +
            //        vstrDate.Substring(6, 2) + " " +
            //        vstrDate.Substring(8, 2) + ":" +
            //        vstrDate.Substring(10, 2) + ":" +
            //        vstrDate.Substring(12, 2);

            //fnCount++;
            //ShowGeneralLogDataToGrid(fnCount, vstrLogId, vstrEnrollNumber, vstrVerifyMode, vstrInOutMode, vDate, true, e.astrClientIP, e.anClientPort, vstrDeviceID);
            //txtStatus.Text = "Last Log :{ user_id: " + vstrEnrollNumber + ", io_time :" + vDate + "}";
            //showLogImage(e.astrLogImage);

            //JObject jobjRespond = new JObject();
            //jobjRespond.Add("log_id", vstrLogId);
            //jobjRespond.Add("result", "OK");
            //try
            //{
            //    vstrEmergency = jobjTest["emergency"].ToString();
            //    vstrIsSupportStringID = jobjTest["is_support_string_id"].ToString();

            //    if (vstrEmergency.Equals("yes"))
            //    {
            //        if (txtActiveId.Text != "" && txtActiveId.Text == vstrEnrollNumber)
            //        {
            //            jobjRespond.Add("mode", "open");
            //        }
            //        else
            //        {
            //            jobjRespond.Add("mode", "nothing");
            //        }
            //    }
            //    else
            //    {
            //        jobjRespond.Add("mode", "nothing");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    jobjRespond.Add("mode", "nothing");
            //}
            //vstrResponse = jobjRespond.ToString();
            //AxRealSvrOcxTcp1.SendRtLogResponseV3(e.astrClientIP, e.anClientPort, vstrResponse);
        }

        private static void AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage(object sender, AxRealSvrOcxTcpLib._DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent e)
        {
            //String vstrLogId;
            //String vstrEnrollNumber;
            //String vstrDeviceID;
            //String vstrVerifyMode;
            //String vstrInOutMode;
            //String vstrDate;
            //String vDate;
            //String vstrResponse;

            //JObject jobjTest = JObject.Parse(e.astrLogText);

            //vstrLogId = jobjTest["log_id"].ToString();
            //vstrEnrollNumber = jobjTest["user_id"].ToString();
            //vstrDeviceID = jobjTest["fk_device_id"].ToString();
            //vstrVerifyMode = jobjTest["verify_mode"].ToString();
            //vstrInOutMode = jobjTest["io_mode"].ToString();
            //vstrDate = jobjTest["io_time"].ToString();
            //vDate = vstrDate.Substring(0, 4) + "-" +
            //        vstrDate.Substring(4, 2) + "-" +
            //        vstrDate.Substring(6, 2) + " " +
            //        vstrDate.Substring(8, 2) + ":" +
            //        vstrDate.Substring(10, 2) + ":" +
            //        vstrDate.Substring(12, 2);
            //fnCount++;
            //ShowGeneralLogDataToGrid(fnCount, vstrLogId, vstrEnrollNumber, vstrVerifyMode, vstrInOutMode, vDate, true, e.astrClientIP, e.anClientPort, vstrDeviceID);
            //txtStatus.Text = "Last Log :{ user_id: " + vstrEnrollNumber + ", io_time :" + vDate + "}";
            //JObject jobjRespond = new JObject();
            //jobjRespond.Add("log_id", vstrLogId);
            //jobjRespond.Add("result", "OK");
            //vstrResponse = jobjRespond.ToString();
            //showLogImage(e.astrLogImage);
            //AxRealSvrOcxTcp1.SendResponse(e.astrClientIP, e.anClientPort, vstrResponse);
        }


        //private static void FpZk_OnImageReceived(object sender, IZKFPEngXEvents_OnImageReceivedEvent e)
        //{
        //    if (FpCallBack.OnImageReceived != null)
        //    {
        //        //获取指纹图像
        //        Bitmap bmp = new Bitmap(_zkEngine.ImageWidth, _zkEngine.ImageHeight);
        //        Graphics g = Graphics.FromImage(bmp);
        //        int dc = g.GetHdc().ToInt32();
        //        _zkEngine.PrintImageAt(dc, 0, 0, bmp.Width, bmp.Height);
        //        g.Dispose();
        //        //回调
        //        FpCallBack.OnImageReceived(bmp);
        //    }
        //}

        //private static void FpZk_OnEnroll(object sender, IZKFPEngXEvents_OnEnrollEvent e)
        //{
        //    if (FpCallBack.OnEnroll == null) return;
        //    byte[] sigGen = (byte[])_zkEngine.GetTemplate();
        //    if (e.actionResult)
        //    {
        //        //_zkEngine.AddRegTemplateToFPCacheDB(_cachePointer, _curRegUser, sigGen);
        //        FpCallBack.OnEnroll(sigGen);
        //    }
        //    else
        //    {
        //        FpCallBack.OnEnroll(null);
        //    }
        //}

        //private static void FpZk_OnEnrollProgressChanged(object sender, IZKFPEngXEvents_OnFeatureInfoEvent e)
        //{
        //    if (_zkEngine.IsRegister)
        //    {
        //        if (_zkEngine.EnrollIndex - 1 > 0)
        //        {
        //            if (FpCallBack.OnEnrollProgressChanged != null)
        //            {
        //                FpCallBack.OnEnrollProgressChanged(_zkEngine.EnrollIndex - 1, -1);
        //            }
        //        }
        //    }
        //}

        //private static void FpZk_OnFingerTouching(object sender, EventArgs e)
        //{
        //    if (FpCallBack.OnTouching != null)
        //    {
        //        FpCallBack.OnTouching();
        //    }
        //}

        //private static void FpZk_OnFingerLeaving(object sender, EventArgs e)
        //{
        //    if (FpCallBack.OnLeaving != null)
        //    {
        //        FpCallBack.OnLeaving();
        //    }
        //}

        //private static void FpZk_OnCapture(object sender, IZKFPEngXEvents_OnCaptureEvent e)
        //{
        //    if (FpCallBack.OnUserRecognised == null) return;
        //    //防止线程重入
        //    lock (CaptureLock)
        //    {
        //        int score = 0, matchCount = 0;
        //        //匹配指纹
        //        int userId = _zkEngine.IdentificationInFPCacheDB(_cachePointer, _zkEngine.GetTemplate(), ref score,
        //            ref matchCount);
        //        //去除连续扫描成功的多余请求
        //        if (userId != -1)
        //        {
        //            if (userId == _lastUserId)
        //            {
        //                if ((DateTime.Now - _lastUserAccess).TotalSeconds <= 5)
        //                {
        //                    FpCallBack.OnUserRecognised(userId, 1);
        //                    return;
        //                }
        //            }
        //            _lastUserId = userId;
        //            _lastUserAccess = DateTime.Now;
        //        }
        //        //拒绝未达标识别结果
        //        if (score < _securityLevel)
        //        {
        //            _lastMark = -1;
        //            FpCallBack.OnUserRecognised(-1, 1);
        //            return;
        //        }
        //        //识别成功，返回结果
        //        _lastMark = score;
        //        _lastUserId = userId % FpConfig.RegDivider;
        //        FpCallBack.OnUserRecognised(_lastUserId, 1);
        //    }
        //}
    }
}
