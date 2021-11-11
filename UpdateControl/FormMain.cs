using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using UpdateControl.UpdateService;

// ReSharper disable StringLastIndexOfIsCultureSpecific.1
// ReSharper disable LocalizableElement

namespace UpdateControl
{
    public partial class FormMain : Form
    {
        private readonly MainService _appService = new MainService();
        private string _iniPath = "", _author = "";

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //_appService.Url = "http://localhost:49731/MainService.asmx";
            CenterToScreen();
            Directory.SetCurrentDirectory(Application.StartupPath);
            _iniPath = Directory.GetCurrentDirectory();
            if (!_iniPath.EndsWith("\\"))
            {
                _iniPath += "\\";
            }
            _iniPath += "Config.ini";
            _author = INIHelper.Read("AppUpdate", "Author", _iniPath);
            if (string.IsNullOrEmpty(_author))
            {
                Form_AuthorConfig acForm = new Form_AuthorConfig();
                acForm.ShowDialog();
            }
            else
            {
                RefreshUnit();
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            //基本输入校验
            if (!(cb_upd_app.SelectedIndex >= 0 && cb_upd_app.SelectedIndex >= 0))
            {
                MessageBox.Show("请选择要更新的程序");
                return;
            }
            if (tb_upd_ver.Text.Length == 0)
            {
                MessageBox.Show("请输入新的版本号");
                return;
            }
            if (tb_AppPath.Text.Length == 0)
            {
                MessageBox.Show("请选择更新文件路径");
                return;
            }
            if (!tb_AppPath.Text.ToLower().EndsWith(".exe") || !File.Exists(tb_AppPath.Text))
            {
                MessageBox.Show("程序路径无效");
                return;
            }

            string productName = "";
            System.Diagnostics.FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(tb_AppPath.Text);
            if (fv.FileVersion != null)
            {
                productName = fv.ProductName;
            }

            //获取本地程序路径
            string localPath = tb_AppPath.Text.Substring(0, tb_AppPath.Text.LastIndexOf("\\"));
            if (!localPath.EndsWith("\\")) localPath += "\\";
            //获取更新参数
            string updUnit = cb_upd_unit.SelectedItem.ToString(),
                updApp = cb_upd_app.SelectedItem.ToString();
            //基本检查
            string configPName = INIHelper.Read("AppUpdate", updUnit + "-" + updApp + "-SIG", _iniPath);
            if (!string.IsNullOrEmpty(configPName))
            {
                if (!configPName.Equals(productName))
                {
                    DialogResult dr = MessageBox.Show(string.Format("上次更新时使用的产品名称为{0},您确定操作正确吗?", configPName)
                        , "警告", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }
            //FTP连接设置
            Util_FTP ftpClient = new Util_FTP("10.215.91.115:21", "", "FtpUser", "Xlt62142100");
            //Util_FTP ftpClient = new Util_FTP("localhost:21", "", "FtpUser", "Xlt62142100");
            if (!cb_SelfUpload.Checked)
            {
                //清除上一版本文件
                if (_appService.DelAppFile(updUnit, updApp) <= 0)
                {
                    MessageBox.Show("清除上一版本程序文件失败");
                    return;
                }
                m_UpdProgress.Value = 20;
                Application.DoEvents();
                //上传本地文件
                if (cb_IncludeDir.Checked)
                {
                    DirectoryInfo udInfo = new DirectoryInfo(localPath);
                    UploadDir(ftpClient, localPath, updUnit + "/" + updApp, udInfo.GetDirectories().Length + 1);
                }
                else
                {
                    UploadDir(ftpClient, localPath, updUnit + "/" + updApp, 1);
                }
            }
            //提交更新信息
            int execResult = _appService.GenUpdateInfo(updApp, updUnit,
                tb_upd_ver.Text, tb_upd_log.Text);
            m_UpdProgress.Value = 100;
            if (execResult > 0)
            {
                INIHelper.Write("AppUpdate", updUnit + "-" + updApp, tb_AppPath.Text, _iniPath);
                INIHelper.Write("AppUpdate", updUnit + "-" + updApp + "-SIG", productName, _iniPath);
                MessageBox.Show("完成");
            }
            else if (execResult == -1)
            {
                MessageBox.Show("无效App，请检查输入");
            }
            else if (execResult == -10)
            {
                MessageBox.Show("未找到更新文件");
            }
            else
            {
                MessageBox.Show("未知错误");
            }
        }

        private void UploadDir(Util_FTP ftpInst, string localPath, string serverPath, int progFactor)
        {
            int i;
            DirectoryInfo udInfo = new DirectoryInfo(localPath);
            FileInfo[] updFiles = udInfo.GetFiles();
            if (updFiles.Length == 0)
            {
                MessageBox.Show("找不到本地程序文件");
                return;
            }
            ftpInst.GotoDirectory(serverPath, true);
            if (updFiles.Length != 0)
            {
                for (i = 0; i < updFiles.Length; i++)
                {
                    if (!cb_IncludeINI.Checked)
                    {
                        if (updFiles[i].Name.ToUpper().EndsWith(".INI"))
                        {
                            continue;
                        }
                    }
                    string curPath = localPath + updFiles[i].Name;
                    ftpInst.Upload(curPath);
                    AddProgress((int)((float)1 / updFiles.Length * 80 / progFactor));  
                }
            }
            else
            {
                AddProgress((int)((float)80 / progFactor));
            }
            if (cb_IncludeDir.Checked)
            {
                DirectoryInfo[] updDirs = udInfo.GetDirectories();
                for (i = 0; i < updDirs.Length; i++)
                {
                    ftpInst.MakeDir(updDirs[i].Name);
                }
                for (i = 0; i < updDirs.Length; i++)
                {
                    string dirPath = updDirs[i].FullName;
                    if (!dirPath.EndsWith("\\")) dirPath += "\\";
                    UploadDir(ftpInst, dirPath, serverPath + "/" + updDirs[i].Name, progFactor);
                }
            }
        }

        private void AddProgress(int relProg)
        {
            if (m_UpdProgress.Value + relProg > 100) m_UpdProgress.Value = 100;
            else m_UpdProgress.Value += relProg;
            Application.DoEvents();
        }

        private void RefreshUnit()
        {
            DataSet unitSet = _appService.GetAvailableUnit(_author);
            if (Util_Data.IsDataValid(unitSet))
            {
                DataTable dt = unitSet.Tables[0];
                cb_upd_unit.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cb_upd_unit.Items.Add(dt.Rows[i]["Unit"]);
                }
                cb_upd_unit.SelectedIndex = 0;
                RefreshApp();
            }
        }

        private void RefreshApp()
        {
            DataSet appSet = _appService.GetAvailableAppByUnit(cb_upd_unit.Text, _author);
            if (Util_Data.IsDataValid(appSet))
            {
                DataTable dt = appSet.Tables[0];
                cb_upd_app.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cb_upd_app.Items.Add(dt.Rows[i]["AppName"]);
                }
                cb_upd_app.SelectedIndex = 0;
            }
            else
            {
                cb_upd_app.Text = "";
            }
        }

