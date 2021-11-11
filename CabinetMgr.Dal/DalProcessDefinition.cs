//using Domain.ShangHaiMeasure.Domain;

namespace CabinetMgr.Dal
{
    public class DalProcessDefinition
    {
        /*
        public static IList SearchProcessDefinition()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShanHaiMeasure);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ProcessDefinition));
                    return pQuery.List();
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static ProcessDefinition GetProcessDefinition(string processId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShanHaiMeasure);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ProcessDefinition))
                        .Add(Restrictions.Eq("Id", processId));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (ProcessDefinition) result[0];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int SetProcessDefinitionImage(string processId, byte[] image)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShanHaiMeasure);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ProcessDefinition))
                        .Add(Restrictions.Eq("Id", processId));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            var item = (ProcessDefinition) result[0];
                            item.ProcessImage = image;
                            session.SaveOrUpdate(item);
                            transaction.Commit();
                            return 1;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }*/
    }
}
