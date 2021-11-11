using Domain.ZWStock.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DbHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalItemCategories
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<ItemCategories> SearchItemCategories()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ItemCategories))
                        .Add(Restrictions.Eq("ItemType", 2));
                    return pQuery.List<ItemCategories>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static ItemCategories GetItemCategories(string Id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ItemCategories))
                        .Add(Restrictions.Eq("Id", Id));
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (ItemCategories)result[0];
                    }
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
