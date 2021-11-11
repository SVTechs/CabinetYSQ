using Domain.MtToolDb.Domain;
using FluentNHibernate.Mapping;

namespace Domain.MtToolDb.Mapping
{
    public class MapMtToolInfo : ClassMap<MtToolInfo>
    {

        public MapMtToolInfo()
        {
            Table("B_Material");
            Id(x => x.Id).GeneratedBy.Identity().Column("ID");
            Map(x => x.CodeNo).Column("CodeNo").Not.Nullable();
            Map(x => x.BCodeTypeId).Column("B_CodeTypeID").Not.Nullable();
            Map(x => x.Name).Column("Name").Not.Nullable();
            Map(x => x.Model).Column("Model").Not.Nullable();
            Map(x => x.BCategoryId).Column("B_CategoryID").Not.Nullable();
            Map(x => x.BrandName).Column("BrandName").Not.Nullable();
            Map(x => x.BUnitId).Column("B_UnitID").Not.Nullable();
            Map(x => x.Place).Column("Place").Not.Nullable();
            Map(x => x.Remark).Column("Remark").Not.Nullable();
            Map(x => x.Pym).Column("PYM").Not.Nullable();
            Map(x => x.Month).Column("Month").Not.Nullable();
            Map(x => x.PublishManId).Column("PublishManID").Not.Nullable();
            Map(x => x.PublishTime).Column("PublishTime").Not.Nullable();
            Map(x => x.State).Column("State").Not.Nullable();
            Map(x => x.IsCheck).Column("IsCheck").Not.Nullable();
            Map(x => x.IsDetect).Column("IsDetect").Not.Nullable();
            Map(x => x.CabinetName).Column("CabinetName").Not.Nullable();

            References(x => x.ToolCateInfo)
                .ReadOnly() //防止Insert/Update等操作
                .NotFound.Ignore() //连接目标不存在时忽略
                .Column("B_CategoryID") //使用本表的BCategoryId进行连接
                .Cascade.None();
        }
    }
}
