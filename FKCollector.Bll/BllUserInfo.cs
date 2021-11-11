using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ServerMain.Domain;
using FKCollector.Dal;
using Utilities.DbHelper;

namespace FKCollector.Bll
{
    public class BllUserInfo
    {
        public static int SearchUserInfo(string fullName, string orgId, out IList<UserInfo> userList)
        {
            return DalUserInfo.SearchUserInfo(fullName, orgId, out userList);
        }

        public static void FixTemplateId()
        {
            IList<UserInfo> userList = DalUserInfo.SearchUserInfoWithoutTemplateId();
            if (SqlDataHelper.IsDataValid(userList))
            {
                int baseId = 3000;
                for (int i = 0; i < userList.Count; i++)
                {
                    userList[i].TemplateUserId = baseId++;
                    DalUserInfo.UpdateUserInfo(userList[i]);
                }
            }
        }

        public static int GetUserInfoCount()
        {
            return DalUserInfo.GetUserInfoCount();
        }

        public static int GetUserInfo(string id, out UserInfo userInfo)
        {
            return DalUserInfo.GetUserInfo(id, out userInfo);
        }

        public static UserInfo GetUserInfoByUserName(string userName)
        {
            return DalUserInfo.GetUserInfoByUserName(userName);
        }

        public static int AddUserInfo(
            string userName,
            string password,
            string fullName,
            string sex,
            int age,
            string tel,
            string adress,
            string email,
            int userState,
            string createUser,
            string updateUser,
            string orgId,
            string cardNum,
            string empName,
            byte[] leftTemplate,
            byte[] rightTemplate,
            string newLeftTemplate,
            string newRightTemplate)
        {
            return DalUserInfo.AddUserInfo(userName, password, fullName, sex, age, tel, adress, email, userState, createUser, updateUser, orgId, cardNum, empName, leftTemplate, rightTemplate, newLeftTemplate, newRightTemplate);
        }

        public static int ModifyUserInfo(string id, string fullName, string sex, int age,
            string tel, string orgId, string empName)
        {
            return DalUserInfo.ModifyUserInfo(id, fullName, sex, age, tel, orgId, empName);
        }

        public static int ModifyUserFeature(string id, int type, string feature)
        {
            return DalUserInfo.ModifyUserFeature(id, type, feature);
        }

        public static int BatchUpdateUserInfo(IList<UserInfo> recordList)
        {
            if (!SqlDataHelper.IsDataValid(recordList)) return -100;
            return DalUserInfo.BatchUpdateUserInfo(recordList);
        }

        public static int UpdateUserInfo(UserInfo itemRecord)
        {
            if (itemRecord == null) return -100;
            return DalUserInfo.UpdateUserInfo(itemRecord);
        }

        public static int DeleteUserInfo(string id)
        {
            return DalUserInfo.DeleteUserInfo(id);
        }
    }
}
