using Domain.ServerMain.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ServerMain.Mapping
{
    public class MapBorrowRecord : ClassMap<BorrowRecord>
    {
        public MapBorrowRecord()
        {
            Table("BorrowRecord");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.ToolId).Column("ToolId");
            Map(x => x.ToolName).Column("ToolName");
            Map(x => x.ToolPosition).Column("ToolPosition");
            Map(x => x.WorkerId).Column("WorkerId");
            Map(x => x.WorkerName).Column("WorkerName");
            Map(x => x.EventTime).Column("EventTime");
            Map(x => x.Status).Column("Status");
            Map(x => x.ReturnTime).Column("ReturnTime");
            Map(x => x.ExpireConfirm).Column("ExpireConfirm");
            Map(x => x.ExpireComment).Column("ExpireComment");
            Map(x => x.DataOwner).Column("DataOwner");
        }
    }
}
