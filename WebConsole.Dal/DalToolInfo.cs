using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalToolInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<ToolInfo> SearchToolInfo()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo));
                    return pQuery.List<ToolInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<ToolInfo> SearchToolInfo(int toolPositionType, int toolPosition, string toolName, string toolSpec, string cabinetName)
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
                    }
                    if (toolPosition >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("ToolPosition", toolPosition));
                    }
                    if (!string.IsNullOrEmpty(toolName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolName", toolName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolSpec", toolSpec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(cabinetName))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", cabinetName));
                    }
                    return pQuery.List<ToolInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<ToolInfo> SearchToolInfo(string toolName, string toolSpec, string cabinetName, int dataStart, int dataCount)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .AddOrder(Order.Asc("DataOwner"))
                        .AddOrder(Order.Asc("ToolPosition"));
                    if (!string.IsNullOrEmpty(toolName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolName", toolName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolSpec", toolSpec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(cabinetName))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", cabinetName));
                    }
                    pQuery.SetFirstResult(dataStart);
                    pQuery.SetMaxResults(dataCount);
                    return pQuery.List<ToolInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }


        public static int GetToolInfoCount()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo));
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int GetToolInfoCount(string toolName, string toolSpec, string dataOwner)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo));
                    if (!string.IsNullOrEmpty(toolName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolName", toolName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(toolSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("ToolSpec", toolSpec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(dataOwner))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", dataOwner));
                    }
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static ToolInfo GetToolInfo(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (ToolInfo)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<string> GetToolType()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .SetProjection(Projections.Distinct(Projections.Property("ToolType")));
                    return pQuery.List<string>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<string> GetToolNameList()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .SetProjection(Projections.Distinct(Projections.Property("ToolName")));
                    return pQuery.List<string>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int AddToolInfo(
            string toolName,
            string toolCode,
            string toolSpec,
            string toolType,
            string hardwareId,
            string cardId,
            string standardRange,
            decimal deviationPositive,
            decimal deviationNegative,
            int toolPositionType,
            int toolPosition,
            string toolGrid,
            DateTime checkTime,
            DateTime nextCheckTime,
            float checkInterval,
            string checkIntervalType,
            string toolManager,
            string comment,
            string toolOperator,
            DateTime operateTime,
            int rtStatus,
            int toolStatus,
            string dataOwner)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ToolInfo itemRecord = new ToolInfo
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
                            CheckTime = checkTime,
                            NextCheckTime = nextCheckTime,
                            CheckInterval = checkInterval,
                            CheckIntervalType = checkIntervalType,
                            ToolManager = toolManager,
                            Comment = comment,
                            Operator = toolOperator,
                            OperateTime = operateTime,
                            RtStatus = rtStatus,
                            ToolStatus = toolStatus,
                            DataOwner = dataOwner,
                        };
                        session.SaveOrUpdate(itemRecord);
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

        public static int ModifyToolInfo(
            string id,
            string toolName,
            string toolCode,
            string toolSpec,
            string toolType,
            string hardwareId,
            string cardId,
            string standardRange,
            decimal deviationPositive,
            decimal deviationNegative,
            int toolPositionType,
            int toolPosition,
            string toolGrid,
            DateTime checkTime,
            DateTime nextCheckTime,
            float checkInterval,
            string checkIntervalType,
            string toolManager,
            string comment,
            string toolOperator,
            DateTime operateTime,
            int rtStatus,
            int toolStatus,
            string dataOwner)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolInfo itemRecord = (ToolInfo)result[0];
                            itemRecord.ToolName = toolName;
                            itemRecord.ToolCode = toolCode;
                            itemRecord.ToolSpec = toolSpec;
                            itemRecord.ToolType = toolType;
                            itemRecord.HardwareId = hardwareId;
                            itemRecord.CardId = cardId;
                            itemRecord.StandardRange = standardRange;
                            itemRecord.DeviationPositive = deviationPositive;
                            itemRecord.DeviationNegative = deviationNegative;
                            itemRecord.ToolPositionType = toolPositionType;
                            itemRecord.ToolPosition = toolPosition;
                            itemRecord.ToolGrid = toolGrid;
                            itemRecord.CheckTime = checkTime;
                            itemRecord.NextCheckTime = nextCheckTime;
                            itemRecord.CheckInterval = checkInterval;
                            itemRecord.CheckIntervalType = checkIntervalType;
                            itemRecord.ToolManager = toolManager;
                            itemRecord.Comment = comment;
                            itemRecord.Operator = toolOperator;
                            itemRecord.OperateTime = operateTime;
                            itemRecord.RtStatus = rtStatus;
                            itemRecord.ToolStatus = toolStatus;
                            itemRecord.DataOwner = dataOwner;
                            session.SaveOrUpdate(itemRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int BatchUpdateToolInfo(IList<ToolInfo> recordList)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        for (int i = 0; i < recordList.Count; i++)
                        {
                            session.Update(recordList[i]);
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

        public static int UpdateToolInfo(ToolInfo itemRecord)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(itemRecord);
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

        public static int DeleteToolInfo(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolInfo itemRecord = (ToolInfo)result[0];
                            session.Delete(itemRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }
    }
}
