using Domain.Main.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Main.Mapping
{
    public class MapToolCheckRecord: ClassMap<ToolCheckRecord> {

        public MapToolCheckRecord()
        {
            Table("ToolCheckRecord");
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.ToolId).Column("ToolId");
            Map(x => x.ToolName).Column("ToolName");
            Map(x => x.ToolSpec).Column("ToolSpec");
            Map(x => x.ChkCount).Column("ChkCount");
            Map(x => x.ChkValue1).Column("ChkValue1");
            Map(x => x.ChkValue2).Column("ChkValue2");
            Map(x => x.ChkValue3).Column("ChkValue3");
            Map(x => x.ChkValue4).Column("ChkValue4");
            Map(x => x.ChkValue5).Column("ChkValue5");
            Map(x => x.ChkValue6).Column("ChkValue6");
            Map(x => x.AvgValue).Column("AvgValue");
            Map(x => x.StdValue).Column("StdValue");
            Map(x => x.DeviationRate).Column("DeviationRate");
            Map(x => x.DeviationPositive).Column("DeviationPositive");
            Map(x => x.DeviationNegative).Column("DeviationNegative");
            Map(x => x.ChkResult).Column("ChkResult");
            Map(x => x.ChkTime).Column("ChkTime");
            Map(x => x.ChkUser).Column("ChkUser");
            Map(x => x.ChkUserMutual).Column("ChkUserMutual");
            Map(x => x.Comment).Column("Comment");
        }
    }
}
