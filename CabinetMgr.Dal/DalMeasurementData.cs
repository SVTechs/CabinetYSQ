using System;
using System.Collections;
using CabinetMgr.Dal.NhUtils;
using Domain.Main.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;

namespace CabinetMgr.Dal
{
    public class DalMeasurementData
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int AddMeasurementData(string workUserInfoId, string deviceCode, string trainType,
            string trainNum, string process, string toolCode, int toolType, string defaultJobValue, string taskUser)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //保存检查记录
                        MeasurementData idRecord = new MeasurementData
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            WorkUserInfoId = workUserInfoId,
                            DeviceCode = deviceCode,
                            ToolType = toolType,
                            TrainNum = trainNum,
                            Process = process,
                            ToolCode = toolCode,
                            TrainType = trainType,
                            DefaultJobValue = defaultJobValue,
                            TaskUser = taskUser,
                            CreateDate = DateTime.Now
                        };
                        session.SaveOrUpdate(idRecord);
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

        public static int DeleteAll()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MeasurementData));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            for (int i = 0; i < recordList.Count; i++)
                            {
                                MeasurementData idRecord = (MeasurementData)recordList[i];
                                session.Delete(idRecord);
                            }
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

        public static int SaveResult(string toolCode, float dataValue)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MeasurementData))
                        .Add(Restrictions.Eq("ToolCode", toolCode))
                        .AddOrder(Order.Desc("CreateDate"))
                        .SetFirstResult(0)
                        .SetMaxResults(1);
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            MeasurementData idRecord = (MeasurementData)recordList[0];
                            if (idRecord.DataValue?.Length >= 450) return -210;
                            idRecord.DataValue += dataValue + ",";
                            session.SaveOrUpdate(idRecord);
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

        /// <summary>
        /// 查询测量数据
        /// </summary>
        /// <returns></returns>
        public static MeasurementData GetMeasurementData(string recordId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MeasurementData))
                        .Add(Restrictions.Eq("Id", recordId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (MeasurementData)recordList[0];
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

        /// <summary>
        /// 查询测量数据
        /// </summary>
        /// <returns></returns>
        public static MeasurementData GetMeasurementDataByToolCode(string toolCode)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(MeasurementData))
                        .Add(Restrictions.Eq("ToolCode", toolCode))
                        .AddOrder(Order.Desc("CreateDate"))
                        .SetFirstResult(0)
                        .SetMaxResults(1); ;
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        return (MeasurementData)recordList[0];
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
    }
}
