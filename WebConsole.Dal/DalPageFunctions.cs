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
    public class DalPageFunctions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<PageFunctions> SearchPageFunctions()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PageFunctions))
                        .AddOrder(Order.Asc("FunctionMenu"))
                        .AddOrder(Order.Asc("FunctionOrder"));
                    return pQuery.List<PageFunctions>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static PageFunctions GetPageFunctions(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PageFunctions))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (PageFunctions)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int AddPageFunction(string funcName, int funcOrder, string funcMenu, string funcDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        PageFunctions riRecord = new PageFunctions
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            FunctionName = funcName,
                            FunctionOrder = funcOrder,
                            FunctionMenu = funcMenu,
                            FunctionDesp = funcDesp
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

        public static int ModifyPageFunction(string funcId, string funcName, int funcOrder, string funcDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ICriteria pQuery = session.CreateCriteria(typeof(PageFunctions))
                            .Add(Restrictions.Eq("Id", funcId));
                        IList result = pQuery.List();
                        if (SqlDataHelper.IsDataValid(result))
                        {
                            PageFunctions idRecord = (PageFunctions)result[0];
                            idRecord.FunctionName = funcName;
                            idRecord.FunctionOrder = funcOrder;
                            idRecord.FunctionDesp = funcDesp;
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

        public static int DeletePageFunctions(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PageFunctions))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            PageFunctions itemRecord = (PageFunctions)result[0];
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

        public static int DeletePageFunctionsByMenu(ISession session, string menuId)
        {
            try
            {
                    ICriteria pQuery = session.CreateCriteria(typeof(PageFunctions))
                        .Add(Restrictions.Eq("FunctionMenu", menuId));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            PageFunctions itemRecord = (PageFunctions) result[i];
                            session.Delete(itemRecord);
                        }
                    }
                    return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }
    }
}
