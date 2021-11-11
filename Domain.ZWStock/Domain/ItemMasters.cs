using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.ZWStock.Domain
{
    public class ItemMasters
    {
        public virtual string Id { get; set; }
        public virtual string Spec { get; set; }
        public virtual string Unit { get; set; }
        public virtual int Price { get; set; }
        public virtual float AlarmBalance { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string Barcode { get; set; }
        public virtual float SafeQuantiry { get; set; }
        public virtual string ItemCategoryId { get; set; }
        public virtual string Creator { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual string LastModifier { get; set; }
        public virtual DateTime? LastModifiedTime { get; set; }
        public virtual int SortField { get; set; }
        public virtual int IsDeleted { get; set; }
        public virtual string Name { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Alias { get; set; }
        public virtual string TrainTypeId { get; set; }

        public virtual IList<StoreroomBinItemMasters> StoreroomBinItemMastersesList { get; set; }
        public virtual string Position { get; set; }
    }
}
