using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalBorrowRecord
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int GetBorrowRecordCount(DateTime timeStart, DateTime timeEnd, string workerId, string cabinetName, int toolStatus)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Ge("EventTime", timeStart))
                        .Add(Restrictions.Lt("EventTime", timeEnd));
                    if (!string.IsNullOrEmpty(workerId))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("WorkerId", workerId));
                    }
                    if (!string.IsNullOrEmpty(cabinetName))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", cabinetName));
                    }
                    if (toolStatus >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("Status", toolStatus));
                    }
                    ProjectionList reqProjections = Projections.ProjectionList().Add(Projections.RowCount());
                    IList recordList = pQuery.SetProjection(reqProjections).List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (int)recordList[0];
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

        public static int GetBorrowRecordCount(string toolId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord));
                    pQuery = pQuery.Add(Restrictions.Eq("ToolId", toolId));
                    //if (!string.IsNullOrEmpty(toolId))
                    //{
                    //    pQuery = pQuery.Add(Restrictions.Eq("ToolId", toolId));
                    //}
                    ProjectionList reqProjections = Projections.ProjectionList().Add(Projections.RowCount());
                    IList recordList = pQuery.SetProjection(reqProjections).List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (int)recordList[0];
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

        public static BorrowRecord GetLastBorrowRecord(string toolId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord));
                    pQuery = pQuery.Add(Restrictions.Eq("ToolId", toolId))
                        .AddOrder(Order.Desc("EventTime"));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return recordList[0] as BorrowRecord;
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

        public static IList<BorrowRecord> SearchBorrowRecord(DateTime timeStart, DateTime timeEnd, string workerId, string cabinetName, int toolStatus, int dataStart, int dataCount)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Ge("EventTime", timeStart))
                        .Add(Restrictions.Lt("EventTime", timeEnd));
                    if (!string.IsNullOrEmpty(workerId))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("WorkerId", workerId));
                    }
                    if (!string.IsNullOrEmpty(cabinetName))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", cabinetName));
                    }
                    if (toolStatus >= 0)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("Status", toolStatus));
                    }
                    pQuery = pQuery.AddOrder(Order.Desc("EventTime"));
                    pQuery.SetFirstResult(dataStart);
                    if (dataCount > 0)
                    {
                        pQuery.SetMaxResults(dataCount);

                    }
                    return pQuery.List<BorrowRecord>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<BorrowRecord> SearchBorrowRecord(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Ge("EventTime", timeStart))
                        .Add(Restrictions.Lt("EventTime", timeEnd));
                    pQuery = pQuery.AddOrder(Order.Desc("EventTime"));
                    return pQuery.List<BorrowRecord>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<BorrowRecord> SearchBorrowRecord(string toolId, int dataStart, int dataCount)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord));
                    pQuery = pQuery.Add(Restrictions.Eq("ToolId", toolId));
                    //if (!string.IsNullOrEmpty(toolId))
                    //{
                    //    pQuery = pQuery.Add(Restrictions.Eq("ToolId", toolId));
                    //}
                    pQuery = pQuery.AddOrder(Order.Desc("EventTime"));
                    pQuery.SetFirstResult(dataStart);
                    pQuery.SetMaxResults(dataCount);
                    return pQuery.List<BorrowRecord>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<BorrowRecord> SearchBorrowRecordsByWorkId(DateTime timeStart, DateTime timeEnd, string cabinetName, string workerId, int dataStart, int dataCount)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Ge("EventTime", timeStart))
                        .Add(Restrictions.Lt("EventTime", timeEnd));
                    if (!string.IsNullOrEmpty(cabinetName))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", cabinetName));
                    }
                    if (!string.IsNullOrEmpty(workerId))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("WorkerId", workerId));
                    }
                    pQuery = pQuery.AddOrder(Order.Desc("EventTime"));
                    pQuery.SetFirstResult(dataStart);
                    pQuery.SetMaxResults(dataCount);
                    return pQuery.List<BorrowRecord>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static bool Borrowed(string workerId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Eq("WorkerId", workerId))
                        .Add(Restrictions.Eq("Status", 0));
                    pQuery = pQuery.AddOrder(Order.Desc("EventTime"));
                    return pQuery.List().Count > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return false;
        }
    }
}
