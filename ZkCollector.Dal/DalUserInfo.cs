using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using ZkCollector.Dal.NhUtils;

namespace ZkCollector.Dal
{
    public class DalUserInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int SearchUserInfo(string fullName, string orgId, out IList<UserInfo> userList)
        {
            userList = null;
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo));
                    if (!string.IsNullOrEmpty(fullName))
                    {
                        pQuery = pQuery.Add(Restrictions.Like("FullName", fullName, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(orgId))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("OrgId", orgId));
                    }
                    userList = pQuery.List<UserInfo>();
                    return userList.Count;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return -200;
            }
        }

        public static IList<UserInfo> SearchUserInfoWithoutTemplateId()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.IsNull("TemplateUserId"));
                    return pQuery.List<UserInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int GetUserInfoCount()
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
            return -200;
        }

        public static int GetUserInfo(string id, out UserInfo userInfo)
        {
            userInfo = null;
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        userInfo = (UserInfo)result[0];
                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return -200;
            }
        }

        public static UserInfo GetUserInfoByUserName(string userName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("UserName", userName));
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

        public static int AddUserInfo(
                    string userName,
                    string password,
                    string fullName,
                    string sex,
                    int age,
                    string tel,
                    string adress,
                    string email,
                    int userState,
                    string createUser,
                    string updateUser,
                    string orgId,
                    string cardNum,
                    string empName,
                    byte[] leftTemplate,
                    byte[] rightTemplate,
                    string newLeftTemplate,
                    string newRightTemplate)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        UserInfo itemRecord = new UserInfo
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            UserName = userName,
                            Password = password,
                            FullName = fullName,
                            Sex = sex,
                            Age = age,
                            Tel = tel,
                            Adress = adress,
                            Email = email,
                            UserState = userState,
                            Createtime = DateTime.Now,
                            CreateUser = createUser,
                            Updatetime = DateTime.Now,
                            UpdateUser = updateUser,
                            OrgId = orgId,
                            CardNum = cardNum,
                            EmpName = empName,
                            LeftTemplate = leftTemplate,
                            RightTemplate = rightTemplate,
                            NewLeftTemplate = newLeftTemplate,
                            NewRightTemplate = newRightTemplate,
                        };
                        session.SaveOrUpdate(itemRecord);
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

        public static int ModifyUserInfo(string id, string fullName, string sex, int age,
                    string tel, string orgId, string empName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            UserInfo itemRecord = (UserInfo)result[0];
                            itemRecord.FullName = fullName;
                            itemRecord.Sex = sex;
                            itemRecord.Age = age;
                            itemRecord.Tel = tel;
                            itemRecord.Updatetime = DateTime.Now;
                            itemRecord.OrgId = orgId;
                            itemRecord.EmpName = empName;
                            session.SaveOrUpdate(itemRecord);
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

        public static int ModifyUserFeature(string id, int type, string feature)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            UserInfo itemRecord = (UserInfo)result[0];
                            switch (type)
                            {
                                case 0:
                                    itemRecord.NewLeftTemplate = feature;
                                    break;
                                case 1:
                                    itemRecord.NewRightTemplate = feature;
                                    break;
                                case 2:
                                    itemRecord.LeftTemplateV10 = feature;
                                    break;
                                case 3:
                                    itemRecord.RightTemplateV10 = feature;
                                    break;
                            }
                            itemRecord.Updatetime = DateTime.Now.AddMinutes(5);
                            session.SaveOrUpdate(itemRecord);
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

        public static int BatchUpdateUserInfo(IList<UserInfo> recordList)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        for (int i = 0; i < recordList.Count; i++)
                        {
                            session.Update(recordList[i]);
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

        public static int UpdateUserInfo(UserInfo itemRecord)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        itemRecord.Updatetime = DateTime.Now;
                        session.Update(itemRecord);
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

        public static int DeleteUserInfo(string id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(UserInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            UserInfo itemRecord = (UserInfo)result[0];
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
