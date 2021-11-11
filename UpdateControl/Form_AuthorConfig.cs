using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UpdateControl
{
    public partial class Form_AuthorConfig : Form
    {
        public Form_AuthorConfig()
        {
            InitializeComponent();
        }

        private void Form_AuthorConfig_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            Directory.SetCurrentDirectory(Application.StartupPath);
            string INIPath = Directory.GetCurrentDirectory();
            if (!INIPath.EndsWith("\\"))
            {
                INIPath += "\\";
            }
            INIPath += "Config.ini";

            if (tb_AuthorName.Text.Length == 0)
            {
                MessageBox.Show("请输入用户标识");
                return;
            }
            INIHelper.Write("AppUpdate", "Author", tb_AuthorName.Text, INIPath);
            MessageBox.Show("设置已保存，请重新启动程序");
            Application.Exit();
        }

        private void Form_AuthorConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
