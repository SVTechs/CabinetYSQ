using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Utilities.DbHelper;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllPermissionSettings
    {

        public static IList<PermissionSettings> SearchPermissionSettings()
        {
            return DalPermissionSettings.SearchPermissionSettings();
        }

        public static IList GetFullPermissionSettings(string userId, List<string>roleList)
        {
            IList permList = new ArrayList();
            if (!string.IsNullOrEmpty(userId))
            {
                IList<PermissionSettings> userPerm = DalPermissionSettings.GetOwnerPermissionSettings(userId);
                if (SqlDataHelper.IsDataValid(userPerm))
                {
                    foreach (var permObj in userPerm)
                    {
                        permList.Add(permObj);
                    }
                }
            }
            if (roleList != null && roleList.Count > 0)
            {
                for (int i = 0; i < roleList.Count; i++)
                {
                    IList<PermissionSettings> userPerm = DalPermissionSettings.GetOwnerPermissionSettings(roleList[i]);
                    if (SqlDataHelper.IsDataValid(userPerm))
                    {
                        foreach (var permObj in userPerm)
                        {
                            permList.Add(permObj);
                        }
                    }
                }
            }
            return permList;
        }

        public static IList<PermissionSettings> GetOwnerPermissionSettings(string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId)) return null;
            return DalPermissionSettings.GetOwnerPermissionSettings(ownerId);
        }

        public static int SetOwnerPermission(string ownerType, string ownerId, string permId)
        {
            if (string.IsNullOrEmpty(ownerId)) return -100;
            permId = (permId ?? "").Replace(" ", "").TrimEnd(',');
            string[] permList = permId.Split(',');
            return DalPermissionSettings.SetOwnerPermission(ownerType, ownerId, permList);
        }

        public static int DeletePermissionSettings(string id)
        {
            if (string.IsNullOrEmpty(id)) return -100;
            return DalPermissionSettings.DeletePermissionSettings(id);
        }
    }
}
