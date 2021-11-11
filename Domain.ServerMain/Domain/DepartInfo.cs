using System;
using System.Collections.Generic;

namespace Domain.ServerMain.Domain
{
    public class DepartInfo
    {
        public virtual string Id { get; set; }

        public virtual string DepartName { get; set; }

        public virtual int TreeLevel { get; set; }

        public virtual string TreeParent { get; set; }

        public virtual int? DepartOrder { get; set; }

        public virtual string DepartDesp { get; set; }

        public virtual DateTime? LastChanged { get; set; }

        //(非数据库对象)下级菜单列表
        public virtual List<DepartInfo> TreeChildren { get; set; } = new List<DepartInfo>();
    }
}
