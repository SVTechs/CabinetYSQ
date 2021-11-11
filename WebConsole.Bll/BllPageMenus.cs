using System.Collections.Generic;
using Domain.ServerMain.Domain;
using WebConsole.Dal;

namespace WebConsole.Bll
{
    public class BllPageMenus
    {
        public static PageMenus GetPageMenu(string menuId)
        {
            if (string.IsNullOrEmpty(menuId)) return null;
            return DalPageMenus.GetPageMenu(menuId);
        }

        public static IList<PageMenus> GetPageMenus()
        {
            return DalPageMenus.GetPageMenus();
        }

        public static int AddPageMenu(string menuName, int menuLevel, string menuParent, int menuOrder,
            int menuType, string menuUrl, string menuIcon, int isVisible, string menuDesp)
        {
            if (string.IsNullOrEmpty(menuName) || menuLevel < 0 || menuType < 0)
            {
                return -100;
            }
            return DalPageMenus.AddPageMenu(menuName, menuLevel, menuParent, menuOrder, menuType,
                menuUrl, menuIcon, isVisible, menuDesp);
        }

        public static int ModifyPageMenu(string menuId, string menuName, int menuOrder,
            int menuType, string menuUrl, string menuIcon, int isVisible, string menuDesp)
        {
            if (string.IsNullOrEmpty(menuId) || string.IsNullOrEmpty(menuName) || menuType < 0)
            {
                return -100;
            }
            return DalPageMenus.ModifyPageMenu(menuId, menuName, menuOrder, menuType,
                menuUrl, menuIcon, isVisible, menuDesp);
        }

        public static int DeletePageMenu(string menuId)
        {
            if (string.IsNullOrEmpty(menuId)) return -100;
            return DalPageMenus.DeletePageMenu(menuId);
        }
    }
}
