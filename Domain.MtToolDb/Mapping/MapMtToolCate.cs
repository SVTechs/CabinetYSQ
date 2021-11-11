using Domain.MtToolDb.Domain;
using FluentNHibernate.Mapping;

namespace Domain.MtToolDb.Mapping
{
    public class MapMtToolCate : ClassMap<MtToolCate>
    {

        public MapMtToolCate()
        {
            Table("B_Category");
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            Map(x => x.No).Column("No").Not.Nullable();
            Map(x => x.Name).Column("Name").Not.Nullable();
            Map(x => x.FatherId).Column("FatherID").Not.Nullable();

            HasMany(x => x.ToolList).KeyColumn("Id").LazyLoad().Cascade.None();
        }
    }
}
