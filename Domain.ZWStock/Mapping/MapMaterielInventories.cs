using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ZWStock.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ZWStock.Mapping
{
    public class MapMaterielInventories : ClassMap<MaterielInventories>
    {
        public MapMaterielInventories()
        {
            Table("MaterielInventories");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.Balance).Column("Balance");
            Map(x => x.ItemMasterId).Column("ItemMasterId");
            Map(x => x.StoreroomId).Column("StoreroomId");
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
