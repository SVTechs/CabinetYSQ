using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using Domain.ServerMain.Domain;
using NLog;
using Utilities.DbHelper;

namespace FeatureService
{
    /// <summary>
    /// FeatureUpdateService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://wyc.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class FeatureUpdateService : System.Web.Services.WebService
    {
        //private static string connStr = ConfigurationManager.ConnectionStrings["cabinetServer"].ConnectionString;
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        //[WebMethod]
        //public int UpdateFeature(List<UserInfo> lstUi, string deviceType = "")
        //{
        //    if (lstUi.Count == 0)
        //    {
        //        return -100;
        //    }
        //    SqlParameter[] para;
        //    int r = 0;
        //    switch (deviceType)
        //    {

        //        default:
        //            para = new SqlParameter[]
        //            {
        //                new SqlParameter("@enrollId", SqlDbType.Int),
        //                new SqlParameter("@LeftTemplateV10", SqlDbType.Char),
        //                new SqlParameter("@RightTemplateV10", SqlDbType.Char),
        //                new SqlParameter("@FaceTemplateV10", SqlDbType.Image),
        //                new SqlParameter("@Updatetime", DateTime.Now),
        //            };
        //            try
        //            {
        //                SqlConnection sqlc = new SqlConnection(connStr);
        //                sqlc.Open();
        //                SqlTransaction sqlt = sqlc.BeginTransaction();
        //                for (int i = 0; i < lstUi.Count; i++)
        //                {
        //                    UserInfo ui = lstUi[i];
        //                    string cmd =
        //                        "update userinfo set LeftTemplateV10 = @LeftTemplateV10, RightTemplateV10 = @RightTemplateV10, " +
        //                        "FaceTemplateV10 = @FaceTemplateV10, FpRegistered = 0, Updatetime = @Updatetime where enrollId = @enrollId";
        //                    para[0].Value = ui.EnrollId;
        //                    para[1].Value = ui.LeftTemplateV10;
        //                    para[2].Value = ui.RightTemplateV10;
        //                    para[3].Value = ui.FaceTemplateV10;
        //                    r += SqlHelper.ExecuteNonQuery(sqlt, CommandType.Text, cmd, para);
        //                }
        //                sqlt.Commit();
        //                sqlc.Close();
        //            }
        //            catch (Exception e)
        //            {
        //                Logger.Error(e);
        //            }

        //            break;
        //        case "zoko":
        //            para = new SqlParameter[]
        //            {
        //                new SqlParameter("@enrollId", SqlDbType.Int),
        //                new SqlParameter("@LEFTTEMPLATE", SqlDbType.Image),
        //                new SqlParameter("@RIGHTTEMPLATE", SqlDbType.Image),
        //                new SqlParameter("@FACETEMPLATE", SqlDbType.Image),
        //                new SqlParameter("@Updatetime", DateTime.Now),
        //            };
        //            try
        //            {
        //                SqlConnection sqlc = new SqlConnection(connStr);
        //                sqlc.Open();
        //                SqlTransaction sqlt = sqlc.BeginTransaction();
        //                for (int i = 0; i < lstUi.Count; i++)
        //                {
        //                    UserInfo ui = lstUi[i];
        //                    string cmd =
        //                        "update userinfo set LEFTTEMPLATE = @LEFTTEMPLATE, RIGHTTEMPLATE = @RIGHTTEMPLATE, " +
        //                        "FACETEMPLATE = @FACETEMPLATE, FpRegistered = 0, Updatetime = @Updatetime where enrollId = @enrollId";
        //                    para[0].Value = ui.EnrollId;
        //                    para[1].Value = ui.LeftTemplate;
        //                    para[2].Value = ui.RightTemplate;
        //                    para[3].Value = ui.FaceTemplate;
        //                    r += SqlHelper.ExecuteNonQuery(sqlt, CommandType.Text, cmd, para);
        //                }
        //                sqlt.Commit();
        //                sqlc.Close();
        //            }
        //            catch (Exception e)
        //            {
        //                Logger.Error(e);
        //            }
        //            break;
        //    }
        //    return r;
        //}

        [WebMethod]
        public int UpdateFeature(DataSet ds, string deviceType)
        {
            string connStr = ConfigurationManager.ConnectionStrings["cabinetServer"].ConnectionString;
            if (!SqlDataHelper.IsDataValid(ds))
            {
                return -100;
            }
            SqlParameter[] para;
            DataTable dt = ds.Tables[0];
            int r = 0;
            switch (deviceType)
            {

                default:
                    para = new SqlParameter[]
                    {
                        new SqlParameter("@enrollId", SqlDbType.Char),
                        new SqlParameter("@LeftTemplateV10", SqlDbType.Char),
                        new SqlParameter("@RightTemplateV10", SqlDbType.Char),
                        new SqlParameter("@FaceTemplateV10", SqlDbType.Char),
                        new SqlParameter("@Updatetime", DateTime.Now),
                    };
                    try
                    {
                        SqlConnection sqlc = new SqlConnection(connStr);
                        sqlc.Open();
                        SqlTransaction sqlt = sqlc.BeginTransaction();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            string cmd =
                                "update userinfo set LeftTemplateV10 = @LeftTemplateV10, RightTemplateV10 = @RightTemplateV10, " +
                                "FaceTemplateV10 = @FaceTemplateV10, FpRegistered = 0, Updatetime = @Updatetime where enrollId = @enrollId";
                            string LeftTemplateV10 = dr["LeftTemplateV10"]?.ToString();
                            string RightTemplateV10 = dr["RightTemplateV10"]?.ToString();
                            string faceTemplateV10 = dr["FaceTemplateV10"]?.ToString();
                            para[0].Value = dr["EnrollId"];
                            para[1].Value = LeftTemplateV10;
                            para[2].Value = RightTemplateV10;
                            para[3].Value = faceTemplateV10;
                            r += SqlHelper.ExecuteNonQuery(sqlt, CommandType.Text, cmd, para);
                        }
                        sqlt.Commit();
                        sqlc.Close();
                    }
                    catch (Exception e)
                    {
                        //Logger.Error(e);
                    }

                    break;
                case "zoko":
                    para = new SqlParameter[]
                    {
                        new SqlParameter("@enrollId", SqlDbType.Char),
                        new SqlParameter("@LEFTTEMPLATE", SqlDbType.Image),
                        new SqlParameter("@RIGHTTEMPLATE", SqlDbType.Image),
                        new SqlParameter("@FACETEMPLATE", SqlDbType.Image),
                        new SqlParameter("@Updatetime", DateTime.Now),
                    };
                    try
                    {
                        SqlConnection sqlc = new SqlConnection(connStr);
                        sqlc.Open();
                        SqlTransaction sqlt = sqlc.BeginTransaction();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            string cmd =
                                "update userinfo set LEFTTEMPLATE = @LEFTTEMPLATE, RIGHTTEMPLATE = @RIGHTTEMPLATE, " +
                                "FACETEMPLATE = @FACETEMPLATE, FpRegistered = 0, Updatetime = @Updatetime where enrollId = @enrollId";
                            byte[] lefttemplate = dr["LEFTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["LEFTTEMPLATE"];
                            byte[] righttemplate = dr["RIGHTTEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["RIGHTTEMPLATE"];
                            byte[] facetemplate = dr["FACETEMPLATE"] == System.DBNull.Value ? new byte[0] : (byte[])dr["FACETEMPLATE"];
                            para[0].Value = dr["EnrollId"];
                            para[1].Value = lefttemplate;
                            para[2].Value = righttemplate;
                            para[3].Value = facetemplate;
                            r += SqlHelper.ExecuteNonQuery(sqlt, CommandType.Text, cmd, para);
                        }
                        sqlt.Commit();
                        sqlc.Close();
                    }
                    catch (Exception e)
                    {
                        //Logger.Error(e);
                    }
                    break;
            }
            return r;
        }

    }
}
