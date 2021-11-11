using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapRoleInfo : ClassMap<RoleInfo>
    {
        public MapRoleInfo()
        {
            Table("RoleInfo");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.RoleName).Column("RoleName");
            Map(x => x.TreeLevel).Column("TreeLevel");
            Map(x => x.TreeParent).Column("TreeParent");
            Map(x => x.RoleOrder).Column("RoleOrder");
            Map(x => x.RoleDesp).Column("RoleDesp");
            Map(x => x.LastChanged).Column("LastChanged");
        }
    }
}
