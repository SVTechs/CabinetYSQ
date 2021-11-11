using System;
using System.Collections;
using CabinetMgr.Dal.NhUtils;
using Domain.Main.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;

namespace CabinetMgr.Dal
{
    public class DalReturnRecord
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int AddReturnRecord(int toolPosition, string workerId, string workerName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        DateTime retTime = DateTime.Now;
                        BorrowRecord br = DalBorrowRecord.GetBorrowRecordFr(toolPosition);
                        string borrowId = br == null ? "" : br.Id;
                        //保存还回记录
                        ReturnRecord idRecord = new ReturnRecord()
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            BorrowRecord = borrowId,
                            WorkerId = workerId,
                            WorkerName = workerName,
                            EventTime = retTime,
                            SyncStatus = 0
                        };
                        session.SaveOrUpdate(idRecord);
                        //更新借取记录
                        if (br != null)
                        {
                            br.ExpireComment = "";
                            br.Status = 20;
                            br.ReturnTime = retTime;
                            br.SyncStatus = 0;
                            session.SaveOrUpdate(br);
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

    }
}
