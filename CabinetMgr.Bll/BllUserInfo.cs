using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using CabinetMgr.Dal;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using Utilities.DbHelper;

namespace CabinetMgr.Bll
{
    public class BllUserInfo
    {
        public static UserInfo GetUser(int templateId)
        {
            return DalUserInfo.GetUser(templateId);
        }

        public static UserInfo GetUser(string userName)
        {
            return DalUserInfo.GetUser(userName);
        }

        public static IList SearchUserByGroup(string orgId)
        {
            return DalUserInfo.SearchUserByGroup(orgId);
        }

        public static IList<UserInfo> SearchUserByGroup(string[] orgId)
        {
            return DalUserInfo.SearchUserByGroup(orgId);
        }

        public static IList<UserInfo> SearchUserNotRegistered()
        {
            return DalUserInfo.SearchUserNotRegistered();
        }

        public static int SetAsRegistered(IList<UserInfo> userList)
        {
            return DalUserInfo.SetAsRegistered(userList);
        }

        /*
        public static Domain.Qcshkf.Domain.UserInfo GetServerUser(int templateId)
        {
            return DalUserInfo.GetServerUser(templateId);
        }*/

        public static int RegisterFingerPrint(DataSet newInfo, string allowedGroup)
        {
            if (!SqlDataHelper.IsDataValid(newInfo) || string.IsNullOrEmpty(allowedGroup))
            {
                return 0;
            }
            try
            {
                Hashtable groupHt = new Hashtable();
                string[] groupArray = allowedGroup.Split(',');
                for (int i = 0; i < groupArray.Length; i++)
                {
                    groupHt[groupArray[i]] = 1;
                }
                DataTable dt = newInfo.Tables[0];
                List<FpZkIFace.IfaceUser> ifList = new List<FpZkIFace.IfaceUser>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr["OrgId"] != null && groupHt[dr["OrgId"].ToString()] != null)
                    {
                        //指定班组成员，注册指纹
                        FpZkIFace.IfaceUser ifUser = new FpZkIFace.IfaceUser(dr["FullName"].ToString(),
                            dr["TemplateUserId"].ToString(), dr["LeftTemplateV10"].ToString(),
                            dr["RightTemplateV10"].ToString());
                        ifList.Add(ifUser);
                    }
                }
                return FpZkIFace.UploadUserInfo(ifList);

            }
            catch (Exception)
            {
                return -100;
            }
        }
    }
}

