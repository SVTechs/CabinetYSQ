using System;

namespace Domain.ServerMain.Domain
{
    public class RoleSettings
    {

        public virtual string Id { get; set; }

        public virtual string UserId { get; set; }

        public virtual string RoleId { get; set; }

        public virtual DateTime AddTime { get; set; }
    }
}
