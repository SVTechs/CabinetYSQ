using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Threading;


/// <summary>
/// MainService 的摘要说明
/// </summary>
[WebService(Namespace = "http://jhy.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class MainService : System.Web.Services.WebService
{
    public MainService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public DataSet GetAllUnit()
    {
        string SqlCmd = @"select distinct(Unit) from AppInfo where Author is NULL or Author = '' ";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, null);
        return InfoSet;
    }

    [WebMethod]
    public DataSet GetAvailableUnit(string author)
    {
        author = Util_String.SQLFilter(author);
        string SqlCmd = @"select distinct(Unit) from AppInfo ";
        if (string.IsNullOrEmpty(author)) SqlCmd += " where Author is NULL or Author = '' ";
        else SqlCmd += string.Format(" where Author = '{0}' ", author);
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, null);
        return InfoSet;
    }

    [WebMethod]
    public DataSet GetAppByUnit(string Unit)
    {
        Unit = Util_String.SQLFilter(Unit);
        string SqlCmd = string.Format(@"select distinct(AppName) from AppInfo where Unit = '{0}'
            and (Author is NULL or Author = '') ", Util_String.SQLFilter(Unit));
        return SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, null);
    }

    [WebMethod]
    public DataSet GetAvailableAppByUnit(string Unit, string author)
    {
        Unit = Util_String.SQLFilter(Unit);
        string SqlCmd = string.Format(@"select distinct(AppName) from AppInfo where Unit = '{0}'",
            Util_String.SQLFilter(Unit));
        if (string.IsNullOrEmpty(author)) SqlCmd += " and (Author is NULL or Author = '') ";
        else SqlCmd += string.Format(" and Author = '{0}' ", author);
        return SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, null);
    }

    [WebMethod]
    public string GetServerVersion(string AppName, string Unit)
    {
        SqlParameter[] para = new SqlParameter[2];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Unit", Unit);
        string SqlCmd = @"select * from AppInfo where AppName = @AppName and Unit = @Unit";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        if (Util_Data.IsDataValid(InfoSet))
        {
            return InfoSet.Tables[0].Rows[0]["AppVer"].ToString();
        }
        return "";
    }

    [WebMethod]
    public string GetFileUpdList(string AppName, string Unit)
    {
        SqlParameter[] para = new SqlParameter[2];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Unit", Unit);
        string SqlCmd = @"select * from AppInfo where AppName = @AppName and Unit = @Unit";
        DataSet InfoSet = SqlHelper.GetDataSet(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        if (Util_Data.IsDataValid(InfoSet))
        {
            return InfoSet.Tables[0].Rows[0]["FileList"].ToString();
        }
        return "";
    }

    [WebMethod]
    public byte[] ReqFile(string Unit, string AppName, int ReqType, string FileName)
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
        switch (ReqType)
        {
            case 0:
                ReqPath += @"Resource\Update\";
                break;
            case 1:
                ReqPath += @"Resource\Updater\";
                break;
        }
        ReqPath += Unit + "\\" + AppName + "\\" + FileName;
        FileStream OPFile = File.OpenRead(ReqPath);
        OPByte = new byte[OPFile.Length];
        OPFile.Read(OPByte, 0, (int)OPFile.Length);
        OPFile.Close();
        return OPByte;
    }

    [WebMethod]
    public string ReqFileUrl(string Unit, string AppName, int ReqType, string FileName)
    {
        string ReqPath = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        ReqPath = ReqPath.Substring(0, ReqPath.LastIndexOf("/") + 1);
        switch (ReqType)
        {
            case 0:
                ReqPath += @"Resource/Update/";
                break;
            case 1:
                ReqPath += @"Resource/Updater/";
                break;
        }
        ReqPath += Unit + "/" + AppName + "/" + FileName;
        return ReqPath;
    }

    [WebMethod]
    public int DelAppFile(string Unit, string AppName)
    {
        string ReqPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        ReqPath = ReqPath.Substring(0, ReqPath.LastIndexOf("\\") + 1);
        ReqPath += @"Resource/Update/";
        ReqPath += Unit + "/" + AppName + "/";
        try
        {
            Directory.Delete(ReqPath, true);
        }
        catch (Exception) 
        { }
        int retryCount = 0;
        while (Directory.Exists(ReqPath))
        {
            try
            {
                Directory.Delete(ReqPath, true);
            }
            catch (Exception)
            { }
            Thread.Sleep(1000);
            retryCount++;
            if (retryCount > 10) break;
        }
        if (Directory.Exists(ReqPath))
        {
            return 0;
        }
        Directory.CreateDirectory(ReqPath);
        return 1;
    }

    [WebMethod]
    public int AddApp(string AppName, string Unit)
    {
        SqlParameter[] para = new SqlParameter[2];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Unit", Unit);

        string SqlCmd = @"insert into AppInfo(AppName, Unit) values(@AppName, @Unit)";
        SqlHelper.ExecuteNonQuery(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);

        string ReqPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        if (!ReqPath.EndsWith("\\"))
        {
            ReqPath += "\\";
        }
        Directory.CreateDirectory(ReqPath + @"Resource\Update\" + Unit + "\\" + AppName);
        return 1;
    }

    [WebMethod]
    public int GenUpdateInfo(string AppName, string Unit, string NewVer, string UpdLog)
    {
        SqlParameter[] para = new SqlParameter[2];
        para[0] = new SqlParameter("@AppName", AppName);
        para[1] = new SqlParameter("@Unit", Unit);

        string SqlCmd = @"select * from AppInfo where AppName = @AppName and Unit = @Unit";
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
        PhysicalPath += @"Resource\Update\" + Unit + "\\" + AppName + "\\";
        //检查一级目录文件
        DirectoryInfo UdInfo = new DirectoryInfo(PhysicalPath);
        FileInfo[] UpdFiles = UdInfo.GetFiles();
        if (UpdFiles.Length == 0)
        {
            return -10;
        }
        string md5Sig = GenFileSig(PhysicalPath, PhysicalPath.Length);
        SqlCmd = string.Format(@"update AppInfo set AppVer = '{0}',FileList = '{1}', UpdateDate = '{2}'
            where AppName = @AppName and Unit = @Unit", Util_String.SQLFilter(NewVer), md5Sig, 
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        SqlHelper.ExecuteNonQuery(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        SqlCmd = string.Format(@"insert into UpdateLog(AppID, NewVer, UpdDate, UpdLog) values('{0}', '{1}', '{2}', '{3}')", 
            AppID, Util_String.SQLFilter(NewVer), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Util_String.SQLFilter(UpdLog));
        SqlHelper.ExecuteNonQuery(SqlHelper.LocalDB, CommandType.Text, SqlCmd, para);
        return 1;
    }

    private string GenFileSig(string fileDir, int basicLen)
    {
        string md5Sig = "";
        DirectoryInfo UdInfo = new DirectoryInfo(fileDir);
        FileInfo[] UpdFiles = UdInfo.GetFiles();
        foreach (FileInfo CurFile in UpdFiles)
        {
            string Md5 = Util_FileProc.GetMD5HashFromFile(fileDir + CurFile.Name);
            md5Sig += string.Format("{0}${1}|", fileDir.Substring(basicLen) + CurFile.Name, Md5);
        }
        DirectoryInfo[] UpdDirs = UdInfo.GetDirectories();
        foreach (DirectoryInfo dir in UpdDirs)
        {
            string dirPath = dir.FullName;
            if (!dirPath.EndsWith("\\")) dirPath += "\\";
            md5Sig += GenFileSig(dirPath, basicLen);
        }
        return md5Sig;
    }
}
