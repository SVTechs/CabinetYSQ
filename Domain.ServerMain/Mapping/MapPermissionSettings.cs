using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapPermissionSettings : ClassMap<PermissionSettings>
    {
        public MapPermissionSettings()
        {
            Table("PermissionSettings");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.OwnerType).Column("OwnerType");
            Map(x => x.OwnerId).Column("OwnerId");
            Map(x => x.AccessType).Column("AccessType");
            Map(x => x.AccessId).Column("AccessId");
            Map(x => x.AddTime).Column("AddTime");
        }
    }
}
