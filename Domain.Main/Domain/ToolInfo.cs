using System;

namespace Domain.Main.Domain
{
    public class ToolInfo
    {
        public virtual string Id { get; set; }
        public virtual string ToolName { get; set; }
        public virtual string ToolCode { get; set; }
        public virtual string ToolSpec { get; set; }
        public virtual string ToolTypeId { get; set; }
        public virtual string ToolType { get; set; }
        public virtual string HardwareId { get; set; }
        public virtual string CardId { get; set; }
        public virtual string ServerIdent { get; set; }
        public virtual int ServerIoStatus { get; set; }
        public virtual int ServerSpStatus { get; set; }
        public virtual string StandardRange { get; set; }
        public virtual float DeviationPositive { get; set; }
        public virtual float DeviationNegative { get; set; }
        /// <summary>
        /// 存放位置类型 0=上层
        /// </summary>
        public virtual int ToolPositionType { get; set; }
        public virtual int ToolPosition { get; set; }
        public virtual string ToolGrid { get; set; }
        public virtual DateTime CheckTime { get; set; }
        public virtual DateTime NextCheckTime { get; set; }
        public virtual float CheckInterval { get; set; }
        public virtual string CheckIntervalType { get; set; }
        public virtual string ToolManager { get; set; }
        public virtual string Comment { get; set; }
        public virtual string Operator { get; set; }
        public virtual DateTime OperateTime { get; set; }
        public virtual int RtStatus { get; set; }
        /// <summary>
        /// 工具状态 0=正常 10=维修
        /// </summary>
        public virtual int ToolStatus { get; set; }
        public virtual int SyncStatus { get; set; }
    }
}
