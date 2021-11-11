using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;

/// <summary>
/// MobileService 的摘要说明
/// </summary>
[WebService(Namespace = "http://jhy.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class MobileService : System.Web.Services.WebService {

    public MobileService () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public DataSet GetAllUnit()
    {
        string SqlCmd = @"select distinct(Unit) from MobileAppInfo";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, null);
        return InfoSet;
    }

    [WebMethod]
    public DataSet GetAppByUnit(string Unit)
    {
        string SqlCmd = string.Format(@"select distinct(AppName) from MobileAppInfo where Unit = '{0}'",
            Util_String.SQLFilter(Unit));
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, null);
        return InfoSet;
    }

    [WebMethod]
    public string GetServerVersion(string AppName, int Platform, string Unit)
    {
        SqlParameter[] para = new SqlParameter[3];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Platform", Platform);
        para[2] = new SqlParameter("@Unit", Unit);
        string SqlCmd = @"select * from MobileAppInfo where AppName = @AppName and Platform = @Platform and Unit = @Unit";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        if (Util_Data.IsDataValid(InfoSet))
        {
            return InfoSet.Tables[0].Rows[0]["AppVer"].ToString();
        }
        return "";
    }

    [WebMethod]
    public string GetServerVersionWithDesp(string AppName, int Platform, string Unit)
    {
        SqlParameter[] para = new SqlParameter[3];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Platform", Platform);
        para[2] = new SqlParameter("@Unit", Unit);
        string SqlCmd = @"select * from MobileAppInfo where AppName = @AppName and Platform = @Platform and Unit = @Unit";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        if (Util_Data.IsDataValid(InfoSet))
        {
            string AppVer = InfoSet.Tables[0].Rows[0]["AppVer"].ToString();
            string AppID = InfoSet.Tables[0].Rows[0]["ID"].ToString();
            SqlCmd = string.Format(@"select top 1 * from MobileUpdateLog where AppID = '{0}' order by UpdDate DESC", AppID);
            string UpdLog = "";
            InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
            if (Util_Data.IsDataValid(InfoSet))
            {
                UpdLog = InfoSet.Tables[0].Rows[0]["UpdLog"].ToString();
            }
            return AppVer + "|" + UpdLog;
        }
        return "";
    }

    [WebMethod]
    public string GetAppFile(string AppName, int Platform, string Unit)
    {
        SqlParameter[] para = new SqlParameter[3];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Platform", Platform);
        para[2] = new SqlParameter("@Unit", Unit);
        string SqlCmd = @"select * from MobileAppInfo where AppName = @AppName and Platform = @Platform and Unit = @Unit";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        if (Util_Data.IsDataValid(InfoSet))
        {
            return InfoSet.Tables[0].Rows[0]["AppFile"].ToString();
        }
        return "";
    }

    [WebMethod]
    public byte[] DownloadApp(string AppName, int Platform, string Unit)
    {
        string AppFile = GetAppFile(AppName, Platform, Unit);
        return ReqFile(Unit, AppName, AppFile);
    }

    [WebMethod]
    public byte[] ReqFile(string Unit, string AppName, string FileName)
    {
        byte[] OPByte = null;
        if (FileName.StartsWith(".."))
        {
            return null;
        }
        string ReqPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (!ReqPath.EndsWith("\\"))
        {
            ReqPath += "\\";
        }
        ReqPath += @"Resource\App\";
        ReqPath += Unit + "\\" + AppName + "\\" + FileName;
        FileStream OPFile = File.OpenRead(ReqPath);
        OPByte = new byte[OPFile.Length];
        OPFile.Read(OPByte, 0, (int)OPFile.Length);
        OPFile.Close();
        return OPByte;
    }

    [WebMethod]
    public int AddApp(string AppName, string Unit)
    {
        SqlParameter[] para = new SqlParameter[2];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Unit", Unit);

        string SqlCmd = @"insert into MobileAppInfo(AppName, Unit) values(@AppName, @Unit)";
        SqlHelper.ExecuteNonQuery(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);

        string ReqPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (!ReqPath.EndsWith("\\"))
        {
            ReqPath += "\\";
        }
        Directory.CreateDirectory(ReqPath + @"Resource\App\" + Unit + "\\" + AppName);
        return 1;
    }

    [WebMethod]
    public int GenUpdateInfo(string AppName, string Unit, string NewVer, string UpdLog)
    {
        SqlParameter[] para = new SqlParameter[2];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Unit", Unit);

        string SqlCmd = @"select * from MobileAppInfo where AppName = @AppName and Unit = @Unit";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        if (!Util_Data.IsDataValid(InfoSet))
        {
            return -1;
        }
        string AppID = InfoSet.Tables[0].Rows[0]["ID"].ToString();
        string PhysicalPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (!PhysicalPath.EndsWith("\\"))
        {
            PhysicalPath += "\\";
        }
        PhysicalPath += @"Resource\App\" + Unit + "\\" + AppName + "\\";
        DirectoryInfo UdInfo = new DirectoryInfo(PhysicalPath);
        FileInfo[] UpdFiles = UdInfo.GetFiles();
        if (UpdFiles.Length == 0)
        {
            return -10;
        }
        SqlCmd = string.Format(@"update AppInfo set AppVer = '{0}', AppFile = '{1}', UpdateDate = '{2}'
            where AppName = @AppName and Unit = @Unit", Util_String.SQLFilter(NewVer), UpdFiles[0].Name,
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        SqlHelper.ExecuteNonQuery(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        SqlCmd = string.Format(@"insert into UpdateLog(AppID, UpdDate, UpdLog) values('{0}', '{1}', '{2}')",
            AppID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Util_String.SQLFilter(UpdLog));
        SqlHelper.ExecuteNonQuery(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        return 1;
    }
}
