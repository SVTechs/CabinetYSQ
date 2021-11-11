using System;

namespace Domain.Main.Domain
{
    public class ToolCheckRecord
    {
        public virtual int Id { get; protected set; }
        public virtual string ToolId { get; set; }
        public virtual string ToolName { get; set; }
        public virtual string ToolSpec { get; set; }
        public virtual int ChkCount { get; set; }
        public virtual float ChkValue1 { get; set; }
        public virtual float ChkValue2 { get; set; }
        public virtual float ChkValue3 { get; set; }
        public virtual float ChkValue4 { get; set; }
        public virtual float ChkValue5 { get; set; }
        public virtual float ChkValue6 { get; set; }
        public virtual float AvgValue { get; set; }
        public virtual float StdValue { get; set; }
        public virtual float DeviationRate { get; set; }
        public virtual float DeviationPositive { get; set; }
        public virtual float DeviationNegative { get; set; }
        public virtual string ChkResult { get; set; }
        public virtual DateTime ChkTime { get; set; }
        public virtual string ChkUser { get; set; }
        public virtual string ChkUserMutual { get; set; }
        public virtual string Comment { get; set; }
    }
}
