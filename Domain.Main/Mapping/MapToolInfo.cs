using Domain.Main.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Main.Mapping
{
    public class MapToolInfo : ClassMap<ToolInfo> {

        public MapToolInfo()
        {
            Table("ToolInfo");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.ToolName).Column("ToolName");
            Map(x => x.ToolCode).Column("ToolCode");
            Map(x => x.ToolSpec).Column("ToolSpec");
            Map(x => x.ToolTypeId).Column("ToolTypeId");
            Map(x => x.ToolType).Column("ToolType");
            Map(x => x.HardwareId).Column("HardwareId");
            Map(x => x.CardId).Column("CardId");
            Map(x => x.ServerIdent).Column("ServerIdent");
            Map(x => x.ServerIoStatus).Column("ServerIoStatus");
            Map(x => x.ServerSpStatus).Column("ServerSpStatus");
            Map(x => x.StandardRange).Column("StandardRange");
            Map(x => x.DeviationPositive).Column("DeviationPositive");
            Map(x => x.DeviationNegative).Column("DeviationNegative");
            Map(x => x.ToolPositionType).Column("ToolPositionType");
            Map(x => x.ToolPosition).Column("ToolPosition");
            Map(x => x.ToolGrid).Column("ToolGrid");
            Map(x => x.CheckTime).Column("CheckTime");
            Map(x => x.NextCheckTime).Column("NextCheckTime");
            Map(x => x.CheckInterval).Column("CheckInterval");
            Map(x => x.CheckIntervalType).Column("CheckIntervalType");
            Map(x => x.ToolManager).Column("ToolManager");
            Map(x => x.Comment).Column("Comment");
            Map(x => x.Operator).Column("Operator");
            Map(x => x.OperateTime).Column("OperateTime");
            Map(x => x.RtStatus).Column("RtStatus");
            Map(x => x.ToolStatus).Column("ToolStatus");
            Map(x => x.SyncStatus).Column("SyncStatus");
        }
    }
}
