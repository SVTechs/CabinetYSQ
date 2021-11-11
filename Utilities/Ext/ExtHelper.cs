using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities.Ext
{
    /// <summary>
    /// ExtHelper By JHY
    /// </summary>
    public class ExtHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="menuList"></param>
        /// <returns></returns>
        public static T GetTree<T>(IList menuList) where T : new()
        {
            T topMenu = new T();
            Hashtable menuIndex = new Hashtable();
            List<T> wMenuList = new List<T>();
            //建立顶层菜单
            for (int i = 0; i < menuList.Count; i++)
            {
                T curMenu = (T)menuList[i];
                wMenuList.Add(curMenu);
                //记录索引位置
                PropertyInfo property = typeof(T).GetProperty("Id");
                menuIndex[property.GetValue(curMenu, null)] = i;
                property = typeof(T).GetProperty("TreeLevel");
                if ((int)property.GetValue(curMenu, null) == 0)
                {
                    //顶级菜单项
                    property = typeof(T).GetProperty("TreeChildren");
                    ((List<T>)property.GetValue(topMenu, null)).Add(wMenuList[i]);
                }
                else
                {
                    //查找父级并插入
                    property = typeof(T).GetProperty("TreeParent");
                    int listIndex = (int)menuIndex[property.GetValue(curMenu, null)];
                    property = typeof(T).GetProperty("TreeChildren");
                    ((List<T>)property.GetValue(wMenuList[listIndex], null)).Add(wMenuList[i]);
                }
            }
            return topMenu;
        }
    }
}
