using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using C1.Win.C1Ribbon;
using FKCollector.Bll;
using Utilities.DbHelper;

namespace FKCollector
{
    public partial class FormUserList : C1RibbonForm
    {
        public string UserName, UserOrg;
        public string UserId { get; protected set; }

        public FormUserList()
        {
            InitializeComponent();
        }

        private void FormUserList_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void FormUserList_Shown(object sender, EventArgs e)
        {
            int result = BllUserInfo.SearchUserInfo(UserName, UserOrg, out var userList);
            if (result < 0)
            {
                MessageBox.Show("查询出错，请检查网络");
                Close();
                return;
            }
            if (!string.IsNullOrEmpty(UserName) && result == 0)
            {
                DialogResult dr = MessageBox.Show("未找到相关人员，是否立即创建？", "提示", MessageBoxButtons.OKCancel);
                if (dr != DialogResult.OK)
                {
                    Close();
                    return;
                }
                result = BllUserInfo.AddUserInfo(UserName, "7c4a8d09ca3762af61e59520943dc26494f8941b", UserName,
                    "", 0, "", "", "", 1, "", "", "", "", UserName,
                    Encoding.ASCII.GetBytes("1"), Encoding.ASCII.GetBytes("1"), "", "");
                if (result <= 0)
                {
                    MessageBox.Show("创建失败");
                    Close();
                    return;
                }
                BllUserInfo.SearchUserInfo(UserName, UserOrg, out userList);
                if (!SqlDataHelper.IsDataValid(userList))
                {
                    MessageBox.Show("查询异常");
                    Close();
                    return;
                }
            }
            cResultGrid.DataSource = userList;
            cResultGrid.Cols[1].Visible = false;
            cResultGrid.Cols[2].Caption = "工号";
            cResultGrid.Cols[3].Caption = "密码";
            cResultGrid.Cols[4].Caption = "姓名";
            cResultGrid.Cols[5].Caption = "性别";
            cResultGrid.Cols[6].Caption = "年龄";
            cResultGrid.Cols[7].Caption = "电话";
            for (int i = 8; i < cResultGrid.Cols.Count; i++)
            {
                cResultGrid.Cols[i].Visible = false;
            }

            if (cResultGrid.Rows.Count == 2)
            {
                UserId = cResultGrid.Rows[1][1].ToString();
                Close();
            }
        }

        private void cResultGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HitTestInfo t = cResultGrid.HitTest();
            int row = t.Row;
            if (row > 0)
            {
                UserId = cResultGrid.Rows[row][1].ToString();
                Close();
            }
        }

        private void FormUserList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
