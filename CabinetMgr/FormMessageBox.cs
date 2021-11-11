using System;
using System.Windows.Forms;

namespace CabinetMgr
{
    public partial class FormMessageBox : Form
    {
        public int Result = -1;

        private readonly string _message, _title;
        private readonly int _type, _timeout;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="type">0=确定 1=确定取消</param>
        /// <param name="timeout"></param>
        public FormMessageBox(string message, string title, int type, int timeout)
        {
            InitializeComponent();

            _message = message;
            _title = title;
            _type = type;
            _timeout = timeout;
        }

        private void FormMessageBox_Load(object sender, EventArgs e)
        {
            Text = _title;
            lbMsgContent.Text = _message;
            if (_type == 0)
            {
                btnCancel.Visible = false;
                btnConfirm.Left = this.Width - btnConfirm.Width / 2;
            }
            if (_timeout != 0)
            {
                tmAutoClose.Interval = _timeout;
                tmAutoClose.Enabled = true;
            }
            CenterToScreen();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Result = 10;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Result = 20;
            Close();
        }

        private void tmAutoClose_Tick(object sender, EventArgs e)
        {
            Result = 100;
            Close();
        }
    }
}
