using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapRoleSettings : ClassMap<RoleSettings>
    {

        public MapRoleSettings()
        {
            Table("RoleSettings");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.UserId).Column("UserId");
            Map(x => x.RoleId).Column("RoleId");
            Map(x => x.AddTime).Column("AddTime");
        }
    }
}
