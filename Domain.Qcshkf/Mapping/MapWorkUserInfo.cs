using Domain.Qcshkf.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Qcshkf.Mapping
{
    public class MapWorkUserInfo : ClassMap<WorkUserInfo>
    {
        public MapWorkUserInfo()
        {
            Table("WorkUserInfo");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.ProcessTemplateId).Column("ProcessTemplateId");
            Map(x => x.ProcessTemplateName).Column("ProcessTemplateName");
            Map(x => x.ProcessDefinitionId).Column("ProcessDefinitionId");
            Map(x => x.ProcessDefinitionName).Column("ProcessDefinitionName");
            Map(x => x.ProcessRemarks).Column("ProcessRemarks");
            Map(x => x.ToolCode).Column("ToolCode");
            Map(x => x.ToolType).Column("ToolType");
            Map(x => x.DefaultJobValue).Column("DefaultJobValue");
            Map(x => x.DefaultJobNum).Column("DefaultJobNum");
            Map(x => x.Step).Column("Step");
            Map(x => x.PartsId).Column("PartsId");
            Map(x => x.DeviceCode).Column("DeviceCode");
            Map(x => x.TrainType).Column("TrainType");
            Map(x => x.TrainNum).Column("TrainNum");
            Map(x => x.LevelNum).Column("LevelNum");
            Map(x => x.Process).Column("Process");
            Map(x => x.WorkUserId).Column("WorkUserId");
            Map(x => x.WorkUser).Column("WorkUser");
            Map(x => x.WorkDate).Column("WorkDate");
            Map(x => x.TeamName).Column("TeamName");
            Map(x => x.FinishedDate).Column("FinishedDate");
            Map(x => x.FinishedUser).Column("FinishedUser");
            Map(x => x.Createdate).Column("Createdate");
            Map(x => x.CreateUser).Column("CreateUser");
            Map(x => x.WorkStatus).Column("WorkStatus");
            Map(x => x.ArtificialId).Column("ArtificialId");
        }
    }
}
