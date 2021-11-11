using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ZWStock.Domain
{
    public class StoreroomBins
    {
        public virtual string Id { get; set; }
        public virtual string StoreroomId { get; set; }
        public virtual string ParentId { get; set; }
        public virtual string Creator { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual string LastModifier { get; set; }
        public virtual DateTime? LastModifiedTime { get; set; }
        public virtual int SortField { get; set; }
        public virtual int IsDeleted { get; set; }
        public virtual string Name { get; set; }
        public virtual string Remark { get; set; }

        public virtual int depth { get; set; }

        public virtual IList<StoreroomBins> Children { get; set; }
    }
}
