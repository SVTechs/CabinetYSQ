using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ZWStock
{
    public class Storerooms
    {
        public virtual string Id { get; set; }
        public virtual int StockType { get; set; }
        public virtual string Creator { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual string LastModifier { get; set; }
        public virtual DateTime? LastModifiedTime { get; set; }
        public virtual int SortField { get; set; }
        public virtual int IsDeleted { get; set; }
        public virtual string Name { get; set; }
        public virtual string Remark { get; set; }
    }
}
