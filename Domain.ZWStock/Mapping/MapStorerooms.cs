using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Domain.ZWStock.Mapping
{
    public class MapStorerooms : ClassMap<Storerooms>
    {
        public MapStorerooms()
        {
            Table("Storerooms");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.StockType).Column("StockType");
            Map(x => x.Creator).Column("Creator");
            Map(x => x.CreatedTime).Column("CreatedTime");
            Map(x => x.LastModifier).Column("LastModifier");
            Map(x => x.LastModifiedTime).Column("LastModifiedTime");
            Map(x => x.SortField).Column("SortField");
            Map(x => x.IsDeleted).Column("IsDeleted");
            Map(x => x.Name).Column("Name");
            Map(x => x.Remark).Column("Remark");
        }
    }
}
