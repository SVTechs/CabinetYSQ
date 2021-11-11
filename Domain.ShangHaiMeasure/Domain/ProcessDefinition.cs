using System;

namespace Domain.ShangHaiMeasure.Domain
{
    public class ProcessDefinition
    {
        public virtual string Id { get; set; }
        public virtual string TemplateId { get; set; }
        public virtual int ApprovalType { get; set; }
        public virtual int Sequence { get; set; }
        public virtual string FullName { get; set; }
        public virtual int State { get; set; }
        public virtual string CreateUser { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual DateTime UpdateTime { get; set; }
        public virtual float DefaultValue { get; set; }
        public virtual int DefaultCount { get; set; }
        public virtual float Provision { get; set; }
        public virtual string Standard { get; set; }
        public virtual string Usefulness { get; set; }
        public virtual float Long { get; set; }
        public virtual float Width { get; set; }
        public virtual float Height { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Specification { get; set; }
        public virtual int ImageX { get; set; }
        public virtual int ImageY { get; set; }
        public virtual string SelfPosition { get; set; }
        public virtual string FromPosition { get; set; }
        public virtual string ArtificialId { get; set; }
        public virtual int NumberType { get; set; }
        public virtual byte[] ProcessImage { get; set; }
        public virtual byte[] SpecificationImage { get; set; }
        public virtual int Type { get; set; }
    }
}
