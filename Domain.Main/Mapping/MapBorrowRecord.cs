using Domain.Main.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Main.Mapping
{
    public class MapBorrowRecord : ClassMap<BorrowRecord> {

        public MapBorrowRecord()
        {
            Table("BorrowRecord");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.ToolId).Column("ToolId");
            Map(x => x.ToolName).Column("ToolName");
            Map(x => x.ToolPosition).Column("ToolPosition");
            Map(x => x.HardwareId).Column("HardwareId");
            Map(x => x.WorkerId).Column("WorkerId");
            Map(x => x.WorkerName).Column("WorkerName");
            Map(x => x.EventTime).Column("EventTime");
            Map(x => x.Status).Column("Status");
            Map(x => x.ReturnTime).Column("ReturnTime");
            Map(x => x.ExpireComment).Column("ExpireComment");
            Map(x => x.SyncStatus).Column("SyncStatus");

            References(x => x.RetRecord)
                .ReadOnly() //防止Insert/Update等操作
                .NotFound.Ignore() //连接目标不存在时忽略
                .Column("Id") //使用本表的Id进行连接
                .PropertyRef("BorrowRecord") //指定连接的目标列,目标为主键时无需指定
                .Not.LazyLoad()
                .Cascade.None();
        }
    }
}
