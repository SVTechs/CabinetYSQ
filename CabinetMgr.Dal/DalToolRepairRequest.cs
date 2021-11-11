using System;
using NLog;

namespace CabinetMgr.Dal
{
    public class DalToolRepairRequest
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int AddToolRepairRequest(string toolName, string toolSpec, string cabinetName, string requesterName, string reqComment)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShangHaiDevice1);
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        ToolRepairRequest idRecord = new ToolRepairRequest()
                        {
                            Id = Guid.NewGuid().ToString().ToUpper(),
                            RequestNo = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond,
                            ToolName = toolName,
                            ToolSpec = toolSpec,
                            CabinetName = cabinetName,
                            RequesterName = requesterName,
                            RequestTime = DateTime.Now,
                            RequestStatus = 0,
                            RequestComment = reqComment
                        };
                        session.SaveOrUpdate(idRecord);
                        transaction.Commit();
                        return 1;
                    }
                }*/
                return 0;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }

        public static int SetAsFinished(string toolName, string cabinetName)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShangHaiDevice1);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolRepairRequest))
                        .Add(Restrictions.Eq("ToolName", toolName))
                        .Add(Restrictions.Eq("CabinetName", cabinetName));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolRepairRequest idRecord = (ToolRepairRequest)recordList[0];
                            idRecord.RequestStatus = 20;
                            session.SaveOrUpdate(idRecord);
                            transaction.Commit();
                            return 1;
                        }
                    }
                }*/
                return 0;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return -200;
        }
    }
}
