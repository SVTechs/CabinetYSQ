using CabinetMgr.Dal;
using Domain.Main.Domain;

namespace CabinetMgr.Bll
{
    public class BllReturnRecord
    {
        public static int AddReturnRecord(int toolPosition, string workerId, string workerName)
        {
            return DalReturnRecord.AddReturnRecord(toolPosition, workerId, workerName);
        }

        public static int DeleteAll()
        {
            return DalReturnRecord.DeleteAll();
        }

        public static ReturnRecord GetReturnRecord(string borrowRecordId)
        {
            return DalReturnRecord.GetReturnRecord(borrowRecordId);
        }
    }
}
