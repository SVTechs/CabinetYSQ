using System;
using System.Collections;
using CabinetMgr.Dal;
using Domain.Main.Domain;

namespace CabinetMgr.Bll
{
    public class BllBorrowRecord
    {
        public static int AddBorrowRecord(string toolId, string toolName, int toolPosition, string workerId, string workerName, int hardwareId)
        {
            return DalBorrowRecord.AddBorrowRecord(toolId, toolName, toolPosition, workerId, workerName, hardwareId);
        }

        public static BorrowRecord GetBorrowRecord(int toolPosition, bool showReturned = false)
        {
            return DalBorrowRecord.GetBorrowRecord(toolPosition, showReturned);
        }

        public static BorrowRecord GetLastUnReturnedBorrowRecord(int toolPosition)
        {
            return DalBorrowRecord.GetLastUnReturnedBorrowRecord(toolPosition);
        }

        public static IList SearchBorrowRecord(DateTime timeStart, DateTime timeEnd)
        {
            return DalBorrowRecord.SearchBorrowRecord(timeStart, timeEnd);
        }

        public static int SetAsConfirmed(string borrowId)
        {
            if (string.IsNullOrEmpty(borrowId)) return -100;
            return DalBorrowRecord.SetAsConfirmed(borrowId);
        }

        public static int DeleteAll()
        {
            return DalBorrowRecord.DeleteAll();
        }

        public static int AddExpireComment(string borrowId, string comment)
        {
            if (string.IsNullOrEmpty(borrowId) || string.IsNullOrEmpty(comment)) return -100;
            return DalBorrowRecord.AddExpireComment(borrowId, comment);
        }
    }
}
