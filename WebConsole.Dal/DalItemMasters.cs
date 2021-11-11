using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ZWStock.Domain;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Utilities.DbHelper;
using WebConsole.Dal.NhUtils;

namespace WebConsole.Dal
{
    public class DalItemMasters
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<ItemMasters> GetAllItemMasters()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ItemMasters))
                    .SetFetchMode("StoreroomBinItemMastersesList", FetchMode.Eager);
                    var itemList = pQuery.List<ItemMasters>();
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public static ItemMasters GetItemMastersById(string Id)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ItemMasters))
                        .Add(Restrictions.Eq("Id", Id))
                        .SetFetchMode("StoreroomBinItemMastersesList", FetchMode.Eager); ;
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        return (ItemMasters)result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        public static int GetItemMastersCount(string barCode, string name, string spec, string cabinet)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ItemMasters));
                    if (!string.IsNullOrEmpty(barCode))
                    {
                        pQuery.Add(Restrictions.Eq("BarCode", barCode));
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        pQuery.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(spec))
                    {
                        pQuery.Add(Restrictions.Like("Spec", spec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(cabinet))
                    {
                        StoreroomBins storeroomBins = DalStoreroomBins.GetStoreroomBinsByName(cabinet);
                        IList<StoreroomBinItemMasters> lst =
                            DalStoreroomBinItemMasters.SearchStoreroomBinItemMasterses(storeroomBins);
                        List<string> sbList = new List<string>();
                        foreach (StoreroomBinItemMasters storeroomBinItemMasters in lst)
                        {
                            sbList.Add(storeroomBinItemMasters.ItemMaster_Id);
                        }
                        pQuery.Add(Restrictions.In("Id", sbList));

                    }
                    return (int)pQuery.SetProjection(Projections.RowCount()).UniqueResult();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return -200;
        }

        public static IList<ItemMasters> SearchItemMasters(string barCode, string name, string spec, string cabinet, int dataStart, int dataCount)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(ItemMasters));
                    if (!string.IsNullOrEmpty(barCode))
                    {
                        pQuery.Add(Restrictions.Eq("BarCode", barCode));
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        pQuery.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(spec))
                    {
                        pQuery.Add(Restrictions.Like("Spec", spec, MatchMode.Anywhere));
                    }
                    if (!string.IsNullOrEmpty(cabinet))
                    {
                        StoreroomBins storeroomBins = DalStoreroomBins.GetStoreroomBinsByName(cabinet);
                        IList<StoreroomBinItemMasters> lst =
                            DalStoreroomBinItemMasters.SearchStoreroomBinItemMasterses(storeroomBins);
                        List<string> sbList = new List<string>();
                        foreach (StoreroomBinItemMasters storeroomBinItemMasters in lst)
                        {
                            sbList.Add(storeroomBinItemMasters.ItemMaster_Id);
                        }
                        pQuery.Add(Restrictions.In("Id", sbList));

                    }
                    pQuery.SetFirstResult(dataStart);
                    pQuery.SetMaxResults(dataCount);
                    //pQuery.SetFetchMode("StoreroomBinItemMastersesList", FetchMode.Eager);
                    var itemList = pQuery.List<ItemMasters>();
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public static int UpdateItemMasters(ItemMasters itemRecord)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(itemRecord);
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
