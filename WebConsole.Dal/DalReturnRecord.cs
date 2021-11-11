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
    public class DalReturnRecord
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int DeleteAll()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ReturnRecord));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < recordList.Count; i++)
                            {
                                ReturnRecord idRecord = (ReturnRecord)recordList[i];
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

        public static ReturnRecord GetReturnRecord(string borrowRecordId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ReturnRecord))
                        .Add(Restrictions.Eq("BorrowRecord", borrowRecordId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (ReturnRecord)recordList[0];
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


        public static IList<ReturnRecord> SearchReturnRecord(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ReturnRecord))
                        .Add(Restrictions.Ge("EventTime", timeStart))
                        .Add(Restrictions.Lt("EventTime", timeEnd));
                    pQuery = pQuery.AddOrder(Order.Desc("EventTime"));
                    return pQuery.List<ReturnRecord>();
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
