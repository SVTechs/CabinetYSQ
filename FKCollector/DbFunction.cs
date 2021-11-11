using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NLog;
using Utilities.DbHelper;
using Utilities.Encryption;

namespace FKCollector
{
    public static class DbFunction
    {
        private static string ConnStr = ConfigurationManager.ConnectionStrings["cabinet"].ConnectionString;
        private static string OrgId = ConfigurationManager.AppSettings["orgId"];
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int GetCount()
        {
            try
            {
                object rslt = SqlHelper.ExecuteScalar(ConnStr, CommandType.Text,
                    "select count(1) from UserInfo", null);
                if (rslt != null)
                {
                    return int.Parse(rslt.ToString());
                }
                return -100;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -200;
            }

        }

        //public static int AddUserInfo(string userName, string fullName, string orgId)
        //{
        //    try
        //    {
        //        SqlParameter[] para = new SqlParameter[]
        //        {
        //            new SqlParameter("@ID", Guid.NewGuid()),
        //            new SqlParameter("@userName", userName),
        //            new SqlParameter("@fullName", fullName),
        //            new SqlParameter("@passWord", Md5Encode.Encode("123456", true, "")),
        //            new SqlParameter("@orgId", orgId),
        //            new SqlParameter("@FpRegistered", 0),
        //        };
        //        int rslt = SqlHelper.ExecuteNonQuery(ConnStr, CommandType.Text,
        //            "insert into UserInfo (ID, username, fullname, passWord, orgId, FpRegistered) " +
        //            "values (@ID, @userName, @fullName, @passWord, @orgId, @FpRegistered)", para);
        //        return rslt;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error(e);
        //        return -200;
        //    }

        //}

