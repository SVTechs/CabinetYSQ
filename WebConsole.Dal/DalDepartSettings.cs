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
    public class DalDepartSettings
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList SearchDepartSettings()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(DepartSettings));
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static DepartSettings GetDepartSettings(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(DepartSettings))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (DepartSettings)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<DepartSettings> GetUserDepartSettings(string userId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(DepartSettings))
                        .Add(Restrictions.Eq("UserId", userId));
                    return pQuery.List<DepartSettings>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int SetUserDepart(string userId, string[] departId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //清除原有权限
                        int result = session.CreateQuery(" delete from DepartSettings where UserId = :userId")
                            .SetString("userId", userId)
                            .ExecuteUpdate();
                        if (result < 0)
                        {
                            return -201;
                        }
                        //加入现有权限
                        for (int i = 0; i < departId.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(departId[i])) continue;
                            if (departId[i].ToUpper().Equals("ROOT")) continue;
                            DepartSettings rs = new DepartSettings
                            {
                                Id = Guid.NewGuid().ToString().ToUpper(),
                                UserId = userId,
                                DepartId = departId[i],
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

        public static int DeleteUserDepartSettings(string userId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //清除用户部门
                        int result = session.CreateQuery(" delete from DepartSettings where UserId = :userId")
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
    }
}
