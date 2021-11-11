using System;
using System.Drawing;
using System.Windows.Forms;
using CabinetMgr.RtDelegate;
using NLog;
using Utilities.FileHelper;

namespace CabinetMgr
{
    public partial class FormToolPlan : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Form _curForm;

        //Tab资源
        private readonly Bitmap[] _tabNormal =
        {
            Properties.Resources.tab_task_n
        };
        private readonly Bitmap[] _tabSelected =
        {
            Properties.Resources.tab_task_s
        };

        //预加载窗口
        private Form _missionInfoForm;

        public FormToolPlan()
        {
            InitializeComponent();

            InitForm();
        }

        private void FormToolPlan_Load(object sender, EventArgs e)
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            if (height == 1920 && width == 1080)
            {
                panelWindow.Size = new Size(868, 1583);
            }
        }

        private void FormToolPlan_Shown(object sender, EventArgs e)
        {
            btnMissionInfo.PerformClick();
        }

        private void InitForm()
        {
            try
            {
                //量值窗口
                _missionInfoForm = new FormMissionInfoEx();
                _missionInfoForm.Tag = btnMissionInfo.Tag;
                AddToPanel(_missionInfoForm);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void AddToPanel(Form targetForm)
        {
            targetForm.TopLevel = false;
            targetForm.Width = panelWindow.Width;
            targetForm.Height = panelWindow.Height;
            panelWindow.Controls.Add(targetForm);
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
            int selectedBtn = int.Parse(targetForm.Tag.ToString());
            foreach (var btn in panelTab.Controls)
            {
                var curBtn = btn as Button;
                if (curBtn != null)
                {
                    int btnTag;
                    if (int.TryParse(curBtn.Tag.ToString(), out btnTag))
                    {
                        if (btnTag != selectedBtn) curBtn.BackgroundImage = _tabNormal[btnTag];
                        else curBtn.BackgroundImage = _tabSelected[btnTag];
                    }
                }
            }
            _curForm = targetForm;
            targetForm.Show();
        }

        private void btnMissionInfo_Click(object sender, EventArgs e)
        {
            ShowWindow(_missionInfoForm);
            DelegateMissionInfo.RefreshMission?.Invoke();
        }

        private void panelWindow_SizeChanged(object sender, EventArgs e)
        {
            if (_curForm != null)
            {
                _curForm.Width = panelWindow.Width;
                _curForm.Height = panelWindow.Height;
            }
        }
    }
}
