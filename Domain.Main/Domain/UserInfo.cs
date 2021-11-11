using System;

namespace Domain.Main.Domain
{
    public class UserInfo
    {
        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Sex { get; set; }
        public virtual int Age { get; set; }
        public virtual string Tel { get; set; }
        public virtual string Adress { get; set; }
        public virtual string Email { get; set; }
        public virtual int UserState { get; set; }
        public virtual DateTime Createtime { get; set; }
        public virtual string CreateUser { get; set; }
        public virtual DateTime Updatetime { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string OrgId { get; set; }
        public virtual string OrgName { get; set; }
        public virtual string CardNum { get; set; }
        public virtual string EmpName { get; set; }
        public virtual byte[] LeftTemplate { get; set; }
        public virtual byte[] RightTemplate { get; set; }
        public virtual byte[] FaceTemplate { get; set; }
        public virtual int TemplateUserId { get; set; }
        public virtual string NewLeftTemplate { get; set; }
        public virtual string NewRightTemplate { get; set; }
        public virtual string LeftTemplateV10 { get; set; }
        public virtual string RightTemplateV10 { get; set; }
        public virtual string FaceTemplateV10 { get; set; }
        public virtual int FpRegistered { get; set; }
        public virtual string EnrollId { get; set; }
    }
}
