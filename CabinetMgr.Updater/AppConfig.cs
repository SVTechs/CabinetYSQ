using System.IO;
using System.Reflection;

namespace CabinetMgr.Updater
{
    public class AppConfig
    {
        public static string AppUnit = "新联铁";

        public static string MainModuleName = "CabinetMgr.exe";
        public static string AppName = "智能柜管理R2";
        public static bool AllowClose = true;

        public static bool SkipVerCheck = false;

        public static string TmpDir = "UpdateTemp";
        public static string BakDir = "UpdateBak";

        public static int LoadConfig()
        {
            string configPath = GetConfigPath();
            string configUnit = IniHelper.Read("AppConfig", "AppUnit", configPath);
            if (!string.IsNullOrEmpty(configUnit)) AppUnit = configUnit;
            if (!string.IsNullOrEmpty(MainModuleName) && !string.IsNullOrEmpty(AppUnit) && !string.IsNullOrEmpty(AppName))
            {
                return 1;
            }
            return 0;
        }

        public static string GetConfigPath()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string curDir = exePath.Substring(0, exePath.LastIndexOf('\\'));
            Directory.SetCurrentDirectory(curDir);
            return curDir + "\\Config.ini";
        }
    }
}
