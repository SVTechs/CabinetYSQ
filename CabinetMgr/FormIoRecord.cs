using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;

namespace CabinetMgr
{
    public partial class FormIoRecord : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //Tab资源
        private readonly Bitmap[] _tabNormal =
        {
            Properties.Resources.tab_borrow_n, Properties.Resources.tab_return_n
        };
        private readonly Bitmap[] _tabSelected =
        {
            Properties.Resources.tab_borrow_s, Properties.Resources.tab_return_s
        };

        //预加载窗口
        private Form _borrowRecordForm;

        public FormIoRecord()
        {
            InitializeComponent();

            InitForm();
        }

        private void FormIoRecord_Shown(object sender, EventArgs e)
        {
            btnBorrowRecord.PerformClick();
        }

        private void InitForm()
        {
            try
            {
                //借取窗口
                _borrowRecordForm = new FormBorrowRecord();
                _borrowRecordForm.Tag = btnBorrowRecord.Tag;
                AddToPanel(_borrowRecordForm);
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
            targetForm.Show();
        }

        private void btnBorrowRecord_Click(object sender, EventArgs e)
        {
            ShowWindow(_borrowRecordForm);
        }

        private void FormIoRecord_Load(object sender, EventArgs e)
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            if (height == 1920 && width == 1080)
            {
                panelWindow.Size = new Size(868, 1583);
            }
            if (height == 768 && width == 1024)
            {
                panelWindow.Size = new Size(815, 517);
            }
        }
    }
}
