using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using CabinetMgr.Bll.Web_References.SyncServiceRef;
using CabinetMgr.Config;
using CabinetMgr.Dal;
using Domain.Main.Domain;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;
using Utilities.System;

namespace CabinetMgr.Bll
{
    public class BllSyncManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly SyncService SyncService = new SyncService();

        private static readonly List<List<Type>> DownloadSyncList = new List<List<Type>>(),
            UploadSyncList = new List<List<Type>>();

        private static int _syncInterval = 20000, _syncControl;
        private static bool _timeSyncStatus;

        public enum SyncType
        {
            Download,
            Upload,
            Dual
        }

        /// <summary>
        /// 注册新同步项目组
        /// </summary>
        /// <param name="syncType">0=下载,1=上传</param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public static int RegisterSyncItem(SyncType syncType, params Type[] itemType)
        {
            try
            {
                List<Type> curList = new List<Type>();
                for (int i = 0; i < itemType.Length; i++)
                {
                    if (syncType == SyncType.Download && !IsDownloadItemValid(itemType[i]))
                    {
                        return -101;
                    }
                    if (syncType == SyncType.Upload && !IsUploadItemValid(itemType[i]))
                    {
                        return -102;
                    }
                    curList.Add(itemType[i]);
                }
                if (syncType == 0)
                {
                    DownloadSyncList.Add(curList);
                }
                else
                {
                    UploadSyncList.Add(curList);
                }
                return 1;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -100;
        }

        /// <summary>
        /// 判断表结构是否符合要求
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private static bool IsDownloadItemValid(Type itemType)
        {
            bool hasId = false;
            PropertyInfo[] itemProperties = itemType.GetProperties();
            for (int i = 0; i < itemProperties.Length; i++)
            {
                if (itemProperties[i].Name.Equals("Id"))
                {
                    hasId = true;
                    break;
                }
            }
            if (hasId) return true;
            return false;
        }

        /// <summary>
        /// 判断表结构是否符合要求
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private static bool IsUploadItemValid(Type itemType)
        {
            bool hasId = false, hasSyncStatus = false;
            PropertyInfo[] itemProperties = itemType.GetProperties();
            for (int i = 0; i < itemProperties.Length; i++)
            {
                if (itemProperties[i].Name.Equals("Id"))
                {
                    hasId = true;
                    continue;
                }
                if (itemProperties[i].Name.Equals("SyncStatus"))
                {
                    hasSyncStatus = true;
                }
            }
            if (hasId && hasSyncStatus) return true;
            return false;
        }

        /// <summary>
        /// 启动同步
        /// </summary>
        /// <returns></returns>
        public static int StartSync()
        {
            try
            {
                _syncControl = 1;
                //设置更新服务地址
                if (!string.IsNullOrEmpty(AppConfig.SyncServiceUrl))
                {
                    SyncService.Url = AppConfig.SyncServiceUrl;
                }
                //自动校时
                _timeSyncStatus = false;
                Thread timeSyncThread = new Thread(SyncSystemTime) { IsBackground = true };
                timeSyncThread.Start();
                //启动下载任务
                for (int i = 0; i < DownloadSyncList.Count; i++)
                {
                    Thread syncThread = new Thread(DownloadSyncWorker) { IsBackground = true };
                    syncThread.Start(DownloadSyncList[i]);
                }
                ////启动上传任务
                for (int i = 0; i < UploadSyncList.Count; i++)
                {
                    Thread syncThread = new Thread(UploadSyncWorker) { IsBackground = true };
                    syncThread.Start(UploadSyncList[i]);
                }
                ////启动其他同步功能
                Thread extSyncThread = new Thread(ExtSyncWorker) { IsBackground = true };
                extSyncThread.Start();
                return 1;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -100;
        }

        private static void SyncSystemTime()
        {
            while (true)
            {
                if (_syncControl == 0) break;
                try
                {
                    string serverTime = SyncService.GetServerTime();
                    if (Math.Abs((DateTime.Now - DateTime.Parse(serverTime)).TotalMinutes) > 1)
                    {
                        SysHelper.SetSystemTime(serverTime);
                        if ((DateTime.Now - DateTime.Parse(serverTime)).TotalMinutes <= 1)
                        {
                            _timeSyncStatus = true;
                        }
                    }
                    else
                    {
                        _timeSyncStatus = true;
                    }
                    Thread.Sleep(10000);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 设置同步间隔(秒)
        /// </summary>
        /// <param name="syncInterval"></param>
        /// <returns></returns>
        public static int SetSyncInterval(int syncInterval)
        {
            _syncInterval = syncInterval * 1000;
            return 1;
        }

        /// <summary>
        /// 下载工作线程
        /// </summary>
        /// <param name="typeListObj"></param>
        private static void DownloadSyncWorker(object typeListObj)
        {
            while (true)
            {
                if (_syncControl == 0) break;
                if (!_timeSyncStatus)
                {
                    Thread.Sleep(10000);
                    continue;
                }
                try
                {
                    List<Type> syncList = (List<Type>)typeListObj;
                    for (int i = 0; i < syncList.Count; i++)
                    {
                        RuntimeInfo syncInfo = BllRuntimeInfo.GetRuntimeInfo("RaySync" + syncList[i].Name);
                        DateTime lastSync = (syncInfo == null || syncInfo.KeyValue1 == null)
                            ? Env.MinTime
                            : DateTime.Parse(syncInfo.KeyValue1);
                        DateTime syncStart = DateTime.Now;
                        DataSet changedInfo = SyncService.DownloadData(syncList[i].Name, lastSync);
                        if (SqlDataHelper.IsDataValid(changedInfo))
                        {
                            int updateRsult = DalSyncManager.UpdateData(syncList[i], changedInfo);
                            if (updateRsult > 0)
                            {
                                BllRuntimeInfo.SetRuntimeInfo("RaySync" + syncList[i].Name,
                                    syncStart.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
                Thread.Sleep(_syncInterval);
            }
        }

        /// <summary>
        /// 上传工作线程
        /// </summary>
        /// <param name="typeListObj"></param>
        private static void UploadSyncWorker(object typeListObj)
        {
            while (true)
            {
                if (_syncControl == 0) break;
                if (!_timeSyncStatus)
                {
                    Thread.Sleep(10000);
                    continue;
                }
                try
                {
                    List<Type> syncList = (List<Type>)typeListObj;
                    for (int i = 0; i < syncList.Count; i++)
                    {
                        DataSet localSet = DalSyncManager.GetLocalData(syncList[i]);
                        if (SqlDataHelper.IsDataValid(localSet))
                        {
                            int result = SyncService.UploadData(syncList[i].Name, localSet, AppConfig.CabinetName);
                            if (result > 0)
                            {
                                DalSyncManager.SetAsUploaded(syncList[i], localSet);
                                DalRuntimeInfo.SetRuntimeInfo("Upload"+syncList[i].Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                Thread.Sleep(_syncInterval);
            }
        }

        private static void ExtSyncWorker()
        {
            while (true)
            {
                try
                {
                    //工具信息下载
                    string lastSyncTime = Env.MinTime.ToString("yyyy-MM-dd HH:mm:ss");
                    RuntimeInfo lastSync = BllRuntimeInfo.GetRuntimeInfo("SrvToolSync");
                    if (lastSync != null)
                    {
                        lastSyncTime = lastSync.KeyValue1;
                    }
                    string syncTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    IList serverToolList = BllToolInfo.SearchServerToolInfo(lastSyncTime, AppConfig.CabinetName);
                    if (SqlDataHelper.IsDataValid(serverToolList))
                    {
                        int result = DalToolInfo.UpdateServerInfo(serverToolList);
                        if (result > 0)
                        {
                            DalRuntimeInfo.SetRuntimeInfo("SrvToolSync", syncTime);
                        }
                    }
                    ////工具信息上传
                     IList toolList = BllToolInfo.SearchChangedToolInfo();
                    if (SqlDataHelper.IsDataValid(toolList))
                    {
                        DataSet toolSet = SqlDataHelper.ConvertToDataSet<ToolInfo>(toolList);
                        int result = SyncService.UploadData("ToolInfo", toolSet, AppConfig.CabinetName);
                        if (result > 0)
                        {
                            DalSyncManager.SetAsUploaded(typeof(ToolInfo), toolSet);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    // ignored
                }
                Thread.Sleep(1000 * 40);
            }
        }
    }


}
