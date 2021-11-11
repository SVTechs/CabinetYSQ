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
    public class DalRoleSettings
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<RoleSettings> SearchRoleSettings()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RoleSettings));
                    return pQuery.List<RoleSettings>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<RoleSettings> GetUserRoleSettings(string userId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RoleSettings))
                        .Add(Restrictions.Eq("UserId", userId));
                    return pQuery.List<RoleSettings>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static RoleSettings GetRoleSettings(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RoleSettings))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (RoleSettings)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int SetUserRole(string userId, string[]roleId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //清除原有权限
                        int result = session.CreateQuery(" delete from RoleSettings where UserId = :userId")
                            .SetString("userId", userId)
                            .ExecuteUpdate();
                        if (result < 0)
                        {
                            return -201;
                        }
                        //加入现有权限
                        for (int i = 0; i < roleId.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(roleId[i])) continue;
                            if (roleId[i].ToUpper().Equals("ROOT")) continue;
                            RoleSettings rs = new RoleSettings
                            {
                                Id = Guid.NewGuid().ToString().ToUpper(),
                                UserId = userId,
                                RoleId = roleId[i],
                                AddTime = DateTime.Now
                            };
                            session.SaveOrUpdate(rs);
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

        public static int DeleteUserRoleSettings(string userId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //清除用户权限
                        int result = session.CreateQuery(" delete from RoleSettings where UserId = :userId")
                            .SetString("userId", userId)
                            .ExecuteUpdate();
                        if (result < 0)
                        {
                            return -201;
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

        public static int DeleteRoleSettings(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RoleSettings))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            RoleSettings itemRecord = (RoleSettings)result[0];
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
    }
}
