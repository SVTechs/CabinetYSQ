using System;

namespace Domain.ServerMain.Domain
{
    public class PermissionSettings
    {

        public virtual string Id { get; set; }

        public virtual string OwnerType { get; set; }

        public virtual string OwnerId { get; set; }

        public virtual string AccessType { get; set; }

        public virtual string AccessId { get; set; }

        public virtual DateTime AddTime { get; set; }
    }
}
