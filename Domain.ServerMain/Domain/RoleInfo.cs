using System;
using System.Collections.Generic;

namespace Domain.ServerMain.Domain
{
    public class RoleInfo
    {
        public virtual string Id { get; set; }
        public virtual string RoleName { get; set; }
        public virtual int TreeLevel { get; set; }
        public virtual string TreeParent { get; set; }
        public virtual int RoleOrder { get; set; }
        public virtual string RoleDesp { get; set; }
        public virtual DateTime? LastChanged { get; set; }

        //(非数据库对象)下级菜单列表
        public virtual List<RoleInfo> TreeChildren { get; set; } = new List<RoleInfo>();
    }
}
