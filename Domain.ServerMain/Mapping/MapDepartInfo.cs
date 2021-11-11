using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapDepartInfo : ClassMap<DepartInfo>
    {
        public MapDepartInfo()
        {
            Table("DepartInfo");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.DepartName).Column("DepartName");
            Map(x => x.TreeLevel).Column("TreeLevel");
            Map(x => x.TreeParent).Column("TreeParent");
            Map(x => x.DepartOrder).Column("DepartOrder");
            Map(x => x.DepartDesp).Column("DepartDesp");
            Map(x => x.LastChanged).Column("LastChanged");
        }
    }
}
