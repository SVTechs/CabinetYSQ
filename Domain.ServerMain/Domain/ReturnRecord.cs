using System;

namespace Domain.ServerMain.Domain
{
    public class ReturnRecord
    {
        public virtual string Id { get; set; }
        public virtual string BorrowRecord { get; set; }
        public virtual string WorkerId { get; set; }
        public virtual string WorkerName { get; set; }
        public virtual DateTime? EventTime { get; set; }
        public virtual string DataOwner { get; set; }
    }
}
