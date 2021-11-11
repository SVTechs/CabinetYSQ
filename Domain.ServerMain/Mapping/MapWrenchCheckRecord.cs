using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapWrenchCheckRecord : ClassMap<WrenchCheckRecord>
    {
        public MapWrenchCheckRecord()
        {
            Table("WrenchCheckRecord");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.WrenchId).Column("WrenchId");
            Map(x => x.WrenchName).Column("WrenchName");
            Map(x => x.WrenchPosition).Column("WrenchPosition");
            Map(x => x.WorkerId).Column("WorkerId");
            Map(x => x.WorkerName).Column("WorkerName");
            Map(x => x.EventTime).Column("EventTime");
            Map(x => x.Status).Column("Status");
            Map(x => x.DataOwner).Column("DataOwner");
            Map(x => x.PdfFile).Column("PdfFile");
        }
    }
}