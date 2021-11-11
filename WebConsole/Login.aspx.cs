using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
//using Domain.Qcdevice.Domain;
using Domain.ServerMain.Domain;
using Domain.ZWStock.Domain;
using Utilities.DbHelper;
using WebConsole.Bll;

public partial class _Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        tbUserName.InputWrapCls = "input-none-border";
        tbUserName.TriggerWrapCls = "input-none-border";
        tbUserPassword.InputWrapCls = "input-none-border";
        tbUserPassword.TriggerWrapCls = "input-none-border";
    }

    [DirectMethod]
    public void BtnLoginClick()
    {
        string userName = tbUserName.Text.Trim();
        string password = tbUserPassword.Text;
        if (string.IsNullOrEmpty(tbUserName.Text) || string.IsNullOrEmpty(tbUserPassword.Text))
        {
            X.Msg.Alert("提示", "用户名及密码不能为空").Show();
            return;
        }
        UserInfo ui = BllUserInfo.GetUserInfo(userName, password);
        if (ui != null)
        {
            Session["UserInfo"] = ui;
            List<string> roleList =new List<string>(); 
            IList<RoleSettings> roleData = BllRoleSettings.GetUserRoleSettings(ui.Id);
            if (SqlDataHelper.IsDataValid(roleData))
            {
                for (int i = 0; i < roleData.Count; i++)
                {
                    roleList.Add(roleData[i].RoleId);
                }
            }
            IList permList = BllPermissionSettings.GetFullPermissionSettings(ui.Id, roleList);
            if (SqlDataHelper.IsDataValid(permList))
            {
                Hashtable ht = new Hashtable();
                for (int i = 0; i < permList.Count; i++)
                {
                    PermissionSettings ps = (PermissionSettings) permList[i];
                    ht[ps.AccessId] = 1;
                }
                Session["UserPerm"] = ht;
            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["4A2C0DC2-5A7A-4253-A405-ADD590CB88FD"] = 1;
                ht["87920816-41BE-413A-AB97-7633840A6830"] = 1;
                Session["UserPerm"] = ht;
            }
            Response.Redirect("Default.aspx");
        }
        else
        {
            X.Msg.Alert("提示", "用户名或密码错误").Show();
        }
    }
}