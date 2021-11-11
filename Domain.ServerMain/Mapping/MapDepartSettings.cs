using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapDepartSettings : ClassMap<DepartSettings>
    {

        public MapDepartSettings()
        {
            Table("DepartSettings");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.UserId).Column("UserId");
            Map(x => x.DepartId).Column("DepartId");
            Map(x => x.AddTime).Column("AddTime");
        }
    }
}
