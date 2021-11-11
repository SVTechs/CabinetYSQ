using System;

namespace Domain.Qcshkf.Domain
{
    public class WorkUserInfo
    {
        public virtual string Id { get; set; }
        public virtual string ProcessTemplateId { get; set; }
        public virtual string ProcessTemplateName { get; set; }
        public virtual string ProcessDefinitionId { get; set; }
        public virtual string ProcessDefinitionName { get; set; }
        public virtual string ProcessRemarks { get; set; }
        public virtual string ToolCode { get; set; }
        public virtual string ToolType { get; set; }
        public virtual string DefaultJobValue { get; set; }
        public virtual string DefaultJobNum { get; set; }
        public virtual int Step { get; set; }
        public virtual string PartsId { get; set; }
        public virtual string DeviceCode { get; set; }
        public virtual string TrainType { get; set; }
        public virtual string TrainNum { get; set; }
        public virtual int LevelNum { get; set; }
        public virtual string Process { get; set; }
        public virtual string WorkUserId { get; set; }
        public virtual string WorkUser { get; set; }
        public virtual DateTime? WorkDate { get; set; }
        public virtual string TeamName { get; set; }
        public virtual DateTime? FinishedDate { get; set; }
        public virtual string FinishedUser { get; set; }
        public virtual DateTime? Createdate { get; set; }
        public virtual string CreateUser { get; set; }
        /// <summary>
        /// 0=未领取 10=已领取 11=已领取+已拿工具 15=已还工具 20=已完成
        /// </summary>
        public virtual int WorkStatus { get; set; }
        public virtual string ArtificialId { get; set; }
    }
}
