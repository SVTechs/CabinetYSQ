using System;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllReturnRecord
    {

        public static int DeleteAll()
        {
            return DalReturnRecord.DeleteAll();
        }

        public static ReturnRecord GetReturnRecord(string borrowRecordId)
        {
            return DalReturnRecord.GetReturnRecord(borrowRecordId);
        }

        public static IList<ReturnRecord> SearchReturnRecord(DateTime timeStart, DateTime timeEnd)
        {
            return DalReturnRecord.SearchReturnRecord(timeStart, timeEnd);
        }
    }
}
