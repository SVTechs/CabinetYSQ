using System;
using System.Data;
using CabinetService.Dal;
using CabinetService.Dal.NhUtils;
using Utilities.DbHelper;

namespace CabinetService.Bll
{
    public class BllSyncManager
    {
        /// <summary>
        /// 获取变化数据
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="lastChanged"></param>
        /// <param name="dbTarget"></param>
        /// <returns></returns>
        public static DataSet GetChangedData(Type tableType, DateTime lastChanged, NhControl.DbTarget dbTarget)
        {
            if (tableType == null)
            {
                return null;
            }
            return DalSyncManager.GetChangedData(tableType, lastChanged, dbTarget);
        }

        /// <summary>
        /// 保存数据于服务端
        /// </summary>
        /// <param name="tableType"></param>
        /// <param name="infoSet"></param>
        /// <param name="dbTarget"></param>
        /// <returns></returns>
        public static int SaveData(Type tableType, DataSet infoSet, string dataOwner, NhControl.DbTarget dbTarget)
        {
            if (!SqlDataHelper.IsDataValid(infoSet))
            {
                return -100;
            }
            return DalSyncManager.SaveData(tableType, infoSet, dataOwner, dbTarget);
        }
    }
}
