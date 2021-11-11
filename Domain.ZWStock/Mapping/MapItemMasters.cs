using Domain.ZWStock.Domain;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ZWStock.Mapping
{
    public class MapItemMasters : ClassMap<ItemMasters>
    {
        public MapItemMasters()
        {
            Table("ItemMasters");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.Spec).Column("Spec");
            Map(x => x.Unit).Column("Unit");
            Map(x => x.Price).Column("Price");
            Map(x => x.AlarmBalance).Column("AlarmBalance");
            Map(x => x.Manufacturer).Column("Manufacturer");
            Map(x => x.Barcode).Column("Barcode");
            Map(x => x.SafeQuantiry).Column("SafeQuantiry");
            Map(x => x.ItemCategoryId).Column("ItemCategoryId");
            Map(x => x.Creator).Column("Creator");
            Map(x => x.CreatedTime).Column("CreatedTime");
            Map(x => x.LastModifier).Column("LastModifier");
            Map(x => x.LastModifiedTime).Column("LastModifiedTime");
            Map(x => x.SortField).Column("SortField");
            Map(x => x.IsDeleted).Column("IsDeleted");
            Map(x => x.Name).Column("Name");
            Map(x => x.Remark).Column("Remark");
            Map(x => x.Alias).Column("Alias");
            Map(x => x.TrainTypeId).Column("TrainTypeId");

            HasMany(x => x.StoreroomBinItemMastersesList).KeyColumn("ItemMaster_Id").LazyLoad().ReadOnly().Cascade.None();
        }
    }
}
