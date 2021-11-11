using System;
using System.Collections.Generic;
using Domain.Main.Domain;
using NLog;
using Utilities.Net;
using zkemkeeper;

namespace Hardware.DeviceInterface
{
    public class FpZkIFace
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static CZKEMClass _ifaceEngine;
        private static int dwMachineNumber = 1;

        public class IfaceUser
        {
            public string FullName;
            public string TemplateId;
            public string Feature1;
            public string Feature2;

            public IfaceUser(string fullName, string templateId, string feature1, string feature2)
            {
                FullName = fullName;
                TemplateId = templateId;
                Feature1 = feature1;
                Feature2 = feature2;
            }
        }

        public class ZKUserInfo
        {
            public string FullName;
            public string EnrollId;
            public string LeftTemplate;
            public string RightTemplate;
            public string FaceTemplate;

            public ZKUserInfo(string fullName, string enrollId, string leftTemplate, string rightTemplate, string faceTemplate)
            {
                FullName = fullName;
                EnrollId = enrollId;
                LeftTemplate = leftTemplate;
                RightTemplate = rightTemplate;
                FaceTemplate = faceTemplate;
            }
        }

        public static void Prepare()
        {
            //Force Class Init
        }

        /// <summary>
        /// 检查网络
        /// </summary>
        /// <param name="deviceIp"></param>
        /// <param name="devicePort"></param>
        /// <returns></returns>
        public static int CheckConnection(string deviceIp, int devicePort)
        {
            try
            {
                TcpClientEx clientInstance = new TcpClientEx(deviceIp, devicePort, 3000);
                clientInstance.Connect();
                clientInstance.Close();
                return 1;
            }
            catch (Exception)
            {
                return -100;
            }
        }

        /// <summary>
        /// 初始化并注册实时事件
        /// </summary>
        /// <returns></returns>
        public static int Init(string deviceIp, int devicePort)
        {
            _ifaceEngine = new CZKEMClass();
            bool isConnected = _ifaceEngine.Connect_Net(deviceIp, devicePort);
            if (isConnected)
            {
                if (_ifaceEngine.RegEvent(1, 65535))
                {
                    _ifaceEngine.OnDisConnected += OnDisConnected;
                    _ifaceEngine.OnFinger += OnFinger;
                    _ifaceEngine.OnVerify += OnVerify;
                    _ifaceEngine.OnAttTransactionEx += OnAttTransactionEx;
                    _ifaceEngine.OnFingerFeature += OnFingerFeature;
                    _ifaceEngine.OnEnrollFingerEx += OnEnrollFingerEx;
                    _ifaceEngine.OnDeleteTemplate += OnDeleteTemplate;
                    _ifaceEngine.OnNewUser += OnNewUser;
                    _ifaceEngine.OnHIDNum += OnHidNum;
                    _ifaceEngine.OnAlarm += OnAlarm;
                    _ifaceEngine.OnDoor += OnDoor;
                    _ifaceEngine.OnWriteCard += OnWriteCard;
                    _ifaceEngine.OnEmptyCard += OnEmptyCard;
                    return 1;
                }
            }
            return 0;
        }

        public static bool IsConnected()
        {
            if (_ifaceEngine == null) return false;
            string firmVersion = "";
            if (!_ifaceEngine.GetFirmwareVersion(1, ref firmVersion))
            {
                return false;
            }
            if (string.IsNullOrEmpty(firmVersion))
            {
                return false;
            }
            return true;
        }

