using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Ext.Net;
using Utilities.DbHelper;
using Utilities.Ext;
using WebConsole.Bll;

namespace SysConfig
{
    public partial class SysConfig_MenuConfig : System.Web.UI.Page
    {
        private Hashtable _pageFunc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Node rootNode = GetTreeNode();
                if (rootNode != null)
                {
                    menuTreeEdt.Root.Add(rootNode);
                }
                cbMenuVisible.Select(1);
                cbMenuType.Select(0);
            }
        }

        [DirectMethod]
        public Node GetTreeNode()
        {
            if (_pageFunc == null) _pageFunc = BllPageFunctions.GetFunctionTable();
            IList<PageMenus> menuList = BllPageMenus.GetPageMenus();
            if (SqlDataHelper.IsDataValid(menuList))
            {
                PageMenus menuData = ExtHelper.GetTree<PageMenus>((IList)menuList);

                Node pNode = new Node();
                pNode.Text = "所有菜单";
                pNode.CustomAttributes.Add(new ConfigItem("menuName", "所有菜单", ParameterMode.Value));
                pNode.Expanded = true;
                AddToMenu(pNode, menuData);
                return pNode;
            }
            return null;
        }

        private void AddToMenu(Node rootNode, PageMenus menuData)
        {
            if (menuData.MenuName != null)
            {
                rootNode.Text = menuData.MenuName;
                rootNode.CustomAttributes.Add(new ConfigItem("menuName", menuData.MenuName, ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("menuLevel", menuData.TreeLevel.ToString(), ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("menuOrder", menuData.MenuOrder.ToString(), ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("menuUrl", menuData.MenuUrl, ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("isVisible", menuData.IsVisible.ToString(), ParameterMode.Value));
                if (menuData.MenuType == 1 || menuData.MenuType == 2)
                {
                    rootNode.NodeID = "P-" + menuData.Id;
                    //页面节点
                    if (_pageFunc[menuData.Id] != null)
                    {
                        ArrayList al = (ArrayList) _pageFunc[menuData.Id];
                        for (int i = 0; i < al.Count; i++)
                        {
                            PageFunctions pf = (PageFunctions) al[i];
                            Node pNode = new Node();
                            pNode.NodeID = "F-" + pf.Id;
                            pNode.Text = pf.FunctionName;
                            pNode.Leaf = true;
                            pNode.CustomAttributes.Add(new ConfigItem("menuName", pf.FunctionName, ParameterMode.Value));
                            pNode.CustomAttributes.Add(new ConfigItem("menuLevel", "<功能>", ParameterMode.Value));
                            pNode.CustomAttributes.Add(new ConfigItem("menuOrder", pf.FunctionOrder.ToString(), ParameterMode.Value));
                            pNode.CustomAttributes.Add(new ConfigItem("menuUrl", "-", ParameterMode.Value));
                            pNode.CustomAttributes.Add(new ConfigItem("isVisible", "-", ParameterMode.Value));
                            rootNode.Children.Add(pNode);
                        }
                    }
                    else
                    {
                        rootNode.Leaf = true;
                    }
                }
                else
                {
                    rootNode.NodeID = "W-" + menuData.Id;
                    if (menuData.TreeChildren.Count > 0)
                    {
                        rootNode.Expanded = true;
                    }
                }
            }
            for (int i = 0; i < menuData.TreeChildren.Count; i++)
            {
                Node pNode = new Node();
                rootNode.Children.Add(pNode);

                AddToMenu(rootNode.Children[rootNode.Children.Count - 1], menuData.TreeChildren[i]);
            }
        }

        [DirectMethod]
        public void OnAddMenuClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "11C56D90-8D24-4D8F-99BC-CC7B20F01A24"))
            {
                return;
            }
            int menuLevel = 0;
            string menuParent = optNodeId.Value;
            if (!string.IsNullOrEmpty(menuParent) && !menuParent.ToUpper().Equals("OT"))
            {
                PageMenus pm = BllPageMenus.GetPageMenu(menuParent);
                if (pm != null)
                {
                    menuLevel = pm.TreeLevel + 1;
                }
            }
            if (tbMenuName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入菜单名称").Show();
                return;
            }
            int menuOrder;
            if (!int.TryParse(tbMenuOrder.Text, out menuOrder))
            {
                X.Msg.Alert("提示", "请正确输入菜单排序").Show();
                return;
            }
            int menuType = int.Parse(cbMenuType.Value.ToString());
            string menuName = tbMenuName.Text;
            string menuIcon = tbMenuIcon.Text;
            string menuUrl = tbMenuUrl.Text;
            string menuDesp = tbMenuDesp.Text;
            int menuVisible = int.Parse(cbMenuVisible.Value.ToString());
            int result = BllPageMenus.AddPageMenu(menuName, menuLevel, menuParent, menuOrder, menuType,
                menuUrl, menuIcon, menuVisible, menuDesp);
            if (result > 0)
            {
                X.Msg.Alert("提示", "添加成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "添加失败，请检查网络").Show();
            }
        }

        [DirectMethod]
        public void FillEdtInfo(string menuId)
        {
            PageMenus pi = BllPageMenus.GetPageMenu(menuId);
            if (pi != null)
            {
                tbMenuName.Text = pi.MenuName;
                tbMenuIcon.Text = pi.MenuIcon;
                tbMenuOrder.Text = pi.MenuOrder.ToString();
                cbMenuType.Select(pi.MenuType);
                tbMenuUrl.Text = pi.MenuUrl;
                cbMenuVisible.Select(pi.IsVisible);
            }
        }

        [DirectMethod]
        public void OnEdtMenuClick(string menuId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "761C3B6E-B57D-4CBB-88CA-5C643FA0C5E2"))
            {
                return;
            }
            if (tbMenuName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入菜单名称").Show();
                return;
            }
            int menuOrder;
            if (!int.TryParse(tbMenuOrder.Text, out menuOrder))
            {
                X.Msg.Alert("提示", "请正确输入菜单排序").Show();
                return;
            }
            int menuType = int.Parse(cbMenuType.Value.ToString());
            string menuName = tbMenuName.Text;
            string menuIcon = tbMenuIcon.Text;
            string menuUrl = tbMenuUrl.Text;
            string menuDesp = tbMenuDesp.Text;
            int menuVisible = int.Parse(cbMenuVisible.Value.ToString());
            int result = BllPageMenus.ModifyPageMenu(menuId, menuName, menuOrder, menuType, menuUrl, menuIcon,
                menuVisible, menuDesp);
            if (result > 0)
            {
                X.Msg.Alert("提示", "修改成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "修改失败，请检查网络").Show();
            }
        }

        [DirectMethod]
        public void DeletePageMenu(string menuId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "D17DCCC8-1C7B-4090-BC41-88BB15C09D82"))
            {
                return;
            }
            int result = BllPageMenus.DeletePageMenu(menuId);
            if (result > 0)
            {
                X.Msg.Alert("提示", "删除成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "删除失败，请检查网络").Show();
            }
        }

        [DirectMethod]
        public void OnAddFuncClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "807FA4DD-39A6-47A7-BE33-28CD143E6679"))
            {
                return;
            }
            string funcMenu = optNodeId.Value;
            if (string.IsNullOrEmpty(funcMenu))
            {
                X.Msg.Alert("提示", "功能页面无效").Show();
                return;
            }
            if (tbFuncName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入功能名称").Show();
                return;
            }
            int funcOrder;
            if (!int.TryParse(tbFuncOrder.Text, out funcOrder))
            {
                X.Msg.Alert("提示", "请正确输入功能排序").Show();
                return;
            }
            string funcName = tbFuncName.Text;
            string funcDesp = tbFuncDesp.Text;
            int result = BllPageFunctions.AddPageFunction(funcName, funcOrder, funcMenu, funcDesp);
            if (result > 0)
            {
                X.Msg.Alert("提示", "添加成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "添加失败，请检查网络").Show();
            }
        }

        [DirectMethod]
        public void FillEdtFuncInfo(string funcId)
        {
            PageFunctions pi = BllPageFunctions.GetPageFunctions(funcId);
            if (pi != null)
            {
                tbFuncName.Text = pi.FunctionName;
                tbFuncOrder.Text = pi.FunctionOrder.ToString();
                tbFuncDesp.Text = pi.FunctionDesp;
            }
        }

        [DirectMethod]
        public void OnEdtFuncClick(string funcId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "DBE16603-E816-4395-A6C6-FFD5B7F13AA8"))
            {
                return;
            }
            if (string.IsNullOrEmpty(funcId))
            {
                X.Msg.Alert("提示", "指定功能无效").Show();
                return;
            }
            if (tbFuncName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入功能名称").Show();
                return;
            }
            int funcOrder;
            if (!int.TryParse(tbFuncOrder.Text, out funcOrder))
            {
                X.Msg.Alert("提示", "请正确输入功能排序").Show();
                return;
            }
            string funcName = tbFuncName.Text;
            string funcDesp = tbFuncDesp.Text;
            int result = BllPageFunctions.ModifyPageFunction(funcId, funcName, funcOrder, funcDesp);
            if (result > 0)
            {
                X.Msg.Alert("提示", "修改成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "修改失败，请检查网络").Show();
            }
        }

        [DirectMethod]
        public void DeleteFunc(string funcId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "C4D58A7B-8275-4DDD-BDF5-818C6CC6CFE9"))
            {
                return;
            }
            int result = BllPageFunctions.DeletePageFunctions(funcId);
            if (result > 0)
            {
                X.Msg.Alert("提示", "删除成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "删除失败，请检查网络").Show();
            }
        }
    }
}