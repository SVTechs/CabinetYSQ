using System;

namespace Domain.MtToolDb.Domain
{
    public class MtToolInfo
    {

        public virtual int Id { get; set; }

        public virtual string CodeNo { get; set; }

        public virtual int BCodeTypeId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Model { get; set; }

        public virtual int BCategoryId { get; set; }

        public virtual string BrandName { get; set; }

        public virtual int BUnitId { get; set; }

        public virtual string Place { get; set; }

        public virtual string Remark { get; set; }

        public virtual string Pym { get; set; }

        public virtual int Month { get; set; }

        public virtual int PublishManId { get; set; }

        public virtual DateTime PublishTime { get; set; }

        public virtual int State { get; set; }

        public virtual int IsCheck { get; set; }

        public virtual int IsDetect { get; set; }

        public virtual string CabinetName { get; set; }


        public virtual MtToolCate ToolCateInfo { get; set; }
    }
}
