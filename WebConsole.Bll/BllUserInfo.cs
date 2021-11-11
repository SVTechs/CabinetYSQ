using System.Collections.Generic;
using Domain.ServerMain.Domain;
//using Domain.Qcdevice.Domain;
using Utilities.Encryption;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllUserInfo
    {
        public static int GetUserCount(string userName, string realName)
        {
            return DalUserInfo.GetUserCount(userName, realName);
        }

        public static IList<UserInfo> SearchUser(string userName, string realName, int dataStart, int dataCount)
        {
            return DalUserInfo.SearchUser(userName, realName, dataStart, dataCount);
        }

        public static IList<UserInfo> SearchUserByOrg(string orgId)
        {
            return DalUserInfo.SearchUserByOrg(orgId);
        }

        public static IList<object> GetOrgs()
        {
            return DalUserInfo.GetOrgs();
        }

        public static UserInfo GetUserInfo(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return DalUserInfo.GetUserInfo(userId);
        }

        public static UserInfo GetUserInfo(string userName, string userPwd)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
            {
                return null;
            }
            string pwdHash = Md5Encode.Encode(userPwd, true, "");
            return DalUserInfo.GetUserInfo(userName, pwdHash);
        }

        public static string AddUserInfo(string userName, string userPwd, string realName, string userTel)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
            {
                return null;
            }
            string pwdHash = Md5Encode.Encode(userPwd, true, "");
            return DalUserInfo.AddUserInfo(userName, pwdHash, realName, userTel);
        }

        public static int ModifyUserInfo(string userId, string userName, string userPwd, string realName, 
            string userTel)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return -100;
            }
            if (!string.IsNullOrEmpty(userPwd))
            {
                userPwd = Md5Encode.Encode(userPwd, true, "");
            }
            return DalUserInfo.ModifyUserInfo(userId, userName, userPwd, realName, userTel);
        }

        public static int ModifyUserInfo(string userId, string userPwd, string userTel)
        {
            if (!string.IsNullOrEmpty(userPwd))
            {
                userPwd = Md5Encode.Encode(userPwd, true, "");
            }
            return DalUserInfo.ModifyUserInfo(userId, userPwd, userTel);
        }

        public static int DeleteUserInfo(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return -100;
            //清除用户角色配置(非重要)
            DalRoleSettings.DeleteUserRoleSettings(userId);
            //清除用户权限配置(非重要)
            DalPermissionSettings.DeleteOwnerPermissionSettings(userId);
            //清除用户部门配置(非重要)
            DalDepartSettings.DeleteUserDepartSettings(userId);
            //删除用户
            return DalUserInfo.DeleteUserInfo(userId);
        }
    }
}
