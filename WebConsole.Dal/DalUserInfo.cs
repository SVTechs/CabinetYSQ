using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
//using Domain.Qcdevice.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalUserInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int GetUserCount(string userName, string realName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo));
                    if (!string.IsNullOrEmpty(userName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("UserName", userName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(realName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("FullName", realName, MatchMode.Anywhere));
                    }
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return 0;
        }

        public static IList<UserInfo> SearchUser(string userName, string realName, int dataStart, int dataCount)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo));
                    if (!string.IsNullOrEmpty(userName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("UserName", userName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(realName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("FullName", realName, MatchMode.Anywhere));
                    }
                    pQuery = pQuery.AddOrder(Order.Asc("Id"));
                    pQuery.SetFirstResult(dataStart);
                    pQuery.SetMaxResults(dataCount);
                    return pQuery.List<UserInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<UserInfo> SearchUserByOrg(string orgId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo));
                    if (!string.IsNullOrEmpty(orgId))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("OrgId", orgId));
                    }
                    pQuery = pQuery.AddOrder(Order.Asc("Id"));
                    return pQuery.List<UserInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static IList<object> GetOrgs()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession(new SQLWatcher()))
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .SetProjection(Projections.Distinct(Projections.Property("OrgId")));
                    return (IList<object>)pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static UserInfo GetUserInfo(string userId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("Id", userId));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (UserInfo)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static UserInfo GetUserInfo(string userName, string userPwd)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("UserName", userName))
                        .Add(Restrictions.Eq("Password", userPwd));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (UserInfo)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static string AddUserInfo(string userName, string pwdHash, string realName, string userTel)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        string userId = Guid.NewGuid().ToString().ToUpper();
                        UserInfo uiRecord = new UserInfo
                        {
                            Id = userId,
                            UserName = userName,
                            Password = pwdHash,
                            FullName = realName,
                            Tel = userTel,
                            Updatetime = DateTime.Now
                        };
                        session.SaveOrUpdate(uiRecord);
                        transaction.Commit();
                        return userId;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return null;
        }

        public static int ModifyUserInfo(string userId, string userName, string pwdHash,
            string realName, string userTel)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                            .Add(Restrictions.Eq("Id", userId));
                        IList result = pQuery.List();
                        if (SqlDataHelper.IsDataValid(result))
                        {
                            UserInfo idRecord = (UserInfo)result[0];
                            idRecord.UserName = userName;
                            if (!string.IsNullOrEmpty(pwdHash)) idRecord.Password = pwdHash;
                            idRecord.FullName = realName;
                            idRecord.Tel = userTel;
                            idRecord.Updatetime = DateTime.Now;
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

        public static int ModifyUserInfo(string userId, string pwdHash, string userTel)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                            .Add(Restrictions.Eq("Id", userId));
                        IList result = pQuery.List();
                        if (SqlDataHelper.IsDataValid(result))
                        {
                            UserInfo idRecord = (UserInfo)result[0];
                            if (!string.IsNullOrEmpty(pwdHash)) idRecord.Password = pwdHash;
                            idRecord.Tel = userTel;
                            idRecord.Updatetime = DateTime.Now;
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

        public static int DeleteUserInfo(string userId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("Id", userId));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            UserInfo idRecord = (UserInfo)result[0];
                            session.Delete(idRecord);
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
    }
    //public class DalUserInfo
    //{
    //    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    //    public static int GetUserCount(string userName, string realName)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                ICriteria pQuery = session.CreateCriteria(typeof(UserInfo));
    //                if (!string.IsNullOrEmpty(userName))
    //                {
    //                    pQuery = pQuery.Add(Restrictions.Like("UserName", userName, MatchMode.Anywhere));
    //                }
    //                if (!string.IsNullOrEmpty(realName))
    //                {
    //                    pQuery = pQuery.Add(Restrictions.Like("FullName", realName, MatchMode.Anywhere));
    //                }
    //                return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Error(ex);
    //        }
    //        return 0;
    //    }

    //    public static IList<UserInfo> SearchUser(string userName, string realName, int dataStart, int dataCount)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                ICriteria pQuery = session.CreateCriteria(typeof(UserInfo));
    //                if (!string.IsNullOrEmpty(userName))
    //                {
    //                    pQuery = pQuery.Add(Restrictions.Like("UserName", userName, MatchMode.Anywhere));
    //                }
    //                if (!string.IsNullOrEmpty(realName))
    //                {
    //                    pQuery = pQuery.Add(Restrictions.Like("FullName", realName, MatchMode.Anywhere));
    //                }
    //                pQuery = pQuery.AddOrder(Order.Asc("Id"));
    //                pQuery.SetFirstResult(dataStart);
    //                pQuery.SetMaxResults(dataCount);
    //                return pQuery.List<UserInfo>();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Error(ex);
    //        }
    //        return null;
    //    }

    //    public static UserInfo GetUserInfo(string userId)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
    //                    .Add(Restrictions.Eq("Id", userId));
    //                IList result = pQuery.List();
    //                if (SqlDataHelper.IsDataValid(result))
    //                {
    //                    return (UserInfo)result[0];
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Error(ex);
    //        }
    //        return null;
    //    }

    //    public static UserInfo GetUserInfo(string userName, string userPwd)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
    //                    .Add(Restrictions.Eq("UserName", userName))
    //                    .Add(Restrictions.Eq("Password", userPwd));
    //                IList result = pQuery.List();
    //                if (SqlDataHelper.IsDataValid(result))
    //                {
    //                    return (UserInfo)result[0];
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Error(ex);
    //        }
    //        return null;
    //    }

    //    public static string AddUserInfo(string userName, string pwdHash, string realName, string userTel)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                using (var transaction = session.BeginTransaction())
    //                {
    //                    string userId = Guid.NewGuid().ToString().ToUpper();
    //                    UserInfo uiRecord = new UserInfo
    //                    {
    //                        Id = userId,
    //                        UserName = userName,
    //                        Password = pwdHash,
    //                        FullName = realName,
    //                        Tel = userTel,
    //                        LastChanged = DateTime.Now
    //                    };
    //                    session.SaveOrUpdate(uiRecord);
    //                    transaction.Commit();
    //                    return userId;
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.Error(e);
    //        }
    //        return null;
    //    }

    //    public static int ModifyUserInfo(string userId, string userName, string pwdHash,
    //        string realName, string userTel)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                using (var transaction = session.BeginTransaction())
    //                {
    //                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
    //                        .Add(Restrictions.Eq("Id", userId));
    //                    IList result = pQuery.List();
    //                    if (SqlDataHelper.IsDataValid(result))
    //                    {
    //                        UserInfo idRecord = (UserInfo)result[0];
    //                        idRecord.UserName = userName;
    //                        if (!string.IsNullOrEmpty(pwdHash)) idRecord.Password = pwdHash;
    //                        idRecord.FullName = realName;
    //                        idRecord.Tel = userTel;
    //                        idRecord.LastChanged = DateTime.Now;
    //                        session.SaveOrUpdate(idRecord);
    //                        transaction.Commit();
    //                        return 1;
    //                    }
    //                    return 0;
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.Error(e);
    //        }
    //        return -200;
    //    }

    //    public static int ModifyUserInfo(string userId, string pwdHash, string userTel)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                using (var transaction = session.BeginTransaction())
    //                {
    //                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
    //                        .Add(Restrictions.Eq("Id", userId));
    //                    IList result = pQuery.List();
    //                    if (SqlDataHelper.IsDataValid(result))
    //                    {
    //                        UserInfo idRecord = (UserInfo)result[0];
    //                        if (!string.IsNullOrEmpty(pwdHash)) idRecord.Password = pwdHash;
    //                        idRecord.Tel = userTel;
    //                        idRecord.LastChanged = DateTime.Now;
    //                        session.SaveOrUpdate(idRecord);
    //                        transaction.Commit();
    //                        return 1;
    //                    }
    //                    return 0;
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.Error(e);
    //        }
    //        return -200;
    //    }

    //    public static int DeleteUserInfo(string userId)
    //    {
    //        try
    //        {
    //            var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcdevice);
    //            using (var session = sessionFactory.OpenSession())
    //            {
    //                ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
    //                    .Add(Restrictions.Eq("Id", userId));
    //                IList result = pQuery.List();
    //                if (SqlDataHelper.IsDataValid(result))
    //                {
    //                    using (var transaction = session.BeginTransaction())
    //                    {
    //                        UserInfo idRecord = (UserInfo)result[0];
    //                        session.Delete(idRecord);
    //                        transaction.Commit();
    //                        return 1;
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.Error(e);
    //        }
    //        return -200;
    //    }
    //}

    public class SQLWatcher : EmptyInterceptor
    {
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            Console.WriteLine(sql.ToString());
            return base.OnPrepareStatement(sql);
        }
        //public virtual NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        //{
        //    return sql;
        //}
    }

}
