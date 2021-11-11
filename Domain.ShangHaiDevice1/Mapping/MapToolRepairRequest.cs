using Domain.ShangHaiDevice1.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ShangHaiDevice1.Mapping
{
    public class MapToolRepairRequest: ClassMap<ToolRepairRequest> {

        public MapToolRepairRequest()
        {
            Table("GJ_bx");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.RequestNo).Column("ApplyNo");
            Map(x => x.RequestTime).Column("ApplyTime");
            Map(x => x.ToolName).Column("GJ_Name");
            Map(x => x.ToolSpec).Column("GJ_Guige");
            Map(x => x.CabinetName).Column("GJG_No");
            Map(x => x.RequesterName).Column("ApplyName");
            Map(x => x.RequestStatus).Column("ApplyStatus");
            Map(x => x.RequestComment).Column("ApplyComment");
        }
    }
}
