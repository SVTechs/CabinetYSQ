using System;
using System.Drawing;
using System.Windows.Forms;
using MiniBlinkPinvoke;
using Utilities.System;

namespace CabinetMgr
{
    public partial class FormLoading : Form
    {
        private BlinkBrowser _webBrowser;

        public FormLoading()
        {
            InitializeComponent();
        }

        private void FormLoading_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            BackColor = Color.White;
            TransparencyKey = BackColor;

            _webBrowser = new BlinkBrowser();
            _webBrowser.Dock = DockStyle.Fill;
            Controls.Add(_webBrowser);

            string path = SysHelper.GetCurrentDirectory();
            _webBrowser.Url = path + "Loading.html";
            _webBrowser.DocumentReadyCallback += DocumentReadyCallback;
        }

        private void DocumentReadyCallback()
        {
            _webBrowser.SetTransparent();
        }
    }
}
