using System;
using System.Collections;
using CabinetMgr.Config;
using CabinetMgr.Dal.NhUtils;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;

namespace CabinetMgr.Dal
{
    public class DalToolInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int AddToolInfo(string toolName, string toolCode, string toolSpec, int toolTypeId, string toolType,
            string hardwareId, string cardId,
            string standardRange, float deviationPositive, float deviationNegative, int toolPositionType,
            int toolPosition, string toolGrid,
            float checkInterval, string checkIntervalType, string toolManager, string comment, string operatorName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        DateTime curTime = DateTime.Now;
                        DateTime nextCheck = curTime;
                        switch (checkIntervalType)
                        {
                            case "天":
                                nextCheck = nextCheck.AddDays(checkInterval);
                                break;
                            case "月":
                                nextCheck = nextCheck.AddMonths((int)checkInterval);
                                break;
                            case "年":
                                nextCheck = nextCheck.AddYears((int)checkInterval);
                                break;
                        }
                        ToolInfo tiRecord = new ToolInfo
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            ToolName = toolName,
                            ToolCode = toolCode,
                            ToolSpec = toolSpec,
                            ToolType = toolType,
                            HardwareId = hardwareId,
                            CardId = cardId,
                            StandardRange = standardRange,
                            DeviationPositive = deviationPositive,
                            DeviationNegative = deviationNegative,
                            ToolPositionType = toolPositionType,
                            ToolPosition = toolPosition,
                            ToolGrid = toolGrid,
                            CheckTime = curTime,
                            NextCheckTime = nextCheck,
                            CheckInterval = checkInterval,
                            CheckIntervalType = checkIntervalType,
                            ToolManager = toolManager,
                            Comment = comment,
                            Operator = operatorName,
                            OperateTime = curTime,
                            SyncStatus = 0
                        };
                        session.SaveOrUpdate(tiRecord);
                        transaction.Commit();
                        return 1;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        public static int ModifyToolInfo(string identId, string toolName, string toolCode, string toolSpec,
            string toolType, string hardwareId, string cardId,
            string standardRange, float deviationPositive, float deviationNegative, int toolPositionType,
            int toolPosition, string toolGrid,
            float checkInterval, string checkIntervalType, string toolManager, string comment, string operatorName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("Id", identId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolInfo idRecord = (ToolInfo)recordList[0];
                            idRecord.ToolName = toolName;
                            idRecord.ToolCode = toolCode;
                            idRecord.ToolSpec = toolSpec;
                            idRecord.ToolType = toolType;
                            idRecord.HardwareId = hardwareId;
                            idRecord.CardId = cardId;
                            idRecord.StandardRange = standardRange;
                            idRecord.DeviationPositive = deviationPositive;
                            idRecord.DeviationNegative = deviationNegative;
                            idRecord.ToolPositionType = toolPositionType;
                            idRecord.ToolPosition = toolPosition;
                            idRecord.ToolGrid = toolGrid;
                            idRecord.CheckInterval = checkInterval;
                            idRecord.CheckIntervalType = checkIntervalType;
                            idRecord.ToolManager = toolManager;
                            idRecord.Comment = comment;
                            idRecord.Operator = operatorName;
                            idRecord.OperateTime = DateTime.Now;
                            idRecord.SyncStatus = 0;

                            DateTime nextCheck = idRecord.CheckTime;
                            switch (checkIntervalType)
                            {
                                case "天":
                                    nextCheck = nextCheck.AddDays(checkInterval);
                                    break;
                                case "月":
                                    nextCheck = nextCheck.AddMonths((int)checkInterval);
                                    break;
                                case "年":
                                    nextCheck = nextCheck.AddYears((int)checkInterval);
                                    break;
                            }
                            idRecord.NextCheckTime = nextCheck;
                            session.SaveOrUpdate(idRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        public static int DeleteToolInfo(string identId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("Id", identId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolInfo idRecord = (ToolInfo)recordList[0];
                            session.Delete(idRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        public static int SetAsChecked(ISession session, string identId)
        {
            try
            {
                ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                    .Add(Restrictions.Eq("Id", identId));
                IList recordList = pQuery.List();
                if (SqlDataHelper.IsDataValid(recordList))
                {
                    ToolInfo idRecord = (ToolInfo)recordList[0];
                    DateTime curTime = DateTime.Now;
                    idRecord.CheckTime = curTime;
                    DateTime nextCheck = idRecord.CheckTime;
                    switch (idRecord.CheckIntervalType)
                    {
                        case "天":
                            nextCheck = nextCheck.AddDays(idRecord.CheckInterval);
                            break;
                        case "月":
                            nextCheck = nextCheck.AddMonths((int)idRecord.CheckInterval);
                            break;
                        case "年":
                            nextCheck = nextCheck.AddYears((int)idRecord.CheckInterval);
                            break;
                    }
                    idRecord.NextCheckTime = nextCheck;
                    idRecord.SyncStatus = 0;
                    session.SaveOrUpdate(idRecord);
                    return 1;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        public static int SetToolStatus(string toolCode, int status, string operatorName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("ToolCode", toolCode));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolInfo idRecord = (ToolInfo)recordList[0];
                            idRecord.ToolStatus = status;
                            idRecord.Operator = operatorName;
                            idRecord.OperateTime = DateTime.Now;
                            idRecord.SyncStatus = 0;
                            session.SaveOrUpdate(idRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SetToolStatusById(string toolId, int status, string operatorName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("Id", toolId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolInfo idRecord = (ToolInfo)recordList[0];
                            idRecord.ToolStatus = status;
                            idRecord.Operator = operatorName;
                            idRecord.OperateTime = DateTime.Now;
                            idRecord.SyncStatus = 0;
                            session.SaveOrUpdate(idRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int UpdateRtStatus(CabinetCallback.ToolStatus[] toolStatusList)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("ToolPositionType", 0));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < recordList.Count; i++)
                            {
                                ToolInfo idRecord = (ToolInfo)recordList[i];
                                if (idRecord.ToolPosition >= 0)
                                {
                                    try
                                    {
                                        idRecord.RtStatus = toolStatusList[idRecord.ToolPosition - 1].Status;
                                        idRecord.SyncStatus = 0;
                                        session.SaveOrUpdate(idRecord);
                                    }
                                    catch (Exception)
                                    {
                                        // tool info not set,ignore
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SetRtStatus(string toolCode, int status)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("ToolCode", toolCode));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolInfo idRecord = (ToolInfo)recordList[0];
                            idRecord.RtStatus = status;
                            idRecord.SyncStatus = 0;
                            session.SaveOrUpdate(idRecord);
                            transaction.Commit();
                        }
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        /// <summary>
        /// 查询工具信息
        /// </summary>
        /// <returns></returns>
        public static ToolInfo GetToolInfoByHardwareId(string hardwareId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("HardwareId", hardwareId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (ToolInfo)recordList[0];
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 查询工具信息
        /// </summary>
        /// <returns></returns>
        public static ToolInfo GetToolInfoById(string toolId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("Id", toolId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (ToolInfo)recordList[0];
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 查询工具信息
        /// </summary>
        /// <returns></returns>
        public static ToolInfo GetToolInfo(string toolCode)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("ToolCode", toolCode));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (ToolInfo)recordList[0];
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 查询工具信息
        /// </summary>
        /// <returns></returns>
        public static ToolInfo GetToolInfoByCard(string cardId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("CardId", cardId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (ToolInfo)recordList[0];
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 查询工具信息
        /// </summary>
        /// <returns></returns>
        public static ToolInfo GetUpperToolInfo(int toolPosition)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("ToolPositionType", 0))
                        .Add(Restrictions.Eq("ToolPosition", toolPosition));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (ToolInfo)recordList[0];
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList GetToolCategory()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo));

                    ProjectionList reqProjections = Projections.ProjectionList();
                    reqProjections.Add(Projections.Property("ToolTypeId"));
                    reqProjections.Add(Projections.Property("ToolType"));
                    pQuery.SetProjection(Projections.Distinct(reqProjections));

                    pQuery = pQuery.AddOrder(Order.Asc("ToolTypeId"));
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }


        public static IList SearchDrawerToolInfo()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("ToolPositionType", 10));
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList SearchToolInfo(int toolPositionType, int toolPosition, string toolName, string toolSpec, string toolType)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .AddOrder(Order.Asc("ToolPositionType"))
                        .AddOrder(Order.Asc("ToolPosition"));
                    if (toolPositionType >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("ToolPositionType", toolPositionType));
                        pQuery = pQuery.Add(Restrictions.Ge("ToolPosition", 0));
                    }
                    if (toolPosition >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("ToolPosition", toolPosition));
                    }
                    else
                    {
                        pQuery = pQuery.Add(Restrictions.Ge("ToolPosition", toolPosition));
                    }
                    if (!string.IsNullOrEmpty(toolName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolName", toolName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolSpec", toolSpec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolType))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolType", toolType, MatchMode.Anywhere));
                    }
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList SearchChangedToolInfo()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .AddOrder(Order.Asc("ToolPositionType"))
                        .AddOrder(Order.Asc("ToolPosition"))
                        .Add(Restrictions.Ge("ToolPosition", -1))
                        .Add(Restrictions.Eq("SyncStatus", 0));
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList SearchExpiredToolInfo(int toolPositionType)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Le("NextCheckTime", DateTime.Now))
                        .AddOrder(Order.Asc("ToolPositionType"))
                        .AddOrder(Order.Asc("ToolPosition"));
                    if (toolPositionType >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("ToolPositionType", toolPositionType));
                    }
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int UpdateServerInfo(IList serverToolList)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        for (int i = 0; i < serverToolList.Count; i++)
                        {
                            ToolInfo srvInfo = (ToolInfo)serverToolList[i];
                            ToolInfo localInfo = GetToolInfoByServerId(session, srvInfo.ServerIdent);
                            if (localInfo == null)
                            {
                                localInfo = new ToolInfo()
                                {
                                    Id = srvInfo.ServerIdent,
                                    ToolPositionType = 0,
                                    ToolPosition = int.Parse(srvInfo.Comment),
                                    ToolCode = srvInfo.Comment,
                                    HardwareId = srvInfo.Comment,
                                    CheckInterval = 0,
                                    CheckTime = DateTime.Now,
                                    NextCheckTime = Env.MaxTime,
                                    ToolName = srvInfo.ToolName,
                                    ToolTypeId = srvInfo.ToolTypeId,
                                    ToolType = srvInfo.ToolType,
                                    ToolSpec = srvInfo.ToolSpec,
                                    Comment = srvInfo.Comment,
                                    OperateTime = srvInfo.OperateTime,

                                    SyncStatus = 1
                                };
                                session.Save(localInfo);
                            }
                            else
                            {
                                localInfo.ToolName = srvInfo.ToolName;
                                localInfo.ToolTypeId = srvInfo.ToolTypeId;
                                localInfo.ToolType = srvInfo.ToolType;
                                localInfo.ToolSpec = srvInfo.ToolSpec;
                                localInfo.Comment = srvInfo.Comment;
                                localInfo.OperateTime = srvInfo.OperateTime;
                                session.Update(localInfo);
                            }
                        }
                        transaction.Commit();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static ToolInfo GetToolInfoByServerId(ISession session, string codeNo)
        {
            try
            {
                ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                    .Add(Restrictions.Eq("Id", codeNo));
                IList recordList = pQuery.List();
                if (SqlDataHelper.IsDataValid(recordList))
                {
                    return (ToolInfo)recordList[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }
    }
}
