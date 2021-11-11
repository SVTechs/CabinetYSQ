using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllWrenchCheckRecord
    {
        public static IList<WrenchCheckRecord> SearchWrenchCheckRecord(string wrenchId, int dataStart, int dataCount, out Exception exception)
        {
            return DalWrenchCheckRecord.SearchWrenchCheckRecord(wrenchId, dataStart, dataCount, out exception);
        }

        public static int GetWrenchCheckRecordCount(string wrenchId, out Exception exception)
        {
            return DalWrenchCheckRecord.GetWrenchCheckRecordCount(wrenchId, out exception);
        }

        public static WrenchCheckRecord GetWrenchCheckRecord(string id, out Exception exception)
        {
            return DalWrenchCheckRecord.GetWrenchCheckRecord(id, out exception);
        }
        public static int AddWrenchCheckRecord(string wrenchId, string wrenchName, int wrenchPosition, string workerId, string workerName, DateTime eventTime, string status, string dataOwner, string pdfFile, out Exception exception)
        {
            return DalWrenchCheckRecord.AddWrenchCheckRecord(wrenchId, wrenchName, wrenchPosition, workerId, workerName, eventTime, status, dataOwner, pdfFile, out exception);
        }

        public static int ModifyWrenchCheckRecord(string id, string wrenchId, string wrenchName, int wrenchPosition, string workerId, string workerName, DateTime eventTime, string status, string dataOwner, string pdfFile, out Exception exception)
        {
            return DalWrenchCheckRecord.ModifyWrenchCheckRecord(id, wrenchId, wrenchName, wrenchPosition, workerId, workerName, eventTime, status, dataOwner, pdfFile, out exception);
        }

        public static int SaveWrenchCheckRecord(WrenchCheckRecord wrenchCheckRecord, out Exception exception)
        {
            return DalWrenchCheckRecord.SaveWrenchCheckRecord(wrenchCheckRecord, out exception);
        }

        public static int UpdateWrenchCheckRecord(WrenchCheckRecord wrenchCheckRecord, out Exception exception)
        {
            return DalWrenchCheckRecord.UpdateWrenchCheckRecord(wrenchCheckRecord, out exception);
        }

        public static int DeleteWrenchCheckRecord(string id, out Exception exception)
        {
            return DalWrenchCheckRecord.DeleteWrenchCheckRecord(id, out exception);
        }

    }
}
