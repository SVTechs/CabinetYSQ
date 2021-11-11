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
using WebConsole.Config;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalWrenchInfo
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<WrenchInfo> SearchWrenchInfo(string wrenchCode, string wrenchSpec, string dataOwner, int dataStart, int dataCount, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchInfo));
                    if (!string.IsNullOrEmpty(wrenchCode))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("WrenchCode", wrenchCode, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(wrenchSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("WrenchSpec", wrenchSpec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(dataOwner))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", dataOwner));
                    }
                    pQuery.SetFirstResult(dataStart);
                    if (dataCount > 0)
                    {
                        pQuery.SetMaxResults(dataCount);
                    }
                    pQuery.AddOrder(Order.Asc("WrenchPosition"))
                        .AddOrder(Order.Asc("DataOwner"));
                    exception = null;
                    return pQuery.List<WrenchInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                exception = ex;
            }
            return null;
        }

        public static int GetWrenchInfoCount(out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolInfo));
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

        public static int GetWrenchInfoCount(string wrenchCode, string wrenchSpec, string dataOwner, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchInfo));
                    if (!string.IsNullOrEmpty(wrenchCode))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("WrenchCode", wrenchCode, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(wrenchSpec))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("WrenchSpec", wrenchSpec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(dataOwner))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("DataOwner", dataOwner));
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

        public static WrenchInfo GetWrenchInfo(string id, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    exception = null;
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (WrenchInfo)result[0];
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

        public static int AddWrenchInfo(string wrenchName, string wrenchCode, string wrenchSpec, string standardRange, int wrenchPosition, int checkInterval, string checkIntervalType, string dataOwner, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        WrenchInfo itemRecord = new WrenchInfo
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            WrenchName = wrenchName,
                            WrenchCode = wrenchCode,
                            WrenchSpec = wrenchSpec,
                            StandardRange = standardRange,
                            WrenchPosition = wrenchPosition,
                            CheckTime = Env.MinTime,
                            NextCheckTime = Env.MinTime,
                            CheckInterval = checkInterval,
                            CheckIntervalType = checkIntervalType,
                            DataOwner = dataOwner,
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

        public static int ModifyWrenchInfo(string id, string wrenchName, string wrenchCode, string wrenchSpec, string standardRange, int wrenchPosition, int checkInterval, string checkIntervalType, string dataOwner, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            WrenchInfo itemRecord = (WrenchInfo)result[0];
                            itemRecord.WrenchName = wrenchName;
                            itemRecord.WrenchCode = wrenchCode;
                            itemRecord.WrenchSpec = wrenchSpec;
                            itemRecord.StandardRange = standardRange;
                            itemRecord.WrenchPosition = wrenchPosition;
                            itemRecord.CheckInterval = checkInterval;
                            itemRecord.CheckIntervalType = checkIntervalType;
                            itemRecord.DataOwner = dataOwner;
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

        public static int SaveWrenchInfo(WrenchInfo wrenchInfo, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(wrenchInfo);
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

        public static int UpdateWrenchInfo(WrenchInfo wrenchInfo, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(wrenchInfo);
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

        public static int DeleteWrenchInfo(string id, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            WrenchInfo itemRecord = (WrenchInfo)result[0];
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

        public static IList<WrenchInfo> SearchWrenchInfoFile(string pdfFile, out Exception exception)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WrenchInfo));
                    if (!string.IsNullOrEmpty(pdfFile))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("PdfFile", pdfFile));
                    }
                    exception = null;
                    return pQuery.List<WrenchInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                exception = ex;
            }
            return null;
        }

    }
}
