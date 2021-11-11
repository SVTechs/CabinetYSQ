using Domain.ZWStock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllItemMasters
    {
        public static IList<ItemMasters> GetAllItemMasters()
        {
            return DalItemMasters.GetAllItemMasters();
        }

        public static ItemMasters GetItemMastersById(string Id)
        {
            return DalItemMasters.GetItemMastersById(Id);
        }

        public static int GetItemMastersCount(string barCode, string name, string spec, string cabinet)
        {
            return DalItemMasters.GetItemMastersCount(barCode, name, spec, cabinet);
        }

        public static IList<ItemMasters> SearchItemMasters(string barCode, string name, string spec, string cabinet, int dataStart, int dataCount)
        {
            return DalItemMasters.SearchItemMasters(barCode, name, spec, cabinet, dataStart, dataCount);
        }

        public static int UpdateItemMasters(ItemMasters itemRecord)
        {
            if (itemRecord == null) return -100;
            return DalItemMasters.UpdateItemMasters(itemRecord);
        }
    }
}
