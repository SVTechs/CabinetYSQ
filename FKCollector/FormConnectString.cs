using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities.DbHelper;

namespace FKCollector
{
    public partial class FormConnectString : C1.Win.C1Ribbon.C1RibbonForm
    {
        public FormConnectString()
        {
            InitializeComponent();
        }

        private void btnConnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbConnStr.Text.Trim()))
            {
                MessageBox.Show("连接字符串不能为空");
                return;
            }
            string connStr = tbConnStr.Text.Trim();
            try
            {
                if (SqlHelper.GetConn(connStr) != null)
                {
                    //ConfigurationManager.ConnectionStrings["cabinet"].ConnectionString = connStr;
                    //ConfigurationManager.RefreshSection("ConnectionStrings");
                    //MessageBox.Show("连接修改成功,请重启程序");
                    //Application.Exit();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("连接失败,请检查字符串");
                return;
            }

        }

        private void FormConnectString_Load(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["cabinet"].ConnectionString;
            tbConnStr.Text = connStr;
        }
    }
}
