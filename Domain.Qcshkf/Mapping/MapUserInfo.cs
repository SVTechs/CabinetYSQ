﻿using Domain.Qcshkf.Domain;
using FluentNHibernate.Mapping;

namespace Domain.Qcshkf.Mapping
{
    public class MapUserInfo : ClassMap<UserInfo>
    {
        public MapUserInfo()
        {
            Table("Sys_User");
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
            Map(x => x.LastChanged).Column("Updatetime");
            Map(x => x.UpdateUser).Column("UpdateUser");
            Map(x => x.OrgId).Column("OrgId");
            //Map(x => x.CardNum).Column("CardNum");
            //Map(x => x.EmpName).Column("EmpName");
            Map(x => x.LeftTemplate).Column("LEFTTEMPLATE");
            Map(x => x.RightTemplate).Column("RIGHTTEMPLATE");
            Map(x => x.TemplateUserId).Generated.Always().Column("TemplateUserId");
            Map(x => x.NewLeftTemplate).Column("NewLeftTemplate");
            Map(x => x.NewRightTemplate).Column("NewRightTemplate");
            Map(x => x.LeftTemplateV10).Column("LeftTemplateV10");
            Map(x => x.RightTemplateV10).Column("RightTemplateV10");
        }
    }
}
