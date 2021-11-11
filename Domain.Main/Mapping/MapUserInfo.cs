using Domain.Main.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Main.Mapping
{
    public class MapUserInfo : ClassMap<UserInfo>
    {
        public MapUserInfo()
        {
            Table("UserInfo");
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.UserName).Column("UserName");
            Map(x => x.Password).Column("Password");
            Map(x => x.FullName).Column("FullName");
            Map(x => x.Sex).Column("Sex");
            Map(x => x.Age).Column("Age");
            Map(x => x.Tel).Column("Tel");
            Map(x => x.Adress).Column("Adress");
            Map(x => x.Email).Column("Email");
            Map(x => x.UserState).Column("UserState");
            Map(x => x.Createtime).Column("Createtime");
            Map(x => x.CreateUser).Column("CreateUser");
            Map(x => x.Updatetime).Column("Updatetime");
            Map(x => x.UpdateUser).Column("UpdateUser");
            Map(x => x.OrgId).Column("OrgId");
            Map(x => x.OrgName).Column("OrgName");
            //Map(x => x.CardNum).Column("CardNum");
            //Map(x => x.EmpName).Column("EmpName");
            Map(x => x.LeftTemplate).Length(int.MaxValue).Column("LEFTTEMPLATE");
            Map(x => x.RightTemplate).Length(int.MaxValue).Column("RIGHTTEMPLATE");
            Map(x => x.FaceTemplate).Length(int.MaxValue).Column("FACETEMPLATE");
            Map(x => x.TemplateUserId).Generated.Always().Column("TemplateUserId");
            Map(x => x.NewLeftTemplate).Length(int.MaxValue).Column("NewLeftTemplate");
            Map(x => x.NewRightTemplate).Length(int.MaxValue).Column("NewRightTemplate");
            Map(x => x.LeftTemplateV10).Length(int.MaxValue).Column("LeftTemplateV10");
            Map(x => x.RightTemplateV10).Length(int.MaxValue).Column("RightTemplateV10");
            Map(x => x.FaceTemplateV10).Length(int.MaxValue).Column("FaceTemplateV10");
            Map(x => x.FpRegistered).Generated.Always().Column("FpRegistered");
            Map(x => x.EnrollId).Column("EnrollId");
        }
    }
}
