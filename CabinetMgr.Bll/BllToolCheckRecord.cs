using System;
using System.Collections;
using CabinetMgr.Dal;

namespace CabinetMgr.Bll
{
    public class BllToolCheckRecord
    {
        public static IList SearchToolCheckRecord(string toolId, DateTime timeStart, DateTime timeEnd)
        {
            if (string.IsNullOrEmpty(toolId)) return null;
            return DalToolCheckRecord.SearchToolCheckRecord(toolId, timeStart, timeEnd);
        }

        public static int AddToolCheckRecord(string toolId, string toolName, string toolSpec,
            float stdValue, float deviationPositive, float deviationNegative, string chkUser)
        {
            return DalToolCheckRecord.AddToolCheckRecord(toolId, toolName, toolSpec, stdValue, deviationPositive, deviationNegative, chkUser);
        }

        public static int SaveChkValue(int recordId, int storePosition, float chkValue)
        {
            if (recordId < 0) return -100;
            return DalToolCheckRecord.SaveChkValue(recordId, storePosition, chkValue);
        }

        public static int SetChkUserMutal(int recordId, string userName)
        {
            if (recordId < 0) return -100;
            return DalToolCheckRecord.SetChkUserMutal(recordId, userName);
        }

        public static int AddComment(int recordId, string comment)
        {
            return DalToolCheckRecord.AddComment(recordId, comment);
        }

        public static int ResetRecord()
        {
            return DalToolCheckRecord.ResetRecord();
        }
    }
}
