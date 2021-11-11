using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapCabinetInfo : ClassMap<CabinetInfo>
    {

        public MapCabinetInfo()
        {
            Table("CabinetInfo");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.CabinetName).Column("CabinetName");
            Map(x => x.CabinetAlias).Column("CabinetAlias");
            Map(x => x.CabinetOrder).Column("CabinetOrder");
        }
    }
}
