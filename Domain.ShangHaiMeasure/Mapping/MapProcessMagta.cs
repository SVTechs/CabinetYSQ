using Domain.ShangHaiMeasure.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ShangHaiMeasure.Mapping
{
    public class MapProcessMagta : ClassMap<ProcessMagta>
    {
        public MapProcessMagta()
        {
            Table("ProcessMagta");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.ProcessDefinitionId).Column("ProcessDefinitionId");
            Map(x => x.Sequence).Column("Sequence");
            Map(x => x.DisplayLocation).Column("DisplayLocation");
        }
    }
}
