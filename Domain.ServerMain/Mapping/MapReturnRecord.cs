using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapReturnRecord : ClassMap<ReturnRecord>
    {
        public MapReturnRecord()
        {
            Table("ReturnRecord");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.BorrowRecord).Column("BorrowRecord");
            Map(x => x.WorkerId).Column("WorkerId");
            Map(x => x.WorkerName).Column("WorkerName");
            Map(x => x.EventTime).Column("EventTime");
            Map(x => x.DataOwner).Column("DataOwner");
        }
    }
}
