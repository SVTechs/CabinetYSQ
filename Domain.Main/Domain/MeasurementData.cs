using System;

namespace Domain.Main.Domain
{
    public class MeasurementData
    {
        public virtual string Id { get; set; }
        public virtual string WorkUserInfoId { get; set; }
        public virtual string DeviceCode { get; set; }
        public virtual string TrainType { get; set; }
        public virtual string TrainNum { get; set; }
        public virtual string Process { get; set; }
        public virtual string ToolCode { get; set; }
        public virtual int ToolType { get; set; }
        public virtual string DefaultJobValue { get; set; }
        public virtual string DataValue { get; set; }
        public virtual int JobResult { get; set; }
        public virtual string TaskUser { get; set; }
        public virtual DateTime? TaskStartDate { get; set; }
        public virtual DateTime? TaskEndDate { get; set; }
        public virtual DateTime CreateDate { get; set; }
    }
}
