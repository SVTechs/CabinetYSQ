using System.Collections.Generic;

namespace Domain.MtToolDb.Domain
{
    public class MtToolCate
    {
        public virtual int Id { get; set; }

        public virtual string No { get; set; }

        public virtual string Name { get; set; }

        public virtual int FatherId { get; set; }

        public virtual IList<MtToolInfo> ToolList { get; set; }
    }
}
