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
    public class DalDepartInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int GetDepartCount()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(DepartInfo));
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return 0;
        }

        public static IList<DepartInfo> SearchDepartInfo()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(DepartInfo))
                        .AddOrder(Order.Asc("TreeLevel"))
                        .AddOrder(Order.Asc("DepartOrder"));
                    return pQuery.List<DepartInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static DepartInfo GetDepartInfo(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(DepartInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (DepartInfo)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int AddDepartInfo(string departName, int departLevel, string departParent,
            int departOrder, string departDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        DepartInfo riRecord = new DepartInfo
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            DepartName = departName,
                            TreeLevel = departLevel,
                            TreeParent = departParent,
                            DepartOrder = departOrder,
                            DepartDesp = departDesp,
                            LastChanged = DateTime.Now
                        };
                        session.SaveOrUpdate(riRecord);
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

        public static int ModifyDepartInfo(string departId, string departName, int departOrder, string departDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ICriteria pQuery = session.CreateCriteria(typeof(DepartInfo))
                            .Add(Restrictions.Eq("Id", departId));
                        IList result = pQuery.List();
                        if (SqlDataHelper.IsDataValid(result))
                        {
                            DepartInfo idRecord = (DepartInfo)result[0];
                            idRecord.DepartName = departName;
                            idRecord.DepartOrder = departOrder;
                            idRecord.DepartDesp = departDesp;
                            idRecord.LastChanged = DateTime.Now;
                            session.SaveOrUpdate(idRecord);
                            transaction.Commit();
                            return 1;
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        public static int DeleteDepartInfo(string id)
        {
            try
            {
                IList<DepartInfo> departList = SearchDepartInfo();
                IList delList = new ArrayList(), queueList = new ArrayList();
                //删除指定部门
                for (int i = 0; i < departList.Count; i++)
                {
                    if (departList[i].Id == id)
                    {
                        delList.Add(departList[i]);
                    }
                    if (departList[i].TreeParent == id)
                    {
                        delList.Add(departList[i]);
                        queueList.Add(departList[i].Id);
                    }
                }
                //删除所有子级关联菜单
                while (queueList.Count > 0)
                {
                    string parentId = (string)queueList[0];
                    for (int i = 0; i < departList.Count; i++)
                    {
                        if (departList[i].TreeParent == parentId)
                        {
                            delList.Add(departList[i]);
                            queueList.Add(departList[i].Id);
                        }
                    }
                    queueList.RemoveAt(0);
                }
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        for (int i = 0; i < delList.Count; i++)
                        {
                            DepartInfo idRecord = (DepartInfo)delList[i];
                            session.Delete(idRecord);
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
    }
}
