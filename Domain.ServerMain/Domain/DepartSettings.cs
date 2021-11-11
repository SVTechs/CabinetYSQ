using System;

namespace Domain.ServerMain.Domain
{
    public class DepartSettings
    {
        public virtual string Id { get; set; }

        public virtual string UserId { get; set; }

        public virtual string DepartId { get; set; }

        public virtual DateTime? AddTime { get; set; }
    }
}
