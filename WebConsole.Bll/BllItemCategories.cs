using Domain.ZWStock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllItemCategories
    {
        public static IList<ItemCategories> SearchItemCategories()
        {
            return DalItemCategories.SearchItemCategories();
        }

        public static ItemCategories GetItemCategories(string Id)
        {
            return DalItemCategories.GetItemCategories(Id);
        }


    }
}
