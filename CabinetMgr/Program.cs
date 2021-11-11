using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using NLog;
using Utilities.System;

namespace CabinetMgr
{
    static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static Mutex mutex;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SysHelper.SetProcessDPIAware();
            }

            bool createNew;
            // ReSharper disable once UnusedVariable
            mutex = new Mutex(true, Application.ProductName, out createNew);

            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                if (createNew)
                {
                    WebRequest.DefaultWebProxy = null;
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    //身份认证
                    /*
                    FormLogin lgForm = new FormLogin();
                    lgForm.ShowDialog();
                    if (AppRt.LoginUser == null)
                    {
                        Environment.Exit(0);
                    } */
                    Application.Run(new FormMain());
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Logger.Error(ex);
                }
                catch (Exception)
                {
                    // ignored
                }
                Environment.Exit(0);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            try
            {
                Logger.Error(ex);
                Logger.Error(ex.Exception.StackTrace);
            }
            catch
            {
                // ignored
            }
            Environment.Exit(0);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            try
            {
                Logger.Error(ex);
            }
            catch
            {
                // ignored
            }
            Environment.Exit(0);
        }
    }
}
