using System;
using System.Windows.Forms;
using CabinetMgr.Bll;
using CabinetMgr.RtDelegate;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormToolCheckComment : Form
    {
        public int RecordId = -1;

        public FormToolCheckComment()
        {
            InitializeComponent();
        }

        private void FormmToolCheckComment_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void FormmToolCheckComment_Shown(object sender, EventArgs e)
        {
            if (RecordId < 0)
            {
                MessageBox.Show("信息异常");
                this.Close();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbComment.Text.Length == 0)
            {
                MessageBox.Show("请输入备注信息");
                return;
            }
            int result = BllToolCheckRecord.AddComment(RecordId, tbComment.Text);
            if (result <= 0)
            {
                MessageBox.Show("添加备注失败");
                return;
            }
            if (DelegateToolCheck.RefreshToolCheckInfo != null) DelegateToolCheck.RefreshToolCheckInfo();
            this.Close();
        }
    }
}
