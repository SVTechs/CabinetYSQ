using System;

namespace Domain.ServerMain.Domain
{
    public class BorrowRecord
    {
        public virtual string Id { get; set; }
        public virtual string ToolId { get; set; }
        public virtual string ToolName { get; set; }
        public virtual int? ToolPosition { get; set; }
        public virtual string WorkerId { get; set; }
        public virtual string WorkerName { get; set; }
        public virtual DateTime? EventTime { get; set; }
        public virtual int? Status { get; set; }
        public virtual DateTime? ReturnTime { get; set; }
        public virtual int? ExpireConfirm { get; set; }
        public virtual string ExpireComment { get; set; }
        public virtual string DataOwner { get; set; }
    }
}
