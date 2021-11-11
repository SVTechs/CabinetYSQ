using System;
using System.Collections;
using System.Collections.Generic;
//using Domain.Qcdevice.Domain;
using Domain.ServerMain.Domain;
using Ext.Net;
using Utilities.DbHelper;
using Utilities.Ext;
using WebConsole.Bll;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //显示登录用户
        UserInfo ui = (UserInfo) Session["UserInfo"];
        lbUserName.Text = "用户: " + ui.FullName;
        btnProfileEdit.AddContainerCls("profile-edit");
        btnLogOut.AddContainerCls("log-out");

        //显示左侧菜单
        IList<PageMenus> menuList = BllPageMenus.GetPageMenus();
        if (SqlDataHelper.IsDataValid(menuList))
        {
            PageMenus menuData = ExtHelper.GetTree<PageMenus>((IList)menuList);

            Node pNode = new Node();
            menuTree.Root.Add(pNode);

            AddToMenu(menuTree.Root.Primary, menuData);
        }
    }

    private void AddToMenu(Node rootNode, PageMenus menuData)
    {
        if (menuData.MenuName != null)
        {
            rootNode.NodeID = menuData.Id;
            rootNode.Text = menuData.MenuName;
            rootNode.IconCls = string.IsNullOrEmpty(menuData.MenuIcon) ? "fa fa-angle-right" : menuData.MenuIcon;
            UserInfo ui = (UserInfo)Session["UserInfo"];
            if (menuData.MenuType == 1)
            {
                rootNode.CustomAttributes.Add(
                    new ConfigItem("url", (menuData.MenuUrl ?? "").Replace("{UserName}", ui.UserName)));
                rootNode.Leaf = true;
            }
            else if (menuData.MenuType == 2)
            {
                rootNode.CustomAttributes.Add(
                    new ConfigItem("url", "nw:" + (menuData.MenuUrl ?? "").Replace("{UserName}", ui.UserName)));
                rootNode.Leaf = true;
            }
        }
        for (int i = 0; i < menuData.TreeChildren.Count; i++)
        {
            Hashtable ht = (Hashtable) Session["UserPerm"];
            if (menuData.TreeChildren[i].IsVisible == 1)
            {
                string childId = menuData.TreeChildren[i].Id;
                if ((int) (ht[childId] ?? 0) == 1)
                {
                    Node pNode = new Node();
                    rootNode.Children.Add(pNode);

                    AddToMenu(rootNode.Children[rootNode.Children.Count - 1], menuData.TreeChildren[i]);
                }
            }
        }
    }

    protected void btnLogOut_OnDirectClick(object sender, DirectEventArgs e)
    {
        Session["UserPerm"] = null;
        Session["UserInfo"] = null;
        Response.Redirect("Login.aspx");
    }

    [DirectMethod]
    public void FillProfileInfo()
    {
        if (Session["UserPerm"] == null || Session["UserInfo"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }
        //显示登录用户
        UserInfo ui = (UserInfo) Session["UserInfo"];
        tbEdtProfileTel.Text = ui.Tel;
    }

    [DirectMethod]
    public int OnEdtProfileClick()
    {
        if (tbEdtProfilePwd.Text.Length > 0)
        {
            if (tbEdtProfilePwd.Text != tbEdtProfileNewPwd.Text)
            {
                X.Msg.Alert("提示", "密码输入不一致").Show();
                return -10;
            }
        }
        if (Session["UserPerm"] == null || Session["UserInfo"] == null)
        {
            Response.Redirect("Login.aspx");
            return -5;
        }
        UserInfo ui = (UserInfo)Session["UserInfo"];
        int result = BllUserInfo.ModifyUserInfo(ui.Id, tbEdtProfilePwd.Text, tbEdtProfileTel.Text);
        if (result > 0)
        {
            Session["UserPerm"] = null;
            Session["UserInfo"] = null;
        }
        return result;
    }
}