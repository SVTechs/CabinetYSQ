using System;
using System.Collections;
using System.Collections.Generic;
//using Domain.Qcdevice.Domain;
using Domain.ServerMain.Domain;
using Ext.Net;
using Utilities.DbHelper;
using WebConsole.Bll;
using WebConsole.Config;

// ReSharper disable MergeSequentialChecks
// ReSharper disable MergeConditionalExpression

namespace UserConfig
{
    public partial class UserConfig_UserInfo : System.Web.UI.Page
    {
        private Hashtable _pageFunc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //部门列表
                Node rootNode = GetDepartNode(null);
                if (rootNode != null)
                {
                    TpDeptList.Root.Add(rootNode);
                }

                //角色列表
                Node roleNode = GetRoleNode(null);
                if (roleNode != null)
                {
                    TpRoleList.Root.Add(roleNode);
                }

                //功能列表
                Node funcNode = GetFuncNode(null);
                if (funcNode != null)
                {
                    TpPermList.Root.Add(funcNode);
                }
            }
            if (!X.IsAjaxRequest)
            {
                Session["SearchUserName"] = "";
                Session["SearchRealName"] = "";
            }
        }

        #region 部门树生成

        [DirectMethod]
        public Node GetDepartNode(Hashtable extTable)
        {
            IList<DepartInfo> menuList = BllDepartInfo.SearchDepartInfo();
            if (SqlDataHelper.IsDataValid(menuList))
            {
                DepartInfo menuData = GetDeptTree((IList)menuList);

                Node pNode = new Node();
                pNode.Text = "所有部门";
                pNode.Expanded = true;
                AddToMenu(pNode, menuData, extTable);
                return pNode;
            }
            return null;
        }

        private void AddToMenu(Node rootNode, DepartInfo menuData, Hashtable extTable)
        {
            if (menuData.DepartName != null)
            {
                rootNode.Text = menuData.DepartName;
                rootNode.NodeID = menuData.Id;
                rootNode.IconCls = null;
                rootNode.Checked = extTable != null && extTable[menuData.Id] != null;
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

                AddToMenu(rootNode.Children[rootNode.Children.Count - 1], menuData.TreeChildren[i], extTable);
            }
        }

        private DepartInfo GetDeptTree(IList menuList)
        {
            DepartInfo topMenu = new DepartInfo();
            Hashtable menuIndex = new Hashtable();
            List<DepartInfo> wMenuList = new List<DepartInfo>();
            //建立顶层菜单
            for (int i = 0; i < menuList.Count; i++)
            {
                DepartInfo curMenu = (DepartInfo)menuList[i];
                wMenuList.Add(curMenu);
                //记录索引位置
                menuIndex[curMenu.Id] = i;
                if (curMenu.TreeLevel == 0)
                {
                    //顶级菜单项
                    topMenu.TreeChildren.Add(wMenuList[i]);
                }
                else
                {
                    //查找父级并插入
                    int listIndex = (int)menuIndex[curMenu.TreeParent];
                    wMenuList[listIndex].TreeChildren.Add(wMenuList[i]);
                }
            }
            return topMenu;
        }

        #endregion

        #region 角色树生成
        public Node GetRoleNode(Hashtable extRole)
        {
            IList<RoleInfo> menuList = BllRoleInfo.SearchRoleInfo();
            if (SqlDataHelper.IsDataValid(menuList))
            {
                RoleInfo menuData = GetRoleTree((IList)menuList);

                Node pNode = new Node();
                pNode.Text = "所有角色";
                pNode.Expanded = true;
                AddToMenu(pNode, menuData, extRole);
                return pNode;
            }
            return null;
        }

        private void AddToMenu(Node rootNode, RoleInfo menuData, Hashtable extRole)
        {
            if (menuData.RoleName != null)
            {
                rootNode.Text = menuData.RoleName;
                rootNode.NodeID = menuData.Id;
                rootNode.IconCls = null;
                rootNode.Checked = extRole != null && extRole[menuData.Id] != null;
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

                AddToMenu(rootNode.Children[rootNode.Children.Count - 1], menuData.TreeChildren[i], extRole);
            }
        }

        private RoleInfo GetRoleTree(IList menuList)
        {
            RoleInfo topMenu = new RoleInfo();
            Hashtable menuIndex = new Hashtable();
            List<RoleInfo> wMenuList = new List<RoleInfo>();
            //建立顶层菜单
            for (int i = 0; i < menuList.Count; i++)
            {
                RoleInfo curMenu = (RoleInfo)menuList[i];
                wMenuList.Add(curMenu);
                //记录索引位置
                menuIndex[curMenu.Id] = i;
                if (curMenu.TreeLevel == 0)
                {
                    //顶级菜单项
                    topMenu.TreeChildren.Add(wMenuList[i]);
                }
                else
                {
                    //查找父级并插入
                    int listIndex = (int)menuIndex[curMenu.TreeParent];
                    wMenuList[listIndex].TreeChildren.Add(wMenuList[i]);
                }
            }
            return topMenu;
        }
        #endregion

        #region 功能树生成
        public Node GetFuncNode(Hashtable extTable)
        {
            if (_pageFunc == null) _pageFunc = BllPageFunctions.GetFunctionTable();
            IList<PageMenus> menuList = BllPageMenus.GetPageMenus();
            if (SqlDataHelper.IsDataValid(menuList))
            {
                PageMenus menuData = GetMenuTree((IList)menuList);

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

        private PageMenus GetMenuTree(IList menuList)
        {
            PageMenus topMenu = new PageMenus();
            Hashtable menuIndex = new Hashtable();
            List<PageMenus> wMenuList = new List<PageMenus>();
            //建立顶层菜单
            for (int i = 0; i < menuList.Count; i++)
            {
                PageMenus curMenu = (PageMenus)menuList[i];
                wMenuList.Add(curMenu);
                //记录索引位置
                menuIndex[curMenu.Id] = i;
                if (curMenu.TreeLevel == 0)
                {
                    //顶级菜单项
                    topMenu.TreeChildren.Add(wMenuList[i]);
                }
                else
                {
                    //查找父级并插入
                    int listIndex = (int)menuIndex[curMenu.TreeParent];
                    wMenuList[listIndex].TreeChildren.Add(wMenuList[i]);
                }
            }
            return topMenu;
        }
        #endregion

        protected void UserGridData_OnReadData_Refresh(object sender, StoreReadDataEventArgs e)
        {
            e.Total = BllUserInfo.GetUserCount((Session["SearchUserName"] ?? "").ToString(),
                (Session["SearchRealName"] ?? "").ToString());
            IList<UserInfo> userList = BllUserInfo.SearchUser((Session["SearchUserName"] ?? "").ToString(), 
                (Session["SearchRealName"] ?? "").ToString(), e.Start, e.Limit);
            if (SqlDataHelper.IsDataValid(userList))
            {
                List<object[]> listData = new List<object[]>();
                for (int i = 0; i < userList.Count; i++)
                {
                    UserInfo ui = userList[i];
                    listData.Add(new object[] {ui.Id, ui.UserName, ui.FullName, ui.Tel, 
                        ui.Updatetime != null ? ui.Updatetime.Date.ToString("yyyy-MM-dd HH:mm") : ""});
                }
                ((Store)sender).DataSource = listData;
            }
        }

        [DirectMethod]
        public void OnAddUserClick(string deptList)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "7C33BD62-C0C3-4693-9E51-002D4C1601A2"))
            {
                return;
            }
            if (tbUserName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入用户名称").Show();
                return;
            }
            string userName = tbUserName.Text;
            string userPwd = tbUserPwd.Text;
            string realName = tbRealName.Text;
            string userTel = tbUserTel.Text;
            if (string.IsNullOrWhiteSpace(userPwd))
            {
                userPwd = Env.DefaultPwd;
            }
            string userId = BllUserInfo.AddUserInfo(userName, userPwd, realName, userTel);
            if (string.IsNullOrEmpty(userId))
            {
                X.Msg.Alert("提示", "添加失败，请检查网络").Show();
                return;
            }
            int result = BllDepartSettings.SetUserDepart(userId, deptList);
            if (result < 0)
            {
                X.Msg.Alert("提示", "用户部门保存失败").Show();
                return;
            }
            X.Msg.Alert("提示", "添加成功").Show();
        }

        [DirectMethod]
        public void FillEdtInfo(string userId)
        {
            UserInfo ui = BllUserInfo.GetUserInfo(userId);
            if (ui != null)
            {
                tbUserId.Text = ui.Id;
                tbUserPwd.Text = "";
                tbUserName.Text = ui.UserName;
                tbRealName.Text = ui.FullName;
                tbUserTel.Text = ui.Tel;
            }
        }

        [DirectMethod]
        public void OnEdtUserClick(string deptList)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "1F8AD215-CA41-41E4-A420-4E21547BB7B8"))
            {
                return;
            }
            string userId = tbUserId.Text;
            if (string.IsNullOrWhiteSpace(userId))
            {
                X.Msg.Alert("提示", "请选择有效用户").Show();
                return;
            }
            if (tbUserName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入用户名称").Show();
                return;
            }
            string userName = tbUserName.Text;
            string userPwd = tbUserPwd.Text;
            string realName = tbRealName.Text;
            string userTel = tbUserTel.Text;
            if (string.IsNullOrWhiteSpace(userPwd))
            {
                userPwd = Env.DefaultPwd;
            }
            int result = BllUserInfo.ModifyUserInfo(userId, userName, userPwd, realName, userTel);
            if (result < 0)
            {
                X.Msg.Alert("提示", "修改失败，请检查网络").Show();
            }
            result = BllDepartSettings.SetUserDepart(userId, deptList);
            if (result < 0)
            {
                X.Msg.Alert("提示", "修改失败，请检查网络").Show();
            }
            X.Msg.Alert("提示", "修改成功").Show();
        }

        [DirectMethod]
        public void DeleteUserInfo(string userId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "28741EE8-662C-412E-A008-B129462B64F1"))
            {
                return;
            }
            int result = BllUserInfo.DeleteUserInfo(userId);
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
        public Node FillDeptInfo(string userId)
        {
            IList<DepartSettings> departList = BllDepartSettings.GetUserDepartSettings(userId);
            if (SqlDataHelper.IsDataValid(departList))
            {
                Hashtable departTable = new Hashtable();
                for (int i = 0; i < departList.Count; i++)
                {
                    departTable[departList[i].DepartId] = 1;
                }
                return GetDepartNode(departTable);
            }
            return GetDepartNode(null);
        }

        [DirectMethod]
        public Node FillRoleInfo(string userId)
        {
            tbRoleUserId.Text = userId;
            IList<RoleSettings> roleList = BllRoleSettings.GetUserRoleSettings(userId);
            if (SqlDataHelper.IsDataValid(roleList))
            {
                Hashtable roleTable = new Hashtable();
                for (int i = 0; i < roleList.Count; i++)
                {
                    roleTable[roleList[i].RoleId] = 1;
                }
                return GetRoleNode(roleTable);
            }
            return GetRoleNode(null);
        }

        [DirectMethod]
        public void SetUserRole(string roleId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "558FD933-40AF-43A8-9DAD-F82B0273D7FD"))
            {
                return;
            }
            string userId = tbRoleUserId.Text;
            if (string.IsNullOrWhiteSpace(userId))
            {
                X.Msg.Alert("提示", "请选择有效用户").Show();
                return;
            }
            int result = BllRoleSettings.SetUserRole(userId, roleId);
            if (result > 0)
            {
                X.Msg.Alert("提示", "设置成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "设置失败，请检查网络").Show();
            }
        }

        [DirectMethod]
        public Node FillPermInfo(string roleId)
        {
            tbPermUserId.Text = roleId;
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
        public void SetUserPerm(string permId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "0120E3E1-8AC4-4EB0-BE10-7291AABE72DD"))
            {
                return;
            }
            string userId = tbPermUserId.Text;
            if (string.IsNullOrWhiteSpace(userId))
            {
                X.Msg.Alert("提示", "请选择有效用户").Show();
                return;
            }
            int result = BllPermissionSettings.SetOwnerPermission("User", userId, permId);
            if (result > 0)
            {
                X.Msg.Alert("提示", "设置成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", "设置失败，请检查网络").Show();
            }
        }


        [DirectMethod]
        public void SaveQueryInput()
        {
            Session["SearchUserName"] = tbSearchUserName.Text;
            Session["SearchRealName"] = tbSearchRealName.Text;
        }
    }
}
