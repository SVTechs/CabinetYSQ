using System;
using System.Collections;
using NLog;

namespace CabinetMgr.Dal
{
    public class DalToolPurchaseInfo
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int AddToolPurchaseInfo(string toolName, string toolSpec, int toolCount, string cabinetNo, string requesterName, 
            int requestStatus)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShangHaiDevice1);
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ToolPurchaseInfo idRecord = new ToolPurchaseInfo
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            RequestNo = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond,
                            RequestTime = DateTime.Now,
                            ToolName = toolName,
                            ToolSpec = toolSpec,
                            CabinetName = cabinetNo,
                            ToolCount = toolCount,
                            RequesterName = requesterName,
                            RequestStatus = requestStatus,
                        };
                        session.SaveOrUpdate(idRecord);
                        transaction.Commit();
                        return 1;
                    }
                }*/
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        public static IList SearchToolPurchaseInfo(string cabinetName, DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShangHaiDevice1);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolPurchaseInfo))
                        .Add(Restrictions.Ge("RequestTime", timeStart))
                        .Add(Restrictions.Le("RequestTime", timeEnd))
                        .AddOrder(Order.Desc("RequestTime"));
                    if (!string.IsNullOrEmpty(cabinetName))
                    {
                        pQuery = pQuery.Add(Restrictions.Eq("CabinetName", cabinetName));
                    }
                    return pQuery.List();
                }*/
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }
    }
}
