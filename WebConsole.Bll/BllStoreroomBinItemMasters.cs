using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ZWStock.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllStoreroomBinItemMasters
    {
        public static IList<StoreroomBinItemMasters> SearchStoreroomBinItemMasterses(string itemMastersId)
        {
            return DalStoreroomBinItemMasters.SearchStoreroomBinItemMasterses(itemMastersId);
        }
    }
}
