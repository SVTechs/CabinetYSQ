using System;
using CabinetMgr.Bll.Web_References.ToolBarServiceRef;
using Newtonsoft.Json.Linq;
using Utilities.Json;

namespace CabinetMgr.Bll
{
    public class BllToolRepairRequest
    {
        private static readonly ToolBarServiceManage ToolService = new ToolBarServiceManage();

        /*
        public static int AddToolRepairRequest(string toolName, string toolSpec, string cabinetNo, string requesterName, string reqComment)
        {
            return DalToolRepairRequest.AddToolRepairRequest(toolName, toolSpec, cabinetNo, requesterName, reqComment);
        }

        public static int SetAsFinished(string toolName, string cabinetName)
        {
            return DalToolRepairRequest.SetAsFinished(toolName, cabinetName);
        }*/

        public static int AddToolRepairRequest(string userName, string toolCode, string toolName, string position,
            string cabinetNo)
        {
            try
            {
                string result = ToolService.SetBaoXiuDate(userName, toolCode, toolName, position, cabinetNo);
                JObject obj = ConvertJson.ParseJson(result);
                string code = obj["Code"].ToString();
                string msg = obj["Message"].ToString();
                if (code == "1")
                {
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {
                return -100;
            }
        }
    }
}
