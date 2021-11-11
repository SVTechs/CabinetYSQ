using System;

namespace Domain.ServerMain.Domain
{
    public class ToolInfo
    {

        public virtual string Id { get; set; }

        public virtual string ToolName { get; set; }

        public virtual string ToolCode { get; set; }

        public virtual string ToolSpec { get; set; }

        public virtual string ToolType { get; set; }

        public virtual string HardwareId { get; set; }

        public virtual string CardId { get; set; }

        public virtual string StandardRange { get; set; }

        public virtual decimal? DeviationPositive { get; set; }

        public virtual decimal? DeviationNegative { get; set; }

        public virtual int? ToolPositionType { get; set; }

        public virtual int? ToolPosition { get; set; }

        public virtual string ToolGrid { get; set; }

        public virtual DateTime? CheckTime { get; set; }

        public virtual DateTime? NextCheckTime { get; set; }

        public virtual float? CheckInterval { get; set; }

        public virtual string CheckIntervalType { get; set; }

        public virtual string ToolManager { get; set; }

        public virtual string Comment { get; set; }

        public virtual string Operator { get; set; }

        public virtual DateTime? OperateTime { get; set; }

        public virtual int? RtStatus { get; set; }

        public virtual int? ToolStatus { get; set; }

        public virtual string DataOwner { get; set; }
    }
}
