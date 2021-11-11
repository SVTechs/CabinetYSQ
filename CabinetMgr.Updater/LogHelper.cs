using System;
using System.IO;
using System.Reflection;

namespace CabinetMgr.Updater
{
    public class LogHelper
    {
        private static FileStream _logFile;
        private static bool _logAvailable;
        private const string Endl = "\r\n";

        public static bool Init()
        {
            try
            {
                string exePath = Assembly.GetExecutingAssembly().Location;
                string logPath = exePath.Substring(0, exePath.LastIndexOf('\\')) + "\\UpdLog.txt";
                _logFile = File.Open(logPath, FileMode.Append);
            }
            catch (Exception)
            {
                return false;
            }
            if (_logFile == null) return false;
            _logAvailable = true;
            return true;
        }

        public static bool WriteException(string moduleName, Exception e)
        {
            if (!_logAvailable) Init();
            if (!_logAvailable) return false;
            string logInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Endl;
            logInfo += string.Format("{0}:{1}", moduleName, e.Message) + Endl;
            logInfo += string.Format("详情:{0}", e.InnerException) + Endl;
            logInfo += e.StackTrace + Endl;
            byte[] infoByte = System.Text.Encoding.Default.GetBytes(logInfo);
            _logFile.Write(infoByte, 0, infoByte.Length);
            _logFile.Flush();
            return true;
        }

        public static bool WriteLog(string logInfo)
        {
            if (!_logAvailable) Init();
            if (!_logAvailable) return false;
            logInfo = string.Format("{0}\r\n{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logInfo);
            byte[] infoByte = System.Text.Encoding.Default.GetBytes(logInfo);
            _logFile.Write(infoByte, 0, infoByte.Length);
            _logFile.Flush();
            return true;
        }

        public static bool Close()
        {
            if (!_logAvailable) return false;
            _logFile.Close();
            return true;
        }
    }
}