        public static int UploadUserInfo(List<IfaceUser> userList)
        {
            try
            {
                _ifaceEngine.EnableDevice(1, false);
                if (_ifaceEngine.BeginBatchUpdate(1, 1)) //开始缓冲式上传，覆盖已有用户
                {
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (_ifaceEngine.SSR_SetUserInfo(1, userList[i].TemplateId, userList[i].FullName, "159357", 0, true))//upload user information to the memory
                        {
                            if (!string.IsNullOrEmpty(userList[i].Feature1))
                            {
                                _ifaceEngine.SetUserTmpExStr(1, userList[i].TemplateId, 0, 1, userList[i].Feature1);
                            }
                            if (!string.IsNullOrEmpty(userList[i].Feature2))
                            {
                                _ifaceEngine.SetUserTmpExStr(1, userList[i].TemplateId, 1, 1, userList[i].Feature2);
                            }
                        }
                        else
                        {
                            int idwErrorCode = 0;
                            _ifaceEngine.GetLastError(ref idwErrorCode);
                            Logger.Error("Error:" + idwErrorCode);
                            return -1;
                        }
                    }
                    _ifaceEngine.BatchUpdate(1);
                    _ifaceEngine.RefreshData(1);
                    _ifaceEngine.EnableDevice(1, true);
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                try
                {
                    _ifaceEngine.EnableDevice(1, true);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return 0;
        }

        public static bool SetDeviceInfo(List<UserInfo> lst)
        {
            if (_ifaceEngine.ClearData(1, 5))
            {
                _ifaceEngine.RefreshData(1);
            }
            //IList uiList = DbFunction.GetAllUser();
            if (lst.Count > 0)
            {
                List<ZKUserInfo> ifList = new List<ZKUserInfo>();
                for (int i = 0; i < lst.Count; i++)
                {
                    UserInfo ui = (UserInfo)lst[i];
                    if (Char.IsLetter(ui.EnrollId.ToCharArray()[0])) continue;
                    ZKUserInfo ifUser = new ZKUserInfo(ui.FullName, ui.EnrollId.ToString(), ui.LeftTemplateV10,
                        ui.RightTemplateV10, ui.FaceTemplateV10);
                    ifList.Add(ifUser);
                }
                int result = UploadUserInfo(ifList);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static int UploadUserInfo(List<ZKUserInfo> userList)
        {
            try
            {
                _ifaceEngine.EnableDevice(1, false);
                if (_ifaceEngine.BeginBatchUpdate(dwMachineNumber, 1)) //开始缓冲式上传，覆盖已有用户
                {
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (_ifaceEngine.SSR_SetUserInfo(dwMachineNumber, userList[i].EnrollId.Trim(), userList[i].FullName, "159357", 0, true))//upload user information to the memory
                        {
                            if (!string.IsNullOrEmpty(userList[i].LeftTemplate))
                            {
                                _ifaceEngine.SetUserTmpExStr(1, userList[i].EnrollId.Trim(), 0, 1, userList[i].LeftTemplate);
                            }
                            if (!string.IsNullOrEmpty(userList[i].RightTemplate))
                            {
                                _ifaceEngine.SetUserTmpExStr(1, userList[i].EnrollId.Trim(), 1, 1, userList[i].RightTemplate);
                            }
                        }
                        else
                        {
                            int idwErrorCode = 0;
                            _ifaceEngine.GetLastError(ref idwErrorCode);
                            Logger.Error("Error:" + idwErrorCode);
                            return -1;
                        }
                    }
                    _ifaceEngine.BatchUpdate(dwMachineNumber);
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (userList[i].FaceTemplate.Trim().Length <= 0) continue;
                        if (_ifaceEngine.SSR_SetUserInfo(dwMachineNumber, userList[i].EnrollId.Trim(), userList[i].FullName, "159357", 0, true))//upload user information to the memory
                        {

                            if (_ifaceEngine.SetUserFaceStr(dwMachineNumber, userList[i].EnrollId.Trim(), 50, userList[i].FaceTemplate, userList[i].FaceTemplate.Length))
                            {

                            }
                            else
                            {
                                int idwErrorCode = 0;
                                _ifaceEngine.GetLastError(ref idwErrorCode);
                                Logger.Error("Error:" + idwErrorCode);
                                return -1;
                            }
                        }
                    }
                    _ifaceEngine.RefreshData(dwMachineNumber);
                    _ifaceEngine.EnableDevice(dwMachineNumber, true);
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                try
                {
                    _ifaceEngine.EnableDevice(1, true);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return 0;
        }

        public static int ClearUser()
        {
            try
            {
                if (_ifaceEngine.ClearData(1, 5))
                {
                    _ifaceEngine.RefreshData(1);
                    return 1;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return 0;
        }

        private static void OnFinger()
        {
            FpCallBack.OnTouching?.Invoke();
        }

        //After you have placed your finger on the sensor(or swipe your card to the device),this event will be triggered.
        //If you passes the verification,the returned value userid will be the user enrollnumber,or else the value will be -1;
        private static void OnVerify(int iUserId)
        {

        }

        //If your fingerprint(or your card) passes the verification,this event will be triggered
        private static void OnAttTransactionEx(string sEnrollNumber, int iIsInValid, int iAttState, int iVerifyMethod, int iYear, int iMonth, int iDay, int iHour, int iMinute, int iSecond, int iWorkCode)
        {
            FpCallBack.OnUserRecognised?.Invoke(int.Parse(sEnrollNumber), iVerifyMethod);
        }

        //When you have enrolled your finger,this event will be triggered and return the quality of the fingerprint you have enrolled
        private static void OnFingerFeature(int iScore)
        {
            FpCallBack.OnEnrollProgressChanged?.Invoke(-1, iScore);
        }

        //When you are enrolling your finger,this event will be triggered.(The event can only be triggered by TFT screen devices)
        private static void OnEnrollFingerEx(string sEnrollNumber, int iFingerIndex, int iActionResult, int iTemplateLength)
        {
            if (iActionResult == 0)
            {
                FpCallBack.OnManagedEnroll?.Invoke(int.Parse(sEnrollNumber), iTemplateLength);
            }
        }

        //When you have deleted one one fingerprint template,this event will be triggered.
        private static void OnDeleteTemplate(int iEnrollNumber, int iFingerIndex)
        {

        }

        //When you have enrolled a new user,this event will be triggered.
        private static void OnNewUser(int iEnrollNumber)
        {

        }

        //When you swipe a card to the device, this event will be triggered to show you the card number.
        private static void OnHidNum(int iCardNumber)
        {

        }

        //When the dismantling machine or duress alarm occurs, trigger this event.
        private static void OnAlarm(int iAlarmType, int iEnrollNumber, int iVerified)
        {

        }

        //Door sensor event
        private static void OnDoor(int iEventType)
        {

        }

        //When you have emptyed the Mifare card,this event will be triggered.
        private static void OnEmptyCard(int iActionResult)
        {

        }

        //When you have written into the Mifare card ,this event will be triggered.
        private static void OnWriteCard(int iEnrollNumber, int iActionResult, int iLength)
        {

        }

        private static void OnDisConnected()
        {

        }
    }
}
