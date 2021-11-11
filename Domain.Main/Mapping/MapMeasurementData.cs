using Domain.Main.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Main.Mapping
{
    public class MapMeasurementData : ClassMap<MeasurementData>
    {

        public MapMeasurementData()
        {
            Table("MeasurementData");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.WorkUserInfoId).Column("WorkUserInfoId").Not.Nullable();
            Map(x => x.DeviceCode).Column("DeviceCode");
            Map(x => x.TrainType).Column("TrainType");
            Map(x => x.TrainNum).Column("TrainNum");
            Map(x => x.Process).Column("Process");
            Map(x => x.ToolCode).Column("ToolCode");
            Map(x => x.ToolType).Column("ToolType");
            Map(x => x.DefaultJobValue).Column("DefaultJobValue");
            Map(x => x.DataValue).Column("DataValue");
            Map(x => x.JobResult).Column("JobResult");
            Map(x => x.TaskUser).Column("TaskUser");
            Map(x => x.TaskStartDate).Column("TaskStartDate");
            Map(x => x.TaskEndDate).Column("TaskEndDate");
            Map(x => x.CreateDate).Column("CreateDate");
        }
    }
}
