using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ZWStock.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalStoreroomBinItemMasters
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<StoreroomBinItemMasters> SearchStoreroomBinItemMasterses(string itemMastersId)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(StoreroomBinItemMasters))
                        .Add(Restrictions.Eq("ItemMaster_Id", itemMastersId));
                    var itemList = pQuery.List<StoreroomBinItemMasters>();
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public static IList<StoreroomBinItemMasters> SearchStoreroomBinItemMasterses(StoreroomBins storeroomBins)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession(new SQLWatcher()))
                {
                    List<string> RoomBinLst = StoreroomBinsId(storeroomBins).ToList();
                    ICriteria pQuery = session.CreateCriteria(typeof(StoreroomBinItemMasters))
                        .Add(Restrictions.In("StoreroomBin_Id", RoomBinLst));
                    var itemList = pQuery.List<StoreroomBinItemMasters>();
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        private static IList<string> StoreroomBinsId(StoreroomBins storeroomBins)
        {
            IList<string> rslt = new List<string>();
            if (storeroomBins == null) return rslt;
            rslt.Add(storeroomBins.Id);
            if (storeroomBins.Children == null || storeroomBins.Children.Count == 0) return rslt;
            foreach (StoreroomBins sb in storeroomBins.Children)
            {
                rslt.Add(sb.Id);
                AddChildren(sb, rslt);
            }
            return rslt;
        }

        private static void AddChildren(StoreroomBins storeroomBins, IList<string> rslt)
        {
            if (storeroomBins.Children == null || storeroomBins.Children.Count == 0) return;
            foreach (StoreroomBins sb in storeroomBins.Children)
            {
                rslt.Add(sb.Id);
            }
        }

        public class SqlWatcher : EmptyInterceptor
        {
            public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
            {
                System.Diagnostics.Debug.WriteLine("Sql: " + sql);
                return base.OnPrepareStatement(sql);
            }
        }
    }
}
