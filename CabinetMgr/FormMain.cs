using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using NLog;
using Utilities.FileHelper;

// ReSharper disable FunctionNeverReturns
// ReSharper disable UnusedMember.Local
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormMain : Form
    {
        private Form _curForm;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //预加载窗口
        private Form _cabinetStatusForm;
        private Form _toolPlanForm;
        private Form _toolIoForm;
        private Form _toolPurchaseForm;
        private Form _toolCheckForm;
        private Form _toolManageForm;
        private Form _toolReportForm;
        private Form _aboutAppForm;
        private Form _homeForm;

        //按钮资源
        private readonly Bitmap[] _btnNormal =
        {
            Properties.Resources.leftbtn_lzgj, Properties.Resources.leftbtn_jhjl,
            Properties.Resources.leftbtn_gjsl, Properties.Resources.leftbtn_gjjy,
            Properties.Resources.leftbtn_gjzt_n1, Properties.Resources.leftbtn_tzgl,
            Properties.Resources.leftbtn_about_n
        };
        private readonly Bitmap[] _btnSelected =
        {
            Properties.Resources.leftbtn_lzgj_s, Properties.Resources.leftbtn_jhjl_s,
            Properties.Resources.leftbtn_gjsl_s, Properties.Resources.leftbtn_gjjy_s,
            Properties.Resources.leftbtn_gjzt_s1, Properties.Resources.leftbtn_tzgl_s,
            Properties.Resources.leftbtn_about_s
        };

        public FormMain()
        {
            InitializeComponent();

            CabinetCallback.OnCabinetClosed = OnCabinetClosed;
            FpCallBack.OnUserRecognised = OnUserRecognised;
            FKCallBack.OnUserRecognised = OnUserRecognised;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            Size = new Size();
            WindowState = FormWindowState.Maximized;
            FormDeviceLoader dlForm = new FormDeviceLoader();
            dlForm.ShowDialog();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            FKDevice.AddEvents(AxRealSvrOcxTcp1);
            if (AppConfig.DeviceType == 5 && AppRt.CurUser == null)
            {
                WindowState = FormWindowState.Minimized;
            }
            //指纹仪相关处理(URU)
            /*
            InitZkDevice();
            IList userList = BllRedistUserinfo.SearchUser();
            if (!SqlDataHelper.IsDataValid(userList))
            {
                MessageBox.Show("指纹库读取失败");
                Environment.Exit(0);
            }
            InitFpFeature(userList);
            */

            //加载窗口       
            InitForm();
            if (!ShowWindowOnSub(_cabinetStatusForm))
            {
                //未发现副屏
                _cabinetStatusForm.Show();
                _cabinetStatusForm.Hide();
            }
            ShowWindow(_toolReportForm);

            //保证UHF预扫描时间
            tmUhfInit.Enabled = true;
        }

        
        private void FormMain_Shown(object sender, EventArgs e)
        {
        }

        private bool ShowWindowOnSub(Form targetForm)
        {
            Screen subScr = null;
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                Screen cur = Screen.AllScreens[i];
                if (!cur.Primary)
                {
                    subScr = cur;
                    break;
                }
            }
            if (subScr == null && AppConfig.DeviceType != 5) return false;
            if (subScr == null && AppConfig.DeviceType == 5 && AppRt.CurUser == null)
            {
                subScr = Screen.AllScreens[0];

            }
            targetForm.Show();
            Rectangle rect = subScr.Bounds;
            targetForm.Top = rect.Top;
            targetForm.Left = rect.Left;
            targetForm.Width = rect.Width;
            targetForm.Height = rect.Height;
            return true;
        }

        private void ShowWindow(Form targetForm)
        {
            foreach (var form in panelWindow.Controls)
            {
                var curForm = form as Form;
                if (curForm != null)
                {
                    curForm.Hide();
                }
            }
            if (targetForm.Tag != null)
            {
                int selectedBtn = int.Parse(targetForm.Tag.ToString());
                foreach (var btn in panelMenu.Controls)
                {
                    if (btn is Button curBtn)
                    {
                        if (int.TryParse(curBtn.Tag.ToString(), out var btnTag))
                        {
                            if (btnTag != selectedBtn) curBtn.BackgroundImage = _btnNormal[btnTag];
                            else curBtn.BackgroundImage = _btnSelected[btnTag];
                        }
                    }
                }
            }
            _curForm = targetForm;
            targetForm.Show();
        }

        private void InitForm()
        {
            try
            {
                int height = Screen.PrimaryScreen.Bounds.Height;
                int width = Screen.PrimaryScreen.Bounds.Width;
                if (height == 1920 && width == 1080)
                {
                    Location = new Point(0, 0);
                    Height = height;
                    Width = width;
                    panelTop.Size = new Size(1079, 155);
                    panelTop.Location = new Point(0, 0);
                    pictureBox2.Location = new Point(245, 20);
                    pbAvatar.Location = new Point(748, 84);
                    lbUserName.Location = new Point(795, 93);
                    panelMenu.Size = new Size(170, 1766);
                    panelMenu.Location = new Point(0, 154);
                    panelWindow.Size = new Size(908, 1766);
                    panelWindow.Location = new Point(171, 154);

                    btnToolPlan.Visible = false;
                    btnIoRecord.Location = new Point(13, 21);
                    btnToolCheck.Visible = false;
                    btnToolReport.Location = new Point(13, 77);
                    btnToolManage.Location = new Point(13, 134);
                    btnAboutApp.Visible = false;
                }
                if (AppConfig.DeviceType == 6 ||(height == 768 && width == 1024))
                {
                    Location = new Point(0, 0);
                    Height = 768;
                    Width = 1024;
                    panelTop.Size = new Size(1024, 155);
                    panelTop.Location = new Point(0, 0);
                    pictureBox2.Location = new Point(245, 20);
                    pbAvatar.Location = new Point(748, 84);
                    lbUserName.Location = new Point(795, 93);
                    panelMenu.Size = new Size(170, 613);
                    panelMenu.Location = new Point(0, 155);
                    panelWindow.Size = new Size(854, 613);
                    panelWindow.Location = new Point(170, 155);

                    btnToolPlan.Visible = false;
                    btnIoRecord.Location = new Point(13, 9);
                    btnToolCheck.Visible = false;
                    btnToolReport.Location = new Point(13, 66);
                    btnToolManage.Location = new Point(13, 122);
                    btnAboutApp.Location = new Point(13, 178);

                }
                //状态窗口
                _cabinetStatusForm = new FormCabinetStatus();
                //量值工具
                _toolPlanForm = new FormToolPlan { Tag = btnToolPlan.Tag };
                AddToPanel(_toolPlanForm);
                //借还记录
                _toolIoForm = new FormIoRecord { Tag = btnIoRecord.Tag };
                AddToPanel(_toolIoForm);
                //工具申领
                _toolPurchaseForm = new FormToolPurchase { Tag = btnToolPurchase.Tag };
                AddToPanel(_toolPurchaseForm);
                //工具校验
                _toolCheckForm = new FormChecksumWeb { Tag = btnToolCheck.Tag };
                //AddToPanel(_toolCheckForm);
                //工具管理
                _toolManageForm = new FormToolManage { Tag = btnToolManage.Tag };
                AddToPanel(_toolManageForm);
                //初始窗口
                _homeForm = new FormHome { Tag = btnToolPlan.Tag };
                AddToPanel(_homeForm);
                //保修窗口
                _toolReportForm = new FormToolReport { Tag = btnToolReport.Tag };
                AddToPanel(_toolReportForm);
                //关于窗口
                _aboutAppForm = new FormAbout() { Tag = btnAboutApp.Tag };
                AddToPanel(_aboutAppForm);

                //预加载校验窗口
                _toolCheckForm.Show();
                _toolCheckForm.Hide();
                if (AppConfig.DeviceType == 5)
                {
                    btnToolPlan.Visible = false;
                    btnToolCheck.Visible = false;
                    Size = new Size(1, 1);
                }
                if (AppConfig.DeviceType == 6)
                {
                    _cabinetStatusForm.Size = new Size(1, 1);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void AddToPanel(Form targetForm)
        {
            targetForm.TopLevel = false;
            targetForm.Width = panelWindow.Width;
            targetForm.Height = panelWindow.Height;
            panelWindow.Controls.Add(targetForm);
        }

        public void OnUserRecognised(int userId, int method)
        {
            if (AppRt.IsInit)
            {
                MessageBox.Show("系统启动中，请稍后再试");
                return;
            }
            UserInfo loginUser = BllUserInfo.GetUser(userId);
            if (loginUser != null)
            {
                AppRt.CurUser = loginUser;
                //显示当前用户
                lbUserName.Text = (AppRt.CurUser.FullName ?? "") + "(双击退出)";
                //打开柜门
                DeviceLayer.CabinetDevice.OpenAllLight(true);
                DeviceLayer.CabinetDevice.CanDoorOpen(true);
                DeviceLayer.CabinetDevice.UnlockDoor(true);
                DeviceLayer.CabinetDevice.OpenLedGreen(true);
                //DeviceLayer.CabinetDevice.UnlockDoor(false);
                //判断班组并打开抽屉门
                //bool[] drawerStatus = new bool[AppConfig.DrawerCount];
                //for (int i = 0; i < drawerStatus.Length; i++)
                //{
                //    drawerStatus[i] = false;
                //}
                //Type configType = typeof(AppConfig);
                //for (int i = 1; i <= drawerStatus.Length; i++)
                //{
                //    string groupName = "";
                //    //状态检测设备
                //    PropertyInfo property = configType.GetProperty($"Drawer{i}Group");
                //    if (property != null)
                //    {
                //        groupName = (string)property.GetValue(null, null);
                //    }
                //    if (string.IsNullOrEmpty(groupName)) drawerStatus[i - 1] = true;
                //    else
                //    {
                //        string[] permGroups = groupName.Split(',');
                //        for (int j = 0; j < permGroups.Length; j++)
                //        {
                //            if (!string.IsNullOrEmpty(permGroups[j]) && (loginUser.GroupName ?? "").Contains(permGroups[j]))
                //            {
                //                drawerStatus[i - 1] = true;
                //                break;
                //            }
                //        }
                //    }
                //}
                //DeviceLayer.CabinetDevice.UnlockDrawer(drawerStatus);
                //if (AppConfig.DeviceType != 5 && AppConfig.DeviceType != 6)
                //{
                //    using (FormMessageBox messageBox = new FormMessageBox("您是否要领取量值任务?", "提示", 1, 8000))
                //    {
                //        messageBox.ShowDialog();
                //        if (messageBox.Result == 10)
                //        {
                //            AppRt.MissionMode = true;
                //            ShowWindow(_toolPlanForm);
                //        }
                //        else
                //        {
                //            AppRt.MissionMode = false;
                //            RefreshForm();
                //        }
                //    }
                //}
                AppRt.MissionMode = false;
                RefreshForm();
            }
        }

        private void InitZkDevice()
        {
            try
            {
                if (FpZkDevice.Init(this) == 0)
                {
                    MessageBox.Show("指纹仪初始化失败");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请安装指纹仪驱动");
            }
        }

        private void InitFpFeature(IList userList)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                RedisUserInfo ui = (RedisUserInfo)userList[i];
                if (ui.LeftTemplate.Length > 10)
                {
                    FpZkDevice.AddFeature(ui.LeftTemplate, ui.TemplateUserId, 0);
                }
                if (ui.RightTemplate.Length > 10)
                {
                    FpZkDevice.AddFeature(ui.RightTemplate, ui.TemplateUserId, 1);
                }
            }
        }

        /// <summary>
        /// 智能柜关门事件
        /// </summary>
        private void OnCabinetClosed()
        {
            //自动锁住抽屉
            bool[] drawerStatus = new bool[AppConfig.DrawerCount];
            for (int i = 0; i < AppConfig.DrawerCount; i++)
            {
                drawerStatus[i] = false;
            }
            //清除用户信息
            DisplayUser("");
            AppRt.CurUser = null;
            //刷新窗口
            try
            {
                Invoke(new RefreshFormDelegate(RefreshForm));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public delegate void RefreshFormDelegate();

        public void RefreshForm()
        {
            ShowWindow(_curForm);
        }

        private delegate void UpdateTextDelegate(string text);
        public void DisplayUser(string userName)
        {
            try
            {
                if (lbUserName.InvokeRequired)
                {
                    UpdateTextDelegate d = DisplayUser;
                    lbUserName.Invoke(d, userName);
                }
                else
                {
                    lbUserName.Text = "当前用户: " + userName;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            //panelTop.Width = Width;
            //panelMenu.Height = Height - panelTop.Height;
            //panelWindow.Width = Width - panelMenu.Width;
            //panelWindow.Height = Height - panelTop.Height;

            //lbUserName.Left = Width - 200;
            //pbAvatar.Left = lbUserName.Left - pbAvatar.Width - 12;
        }

        private void panelWindow_SizeChanged(object sender, EventArgs e)
        {
            if (_curForm != null)
            {
                _curForm.Width = panelWindow.Width;
                _curForm.Height = panelWindow.Height;
            }
        }

        private void btnToolPlan_Click(object sender, EventArgs e)
        {
            ShowWindow(_toolPlanForm);
        }

        private void btnIoRecord_Click(object sender, EventArgs e)
        {
            ShowWindow(_toolIoForm);
        }

        private void btnToolPurchase_Click(object sender, EventArgs e)
        {
            ShowWindow(_toolPurchaseForm);
        }

        private void btnToolCheck_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作");
                return;
            }
            ((FormChecksumWeb)_toolCheckForm).RefreshForm();
            _toolCheckForm.Show();
        }

        private void btnToolManage_Click(object sender, EventArgs e)
        {
            ShowWindow(_toolManageForm);
        }

        private void btnToolReport_Click(object sender, EventArgs e)
        {
            ShowWindow(_toolReportForm);
        }

        private void btnAboutApp_Click(object sender, EventArgs e)
        {
            ShowWindow(_aboutAppForm);
        }

        private void tmUhfInit_Tick(object sender, EventArgs e)
        {
            lbInitTime.Text = "启动中,剩余:" + Env.UhfDelay + "秒";
            Env.UhfDelay--;
            if (Env.UhfDelay <= 0)
            {
                AppRt.IsInit = false;
                tmUhfInit.Enabled = false;
                lbInitTime.Text = "";
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Alt && e.KeyCode == Keys.F1)
            {
                AppRt.IsInit = false;
                OnUserRecognised(873, 100);
                return;
            }
            if (e.Shift && e.Alt && e.KeyCode == Keys.F2)
            {
                AppRt.IsInit = false;
                OnUserRecognised(1878, 100);
                Show();
                return;
            }
            if (e.Shift && e.Alt && e.KeyCode == Keys.R)
            {
                //重置任务状态
                BllWorkUserInfo.ResetAllStatus();
                //清除借还记录
                BllReturnRecord.DeleteAll();
                BllBorrowRecord.DeleteAll();
                //清除计量数据
                BllMeasurementData.DeleteAll();
                //重置LED状态
                DeviceLayer.CabinetDevice.ResetToolLed();
                DeviceLayer.CabinetDevice.OpenLedRed(false);
                DeviceLayer.CabinetDevice.OpenLedYellow(false);
                //重置工具校验
                BllToolCheckRecord.ResetRecord();
                //清除登录状态
                AppRt.CurUser = null;
                lbUserName.Text = "当前用户: ";
                //窗口刷新
                ShowWindow(_curForm);
                MessageBox.Show("重置完成");
                return;
            }
            if (e.Shift && e.Alt && e.KeyCode == Keys.F12)
            {
                FormDeviceDebug dbForm = new FormDeviceDebug();
                dbForm.Show();
                return;
            }
            
        }

        private void lbUserName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PerformManualLogin();
        }

        private void pbAvatar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PerformManualLogin();
        }

        private void PerformManualLogin()
        {
            if (AppRt.CurUser != null)
            {

                DisplayUser("");
                AppRt.CurUser = null;
                DeviceLayer.CabinetDevice.CanDoorOpen(false);
            }
            else
            {
                FormLogin lgForm = new FormLogin();
                lgForm.Show();
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedGreen(false);
            DeviceLayer.CabinetDevice.OpenLedYellow(false);
            DeviceLayer.CabinetDevice.OpenLedRed(false);
        }
    }
}

