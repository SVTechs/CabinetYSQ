using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Ext.Net;
using Utilities.DbHelper;
using Utilities.Ext;
using WebConsole.Bll;

// ReSharper disable MergeSequentialChecks
// ReSharper disable MergeConditionalExpression

namespace UserConfig
{
    public partial class UserConfig_RoleInfo : System.Web.UI.Page
    {
        private Hashtable _pageFunc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Node roleNode = GetRoleNode();
                if (roleNode != null)
                {
                    roleTreeEdt.Root.Add(roleNode);
                }

                Node funcNode = GetFuncNode(null);
                if (funcNode != null)
                {
                    TpPermList.Root.Add(funcNode);
                }
            }
        }

        #region 角色树生成

        [DirectMethod]
        public Node GetRoleNode()
        {
            IList<RoleInfo> menuList = BllRoleInfo.SearchRoleInfo();
            if (SqlDataHelper.IsDataValid(menuList))
            {
                RoleInfo menuData = ExtHelper.GetTree<RoleInfo>((IList)menuList);

                Node pNode = new Node();
                pNode.Text = "所有角色";
                pNode.CustomAttributes.Add(new ConfigItem("roleName", "所有角色", ParameterMode.Value));
                pNode.Expanded = true;
                AddToMenu(pNode, menuData);
                return pNode;
            }
            return null;
        }

        private void AddToMenu(Node rootNode, RoleInfo menuData)
        {
            if (menuData.RoleName != null)
            {
                rootNode.CustomAttributes.Add(new ConfigItem("roleName", menuData.RoleName, ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("roleLevel", menuData.TreeLevel.ToString(), ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("roleOrder", menuData.RoleOrder.ToString(), ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("roleDesp", menuData.RoleDesp, ParameterMode.Value));
                rootNode.Text = menuData.RoleName;
                rootNode.NodeID = menuData.Id;
                rootNode.IconCls = null;
                if (menuData.TreeChildren.Count == 0)
                {
                    rootNode.Leaf = true;
                }
                else
                {
                    rootNode.Expanded = true;
                }
            }
            for (int i = 0; i < menuData.TreeChildren.Count; i++)
            {
                Node pNode = new Node();
                rootNode.Children.Add(pNode);

                AddToMenu(rootNode.Children[rootNode.Children.Count - 1], menuData.TreeChildren[i]);
            }
        }
        #endregion

        #region 功能树生成

        public Node GetFuncNode(Hashtable extTable)
        {
            if (_pageFunc == null) _pageFunc = BllPageFunctions.GetFunctionTable();
            IList<PageMenus> menuList = BllPageMenus.GetPageMenus();
            if (SqlDataHelper.IsDataValid(menuList))
            {
                PageMenus menuData = ExtHelper.GetTree<PageMenus>((IList)menuList);

                Node pNode = new Node();
                pNode.Text = "系统菜单";
                pNode.Expanded = true;
                AddToMenu(pNode, menuData, extTable);
                return pNode;
            }
            return null;
        }

        private void AddToMenu(Node rootNode, PageMenus menuData, Hashtable extTable)
        {
            if (menuData.MenuName != null)
            {
                rootNode.Text = menuData.MenuName;
                rootNode.Checked = false;
                rootNode.CustomAttributes.Add(new ConfigItem("url", menuData.MenuUrl));
                if (menuData.MenuType == 1)
                {
                    rootNode.NodeID = "P-" + menuData.Id;
                    rootNode.Icon = Icon.None;
                    //页面节点
                    if (_pageFunc[menuData.Id] != null)
                    {
                        ArrayList al = (ArrayList)_pageFunc[menuData.Id];
                        for (int i = 0; i < al.Count; i++)
                        {
                            PageFunctions pf = (PageFunctions)al[i];
                            Node pNode = new Node();
                            pNode.NodeID = "F-" + pf.Id;
                            pNode.Text = pf.FunctionName;
                            pNode.Checked = extTable != null && extTable[pf.Id] != null;
                            pNode.Leaf = true;
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
                    rootNode.Expanded = true;
                }
                rootNode.Checked = extTable != null && extTable[menuData.Id] != null;
            }
            for (int i = 0; i < menuData.TreeChildren.Count; i++)
            {
                Node pNode = new Node();
                rootNode.Children.Add(pNode);

                AddToMenu(rootNode.Children[rootNode.Children.Count - 1], menuData.TreeChildren[i], extTable);
            }
        }
        #endregion

        [DirectMethod]
        public void OnAddRoleClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "4297F33F-F24F-4785-88F6-3C22BFA19BE6"))
            {
                return;
            }
            int roleLevel = 0;
            string roleParent = optNodeId.Value;
            if (!string.IsNullOrEmpty(roleParent))
            {
                RoleInfo pm = BllRoleInfo.GetRoleInfo(roleParent);
                if (pm != null)
                {
                    roleLevel = pm.TreeLevel + 1;
                }
            }
            if (tbRoleName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入角色名称").Show();
                return;
            }
            int roleOrder;
            if (!int.TryParse(tbRoleOrder.Text, out roleOrder))
            {
                X.Msg.Alert("提示", "请正确输入角色排序").Show();
                return;
            }
            string roleName = tbRoleName.Text;
            string roleDesp = tbRoleDesp.Text;
            int result = BllRoleInfo.AddRoleInfo(roleName, roleLevel, roleParent, roleOrder, roleDesp);
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
        public void FillEdtInfo(string roleId)
        {
            RoleInfo ui = BllRoleInfo.GetRoleInfo(roleId);
            if (ui != null)
            {
                tbRoleId.Text = ui.Id;
                tbRoleName.Text = ui.RoleName;
                tbRoleOrder.Text = ui.RoleOrder.ToString();
                tbRoleDesp.Text = ui.RoleDesp;
            }
        }

        [DirectMethod]
        public void OnEdtRoleClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "00A0A80D-B7B9-4481-A3BD-1C706FABE867"))
            {
                return;
            }
            string roleId = tbRoleId.Text;
            if (string.IsNullOrWhiteSpace(roleId))
            {
                X.Msg.Alert("提示", "请选择有效角色").Show();
                return;
            }
            if (tbRoleName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入角色名称").Show();
                return;
            }
            int roleOrder;
            if (!int.TryParse(tbRoleOrder.Text, out roleOrder))
            {
                X.Msg.Alert("提示", "请正确输入角色排序").Show();
                return;
            }
            string roleName = tbRoleName.Text;
            string roleDesp = tbRoleDesp.Text;
            int result = BllRoleInfo.ModifyRoleInfo(roleId, roleName, roleOrder, roleDesp);
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
        public void DeleteRoleInfo(string roleId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "4FA72BD8-227E-4881-9BFF-D6F105769F03"))
            {
                return;
            }
            int result = BllRoleInfo.DeleteRoleInfo(roleId);
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
        public Node FillPermInfo(string roleId)
        {
            tbPermRoleId.Text = roleId;
            IList<PermissionSettings> permList = BllPermissionSettings.GetOwnerPermissionSettings(roleId);
            if (SqlDataHelper.IsDataValid(permList))
            {
                Hashtable roleTable = new Hashtable();
                for (int i = 0; i < permList.Count; i++)
                {
                    roleTable[permList[i].AccessId] = 1;
                }
                return GetFuncNode(roleTable);
            }
            return GetFuncNode(null);
        }

        [DirectMethod]
        public void SetRolePerm(string permId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "3B7B7DEC-E508-4D7B-98B9-8A9F26DD237D"))
            {
                return;
            }
            string roleId = tbPermRoleId.Text;
            if (string.IsNullOrWhiteSpace(roleId))
            {
                X.Msg.Alert("提示", "请选择有效角色").Show();
                return;
            }
            int result = BllPermissionSettings.SetOwnerPermission("Role", roleId, permId);
            if (result > 0)
            {
                X.Msg.Alert("提示", "设置成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "设置失败，请检查网络").Show();
            }
        }
    }
}