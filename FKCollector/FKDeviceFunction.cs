using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NLog;

namespace FKCollector
{
    public static class FKDeviceFunction
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int Init()
        {
            int vnMachineNumber = 1;
            string vstrTelNumber = "";
            int vnWaitDialTime = 3000;
            int vnLicense = 1261;
            //string vpszIPAddress = "192.168.0.201";
            // vpszNetPort = 5005;
            int tmp;
            string vpszIPAddress = ConfigurationManager.AppSettings["deviceIp"];
            int vpszNetPort = int.TryParse(ConfigurationManager.AppSettings["devicePort"], out tmp) ? tmp : 5005;
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
                    return -1;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -1;
            }
        }

        public static List<FKDeviceInfo> GetAllInfo()
        {
            UInt32 vEnrollNumber = 0;
            int vBackupNumber = 0;
            int vPrivilege = 0;
            int vnEnableFlag = 0;
            int vnResultCode;
            List<FKDeviceInfo> lst = new List<FKDeviceInfo>();
            vnResultCode = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
            if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
            {
                Logger.Info("FK_EnableDevice" + vnResultCode);
                return null;
            }
            vnResultCode = FKAttendDLL.FK_ReadAllUserID(FKAttendDLL.nCommHandleIndex);
            if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
            {
                FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
                Logger.Info("FK_ReadAllUserID" + vnResultCode);
                return null;
            }
            
                do
                {
                    vnResultCode = FKAttendDLL.FK_GetAllUserID(FKAttendDLL.nCommHandleIndex, ref vEnrollNumber, ref vBackupNumber, ref vPrivilege, ref vnEnableFlag);
                    if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
                    {
                        if (vnResultCode == (int)enumErrorCode.RUNERR_DATAARRAY_END)
                            vnResultCode = (int)enumErrorCode.RUN_SUCCESS;
                        break;
                    }
                Hashtable ht = GetDeviceInfo(vEnrollNumber, vBackupNumber);
                lst.Add(new FKDeviceInfo(vEnrollNumber, vBackupNumber, (byte[])ht[vBackupNumber]));
            }
                while (true);
            FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
            return lst;
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
                if (ui.LEFTTEMPLATE.Length > 0)
                {
                    vnResultCode1 = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, uint.Parse(ui.EnrollId), 0, 0, ui.LEFTTEMPLATE, 0);
                }
                if (ui.RIGHTTEMPLATE.Length > 0)
                {
                    vnResultCode2 = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, uint.Parse(ui.EnrollId), 1, 0, ui.RIGHTTEMPLATE, 0);
                }
                if (ui.FACETEMPLATE.Length > 0)
                {
                    vnResultCode3 = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, uint.Parse(ui.EnrollId), 12, 0, ui.FACETEMPLATE, 0);
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

        //public static int AddDeviceInfo(UserInfo ui)
        //{
            
        //}

        public static Hashtable GetDeviceInfo(UInt32 enrollId, int backUpNum)
        {
            byte[] byteData = new byte[20080];
            int mnCurPassword = 0;
            int vPrivilege = 1;
            var vnResultCode = FKAttendDLL.FK_GetEnrollData(
                FKAttendDLL.nCommHandleIndex,
                enrollId,
                backUpNum,
                ref vPrivilege,
                byteData,
                ref mnCurPassword);
            if (vnResultCode == (int)enumErrorCode.RUN_SUCCESS)
            {
                Hashtable ht = new Hashtable();
                ht[backUpNum] = byteData;
                return ht;
            }
            return null;
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
    }
}



    public class FKDeviceInfo
    {
        public UInt32 EnrollId;
        public int BackupNum;
        public byte[] ByteData;

        public FKDeviceInfo(UInt32 enrollId, int backupNum, byte[] byteData)
        {
            EnrollId = enrollId;
            BackupNum = backupNum;
            ByteData = byteData;
        }
    }

    


