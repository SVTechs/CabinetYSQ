using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ZWStock;
using Domain.ZWStock.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllStoreroomBins
    {
        public static IList<StoreroomBins> GetAllStoreroomBins()
        {
            return DalStoreroomBins.GetAllStoreroomBins();
        }
    }
}
