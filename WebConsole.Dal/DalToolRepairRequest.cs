using System;
using System.Collections;
using System.Collections.Generic;
//using Domain.ShangHaiDevice1.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalToolRepairRequest
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //public static int GetRequestCount(DateTime timeStart, DateTime timeEnd)
        //{
        //    try
        //    {
        //        var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShangHaiDevice1);
        //        using (var session = sessionFactory.OpenSession())
        //        {
        //            ICriteria pQuery = session.CreateCriteria(typeof(ToolRepairRequest))
        //                .Add(Restrictions.Ge("RequestTime", timeStart))
        //                .Add(Restrictions.Lt("RequestTime", timeEnd));
        //            ProjectionList reqProjections = Projections.ProjectionList().Add(Projections.RowCount());
        //            IList recordList = pQuery.SetProjection(reqProjections).List();
        //            if (SqlDataHelper.IsDataValid(recordList))
        //            {
        //                return (int)recordList[0];
        //            }
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //    return -200;
        //}

        //public static IList SearchRequestRecord(DateTime timeStart, DateTime timeEnd, List<HibernateHelper.OrderEntity> extOrder,
        //    int startIndex, int pageSize)
        //{
        //    try
        //    {
        //        var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ShangHaiDevice1);
        //        using (var session = sessionFactory.OpenSession())
        //        {
        //            ICriteria pQuery = session.CreateCriteria(typeof(ToolRepairRequest))
        //                .Add(Restrictions.Ge("RequestTime", timeStart))
        //                .Add(Restrictions.Lt("RequestTime", timeEnd));
        //            if (extOrder != null && extOrder.Count > 0)
        //            {
        //                List<Order> fullOrder = NhControl.GenerateOrder(extOrder);
        //                if (fullOrder != null)
        //                {
        //                    for (int i = 0; i < fullOrder.Count; i++)
        //                    {
        //                        pQuery.AddOrder(fullOrder[i]);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                pQuery = pQuery.AddOrder(Order.Desc("RequestTime"));
        //            }
        //            pQuery.SetFirstResult(startIndex);
        //            pQuery.SetMaxResults(pageSize);
        //            return pQuery.List();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //    return null;
        //}
    }
}
