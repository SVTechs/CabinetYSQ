using System;

namespace Domain.Main.Domain
{
    public class ReturnRecord
    {
        public virtual string Id { get; set; }
        public virtual string BorrowRecord { get; set; }
        public virtual string WorkerId { get; set; }
        public virtual string WorkerName { get; set; }
        public virtual DateTime EventTime { get; set; }
        public virtual int SyncStatus { get; set; }
    }
}
