using System;
using System.Collections;
using System.Collections.Generic;
using CabinetMgr.Dal.NhUtils;
using Domain.Main.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;

namespace CabinetMgr.Dal
{
    public class DalUserInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static UserInfo GetUser(int templateId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("EnrollId", templateId.ToString()));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (UserInfo)recordList[0];
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

        public static UserInfo GetUser(string userName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("UserName", userName));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (UserInfo)recordList[0];
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

        public static IList SearchUserByGroup(string orgId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("OrgId", orgId));
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<UserInfo> SearchUserByGroup(string []orgId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.In("OrgId", orgId));
                    return pQuery.List<UserInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<UserInfo> SearchUserNotRegistered()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Not(Restrictions.Eq("FpRegistered", 1)));
                    return pQuery.List<UserInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int SetAsRegistered(IList<UserInfo> userList)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        for (int i = 0; i < userList.Count; i++)
                        {
                            userList[i].FpRegistered = 1;
                            session.Update(userList[i]);
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

        /*
        public static Domain.Qcshkf.Domain.UserInfo GetServerUser(int templateId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(CabinetServerDomainQcshkf.Domain.UserInfo))
                        .Add(Restrictions.Eq("TemplateUserId", templateId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (CabinetServerDomainQcshkf.Domain.UserInfo)recordList[0];
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }*/
    }
}
