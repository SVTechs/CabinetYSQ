using System;
using System.IO;
using System.Reflection;
using NLog;
using Utilities.Encryption;
using Utilities.FileHelper;

namespace CabinetMgr.Config
{
    public class ConfigLoader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 自动载入所有配置项
        /// </summary>
        /// <returns></returns>
        public static int LoadConfig()
        {
            try
            {
                //获取配置文件路径
                Env.ConfigPath = GetConfigPath();
                if (File.Exists(Env.ConfigPath))
                {
                    //反射加载所有配置项目
                    string[] sections;
                    IniHelper.GetAllSectionNames(out sections, Env.ConfigPath);
                    Type configType = typeof(AppConfig);
                    for (int i = 0; i < sections.Length; i++)
                    {
                        string[] keys, values;
                        IniHelper.GetAllKeyValues(sections[i], out keys, out values, Env.ConfigPath);
                        for (int j = 0; j < keys.Length; j++)
                        {
                            PropertyInfo property = configType.GetProperty(keys[j]);
                            if (property != null)
                            {
                                if (values[j].StartsWith("ENC:"))
                                {
                                    values[j] = AesEncryption.DecryptAutoCp(values[j].Substring(4), Env.EncSeed);
                                }
                                switch (property.PropertyType.FullName)
                                {
                                    case "System.String":
                                        property.SetValue(null, values[j], null);
                                        break;
                                    case "System.Int32":
                                        property.SetValue(null, int.Parse(values[j]), null);
                                        break;
                                    case "System.Single":
                                        property.SetValue(null, float.Parse(values[j]), null);
                                        break;
                                }
                            }
                        }
                    }
                }
                return CheckConfig();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return -1;
            }
        }

        public static void ExposeConfig()
        {
            string configPath = GetConfigPath();
            Type configType = typeof(AppConfig);
            PropertyInfo []infos = configType.GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                string varName = infos[i].Name;
                string varValue = infos[i].GetValue(null, null).ToString();
                if (!varValue.Contains("="))
                {
                    IniHelper.Write("AppConfig", varName, varValue, configPath);
                }
                else
                {
                    string enc = AesEncryption.EncryptAutoCp(varValue, Env.EncSeed);
                    IniHelper.Write("AppConfig", varName, "ENC:" + enc, configPath);
                }
            }
        }

        /// <summary>
        /// 配置项合法性检测及相关处理
        /// </summary>
        /// <returns></returns>
        private static int CheckConfig()
        {
            //检查单位/程序名称并重新生成FTP路径
            if (string.IsNullOrEmpty(AppConfig.AppUnit) || string.IsNullOrEmpty(AppConfig.AppName))
            {
                return -100;
            }
            return 1;
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns></returns>
        public static string GetConfigPath()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            if (exePath != null) return exePath.Substring(0, exePath.LastIndexOf('\\')) + "\\Config.ini";
            return "";
        }
    }
}
