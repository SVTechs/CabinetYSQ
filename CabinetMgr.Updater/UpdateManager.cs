using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CabinetMgr.Updater.Web_References.UpdaterServiceRef;

// ReSharper disable StringLastIndexOfIsCultureSpecific.1
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace CabinetMgr.Updater
{
    public class UpdateManager
    {
        public delegate void AddMsgDelegate(string col1, string col2, string col3);
        public static AddMsgDelegate ShowMsg;

        public delegate void MsgSwitchDelegate();
        public static MsgSwitchDelegate SwitchToDl;
        public static MsgSwitchDelegate SwitchToMsg;
        public static MsgSwitchDelegate ClearMsg;

        public delegate void UpdateMsgDelegate(int sigColNo, string sigColInfo, int prgPosition, string newPrg,
            int curCount, int fullCount);
        public static UpdateMsgDelegate UpdateMsg;

        private static readonly MainService UpdService = new MainService();

        public static void UpdateDispatcher()
        {
            //版本检查
            string serverVer;
            ShowMsg("提示", "正在检查服务端版本", "");
            try
            {
                serverVer = UpdService.GetServerVersion(AppConfig.AppName, AppConfig.AppUnit);
                ShowMsg("提示", string.Format("最新版本: {0}", serverVer), "");
            }
            catch (Exception)
            {
                ShowMsg("错误", "连接网络失败", "");
                RestartApp();
                return;
            }
            if (string.IsNullOrEmpty(serverVer))
            {
                ShowMsg("提示", "服务端参数配置异常", "");
                RestartApp();
                return;
            }
            if (!AppConfig.SkipVerCheck)
            {
                //版本对比
                string localVer = "";
                if (File.Exists(AppConfig.MainModuleName))
                {
                    try
                    {
                        FileVersionInfo fv = FileVersionInfo.GetVersionInfo(AppConfig.MainModuleName);
                        if (fv.FileVersion != null)
                        {
                            localVer = fv.FileVersion;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                if (localVer.Equals(serverVer))
                {
                    ShowMsg("提示", "程序已是最新版本,无需升级", "");
                    RestartApp();
                    return;
                }
            }
            //进程检查
            string targetModule = AppConfig.MainModuleName.Substring(0, AppConfig.MainModuleName.IndexOf("."));
            Process[] procList = Process.GetProcessesByName(targetModule);
            while (procList.Length > 0)
            {
                ShowMsg("提示", "目标程序正在运行,尝试结束", "");
                for (int i = 0; i < procList.Length; i++)
                {
                    try
                    {
                        procList[i].Kill();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                Thread.Sleep(500);
                procList = Process.GetProcessesByName(targetModule);
            }
            //启动升级流程
            ShowMsg("提示", "开始下载更新信息", "");
            StartUpdate();
        }

        private static void StartUpdate()
        {
            string curDir = Directory.GetCurrentDirectory();
            if (!curDir.EndsWith("\\")) curDir += "\\";
            Hashtable reqList = new Hashtable();
            //获取自身文件名
            string selfName = Assembly.GetExecutingAssembly().Location;
            selfName = selfName.Substring(selfName.LastIndexOf("\\") + 1);
            //建立临时目录
            try
            {
                if (Directory.Exists(AppConfig.TmpDir))
                {
                    Directory.Delete(AppConfig.TmpDir, true);
                }
                Directory.CreateDirectory(AppConfig.TmpDir);
                while (!Directory.Exists(AppConfig.TmpDir))
                {
                    try
                    {
                        Directory.CreateDirectory(AppConfig.TmpDir);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception)
            {
                ShowMsg("错误", "建立临时目录失败", "");
                RestartApp();
                return;
            }
            //建立备份目录
            try
            {
                if (Directory.Exists(AppConfig.BakDir))
                {
                    Directory.Delete(AppConfig.BakDir, true);
                }
                Directory.CreateDirectory(AppConfig.BakDir);
                while (!Directory.Exists(AppConfig.BakDir))
                {
                    try
                    {
                        Directory.CreateDirectory(AppConfig.BakDir);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception)
            {
                ShowMsg("错误", "建立备份目录失败", "");
                RestartApp();
                return;
            }
            //获取文件列表
            ShowMsg("提示", "正在获取更新文件列表", "");
            var temp = UpdService.GetFileUpdList(AppConfig.AppName, AppConfig.AppUnit);
            if (string.IsNullOrEmpty(temp))
            {
                ShowMsg("错误", "获取更新列表失败", "");
                RestartApp();
                return;
            }
            //检查需更新文件
            ShowMsg("提示", "正在检查需要更新的文件", "");
            string[] fileList = temp.Split('|');
            for (int i = 0; i < fileList.Length; i++)
            {
                if (string.IsNullOrEmpty(fileList[i])) continue;
                string[] fileDesp = fileList[i].Split('$');
                string fileName = fileDesp[0], fileMd5 = fileDesp[1];
                if (fileName.ToLower().StartsWith(selfName.ToLower())) continue;
                string localMd5 = FileHelper.GetMd5HashFromFile(fileName);
                if (string.IsNullOrEmpty(localMd5) || !localMd5.Equals(fileMd5))
                {
                    reqList.Add(fileName, fileMd5);
                }
            }
            //下载更新文件
            ShowMsg("提示", string.Format("共有{0}个文件需要更新", reqList.Count), "");
            //切换显示信息
            SwitchToDl();
            ClearMsg();
            int listCount = reqList.Count, curCount = 1;
            if (listCount != 0)
            {
                //显示文件列表
                foreach (DictionaryEntry curFile in reqList)
                {
                    ShowMsg(curFile.Key.ToString(), curDir + curFile.Key, "等待中");
                }
                //下载文件
                foreach (DictionaryEntry curFile in reqList)
                {
                    string curPath = curFile.Key.ToString();
                    string dlPath = AppConfig.TmpDir + "\\" + curPath;
                    string fileUrl = UpdService.ReqFileUrl(AppConfig.AppUnit, AppConfig.AppName, 0, curFile.Key.ToString());
                    int fullProgress = (int)(curCount / (float)listCount * 100);
                    if (curPath.Contains("\\"))
                    {
                        string savePath = AppConfig.TmpDir + "\\" + curPath;
                        savePath = savePath.Substring(0, savePath.LastIndexOf("\\"));
                        if (!Directory.Exists(savePath))
                        {
                            Directory.CreateDirectory(savePath);
                        }
                    }
                dlStart:
                    int dlResult = DownloadFile(fileUrl, dlPath, curPath, fullProgress);
                    while (dlResult < 0)
                    {
                        dlResult = DownloadFile(fileUrl, dlPath, curPath, fullProgress);
                    }
                    string checkSum = FileHelper.GetMd5HashFromFile(dlPath);
                    if (!checkSum.Equals(curFile.Value.ToString()))
                    {
                        goto dlStart;
                    }
                    curCount++;
                }
            }
            //切换显示信息
            SwitchToMsg();
            ClearMsg();
            //更新文件
            bool needRollbak = false;
            ShowMsg("提示", "文件下载完成,正在升级程序", "");
            foreach (DictionaryEntry curFile in reqList)
            {
                string curName = curFile.Key.ToString();
                ShowMsg("提示", "正在更新" + curName, "");
                try
                {
                    if (File.Exists(curName))
                    {
                        if (curName.Contains("\\"))
                        {
                            string bakDir = AppConfig.BakDir + "\\" + curName;
                            bakDir = bakDir.Substring(0, bakDir.LastIndexOf("\\"));
                            if (!Directory.Exists(bakDir)) Directory.CreateDirectory(bakDir);
                        }
                        File.Move(curName, AppConfig.BakDir + "\\" + curName);
                    }
                    if (File.Exists(curName))
                    {
                        ShowMsg("错误", "无法操作文件:" + curName, "");
                        needRollbak = true;
                        break;
                    }
                    if (curName.Contains("\\"))
                    {
                        string localDir = curName.Substring(0, curName.LastIndexOf("\\"));
                        if (!Directory.Exists(localDir)) Directory.CreateDirectory(localDir);
                    }
                    File.Move(AppConfig.TmpDir + "\\" + curName, curName);
                }
                catch (Exception)
                {
                    needRollbak = true;
                    break;
                }
            }
            //判断更新结果并处理
            bool rollBackFailed = false;
            if (needRollbak)
            {
                ShowMsg("提示", "正在撤销升级操作", "");
                //回滚操作
                foreach (DictionaryEntry curFile in reqList)
                {
                    string curName = curFile.Key.ToString();
                    try
                    {
                        if (File.Exists(AppConfig.BakDir + "\\" + curName))
                        {
                            File.Move(AppConfig.BakDir + "\\" + curName, curName);
                        }
                    }
                    catch (Exception)
                    {
                        rollBackFailed = true;
                        break;
                    }
                }
            }
            //显示结果
            if (rollBackFailed)
            {
                ShowMsg("错误", "文件更新失败,请重新下载程序", "");
                string targetModule = AppConfig.MainModuleName.Substring(0, AppConfig.MainModuleName.IndexOf("."));
                Process[] procList = Process.GetProcessesByName(targetModule);
                while (procList.Length > 0)
                {
                    for (int i = 0; i < procList.Length; i++)
                    {
                        procList[i].Kill();
                    }
                    procList = Process.GetProcessesByName(targetModule);
                }
                File.Delete(AppConfig.MainModuleName);
                return;
            }
            //清理文件
            try
            {
                if (Directory.Exists(AppConfig.BakDir))
                {
                    Directory.Delete(AppConfig.BakDir, true);
                }
                if (Directory.Exists(AppConfig.TmpDir))
                {
                    Directory.Delete(AppConfig.TmpDir, true);
                }
            }
            catch (Exception)
            {
                // ignored
            }
            ShowMsg("提示", "更新顺利完成", "");
            RestartApp();
        }

        private static int DownloadFile(string url, string filePath, string fileName, int fullPrg)
        {
            try
            {
                System.Net.HttpWebRequest myrq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                var st = myrp.GetResponseStream();
                if (st == null) return -1;
                Stream so = new FileStream(filePath, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    Application.DoEvents();
                    so.Write(by, 0, osize);
                    UpdateMsg(1, fileName, 3, "下载中", 
                       (int)(totalDownloadedByte / (double)totalBytes * 100), fullPrg);
                    osize = st.Read(by, 0, by.Length);
                    Application.DoEvents();
                }
                UpdateMsg(1, fileName, 3, "下载完成", 100, fullPrg);
                Application.DoEvents();
                so.Close();
                st.Close();
                return 1;
            }
            catch (Exception)
            {
                UpdateMsg(1, fileName, 3, "下载异常", 0, fullPrg);
                return -1;
            }
        }

        public static void CleanUp()
        {
            try
            {
                if (Directory.Exists(AppConfig.BakDir))
                {
                    Directory.Delete(AppConfig.BakDir, true);
                }
                if (Directory.Exists(AppConfig.TmpDir))
                {
                    Directory.Delete(AppConfig.TmpDir, true);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void RestartApp()
        {
            ShowMsg("提示", "程序将在5秒后重新启动", "");
            Thread reThread = new Thread(RestartProc) {IsBackground = true};
            reThread.Start();
        }

        private static void RestartProc()
        {
            Thread.Sleep(5000);
            TryExecute(AppConfig.MainModuleName);
            Environment.Exit(0);
        }

        private static void TryExecute(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    Process.Start(filePath);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
