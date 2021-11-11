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
    public class DalPermissionSettings
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<PermissionSettings> SearchPermissionSettings()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PermissionSettings));
                    return pQuery.List<PermissionSettings>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<PermissionSettings> GetOwnerPermissionSettings(string ownerId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PermissionSettings))
                        .Add(Restrictions.Eq("OwnerId", ownerId));
                    return pQuery.List<PermissionSettings>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int SetOwnerPermission(string ownerType, string ownerId, string[] permId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //清除原有权限
                        int result = session.CreateQuery(" delete from PermissionSettings where OwnerId = :ownerId")
                            .SetString("ownerId", ownerId)
                            .ExecuteUpdate();
                        if (result < 0)
                        {
                            return -201;
                        }
                        //加入现有权限
                        for (int i = 0; i < permId.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(permId[i])) continue;
                            if (permId[i].ToUpper().Equals("ROOT")) continue;
                            PermissionSettings rs = new PermissionSettings
                            {
                                Id = Guid.NewGuid().ToString().ToUpper(),
                                OwnerType = ownerType,
                                OwnerId = ownerId,
                                AccessType = permId[i].StartsWith("W-") || permId[i].StartsWith("P-") ? "Menu" : "Func",
                                AccessId = permId[i].Substring(2),
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

        public static int DeleteOwnerPermissionSettings(string ownerId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //清除用户权限
                        int result = session.CreateQuery(" delete from PermissionSettings where OwnerId = :ownerId")
                            .SetString("ownerId", ownerId)
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

        public static int DeletePermissionSettings(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(PermissionSettings))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            PermissionSettings itemRecord = (PermissionSettings)result[0];
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
