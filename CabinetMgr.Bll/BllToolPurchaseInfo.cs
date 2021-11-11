using System;
using System.Collections;
using CabinetMgr.Dal;

namespace CabinetMgr.Bll
{
    public class BllToolPurchaseInfo
    {
        public static int AddToolPurchaseInfo(string toolName, string toolSpec, int toolCount, string cabinetNo,
            string requesterName, int requestStatus)
        {
            return DalToolPurchaseInfo.AddToolPurchaseInfo(toolName, toolSpec, toolCount, cabinetNo, requesterName,
                requestStatus);
        }

        public static IList SearchToolPurchaseInfo(string cabinetName, DateTime timeStart, DateTime timeEnd)
        {
            return DalToolPurchaseInfo.SearchToolPurchaseInfo(cabinetName, timeStart, timeEnd);
        }
    }
}
