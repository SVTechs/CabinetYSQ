namespace Domain.ShangHaiMeasure.Domain
{
    public class ProcessMagta
    {
        public virtual string Id { get; set; }
        public virtual string ProcessDefinitionId { get; set; }
        public virtual int Sequence { get; set; }
        public virtual string DisplayLocation { get; set; }
    }
}
