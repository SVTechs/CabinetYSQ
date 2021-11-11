using Domain.Main.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Main.Mapping
{
    public class MapRuntimeInfo : ClassMap<RuntimeInfo>
    {
        public MapRuntimeInfo()
        {
            Table("RuntimeInfo");
            Id(x => x.Id);
            Map(x => x.KeyName);
            Map(x => x.KeyValue1);
            Map(x => x.KeyValue2);
            Map(x => x.KeyValue3);
            Map(x => x.KeyValue4);
        }
    }
}
