using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapPageMenus : ClassMap<PageMenus>
    {
        public MapPageMenus()
        {
            Table("PageMenus");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.MenuName).Column("MenuName");
            Map(x => x.MenuCode).Column("MenuCode");
            Map(x => x.TreeLevel).Column("TreeLevel");
            Map(x => x.TreeParent).Column("TreeParent");
            Map(x => x.MenuOrder).Column("MenuOrder");
            Map(x => x.MenuType).Column("MenuType");
            Map(x => x.MenuUrl).Column("MenuUrl");
            Map(x => x.MenuIcon).Column("MenuIcon");
            Map(x => x.IsVisible).Column("IsVisible");
            Map(x => x.MenuDesp).Column("MenuDesp");
        }
    }
}
