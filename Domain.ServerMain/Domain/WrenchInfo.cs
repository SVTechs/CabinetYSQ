using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ServerMain.Domain
{
    public class WrenchInfo
    {
        public virtual string Id { get; set; }

        public virtual string WrenchName { get; set; }

        public virtual string WrenchCode { get; set; }

        public virtual string WrenchSpec { get; set; }

        public virtual string StandardRange { get; set; }

        public virtual int WrenchPosition { get; set; }

        public virtual DateTime CheckTime { get; set; }

        public virtual DateTime NextCheckTime { get; set; }

        public virtual int CheckInterval { get; set; }

        public virtual string CheckIntervalType { get; set; }

        public virtual string DataOwner { get; set; }
    }
}
