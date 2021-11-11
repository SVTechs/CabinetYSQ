using System.Collections.Generic;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllDepartInfo
    {
        public static int GetDepartCount()
        {
            return DalDepartInfo.GetDepartCount();
        }

        public static IList<DepartInfo> SearchDepartInfo()
        {
            return DalDepartInfo.SearchDepartInfo();
        }

        public static DepartInfo GetDepartInfo(string id)
        {
            return DalDepartInfo.GetDepartInfo(id);
        }

        public static int AddDepartInfo(string departName, int departLevel, string departParent,
            int departOrder, string departDesp)
        {
            return DalDepartInfo.AddDepartInfo(departName, departLevel, departParent, departOrder, departDesp);
        }

        public static int ModifyDepartInfo(string departId, string departName, int departOrder, string departDesp)
        {
            if (string.IsNullOrEmpty(departId) || string.IsNullOrEmpty(departName))
            {
                return -100;
            }
            return DalDepartInfo.ModifyDepartInfo(departId, departName, departOrder, departDesp);
        }

        public static int DeleteDepartInfo(string id)
        { 
            return DalDepartInfo.DeleteDepartInfo(id);
        }
    }
}
