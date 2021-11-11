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
    public class DalStoreroomBins
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<StoreroomBins> GetAllStoreroomBins()
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(StoreroomBins));
                    var itemList = pQuery.List<StoreroomBins>();
                    SetDepth(itemList);
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public static StoreroomBins GetStoreroomBinsByName(string name)
        {
            try
            {
                var sessionFactory = NhControl.CreateSessionFactory(NhControl.DbTarget.ZwStockDb);
                using (var session = sessionFactory.OpenSession())
                {
                    ICriteria pQuery = session.CreateCriteria(typeof(StoreroomBins))
                        .Add(Restrictions.Eq("Name", name));
                    var itemList = GetAllStoreroomBins();
                    IList result = pQuery.List();
                    if (SqlDataHelper.IsDataValid(result))
                    {
                        StoreroomBins sb = result[0] as StoreroomBins;
                        AddChildren(sb, itemList);
                        return sb;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
            return null;
        }

        public static List<StoreroomBins> SetDepth(IList<StoreroomBins> all)
        {
            var result = new List<StoreroomBins>();
            var roots = all.Where(p => p.ParentId == null).OrderBy(o => o.SortField);
            foreach (var root in roots)
            {
                root.depth = 0;
                SetChildrenDepth(root, all);
            }
            return result;
        }

        private static void SetChildrenDepth(StoreroomBins sb, IList<StoreroomBins> allStoreroomBins)
        {
            var children = allStoreroomBins.Where(p => p.ParentId == sb.Id).OrderBy(o => o.SortField).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (var v in children)
                {
                    v.depth = sb.depth + 1;
                }
            }
        }

        private static void AddChildren(StoreroomBins storeroomBins, IList<StoreroomBins> lst)
        {
            var children = lst.Where(p => p.ParentId == storeroomBins.Id).OrderBy(o => o.SortField).ToList();
            if (children != null && children.Count > 0)
            {
                foreach (StoreroomBins sb in children)
                {
                    storeroomBins.Children.Add(sb);
                    AddChildren(sb, lst);
                }
            }
        }

    }
}
