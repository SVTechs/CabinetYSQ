using System;
using System.Activities.Expressions;
using System.Configuration;
using System.Data;
using System.Web.Services;
using NLog;
using Utilities.DbHelper;
using Utilities.Json;

/// <summary>
/// ToolService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class ToolService : System.Web.Services.WebService
{

    private static string conn = ConfigurationManager.ConnectionStrings["zwStock"].ConnectionString;// "Data Source=localhost//SQLEXPRESS;Initial Catalog=ZWStock;User Id=sa;Password=XLT@62142100;".;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    [WebMethod]
    public string GetToolBarAll(DateTime lastSync)
    {
        string sqlCmd = string.Format(@"select im.Id, Spec, Unit, Price, im.Creator, im.CreatedTime, im.LastModifier, im.LastModifiedTime
, ic.ItemType, im.ItemCategoryId, im.Alias, ic.Name toolType, im.Name toolName
from ItemMasters im left join ItemCategories ic 
on im.ItemCategoryId = ic.id where ic.ItemType = 2  and (im.CreatedTime >= '{0}' or im.LastModifiedTime >= '{0}')", lastSync.ToString("yyyy-MM-dd HH:mm:ss"));
        //if (!string.IsNullOrEmpty(cabinetName))
        //{
        //    sqlCmd += string.Format(" and CabinetName = '{0}'", cabinetName);
        //}
        DataSet infoSet = SqlHelper.GetDataSet(conn, CommandType.Text, sqlCmd, null);
        if (SqlDataHelper.IsDataValid(infoSet))
        {
            string resJson = ConvertJson.DataSetToJson(infoSet);
            resJson = resJson.Substring(9, resJson.Length - 10);
            return resJson;
        }
        return null;
    }

    [WebMethod]
    public string Test()
    {
        try
        {
            string sqlCmd = string.Format(@"select im.Id, Spec, Unit, Price, im.Creator, im.CreatedTime, im.LastModifier, im.LastModifiedTime
, ic.ItemType, im.ItemCategoryId, im.Alias, ic.Name toolType, im.Name toolName
from ItemMasters im left join ItemCategories ic 
on im.ItemCategoryId = ic.id where ic.ItemType = 2");
            DataSet infoSet = SqlHelper.GetDataSet(conn, CommandType.Text, sqlCmd, null);
            object o = SqlHelper.ExecuteScalar(conn, CommandType.Text,
                string.Format("select Id from StoreroomBins where Name = N'{0}'", "1号工具柜"), null);
            string storeRoomBinId = "";
            Logger.Warn(o == null);
            if (o != null)
            {
                storeRoomBinId = o.ToString().ToUpper();
                //return null;
                Logger.Info(storeRoomBinId);
            }
            DataTable dt = SqlHelper.GetDataTable(conn, CommandType.Text,
                string.Format("select * from StoreroomBinItemMasters where StoreroomBin_Id = '{0}'", storeRoomBinId), null);
            DataTable dtOri = infoSet.Tables[0];
            DataTable dtDest = dtOri.Clone();
            Logger.Info(dt.Rows.Count);
            for (int i = 0; i < dtOri.Rows.Count; i++)
            {
                DataRow dr = dtOri.Rows[i];
                if (dt.Select("ItemMaster_Id = '" + dr["Id"].ToString().ToUpper() + "'").Length > 0)
                {
                    dtDest.ImportRow(dr);
                }
            }
            Logger.Info("dtDest OK");
            DataSet ds = new DataSet();
            ds.Tables.Add(dtDest);
            if (SqlDataHelper.IsDataValid(ds))
            {
                string resJson = ConvertJson.DataSetToJson(ds);
                resJson = resJson.Substring(9, resJson.Length - 10);
                return resJson;
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
      
        return null;
    }

    [WebMethod]
    public string GetAllTool(DateTime lastSync, string cabinetName)
    {
        try
        {
            string sqlCmd = string.Format(@"select im.Id, Spec, Unit, Price, im.Creator, im.CreatedTime, im.LastModifier, im.LastModifiedTime
, ic.ItemType, im.ItemCategoryId, im.Alias, ic.Name toolType, im.Name toolName
from ItemMasters im left join ItemCategories ic 
on im.ItemCategoryId = ic.id where ic.ItemType = 2");
            DataSet infoSet = SqlHelper.GetDataSet(conn, CommandType.Text, sqlCmd, null);
            object o = SqlHelper.ExecuteScalar(conn, CommandType.Text,
                string.Format("select Id from StoreroomBins where Name = N'{0}'", cabinetName), null);

            string storeRoomBinId = "";
            if (o != null)
            {
                storeRoomBinId = o.ToString().ToUpper();
                //return null;
            }
            DataTable dt = SqlHelper.GetDataTable(conn, CommandType.Text,
                string.Format("select * from StoreroomBinItemMasters where StoreroomBin_Id = '{0}'", storeRoomBinId), null);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            DataTable dtOri = infoSet.Tables[0];
            DataTable dtDest = dtOri.Clone();
            for (int i = 0; i < dtOri.Rows.Count; i++)
            {
                DataRow dr = dtOri.Rows[i];
                if (dt.Select("ItemMaster_Id = '" + dr["Id"].ToString().ToUpper() + "'").Length > 0)
                {
                    dtDest.ImportRow(dr);
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dtDest);
            if (SqlDataHelper.IsDataValid(ds))
            {
                string resJson = ConvertJson.DataSetToJson(ds);
                resJson = resJson.Substring(9, resJson.Length - 10);
                return resJson;
            }
        }
        catch (Exception e)
        {
            return null;
        }
      
        return null;
    }
}

