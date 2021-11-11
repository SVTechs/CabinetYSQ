using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Ext.Net;
using Utilities.DbHelper;
using Utilities.Ext;
using WebConsole.Bll;

// ReSharper disable MergeConditionalExpression

namespace UserConfig
{
    public partial class UserConfig_DepartInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Node rootNode = GetTreeNode();
                if (rootNode != null)
                {
                    menuTreeEdt.Root.Add(rootNode);
                }
            }
        }

        #region 部门树生成

        [DirectMethod]
        public Node GetTreeNode()
        {
            IList<DepartInfo> menuList = BllDepartInfo.SearchDepartInfo();
            if (SqlDataHelper.IsDataValid(menuList))
            {
                DepartInfo menuData = ExtHelper.GetTree<DepartInfo>((IList)menuList);

                Node pNode = new Node();
                pNode.Text = "所有部门";
                pNode.CustomAttributes.Add(new ConfigItem("departName", "所有部门", ParameterMode.Value));
                pNode.Expanded = true;
                AddToMenu(pNode, menuData);
                return pNode;
            }
            return null;
        }

        private void AddToMenu(Node rootNode, DepartInfo menuData)
        {
            if (menuData.DepartName != null)
            {
                rootNode.CustomAttributes.Add(new ConfigItem("departName", menuData.DepartName, ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("departLevel", menuData.TreeLevel.ToString(), ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("departOrder", menuData.DepartOrder.ToString(), ParameterMode.Value));
                rootNode.CustomAttributes.Add(new ConfigItem("departDesp", menuData.DepartDesp, ParameterMode.Value));
                rootNode.Text = menuData.DepartName;
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

        [DirectMethod]
        public void OnAddDepartClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "36C00A3C-294F-4D4E-83DC-516337F7FCE3"))
            {
                return;
            }
            int departLevel = 0;
            string departParent = optNodeId.Value;
            if (!string.IsNullOrEmpty(departParent))
            {
                DepartInfo di = BllDepartInfo.GetDepartInfo(departParent);
                if (di != null)
                {
                    departLevel = di.TreeLevel + 1;
                }
            }
            if (tbDepartName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入部门名称").Show();
                return;
            }
            int departOrder;
            if (!int.TryParse(tbDepartOrder.Text, out departOrder))
            {
                X.Msg.Alert("提示", "请正确输入部门排序").Show();
                return;
            }
            string departName = tbDepartName.Text;
            string departDesp = tbDepartDesp.Text;
            int result = BllDepartInfo.AddDepartInfo(departName, departLevel, departParent, departOrder, departDesp);
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
        public void FillEdtInfo(string departId)
        {
            DepartInfo di = BllDepartInfo.GetDepartInfo(departId);
            if (di != null)
            {
                tbDepartId.Text = di.Id;
                tbDepartName.Text = di.DepartName;
                tbDepartOrder.Text = di.DepartOrder.ToString();
                tbDepartDesp.Text = di.DepartDesp;
            }
        }

        [DirectMethod]
        public void OnEdtDepartClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "2F2E4CCD-8108-430F-BEB6-12B7468F6F00"))
            {
                return;
            }
            string departId = tbDepartId.Text;
            if (string.IsNullOrWhiteSpace(departId))
            {
                X.Msg.Alert("提示", "请选择有效部门").Show();
                return;
            }
            if (tbDepartName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入部门名称").Show();
                return;
            }
            int departOrder;
            if (!int.TryParse(tbDepartOrder.Text, out departOrder))
            {
                X.Msg.Alert("提示", "请正确输入角色排序").Show();
                return;
            }
            string departName = tbDepartName.Text;
            string departDesp = tbDepartDesp.Text;
            int result = BllDepartInfo.ModifyDepartInfo(departId, departName, departOrder, departDesp);
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
        public void DeleteDepartInfo(string departId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "380E98BB-5D89-4659-AAF2-0AB04967369F"))
            {
                return;
            }
            int result = BllDepartInfo.DeleteDepartInfo(departId);
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