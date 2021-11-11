using System;
using System.Collections;
using NLog;

// ReSharper disable CoVariantArrayConversion

namespace CabinetMgr.Dal
{
    public class DalWorkUserInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList SearchWorkUserInfo(string workUserId)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WorkUserInfo))
                        .Add(Restrictions.Eq("WorkUserId", workUserId))
                        .Add(Restrictions.Ge("WorkStatus", 0))
                        .Add(Restrictions.Lt("WorkStatus", 15))
                        .AddOrder(Order.Asc("Step"));
                    return pQuery.List();
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int ResetAllStatus()
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WorkUserInfo));
                    IList dataList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(dataList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < dataList.Count; i++)
                            {
                                WorkUserInfo wui = (WorkUserInfo)dataList[i];
                                wui.WorkStatus = 0;
                                session.SaveOrUpdate(wui);
                            }
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SetAsAquired(string id, string userName)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WorkUserInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList dataList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(dataList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            WorkUserInfo wui = (WorkUserInfo) dataList[0];
                            wui.WorkStatus = 10;
                            session.SaveOrUpdate(wui);
                            transaction.Commit();

                            //添加量值记录
                            DalMeasurementData.AddMeasurementData(wui.Id, wui.DeviceCode, wui.TrainType, wui.TrainNum,
                                wui.Process, wui.ToolCode, int.Parse(wui.ToolType), wui.DefaultJobValue, userName);
                            return 1;
                        }
                    }
                    return 0;
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SetAsReturned(string id)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WorkUserInfo))
                        .Add(Restrictions.Eq("Id", id));
                    IList dataList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(dataList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            WorkUserInfo wui = (WorkUserInfo)dataList[0];
                            wui.WorkStatus = 15;
                            session.SaveOrUpdate(wui);
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SetAsAquired(string []idArray, string userName)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WorkUserInfo))
                        .Add(Restrictions.In("Id", idArray));
                    IList dataList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(dataList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < dataList.Count; i++)
                            {
                                WorkUserInfo wui = (WorkUserInfo)dataList[i];
                                if (wui.WorkStatus == 0) wui.WorkStatus = 10;
                                session.SaveOrUpdate(wui);

                                //添加量值记录
                                DalMeasurementData.AddMeasurementData(wui.Id, wui.DeviceCode, wui.TrainType, wui.TrainNum,
                                    wui.Process, wui.ToolCode, int.Parse(wui.ToolType), wui.DefaultJobValue, userName);
                            }
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SetAsTaken(int[] positionArray)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WorkUserInfo))
                        .Add(Restrictions.In("ToolPosition", positionArray));
                    IList dataList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(dataList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < dataList.Count; i++)
                            {
                                WorkUserInfo wui = (WorkUserInfo)dataList[i];
                                wui.WorkStatus = 11;
                                session.SaveOrUpdate(wui);
                            }
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static int SetAsReturned(string[] idArray)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.Qcshkf);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(WorkUserInfo))
                        .Add(Restrictions.In("Id", idArray))
                        .Add(Restrictions.Eq("WorkStatus", 11));
                    IList dataList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(dataList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < dataList.Count; i++)
                            {
                                WorkUserInfo wui = (WorkUserInfo)dataList[i];
                                wui.WorkStatus = 15;
                                session.SaveOrUpdate(wui);
                            }
                            transaction.Commit();
                            return 1;
                        }
                    }
                    return 0;
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }
    }
}
