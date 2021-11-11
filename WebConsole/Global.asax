<%@ Application Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="NLog" %>
<%@ Import Namespace="WebConsole.Config" %>

<script runat="server">

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    void Application_Start(object sender, EventArgs e)
    {
        Application["user_sessions"] = 0;

    #region Aspose License

        MemoryStream ms = new MemoryStream();
        byte[] licenseBytes = Encoding.Default.GetBytes(Env.AsposeLicense);
        ms.Write(licenseBytes, 0, licenseBytes.Length);
        ms.Flush();

        ms.Seek(0, SeekOrigin.Begin);
        Aspose.Words.License docLicense = new Aspose.Words.License();
        docLicense.SetLicense(ms);

        ms.Seek(0, SeekOrigin.Begin);
        Aspose.Cells.License xlsLicense = new Aspose.Cells.License();
        xlsLicense.SetLicense(ms);

        ms.Close();

    #endregion
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  在应用程序关闭时运行的代码

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        Exception ex = Server.GetLastError().GetBaseException();
        Logger.Error(ex);
        Logger.Info(ex.InnerException);
        Server.ClearError();

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e) 
    {
        // 在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
        // 或 SQLServer，则不引发该事件。

    }
       
</script>
