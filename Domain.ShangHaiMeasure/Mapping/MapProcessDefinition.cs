using Domain.ShangHaiMeasure.Domain;
using FluentNHibernate.Mapping;

namespace Domain.ShangHaiMeasure.Mapping
{
    public class MapProcessDefinition : ClassMap<ProcessDefinition> {

        public MapProcessDefinition()
        {
            Table("ProcessDefinition");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.TemplateId).Column("TemplateId");
            Map(x => x.ApprovalType).Column("ApprovalType");
            Map(x => x.Sequence).Column("Sequence");
            Map(x => x.FullName).Column("FullName");
            Map(x => x.State).Column("State");
            Map(x => x.CreateUser).Column("CreateUser");
            Map(x => x.CreateTime).Column("CreateTime");
            Map(x => x.UpdateUser).Column("UpdateUser");
            Map(x => x.UpdateTime).Column("UpdateTime");
            Map(x => x.DefaultValue).Column("DefaultValue");
            Map(x => x.DefaultCount).Column("DefaultCount");
            Map(x => x.Provision).Column("Provision");
            Map(x => x.Standard).Column("Standard");
            Map(x => x.Usefulness).Column("Usefulness");
            Map(x => x.Long).Column("Long");
            Map(x => x.Width).Column("Width");
            Map(x => x.Height).Column("Height");
            Map(x => x.Remark).Column("Remark");
            Map(x => x.Specification).Column("Specification");
            Map(x => x.ImageX).Column("ImageX");
            Map(x => x.ImageY).Column("ImageY");
            Map(x => x.SelfPosition).Column("SelfPosition");
            Map(x => x.FromPosition).Column("FromPosition");
            Map(x => x.ArtificialId).Column("ArtificialId");
            Map(x => x.NumberType).Column("NumberType");
            Map(x => x.ProcessImage).Length(5000000).Column("ProcessImage");
            Map(x => x.SpecificationImage).Column("SpecificationImage");
            Map(x => x.Type).Column("Type");
        }
    }
}
