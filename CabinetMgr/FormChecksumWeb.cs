using System;
using System.Windows.Forms;
using CabinetMgr.Config;
using CabinetMgr.RtVars;
using MiniBlinkPinvoke;

namespace CabinetMgr
{
    public partial class FormChecksumWeb : Form
    {
        private readonly BlinkBrowser _webBrowser;

        public FormChecksumWeb()
        {
            InitializeComponent();

            _webBrowser = new BlinkBrowser();
            _webBrowser.Dock = DockStyle.Bottom;
            _webBrowser.Height = Height - 100;
            Controls.Add(_webBrowser);
        }

        private void FormChecksumWeb_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            tbToolCode.AutoSize = false;
            tbToolCode.Height = btnQuery.Height;

            RefreshForm();
        }

        public void RefreshForm()
        {
            string userName = AppRt.CurUser?.UserName;
            string curData = DateTime.Now.ToString("yyyy-MM-dd");
            string path = AppConfig.ChecksumWebUrl + $"?JianCeDate={curData}&UserCode={userName}&code=" + tbToolCode.Text;
            _webBrowser.Url = path;
            _webBrowser.DocumentReadyCallback += DocumentReadyCallback;
        }

        private void DocumentReadyCallback()
        {
            _webBrowser.InvokeJS("window.scrollTo(45, 150);");
        }

        private void FormChecksumWeb_FormClosing(object sender, FormClosingEventArgs e)
        {
            CenterToScreen();
            tbToolCode.Text = "";
            e.Cancel = true;
            Hide();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }
    }
}
