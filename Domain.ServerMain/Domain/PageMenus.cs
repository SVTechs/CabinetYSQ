using System.Collections.Generic;

namespace Domain.ServerMain.Domain
{
    public class PageMenus
    {
        public virtual string Id { get; set; }
        public virtual string MenuName { get; set; }
        public virtual string MenuCode { get; set; }
        public virtual int TreeLevel { get; set; }
        public virtual string TreeParent { get; set; }
        public virtual int MenuOrder { get; set; }
        public virtual int MenuType { get; set; }
        public virtual string MenuUrl { get; set; }
        public virtual string MenuIcon { get; set; }
        public virtual int IsVisible { get; set; }
        public virtual string MenuDesp { get; set; }

        //(非数据库对象)下级菜单列表
        public virtual List<PageMenus> TreeChildren { get; set; } = new List<PageMenus>();
    }
}
