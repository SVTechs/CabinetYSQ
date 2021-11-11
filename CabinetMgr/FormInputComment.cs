using System;
using System.Windows.Forms;
using CabinetMgr.Bll;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormInputComment : Form
    {
        public string RecordId = "";

        public FormInputComment()
        {
            InitializeComponent();
        }

        private void FormInputComment_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void FormInputComment_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RecordId))
            {
                MessageBox.Show("信息异常");
                Close();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbComment.Text.Length == 0)
            {
                MessageBox.Show("请输入备注信息");
                return;
            }
            int result = BllBorrowRecord.AddExpireComment(RecordId, tbComment.Text);
            if (result <= 0)
            {
                MessageBox.Show("添加备注失败");
                return;
            }
            Close();
        }
    }
}
