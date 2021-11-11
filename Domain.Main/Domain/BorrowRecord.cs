using System;

namespace Domain.Main.Domain
{
    public class BorrowRecord
    {
        public virtual string Id { get; set; }
        public virtual string ToolId { get; set; }
        public virtual string ToolName { get; set; }
        public virtual int ToolPosition { get; set; }
        public virtual int HardwareId { get; set; }
        public virtual string WorkerId { get; set; }
        public virtual string WorkerName { get; set; }
        public virtual DateTime EventTime { get; set; }
        /// <summary>
        /// 0=借出未还 10=超时已确认 11=超时已确认+备注 20=已还
        /// </summary>
        public virtual int Status { get; set; }
        public virtual DateTime ReturnTime { get; set; }
        public virtual string ExpireComment { get; set; }
        public virtual int SyncStatus { get; set; }

        //=================================================
        public virtual ReturnRecord RetRecord { get; set; }
    }
}
  