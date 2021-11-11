using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Domain.ServerMain.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalWrenchCheckRecord
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<WrenchCheckRecord> SearchWrenchCheckRecord(string wrenchId, int dataStart, int dataCount, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchCheckRecord));
                    if (!string.IsNullOrEmpty(wrenchId))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("WrenchId", wrenchId));
                    }
                    pQuery.SetFirstResult(dataStart);
                    if (dataCount > 0)
                    {
                        pQuery.SetMaxResults(dataCount);
                    }
                    pQuery.AddOrder(Order.Desc("EventTime"));
                    exception = null;
                    return pQuery.List<WrenchCheckRecord>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                exception = ex;
            }
            return null;
        }

        public static int GetWrenchCheckRecordCount(string wrenchId, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchCheckRecord));
                    if (!string.IsNullOrEmpty(wrenchId))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("WrenchId", wrenchId));
                    }

                    exception = null;
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                exception = ex;
            }
            return -200;
        }

        public static WrenchCheckRecord GetWrenchCheckRecord(string id, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchCheckRecord))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    exception = null;
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (WrenchCheckRecord)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                exception = ex;
            }
            return null;
        }

        public static int AddWrenchCheckRecord(string wrenchId, string wrenchName, int wrenchPosition, string workerId, string workerName, DateTime eventTime, string status, string dataOwner, string pdfFile, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        WrenchCheckRecord itemRecord = new WrenchCheckRecord
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            WrenchId = wrenchId,
                            WrenchName = wrenchName,
                            WrenchPosition = wrenchPosition,
                            WorkerId = workerId,
                            WorkerName = workerName,
                            EventTime = eventTime,
                            Status = status,
                            DataOwner = dataOwner,
                            PdfFile = pdfFile,
                        };
                        session.SaveOrUpdate(itemRecord);
                        transaction.Commit();
                        exception = null;
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                exception = ex;
            }
            return -200;
        }

        public static int ModifyWrenchCheckRecord(string id, string wrenchId, string wrenchName, int wrenchPosition, string workerId, string workerName, DateTime eventTime, string status, string dataOwner, string pdfFile, out Exception exception)
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
                            WrenchCheckRecord itemRecord = (WrenchCheckRecord)result[0];
                            itemRecord.WrenchId = wrenchId;
                            itemRecord.WrenchName = wrenchName;
                            itemRecord.WrenchPosition = wrenchPosition;
                            itemRecord.WorkerId = workerId;
                            itemRecord.WorkerName = workerName;
                            itemRecord.EventTime = eventTime;
                            itemRecord.Status = status;
                            itemRecord.DataOwner = dataOwner;
                            itemRecord.PdfFile = pdfFile;
                            session.SaveOrUpdate(itemRecord);
                            transaction.Commit();
                            exception = null;
                            return 1;
                        }
                    }
                    exception = null;
                    return 0;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SaveWrenchCheckRecord(WrenchCheckRecord wrenchCheckRecord, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(wrenchCheckRecord);
                        transaction.Commit();
                        exception = null;
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                exception = ex;
            }
            return -200;
        }

        public static int UpdateWrenchCheckRecord(WrenchCheckRecord wrenchCheckRecord, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(wrenchCheckRecord);
                        transaction.Commit();
                        exception = null;
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                Logger.Error(ex);
            }
            return -200;
        }

        public static int DeleteWrenchCheckRecord(string id, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchCheckRecord))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            WrenchCheckRecord itemRecord = (WrenchCheckRecord)result[0];
                            session.Delete(itemRecord);
                            transaction.Commit();
                            exception = null;
                            return 1;
                        }
                    }
                    exception = null;
                    return 0;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                Logger.Error(ex);
            }
            return -200;
        }


    }
}
