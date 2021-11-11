using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ServerMain.Domain
{
    public class WrenchCheckRecord
    {
        public virtual string Id { get; set; }

        public virtual string WrenchId { get; set; }

        public virtual string WrenchName { get; set; }

        public virtual int WrenchPosition { get; set; }

        public virtual string WorkerId { get; set; }

        public virtual string WorkerName { get; set; }

        public virtual DateTime EventTime { get; set; }

        public virtual string Status { get; set; }

        public virtual string DataOwner { get; set; }

        public virtual string PdfFile { get; set; }
    }
}