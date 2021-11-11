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
    public class DalRuntimeInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int GetCardDistCount(bool keepCount)
        {
            RuntimeInfo ri = GetRuntimeInfo("CardDistCount");
            if (ri == null)
            {
                SetRuntimeInfo("CardDistCount", "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return 0;
            }
            DateTime lastDist = DateTime.Parse(ri.KeyValue2);
            if (DateTime.Now.Day == lastDist.Day && DateTime.Now.Hour == lastDist.Hour && DateTime.Now.Minute == lastDist.Minute)
            {
                int distCount = int.Parse(ri.KeyValue1);
                if (distCount >= 15) return -10;
                distCount++;
                SetRuntimeInfo("CardDistCount", distCount.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return distCount;
            }
            SetRuntimeInfo("CardDistCount", "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return 0;
        }

        public static RuntimeInfo GetRuntimeInfo(string keyName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RuntimeInfo))
                        .Add(Restrictions.Eq("KeyName", keyName));
                    IList itemList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(itemList))
                    {
                        return (RuntimeInfo)itemList[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int SetRuntimeInfo(string keyName, string keyValue1, string keyValue2 = null, string keyValue3 = null, string keyValue4 = null)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(RuntimeInfo))
                        .Add(Restrictions.Eq("KeyName", keyName));
                    IList recordList = pQuery.List();
                    using (var transaction = session.BeginTransaction())
                    {
                        RuntimeInfo ci;
                        if (SqlDataHelper.IsDataValid(recordList))
                        {
                            ci = (RuntimeInfo)recordList[0];
                            if (keyValue1 != null) ci.KeyValue1 = keyValue1;
                            if (keyValue2 != null) ci.KeyValue2 = keyValue2;
                            if (keyValue3 != null) ci.KeyValue3 = keyValue3;
                            if (keyValue4 != null) ci.KeyValue4 = keyValue4;
                        }
                        else
                        {
                            ci = new RuntimeInfo
                            {
                                KeyName = keyName,
                                KeyValue1 = keyValue1,
                                KeyValue2 = keyValue2,
                                KeyValue3 = keyValue3,
                                KeyValue4 = keyValue4
                            };
                        }
                        session.SaveOrUpdate(ci);
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
