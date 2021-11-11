using System;

namespace Domain.Main.Domain
{
    public class RedisUserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string EmpName { get; set; }

        public string OrgId { get; set; }

        public string Sex { get; set; }

        public int? Age { get; set; }

        public string Adress { get; set; }

        public string EMail { get; set; }

        public string CellPhoneNo { get; set; }

        public int? State { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateTime { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string CardNum { get; set; }

        public string OrgName { get; set; }

        public byte[] LeftTemplate { get; set; }

        public byte[] RightTemplate { get; set; }

        public int TemplateUserId { get; set; }
    }
}
