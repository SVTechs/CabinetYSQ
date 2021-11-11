using System;
using System.Data;
using System.Reflection;
using System.Web.Services;
using CabinetService.Bll;
using CabinetService.Dal.NhUtils;
using NLog;

/// <summary>
/// SyncService 的摘要说明
/// </summary>
[WebService(Namespace = "http://jhy.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class SyncService : System.Web.Services.WebService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    /// <summary>
    /// 下载变化数据
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="lastSync"></param>
    /// <returns></returns>
    [WebMethod]
    public DataSet DownloadData(string typeName, DateTime lastSync)
    {
        try
        {
            Assembly entityAssembly = Assembly.Load("Domain.ServerMain");
            Type tableType = entityAssembly.GetType("Domain.ServerMain.Domain." + typeName);
            if (tableType != null)
            {
                return BllSyncManager.GetChangedData(tableType, lastSync, NhControl.DbTarget.Local);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }

        //entityAssembly = Assembly.Load("Domain.Qcshkf");
        //tableType = entityAssembly.GetType("Domain.Qcshkf.Domain." + typeName);
        //if (tableType != null)
        //{
        //    return BllSyncManager.GetChangedData(tableType, lastSync, NhControl.DbTarget.Qcshkf);
        //}
        return null;
    }

    /// <summary>
    /// 上传数据
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="infoSet"></param>
    /// <param name="dataOwner"></param>
    /// <returns></returns>
    [WebMethod]
    public int UploadData(string typeName, DataSet infoSet, string dataOwner)
    {
        try
        {
            Assembly entityAssembly = Assembly.Load("Domain.ServerMain");
            Type tableType = entityAssembly.GetType("Domain.ServerMain.Domain." + typeName);
            if (tableType != null)
            {
                return BllSyncManager.SaveData(tableType, infoSet, dataOwner, NhControl.DbTarget.Local);
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        //entityAssembly = Assembly.Load("Domain.Qcshkf");
        //tableType = entityAssembly.GetType("Domain.Qcshkf.Domain." + typeName);
        //if (tableType != null)
        //{
        //    return BllSyncManager.SaveData(tableType, infoSet, dataOwner, NhControl.DbTarget.Qcshkf);
        //}
        return -1;
    }

    /// <summary>
    /// 获取服务器时间
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public string GetServerTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