        private void cb_upd_unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshApp();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (tb_new_unit.Text.Length == 0 || tb_new_app.Text.Length == 0)
            {
                MessageBox.Show("请填写程序信息");
                return;
            }
            _appService.AddApp(tb_new_app.Text, tb_new_unit.Text);
            RefreshUnit();
            MessageBox.Show("完成");
        }

        private void btn_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDlg = new OpenFileDialog();
            ofDlg.Filter = "程序文件|*.exe";
            if (ofDlg.ShowDialog() == DialogResult.OK)
            {
                tb_AppPath.Text = ofDlg.FileName;
                INIHelper.Write("AppUpdate", cb_upd_unit.Text + "-" + cb_upd_app.Text, tb_AppPath.Text, _iniPath);
            }
        }

        private void cb_upd_app_SelectedIndexChanged(object sender, EventArgs e)
        {
            string updUnit = cb_upd_unit.SelectedItem.ToString(),
                updApp = cb_upd_app.SelectedItem.ToString();
            string lastPath = INIHelper.Read("AppUpdate", updUnit + "-" + updApp, _iniPath);
            if (!string.IsNullOrEmpty(lastPath))
            {
                tb_AppPath.Text = lastPath;
            }
            else
            {
                tb_AppPath.Text = "";
            }
        }

        private void tb_AppPath_TextChanged(object sender, EventArgs e)
        {
            bool verFilled = false;
            if (!string.IsNullOrEmpty(tb_AppPath.Text))
            {
                if (File.Exists(tb_AppPath.Text))
                {
                    System.Diagnostics.FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(tb_AppPath.Text);
                    if (fv.FileVersion != null)
                    {
                        verFilled = true;
                        tb_upd_ver.Text = fv.FileVersion;
                    }
                }
            }
            if (!verFilled)
            {
                tb_upd_ver.Text = "";
            }
        }
    }
}
