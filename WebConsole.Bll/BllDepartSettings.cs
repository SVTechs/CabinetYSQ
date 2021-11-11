using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllDepartSettings
    {
        public static IList SearchDepartSettings()
        {
            return DalDepartSettings.SearchDepartSettings();
        }

        public static DepartSettings GetDepartSettings(string id)
        {
            return DalDepartSettings.GetDepartSettings(id);
        }

        public static IList<DepartSettings> GetUserDepartSettings(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return DalDepartSettings.GetUserDepartSettings(userId);
        }

        public static int SetUserDepart(string userId, string departId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return -100;
            }
            departId = (departId ?? "").Replace(" ", "").TrimEnd(',');
            string[] departList = departId.Split(',');
            return DalDepartSettings.SetUserDepart(userId, departList);
        }

        public static int DeleteUserDepartSettings(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return -100;
            return DalDepartSettings.DeleteUserDepartSettings(userId);
        }
    }
}
