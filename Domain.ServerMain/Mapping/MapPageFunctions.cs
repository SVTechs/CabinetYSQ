using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapPageFunctions : ClassMap<PageFunctions>
    {
        public MapPageFunctions()
        {
            Table("PageFunctions");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.FunctionName).Column("FunctionName");
            Map(x => x.FunctionOrder).Column("FunctionOrder");
            Map(x => x.FunctionMenu).Column("FunctionMenu");
            Map(x => x.FunctionDesp).Column("FunctionDesp");
        }
    }
}
