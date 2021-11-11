using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using NLog;
using Utilities.DbHelper;
using zkemkeeper;

namespace FKCollector
{
    public static class ZKDeviceFunction
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static CZKEMClass _ifaceEngine;
        private static int dwMachineNumber = 1;

        public static int Init()
        {
            string deviceIp = ConfigurationManager.AppSettings["deviceIp"];
            //string devicePort = ConfigurationManager.AppSettings["devicePort"];
            int tmp;
            int devicePort = int.TryParse(ConfigurationManager.AppSettings["devicePort"], out tmp) ? tmp : 4370;
            //string deviceIp = "192.168.0.201";
            //int devicePort = 4370;
            try
            {
                _ifaceEngine = new CZKEMClass();
                bool isConnected = _ifaceEngine.Connect_Net(deviceIp, devicePort);
                if (isConnected)
                {
                    if (_ifaceEngine.RegEvent(dwMachineNumber, 65535))
                    {
                        _ifaceEngine.EnableDevice(dwMachineNumber, true);
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -1;
            }
        }

        public static List<ZKDeviceInfo> GetAllInfo()
        {
            string enrollNumber;
            string name;
            string password;
            int privilege;
            bool enabled;
            bool vnResultCode;
            List<ZKDeviceInfo> lst = new List<ZKDeviceInfo>();
            vnResultCode = _ifaceEngine.ReadAllUserID(dwMachineNumber);
            vnResultCode = _ifaceEngine.ReadAllTemplate(dwMachineNumber);
            if (!vnResultCode)
            {
                _ifaceEngine.EnableDevice(dwMachineNumber, true);
                Logger.Info("ZK_ReadAllUserID");
                return null;
            }
            vnResultCode = _ifaceEngine.EnableDevice(dwMachineNumber, false);
            if (!vnResultCode)
            {
                Logger.Info("ZK_EnableDevice");
                return null;
            }
             int idwErrorCode = 0;
            while (_ifaceEngine.SSR_GetAllUserInfo(dwMachineNumber, out enrollNumber, out name, out password, out privilege, out enabled))
            {
                string tmpData = "";
                int tmpLength = 0;
                int flag = 0;
                int iLength = 128 * 1024;
                byte[] byTmpData = new byte[iLength];
                for (int i = 0; i < 10; i++)
                {
                    if (_ifaceEngine.GetUserTmpExStr(dwMachineNumber, enrollNumber, i, out flag, out tmpData, out tmpLength))
                    {
                        lst.Add(new ZKDeviceInfo(int.Parse(enrollNumber), i, new byte[0], tmpData));
                    }
                }
                if (_ifaceEngine.GetUserFaceStr(dwMachineNumber, enrollNumber, 50, ref tmpData, ref tmpLength))
                {
                    byte[] byteAry = new byte[iLength];
                    Array.Copy(byTmpData, byteAry, iLength);
                    lst.Add(new ZKDeviceInfo(int.Parse(enrollNumber), 50, new byte[0], tmpData));
                }
            }
            _ifaceEngine.EnableDevice(dwMachineNumber, true);
            return lst;
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

        public static Hashtable GetDeviceInfo(string enrollNumber)
        {
            string tmpData
                = "";
            int enrollId = int.Parse(enrollNumber);
            byte tmpByte = new byte();// = new byte*(); 
            //byte[] tmpByte = new byte[20080];
            int tmpLength = 0;
            bool vnResultCode;
            Hashtable ht = new Hashtable();
            for (int i = 0; i < 10; i++)
            {
                vnResultCode = _ifaceEngine.GetUserTmpStr(dwMachineNumber, enrollId, i, ref tmpData, ref tmpLength);
                //vnResultCode = _ifaceEngine.GetUserTmp(dwMachineNumber, enrollId, i, ref tmpByte, ref tmpLength);
                //byte[] w = Read(tmpLength, ref tmpByte);
                if (vnResultCode)
                {
                    ht.Add(i, tmpData);
                    //ht[i] = tmpData;
                    //return ht;
                }
            }
            vnResultCode = _ifaceEngine.GetUserFace(dwMachineNumber, enrollNumber, 50, ref tmpByte, ref tmpLength);
            //vnResultCode = _ifaceEngine.GetUserFace(dwMachineNumber, enrollNumber, 50, ref tmpByte, ref tmpLength);
            if (vnResultCode)
            {
                //ht.Add(50, tmpByte);
            }

            return ht;
        }

        //public static byte[] Read(int tmpLength, ref byte tmpByte)
        //{
        //    byte[] buf = new byte[tmpLength];
        //    unsafe
        //    {
        //        fixed (byte* p = &tmpByte)
        //        {

        //            using (UnmanagedMemoryStream ms = new UnmanagedMemoryStream((byte*)p, tmpLength))
        //            {
        //                ms.Read(buf, 0, buf.Length);
        //            }

        //            //using (UnmanagedMemoryStream ms = new UnmanagedMemoryStream((byte*)p, tmpLength, tmpLength, FileAccess.Read))
        //            //{
        //            //    ms.Read(buf, 0, buf.Length);
        //            //}
        //        }
        //    }
        //    return buf;
        //}

        public static void DisConnect()
        {
            _ifaceEngine.EnableDevice(dwMachineNumber, true);
            _ifaceEngine.Disconnect();
        }

        public static string GetName(int enrollId)
        {
            string vName = "";
            string passWord = "";
            int privilege = 0;
            bool enabled = false;
            var vnResultCode = _ifaceEngine.GetUserInfo(dwMachineNumber, enrollId, ref vName, ref passWord, ref privilege, ref enabled);
            if (vnResultCode)
            {
                return vName;
            }
            return null;
        }

        public static int SetName(int enrollId, string name)
        {
            string vName = "";
            var vnResultCode = _ifaceEngine.SetUserInfo(dwMachineNumber, enrollId, name, "", 0, true);
            if (vnResultCode)
            {
                return 1;
            }
            return -100;
        }


    }

    public class ZKDeviceInfo
    {
        public int EnrollId;
        public int BackupNum;
        public byte[] ByteData;
        public string StrData;

        public ZKDeviceInfo(int enrollId, int backupNum, byte[] byteData, string strData)
        {
            EnrollId = enrollId;
            BackupNum = backupNum;
            ByteData = byteData;
            StrData = strData;
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
}

