using System;
using System.Collections;
using System.Collections.Generic;
//using Domain.Qcdevice.Domain;
using Domain.ServerMain.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalRoleInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int GetRoleCount()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo));
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return 0;
        }

        public static IList<RoleInfo> SearchRoleInfo()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RoleInfo))
                        .AddOrder(Order.Asc("TreeLevel"))
                        .AddOrder(Order.Asc("RoleOrder"));
                    return pQuery.List<RoleInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static RoleInfo GetRoleInfo(string roleId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RoleInfo))
                        .Add(Restrictions.Eq("Id", roleId));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (RoleInfo)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int AddRoleInfo(string roleName, int roleLevel, string roleParent, int roleOrder, string roleDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        RoleInfo riRecord = new RoleInfo
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            RoleName = roleName,
                            TreeLevel = roleLevel,
                            TreeParent = roleParent,
                            RoleOrder = roleOrder,
                            RoleDesp = roleDesp,
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

        public static int ModifyRoleInfo(string roleId, string roleName, int roleOrder, string roleDesp)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ICriteria pQuery = session.CreateCriteria(typeof(RoleInfo))
                            .Add(Restrictions.Eq("Id", roleId));
                        IList result = pQuery.List();
                        if (SqlDataHelper.IsDataValid(result))
                        {
                            RoleInfo idRecord = (RoleInfo)result[0];
                            idRecord.RoleName = roleName;
                            idRecord.RoleOrder = roleOrder;
                            idRecord.RoleDesp = roleDesp;
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

        public static int DeleteRoleInfo(string roleId)
        {
            try
            {
                IList<RoleInfo> roleList = SearchRoleInfo();
                IList delList = new ArrayList(), queueList = new ArrayList();
                //删除指定角色
                for (int i = 0; i < roleList.Count; i++)
                {
                    if (roleList[i].Id == roleId)
                    {
                        delList.Add(roleList[i]);
                    }
                    if (roleList[i].TreeParent == roleId)
                    {
                        delList.Add(roleList[i]);
                        queueList.Add(roleList[i].Id);
                    }
                }
                //删除所有子级关联菜单
                while (queueList.Count > 0)
                {
                    string parentId = (string)queueList[0];
                    for (int i = 0; i < roleList.Count; i++)
                    {
                        if (roleList[i].TreeParent == parentId)
                        {
                            delList.Add(roleList[i]);
                            queueList.Add(roleList[i].Id);
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
                            RoleInfo idRecord = (RoleInfo)delList[i];
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
