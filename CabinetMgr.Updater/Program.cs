using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace CabinetMgr.Updater
{
    static class Program
    {
        private static Mutex dupProtect = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            dupProtect = new Mutex(true, Application.ProductName, out var dupChk);

            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                if (dupChk)
                {
                    if (args.Length >= 1)
                    {
                        if (args[0].Contains("SkipVerCheck"))
                        {
                            AppConfig.SkipVerCheck = true;
                        }
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception e)
            {
                try
                {
                    LogHelper.WriteException("Uncatched", e);
                }
                catch (Exception)
                {
                    // ignored
                }
                Application.Restart();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                LogHelper.WriteLog("Uncatched_EX: " + e.Exception.StackTrace + "\r\n" + e.Exception.Message);
            }
            catch
            {
                // ignored
            }
            Process.Start("AppDeamon.exe");
            Thread.Sleep(60000);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                LogHelper.WriteLog("Uncatched_EX: " + e.ExceptionObject);
            }
            catch
            {
                // ignored
            }
            Process.Start("AppDeamon.exe");
            Thread.Sleep(60000);
        }
    }
}
