using System;

namespace Domain.Qcdevice.Domain
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
        public virtual DateTime LastChanged { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string OrgId { get; set; }
        public virtual string CardNum { get; set; }
        public virtual int TemplateUserId { get; set; }
        public virtual byte[] LeftTemplate { get; set; }
        public virtual byte[] RightTemplate { get; set; }
        public virtual string NewLeftTemplate { get; set; }
        public virtual string NewRightTemplate { get; set; }
    }
}
