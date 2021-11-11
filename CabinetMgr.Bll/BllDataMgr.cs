using System;
using System.Threading;
using CabinetMgr.Bll.UpdateServiceRef;
using CabinetMgr.Config;
using CabinetMgr.Dal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utilities.Json;

namespace CabinetMgr.Bll
{
    public class BllDataMgr
    {
        private static readonly UpdateService UpdateService = new UpdateService();

        public static void InitConn()
        {
            Thread dbInitThread = new Thread(InitConnThd) { IsBackground = true };
            dbInitThread.Start();
        }

        private static void InitConnThd()
        {
            DalDataMgr.InitConn();
        }

        public static string GetServerAppVer()
        {
            if (!string.IsNullOrEmpty(AppConfig.UpdateServiceUrl)) UpdateService.Url = AppConfig.UpdateServiceUrl;
            try
            {

                string result = UpdateService.GetAppVersionInfo(AppConfig.AppID, AppConfig.UserID)[1];
                JObject j = ConvertJson.ParseJson(result);
                return j["CurrentVersion"].ToString();
            }
            catch (Exception )
            {
                // ignored
            }
            return null;
        }
    }
}
