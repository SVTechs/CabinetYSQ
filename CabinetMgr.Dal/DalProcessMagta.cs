using System;
using System.Collections;
using NLog;

namespace CabinetMgr.Dal
{
    public class DalProcessMagta
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int GenerateAllProcessMagta()
        {
            try
            {
                /*
                IList pdList = DalProcessDefinition.SearchProcessDefinition();
                if (SqlDataHelper.IsDataValid(pdList))
                {
                    var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShanHaiMeasure);
                    using (var session = sessionFactory.OpenSession())
                    {
                        if (SqlDataHelper.IsDataValid(pdList))
                        {
                            using (var transaction = session.BeginTransaction())
                            {
                                for (int i = 0; i < pdList.Count; i++)
                                {
                                    ProcessDefinition pd = (ProcessDefinition) pdList[i];
                                    for (int j = 1; j <= pd.DefaultCount; j++)
                                    {
                                        ProcessMagta idRecord = new ProcessMagta
                                        {
                                            Id = Guid.NewGuid().ToString().ToUpper(),
                                            ProcessDefinitionId = pd.Id,
                                            Sequence = j,
                                            DisplayLocation = j*10 + "," + 20
                                        };
                                        session.SaveOrUpdate(idRecord);
                                    }         
                                }
                                transaction.Commit();
                                return 1;
                            }
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

        public static IList GetProcessMagta(string processDefinitionId)
        {
            try
            {
                /*
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShanHaiMeasure);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ProcessMagta))
                        .Add(Restrictions.Eq("ProcessDefinitionId", processDefinitionId))
                        .AddOrder(Order.Asc("Sequence"));
                    return pQuery.List();
                }*/
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }
    }
}
