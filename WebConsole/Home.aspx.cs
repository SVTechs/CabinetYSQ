using System;
using Domain.ServerMain.Domain;

public partial class Home : System.Web.UI.Page
{
    public string RealName = "", LastLogin = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //显示登录用户
        UserInfo ui = (UserInfo)Session["UserInfo"];
        RealName = ui.FullName;
    }
}