using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapWrenchInfo : ClassMap<WrenchInfo>
    {
        public MapWrenchInfo()
        {
            Table("WrenchInfo");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.WrenchName).Column("WrenchName");
            Map(x => x.WrenchCode).Column("WrenchCode");
            Map(x => x.WrenchSpec).Column("WrenchSpec");
            Map(x => x.StandardRange).Column("StandardRange");
            Map(x => x.WrenchPosition).Column("WrenchPosition");
            Map(x => x.CheckTime).Column("CheckTime");
            Map(x => x.NextCheckTime).Column("NextCheckTime");
            Map(x => x.CheckInterval).Column("CheckInterval");
            Map(x => x.CheckIntervalType).Column("CheckIntervalType");
            Map(x => x.DataOwner).Column("DataOwner");
        }
    }
}