        public static UserInfo SearchUserInfo(string userName)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@userName", userName),
                };
                DataTable dt = SqlHelper.GetDataTable(ConnStr, CommandType.Text,
                    "select * from UserInfo where userName = @userName", para);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    byte[] lefttemplate = dr["LEFTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["LEFTTEMPLATE"];
                    byte[] righttemplate = dr["RIGHTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["RIGHTTEMPLATE"];
                    byte[] facetemplate = dr["FACETEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FACETEMPLATE"];
                    //string faceTemplateV10 = dr["FaceTemplateV10"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FaceTemplateV10"];
                    UserInfo ui = new UserInfo(dr["UserName"]?.ToString(), dr["FullName"]?.ToString(), dr["OrgId"]?.ToString()
                        , lefttemplate, righttemplate, facetemplate
                        , dr["LeftTemplateV10"]?.ToString(), dr["RightTemplateV10"]?.ToString(), dr["FaceTemplateV10"]?.ToString(), int.Parse(dr["FpRegistered"]?.ToString())
                        , dr["EnrollId"]?.ToString());
                    return ui;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
            return null;
        }

        public static UserInfo SearchUserInfo(string enrollId, string type)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@enrollId",enrollId),
                };
                DataTable dt = SqlHelper.GetDataTable(ConnStr, CommandType.Text,
                    "select * from UserInfo where enrollId = @enrollId", para);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    byte[] lefttemplate = dr["LEFTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["LEFTTEMPLATE"];
                    byte[] righttemplate = dr["RIGHTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["RIGHTTEMPLATE"];
                    byte[] facetemplate = dr["FACETEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FACETEMPLATE"];
                    //byte[] faceTemplateV10 = dr["FaceTemplateV10"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FaceTemplateV10"];
                    UserInfo ui = new UserInfo(dr["UserName"]?.ToString(), dr["FullName"]?.ToString(), dr["OrgId"]?.ToString()
                        , lefttemplate, righttemplate, facetemplate
                        , dr["LeftTemplateV10"]?.ToString(), dr["RightTemplateV10"]?.ToString(), dr["FaceTemplateV10"]?.ToString(), int.Parse(dr["FpRegistered"]?.ToString())
                        , dr["EnrollId"]?.ToString());
                    return ui;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
            return null;
        }

        public static UserInfo SearchUserInfo(UInt32 enrollId)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@enrollId", enrollId.ToString()),
                };
                DataTable dt = SqlHelper.GetDataTable(ConnStr, CommandType.Text,
                    "select * from UserInfo where enrollId = @enrollId", para);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    byte[] lefttemplate = dr["LEFTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["LEFTTEMPLATE"];
                    byte[] righttemplate = dr["RIGHTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["RIGHTTEMPLATE"];
                    byte[] facetemplate = dr["FACETEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FACETEMPLATE"];
                    //byte[] faceTemplateV10 = dr["FaceTemplateV10"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FaceTemplateV10"];
                    UserInfo ui = new UserInfo(dr["UserName"]?.ToString(), dr["FullName"]?.ToString(), dr["OrgId"]?.ToString()
                        , lefttemplate, righttemplate, facetemplate
                        , dr["LeftTemplateV10"]?.ToString(), dr["RightTemplateV10"]?.ToString(), dr["FaceTemplateV10"]?.ToString(), int.Parse(dr["FpRegistered"]?.ToString())
                        , dr["EnrollId"]?.ToString());
                    return ui;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
            return null;
        }

        public static int ModifyUserInfo(string userName, string orgId)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@userName", userName),
                    new SqlParameter("@orgId", orgId),
                };
                int rslt = SqlHelper.ExecuteNonQuery(ConnStr, CommandType.Text,
                    "update UserInfo set OrgId = @orgId where userName = @userName", para);
                return rslt;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -200;
            }
        }

        public static int ModifyFeature(UserInfo ui)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@enrollId", SqlDbType.Char),
                    new SqlParameter("@lEFTTEMPLATE", SqlDbType.Image),
                    new SqlParameter("@rIGHTTEMPLATE", SqlDbType.Image),
                    new SqlParameter("@fACETEMPLATE", SqlDbType.Image),
                    new SqlParameter("@leftTemplateV10", SqlDbType.Char),
                    new SqlParameter("@rightTemplateV10", SqlDbType.Char),
                    new SqlParameter("@faceTemplateV10", SqlDbType.Char),
                };
                para[0].Value = ui.EnrollId;
                para[1].Value = ui.LEFTTEMPLATE;
                para[2].Value = ui.RIGHTTEMPLATE;
                para[3].Value = ui.FACETEMPLATE;
                para[4].Value = ui.LeftTemplateV10;
                para[5].Value = ui.RightTemplateV10;
                para[6].Value = ui.FaceTemplateV10;
                int rslt = SqlHelper.ExecuteNonQuery(ConnStr, CommandType.Text,
                    "update UserInfo set lEFTTEMPLATE = @lEFTTEMPLATE, rIGHTTEMPLATE = @rIGHTTEMPLATE" +
                    ", fACETEMPLATE = @fACETEMPLATE, leftTemplateV10 = @leftTemplateV10" +
                    ", rightTemplateV10 = @rightTemplateV10, faceTemplateV10 = @faceTemplateV10, FpRegistered = 0 where enrollId = @enrollId", para);
                return rslt;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -200;
            }
        }

        public static int SetFailed(List<UInt32> lstFailed)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@enrollId", SqlDbType.Char),
                };
                int rslt = 0;
                for (int i = 0; i < lstFailed.Count; i++)
                {
                    para[0].SqlValue = int.Parse(lstFailed[i].ToString());
                    rslt += SqlHelper.ExecuteNonQuery(ConnStr, CommandType.Text,
                        "update UserInfo set FpRegistered = 0 where enrollId = @enrollId", para);
                }
                return rslt;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -200;
            }
        }

        public static int SetSucceed(List<UInt32> lstSucceed)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@enrollId", SqlDbType.Char),
                };
                int rslt = 0;
                for (int i = 0; i < lstSucceed.Count; i++)
                {
                    para[0].SqlValue = int.Parse(lstSucceed[i].ToString());
                    rslt += SqlHelper.ExecuteNonQuery(ConnStr, CommandType.Text,
                        "update UserInfo set FpRegistered = 1 where enrollId = @enrollId", para);
                }
                return rslt;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return -200;
            }
        }

        public static List<UserInfo> GetAllUser()
        {
            try
            {

                DataTable dt = SqlHelper.GetDataTable(ConnStr, CommandType.Text,
                    "select * from UserInfo", null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<UserInfo> lst = new List<UserInfo>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        byte[] lefttemplate = dr["LEFTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["LEFTTEMPLATE"];
                        byte[] righttemplate = dr["RIGHTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["RIGHTTEMPLATE"];
                        byte[] facetemplate = dr["FACETEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FACETEMPLATE"];
                        //byte[] faceTemplateV10 = dr["FaceTemplateV10"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FaceTemplateV10"];
                        UserInfo ui = new UserInfo(dr["UserName"]?.ToString(), dr["FullName"]?.ToString(), dr["OrgId"]?.ToString()
                            , lefttemplate, righttemplate, facetemplate
                            , dr["LeftTemplateV10"]?.ToString(), dr["RightTemplateV10"]?.ToString(), dr["FaceTemplateV10"]?.ToString(), int.Parse(dr["FpRegistered"]?.ToString())
                            , dr["EnrollId"]?.ToString());
                        lst.Add(ui);
                    }
                    return lst;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
            return null;
        }

        public static Dictionary<string, string> GetAllOrg()
        {
            try
            {

                DataTable dt = SqlHelper.GetDataTable(ConnStr, CommandType.Text,
                    "select distinct orgid,orgname from UserInfo", null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Dictionary<string, string> dicOrg = new Dictionary<string, string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dicOrg.ContainsKey(dr["orgid"]?.ToString())) continue;
                        dicOrg.Add(dr["orgid"]?.ToString(), dr["orgname"]?.ToString());
                    }
                    return dicOrg;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
            return null;
        }

        public static DataSet GetAllUserDataSet()
        {
            try
            {
                string cmd = "select * from UserInfo where 1 = 1 ";
                if (!string.IsNullOrEmpty(OrgId))
                {
                    cmd += " and orgid in (";
                    string[] ary = OrgId.Split(',');
                    for (int i = 0; i < ary.Length; i++)
                    {
                        cmd += "'" + ary[i] + "',";
                    }
                    cmd = cmd.Substring(0, cmd.Length - 1);
                    cmd += ")";
                }
                DataSet ds = SqlHelper.GetDataSet(ConnStr, CommandType.Text, cmd, null);
                if (SqlDataHelper.IsDataValid(ds))
                {
                    return ds;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
            return null;
        }


        public static List<UserInfo> UserByOrgId()
        {
            try
            {
                string cmd = "select * from UserInfo where ";
                if (!string.IsNullOrEmpty(OrgId))
                {
                    cmd += " orgid in (";
                    string[] ary = OrgId.Split(',');
                    for (int i = 0; i < ary.Length; i++)
                    {
                        cmd += "'" + ary[i] + "',";
                    }
                    cmd = cmd.Substring(0, cmd.Length - 1);
                    cmd += ")";
                }
                DataTable dt = SqlHelper.GetDataTable(ConnStr, CommandType.Text, cmd, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<UserInfo> lst = new List<UserInfo>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        byte[] lefttemplate = dr["LEFTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["LEFTTEMPLATE"];
                        byte[] righttemplate = dr["RIGHTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["RIGHTTEMPLATE"];
                        byte[] facetemplate = dr["FACETEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FACETEMPLATE"];
                        //byte[] faceTemplateV10 = dr["FaceTemplateV10"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FaceTemplateV10"];
                        UserInfo ui = new UserInfo(dr["UserName"]?.ToString(), dr["FullName"]?.ToString(), dr["OrgId"]?.ToString()
                            , lefttemplate, righttemplate, facetemplate
                            , dr["LeftTemplateV10"]?.ToString(), dr["RightTemplateV10"]?.ToString(), dr["FaceTemplateV10"]?.ToString(), int.Parse(dr["FpRegistered"]?.ToString())
                            , dr["EnrollId"]?.ToString());
                        lst.Add(ui);
                    }
                    return lst;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
            return null;
        }

    }


    public class UserInfo
    {
        public string UserName;
        public string FullName;
        public string OrgId;
        public byte[] LEFTTEMPLATE;
        public byte[] RIGHTTEMPLATE;
        public byte[] FACETEMPLATE;
        public string LeftTemplateV10;
        public string RightTemplateV10;
        public string FaceTemplateV10;
        public int FpRegistered;
        public string EnrollId;

        public UserInfo(string userName, string fullName, string orgId, byte[] lEFTTEMPLATE, byte[] rIGHTTEMPLATE, byte[] fACETEMPLATE
            , string leftTemplateV10, string rightTemplateV10, string faceTemplateV10, int fpRegistered, string enrollId)
        {
            UserName = userName;
            FullName = fullName;
            OrgId = orgId;
            LEFTTEMPLATE = lEFTTEMPLATE;
            RIGHTTEMPLATE = rIGHTTEMPLATE;
            FACETEMPLATE = fACETEMPLATE;
            LeftTemplateV10 = leftTemplateV10;
            RightTemplateV10 = rightTemplateV10;
            FaceTemplateV10 = faceTemplateV10;
            FpRegistered = fpRegistered;
            EnrollId = enrollId;
        }
    }
}
