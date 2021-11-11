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
    public class DalToolCheckRecord
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList SearchToolCheckRecord(string toolId, DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolCheckRecord))
                        .Add(Restrictions.Ge("ChkTime", timeStart))
                        .Add(Restrictions.Le("ChkTime", timeEnd))
                        .Add(Restrictions.Eq("ToolId", toolId))
                        .AddOrder(Order.Desc("ChkTime"))
                        .SetFirstResult(0)
                        .SetMaxResults(50);
                    return pQuery.List();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int AddToolCheckRecord(string toolId, string toolName, string toolSpec,
            float stdValue, float deviationPositive, float deviationNegative, string chkUser)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        //保存检查记录
                        ToolCheckRecord idRecord = new ToolCheckRecord
                        {
                            ToolId = toolId,
                            ToolName = toolName,
                            ToolSpec = toolSpec,
                            ChkCount = 3,
                            StdValue = stdValue,
                            DeviationPositive = deviationPositive,
                            DeviationNegative = deviationNegative,
                            ChkUser = chkUser,
                            ChkTime = DateTime.Now
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

        public static int SaveChkValue(int recordId, int storePosition, float chkValue)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolCheckRecord))
                        .Add(Restrictions.Eq("Id", recordId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolCheckRecord idRecord = (ToolCheckRecord)recordList[0];
                            switch (storePosition)
                            {
                                case 1:
                                    idRecord.ChkValue1 = chkValue;
                                    break;
                                case 2:
                                    idRecord.ChkValue2 = chkValue;
                                    break;
                                case 3:
                                    idRecord.ChkValue3 = chkValue;
                                    break;
                                case 4:
                                    idRecord.ChkValue4 = chkValue;
                                    break;
                                case 5:
                                    idRecord.ChkValue5 = chkValue;
                                    break;
                                case 6:
                                    idRecord.ChkValue6 = chkValue;
                                    break;
                            }
                            if (storePosition == idRecord.ChkCount)
                            {
                                idRecord.AvgValue = (idRecord.ChkValue1 + idRecord.ChkValue2 + idRecord.ChkValue3) / 3;
                                idRecord.DeviationRate = (idRecord.AvgValue - idRecord.StdValue) / idRecord.StdValue;
                                if (idRecord.DeviationRate <= idRecord.DeviationPositive &&
                                    idRecord.DeviationRate >= 0 - idRecord.DeviationNegative)
                                {
                                    idRecord.ChkResult = "通过";
                                    //更新校验时间
                                    int result = DalToolInfo.SetAsChecked(session, idRecord.ToolId);
                                    if (result <= 0)
                                    {
                                        transaction.Rollback();
                                        return -220;
                                    }
                                }
                                else
                                {
                                    idRecord.ChkResult = "不通过";
                                }
                            }
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

        public static int SetChkUserMutal(int recordId, string userName)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolCheckRecord))
                        .Add(Restrictions.Eq("Id", recordId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolCheckRecord idRecord = (ToolCheckRecord)recordList[0];
                            idRecord.ChkUserMutual = userName;
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

        public static int AddComment(int recordId, string comment)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolCheckRecord))
                        .Add(Restrictions.Eq("Id", recordId));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolCheckRecord idRecord = (ToolCheckRecord)recordList[0];
                            idRecord.Comment = comment;
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

        public static int ResetRecord()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ToolCheckRecord))
                        .Add(Restrictions.Ge("ChkTime", DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))));
                    IList recordList = pQuery.List();
                    if (SqlDataHelper.IsDataValid(recordList))
                    {
                        using (var transaction = session.BeginTransaction())
                        {
                            ToolCheckRecord idRecord = (ToolCheckRecord)recordList[0]; 
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
}
