using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Utilities.DbHelper;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllCabinetInfo
    {

        public static IList<CabinetInfo> SearchCabinetInfo()
        {
            return DalCabinetInfo.SearchCabinetInfo();
        }

        public static int GetCabinetInfoCount()
        {
            return DalCabinetInfo.GetCabinetInfoCount();
        }

        public static string GetCabinetAlias(string cabinetName)
        {
            return DalCabinetInfo.GetCabinetAlias(cabinetName);
        }

        public static CabinetInfo GetCabinetInfo(string id)
        {
            return DalCabinetInfo.GetCabinetInfo(id);
        }

        public static int AddCabinetInfo(string cabinetName, string cabinetAlias, int cabinetOrder)
        {
            return DalCabinetInfo.AddCabinetInfo(cabinetName, cabinetAlias, cabinetOrder);
        }

        public static int ModifyCabinetInfo(string id, string cabinetName, string cabinetAlias, int cabinetOrder)
        {
            return DalCabinetInfo.ModifyCabinetInfo(id, cabinetName, cabinetAlias, cabinetOrder);
        }

        public static int BatchUpdateCabinetInfo(IList<CabinetInfo> recordList)
        {
            if (!SqlDataHelper.IsDataValid(recordList)) return -100;
            return DalCabinetInfo.BatchUpdateCabinetInfo(recordList);
        }

        public static int UpdateCabinetInfo(CabinetInfo itemRecord)
        {
            if (itemRecord == null) return -100;
            return DalCabinetInfo.UpdateCabinetInfo(itemRecord);
        }

        public static int DeleteCabinetInfo(string id)
        {
            return DalCabinetInfo.DeleteCabinetInfo(id);
        }
    }
}
