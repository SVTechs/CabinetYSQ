using System.Collections.Generic;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllRoleSettings
    {

        public static IList<RoleSettings> SearchRoleSettings()
        {
            return DalRoleSettings.SearchRoleSettings();
        }

        public static IList<RoleSettings> GetUserRoleSettings(string userId)
        {
            return DalRoleSettings.GetUserRoleSettings(userId);
        }

        public static RoleSettings GetRoleSettings(string id)
        {
            return DalRoleSettings.GetRoleSettings(id);
        }

        public static int SetUserRole(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return -100;
            }
            roleId = (roleId ?? "").Replace(" ", "").TrimEnd(',');
            string[] roleList = roleId.Split(',');
            return DalRoleSettings.SetUserRole(userId, roleList);
        }

        public static int DeleteUserSettings(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return -100;
            return DalRoleSettings.DeleteUserRoleSettings(userId);
        }

        public static int DeleteRoleSettings(string id)
        {
            return DalRoleSettings.DeleteRoleSettings(id);
        }
    }
}
