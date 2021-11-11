using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ZWStock.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ZWStock.Mapping
{
    public class MapStoreroomBinItemMasters : ClassMap<StoreroomBinItemMasters>
    {
        public MapStoreroomBinItemMasters()
        {
            Table("StoreroomBinItemMasters");
            Map(x => x.StoreroomBin_Id).Column("StoreroomBin_Id");
            Id(x => x.ItemMaster_Id).GeneratedBy.Assigned().Column("ItemMaster_Id");
        }
    }
}
