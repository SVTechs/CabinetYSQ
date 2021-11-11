using System;
using System.Windows.Forms;
using CabinetMgr.Bll;
using Domain.Main.Domain;
using Hardware.DeviceInterface;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbUserName.Text.Length == 0)
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            UserInfo ui = BllUserInfo.GetUser(tbUserName.Text);
            if (ui == null)
            {
                MessageBox.Show("用户名不存在");
                return;
            }
            if (!Char.IsNumber(ui.EnrollId.ToCharArray()[0]))
            {
                return;
            }
            FpCallBack.OnUserRecognised(int.Parse(ui.EnrollId), 200);
            FpCallBack.OnUserRecognised(int.Parse(ui.EnrollId), 200);
            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSubmit.PerformClick();
            }
        }
    }
}
