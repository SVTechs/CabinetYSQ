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
    public class BllWrenchInfo
    {
        public static IList<WrenchInfo> SearchWrenchInfo(string wrenchCode, string wrenchSpec, string dataOwner, int dataStart, int dataCount, out Exception exception)
        {
            return DalWrenchInfo.SearchWrenchInfo(wrenchCode, wrenchSpec, dataOwner, dataStart, dataCount, out exception);
        }

        public static int GetWrenchInfoCount(out Exception exception)
        {
            return DalWrenchInfo.GetWrenchInfoCount(out exception);
        }

        public static int GetWrenchInfoCount(string wrenchCode, string wrenchSpec, string dataOwner, out Exception ex)
        {
            return DalWrenchInfo.GetWrenchInfoCount(wrenchCode, wrenchSpec, dataOwner, out ex);
        }

        public static WrenchInfo GetWrenchInfo(string id, out Exception exception)
        {
            return DalWrenchInfo.GetWrenchInfo(id, out exception);
        }

        public static int AddWrenchInfo(string wrenchName, string wrenchCode, string wrenchSpec, string standardRange, int wrenchPosition, int checkInterval, string checkIntervalType, string dataOwner, out Exception exception)
        {
            return DalWrenchInfo.AddWrenchInfo(wrenchName, wrenchCode, wrenchSpec, standardRange, wrenchPosition, checkInterval, checkIntervalType, dataOwner, out exception);
        }

        public static int ModifyWrenchInfo(string id, string wrenchName, string wrenchCode, string wrenchSpec, string standardRange, int wrenchPosition, int checkInterval, string checkIntervalType, string dataOwner, out Exception exception)
        {
            return DalWrenchInfo.ModifyWrenchInfo(id, wrenchName, wrenchCode, wrenchSpec, standardRange, wrenchPosition, checkInterval, checkIntervalType, dataOwner, out exception);
        }

        public static int SaveWrenchInfo(WrenchInfo wrenchInfo, out Exception exception)
        {
            return DalWrenchInfo.SaveWrenchInfo(wrenchInfo, out exception);
        }

        public static int UpdateWrenchInfo(WrenchInfo wrenchInfo, out Exception exception)
        {
            return DalWrenchInfo.UpdateWrenchInfo(wrenchInfo, out exception);
        }

        public static int DeleteWrenchInfo(string id, out Exception exception)
        {
            return DalWrenchInfo.DeleteWrenchInfo(id, out exception);
        }


        public static IList<WrenchInfo> SearchWrenchInfoFile(string pdfFile, out Exception exception)
        {
            return DalWrenchInfo.SearchWrenchInfoFile(pdfFile, out exception);
        }

    }
}
