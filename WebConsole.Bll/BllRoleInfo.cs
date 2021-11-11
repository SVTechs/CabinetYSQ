using System.Collections.Generic;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllRoleInfo
    {
        public static int GetRoleCount()
        {
            return DalRoleInfo.GetRoleCount();
        }

        public static IList<RoleInfo> SearchRoleInfo()
        {
            return DalRoleInfo.SearchRoleInfo();
        }

        public static RoleInfo GetRoleInfo(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return null;
            return DalRoleInfo.GetRoleInfo(roleId);
        }

        public static int AddRoleInfo(string roleName, int roleLevel, string roleParent, int roleOrder, string roleDesp)
        {
            if (string.IsNullOrEmpty(roleName)) return -100;
            return DalRoleInfo.AddRoleInfo(roleName, roleLevel, roleParent, roleOrder, roleDesp);
        }

        public static int ModifyRoleInfo(string roleId, string roleName, int roleOrder, string roleDesp)
        {
            if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(roleName)) return -100;
            return DalRoleInfo.ModifyRoleInfo(roleId, roleName, roleOrder, roleDesp);
        }

        public static int DeleteRoleInfo(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return -100;
            //清除角色权限配置(非重要)
            DalPermissionSettings.DeleteOwnerPermissionSettings(roleId);
            //删除角色
            return DalRoleInfo.DeleteRoleInfo(roleId);
        }
    }
}
