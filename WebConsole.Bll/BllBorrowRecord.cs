using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllBorrowRecord
    {
        public static int GetBorrowRecordCount(DateTime timeStart, DateTime timeEnd, string workerId, string cabinetName, int toolStatus)
        {
            return DalBorrowRecord.GetBorrowRecordCount(timeStart, timeEnd, workerId, cabinetName, toolStatus);
        }

        public static int GetBorrowRecordCount(string toolId)
        {
            return DalBorrowRecord.GetBorrowRecordCount(toolId);
        }

        public static BorrowRecord GetLastBorrowRecord(string toolId)
        {
            return DalBorrowRecord.GetLastBorrowRecord(toolId);
        }

        public static IList<BorrowRecord> SearchBorrowRecord(DateTime timeStart, DateTime timeEnd, string workerId, string cabinetName, int toolStatus, int dataStart, int dataCount)
        {
            return DalBorrowRecord.SearchBorrowRecord(timeStart, timeEnd, workerId, cabinetName, toolStatus, dataStart, dataCount);
        }

        public static IList<BorrowRecord> SearchBorrowRecord(DateTime timeStart, DateTime timeEnd)
        {
            return DalBorrowRecord.SearchBorrowRecord(timeStart, timeEnd);
        }

        public static IList<BorrowRecord> SearchBorrowRecord(string toolId, int dataStart, int dataCount)
        {
            return DalBorrowRecord.SearchBorrowRecord(toolId, dataStart, dataCount);
        }

        public static IList<BorrowRecord> SearchBorrowRecordsByWorkId(DateTime timeStart, DateTime timeEnd,
            string cabinetName, string workerId, int dataStart, int dataCount)
        {
            return DalBorrowRecord.SearchBorrowRecordsByWorkId(timeStart, timeEnd, cabinetName, workerId, dataStart, dataCount);
        }

        public static bool Borrowed(string workerId)
        {
            return DalBorrowRecord.Borrowed(workerId);
        }
    }
}
