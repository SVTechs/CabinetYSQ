using System;
using System.Web;
using WebConsole.Config;

/// <summary>
/// AuthenticationModule 的摘要说明
/// </summary>
public class AuthenticationModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.AcquireRequestState += context_AcquireRequestState;
    }

    void context_AcquireRequestState(object sender, EventArgs e)
    {
        HttpApplication application = (HttpApplication)sender;
        HttpContext context = application.Context;

        //生成当前路径
        string relPath = context.Request.ApplicationPath;
        if (string.IsNullOrEmpty(relPath) || relPath.Equals("/"))
        {
            Env.AppRoot = "/";
        }
        else
        {
            Env.AppRoot = context.Request.ApplicationPath + "/";
            if (!Env.AppRoot.StartsWith("/")) Env.AppRoot = "/" + Env.AppRoot;
        }

        //基本Session验证
        string requestUrl = application.Request.Url.ToString();
        if (requestUrl.Contains("ArtificialForms")) return;

        string requestPage = requestUrl.Substring(requestUrl.LastIndexOf('/') + 1);
        if (requestPage.ToUpper().Contains(".ASPX") && !requestPage.ToUpper().Equals("LOGIN.ASPX"))
        {
            if (context.Session["UserInfo"] == null || context.Session["UserPerm"] == null)
            {
                //未登录则跳转至登录页面
                context.Response.Redirect(Env.AppRoot + "Login.aspx");
            }
        }
    }

    public void Dispose()
    {

    }
}