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
    public class DalPageMenus
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static PageMenus GetPageMenu(string menuId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PageMenus))
                        .Add(Restrictions.Eq("Id", menuId));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (PageMenus)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<PageMenus> GetPageMenus()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PageMenus))
                        .AddOrder(Order.Asc("TreeLevel"))
                        .AddOrder(Order.Asc("MenuOrder"));
                    return pQuery.List<PageMenus>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int AddPageMenu(string menuName, int menuLevel, string menuParent, int menuOrder,
            int menuType, string menuUrl, string menuIcon, int isVisible, string menuDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        PageMenus piRecord = new PageMenus
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            MenuName = menuName,
                            TreeLevel = menuLevel,
                            TreeParent = menuParent,
                            MenuOrder = menuOrder,
                            MenuType = menuType,
                            MenuUrl = menuUrl,
                            MenuIcon = menuIcon,
                            IsVisible = isVisible,
                            MenuDesp = menuDesp
                        };
                        session.SaveOrUpdate(piRecord);
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

        public static int ModifyPageMenu(string menuId, string menuName, int menuOrder,
            int menuType, string menuUrl, string menuIcon, int isVisible, string menuDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ICriteria pQuery = session.CreateCriteria(typeof(PageMenus))
                            .Add(Restrictions.Eq("Id", menuId));
                        IList result = pQuery.List();
                        if (SqlDataHelper.IsDataValid(result))
                        {
                            PageMenus idRecord = (PageMenus)result[0];
                            idRecord.MenuName = menuName;
                            idRecord.MenuOrder = menuOrder;
                            idRecord.MenuType = menuType;
                            idRecord.MenuUrl = menuUrl;
                            idRecord.MenuIcon = menuIcon;
                            idRecord.IsVisible = isVisible;
                            idRecord.MenuDesp = menuDesp;
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

        public static int DeletePageMenu(string menuId)
        {
            try
            {
                IList<PageMenus> pageMenus = GetPageMenus();
                IList delList = new ArrayList(), queueList = new ArrayList();
                //删除指定菜单
                for (int i = 0; i < pageMenus.Count; i++)
                {
                    if (pageMenus[i].Id == menuId)
                    {
                        delList.Add(pageMenus[i]);
                    }
                    if (pageMenus[i].TreeParent == menuId)
                    {
                        delList.Add(pageMenus[i]);
                        queueList.Add(pageMenus[i].Id);
                    }
                }
                //删除所有子级关联菜单
                while (queueList.Count > 0)
                {
                    string parentId = (string)queueList[0];
                    for (int i = 0; i < pageMenus.Count; i++)
                    {
                        if (pageMenus[i].TreeParent == parentId)
                        {
                            delList.Add(pageMenus[i]);
                            queueList.Add(pageMenus[i].Id);
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
                            PageMenus idRecord = (PageMenus)delList[i];
                            session.Delete(idRecord);
                            if (idRecord.MenuType == 1)
                            {
                                int result = DalPageFunctions.DeletePageFunctionsByMenu(session, idRecord.Id);
                                if (result < 0) return -210;
                            }
                        }
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
    }
}
