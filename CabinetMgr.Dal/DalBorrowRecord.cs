using System;
using System.Collections;
using CabinetMgr.Config;
using CabinetMgr.Dal.NhUtils;
using Domain.Main.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;

namespace CabinetMgr.Dal
{
    public class DalBorrowRecord
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int AddBorrowRecord(string toolId, string toolName, int toolPosition, string workerId, string workerName, int hardwareId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        BorrowRecord idRecord = new BorrowRecord
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            ToolId = toolId,
                            ToolName = toolName,
                            ToolPosition = toolPosition,
                            HardwareId = hardwareId,
                            WorkerId = workerId,
                            WorkerName = workerName,
                            Status = 0,
                            EventTime = DateTime.Now,
                            ReturnTime = Env.MinTime,
                            SyncStatus = 0
                        };
                        session.SaveOrUpdate(idRecord);
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

        public static int SetAsConfirmed(string borrowId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Eq("Id", borrowId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            BorrowRecord idRecord = (BorrowRecord)recordList[0];
                            idRecord.Status = 10;
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

        public static int AddExpireComment(string borrowId, string comment)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Eq("Id", borrowId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            BorrowRecord idRecord = (BorrowRecord)recordList[0];
                            idRecord.Status = 11;
                            idRecord.ExpireComment = comment;
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

        public static int DeleteAll()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < recordList.Count; i++)
                            {
                                BorrowRecord idRecord = (BorrowRecord)recordList[i];
                                session.Delete(idRecord);
                            }
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

        public static BorrowRecord GetBorrowRecord(int toolPosition, bool showReturned)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Eq("ToolPosition", toolPosition))
                        .AddOrder(Order.Desc("EventTime"));
                    if (!showReturned)
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("Status", 0));
                    }
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (BorrowRecord)recordList[0];
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

        public static BorrowRecord GetLastUnReturnedBorrowRecord(int toolPosition)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Eq("ToolPosition", toolPosition))
                        .AddOrder(Order.Desc("EventTime"));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (BorrowRecord)recordList[0];
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

        public static BorrowRecord GetBorrowRecordFr(int toolPosition, bool showReturned = false)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Eq("ToolPosition", toolPosition))
                        .AddOrder(Order.Desc("EventTime"));
                    if (!showReturned)
                    {
                        pQuery = pQuery.Add(Restrictions.Ge("Status", 0))
                            .Add(Restrictions.Lt("Status", 20));
                    }
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (BorrowRecord)recordList[0];
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

        public static IList SearchBorrowRecord(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(BorrowRecord))
                        .Add(Restrictions.Ge("EventTime", timeStart))
                        .Add(Restrictions.Le("EventTime", timeEnd))
                        .AddOrder(Order.Desc("EventTime"))
                        .SetFirstResult(0)
                        .SetMaxResults(30);
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }
    }
}
