using System;
using System.Collections;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using CabinetMgr.RtVars;
//using Domain.ShangHaiDevice1.Domain;
using Utilities.DbHelper;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormToolPurchase : Form
    {
        public FormToolPurchase()
        {
            InitializeComponent();

            InitGrid();
        }

        private void FormToolPurchase_VisibleChanged(object sender, EventArgs e)
        {
            if (cbReqStatus.SelectedIndex < 0) cbReqStatus.SelectedIndex = 0;

            RefreshGrid();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tbToolName.Text.Trim() == "")
            {
                MessageBox.Show("工具名称不能为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tbToolSpec.Text.Trim() == "")
            {
                MessageBox.Show("工具规格不能为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tbToolCount.Text.Trim() == "")
            {
                MessageBox.Show("工具数量不能为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int toolCount;
            if (!int.TryParse(tbToolCount.Text, out toolCount) || toolCount <= 0)
            {
                MessageBox.Show("工具数量应为正整数!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cbReqStatus.Text.Length == 0)
            {
                MessageBox.Show("未设置申请状态!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int result = BllToolPurchaseInfo.AddToolPurchaseInfo(tbToolName.Text, tbToolSpec.Text, toolCount,
                AppConfig.CabinetName, AppRt.CurUser.FullName, cbReqStatus.SelectedIndex);
            if (result > 0)
            {
                foreach (var control in panelToolPurchase.Controls)
                {
                    var textBox = control as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = "";
                    }
                }
                RefreshGrid();
                MessageBox.Show("工具申领单提交成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("工具申领单提交失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGrid()
        {
            cRequestGrid.Cols.Count = 8;
            cRequestGrid.Cols[1].Caption = "Id";
            cRequestGrid.Cols[1].Visible = false;
            cRequestGrid.Cols[2].Caption = "工具名称";
            cRequestGrid.Cols[2].Width = cRequestGrid.Width / 5;
            cRequestGrid.Cols[3].Caption = "工具规格";
            cRequestGrid.Cols[3].Width = cRequestGrid.Width / 5;
            cRequestGrid.Cols[4].Caption = "申请数量";
            cRequestGrid.Cols[5].Caption = "申请状态";
            cRequestGrid.Cols[6].Caption = "申请人";
            cRequestGrid.Cols[7].Caption = "申请时间";
            cRequestGrid.Cols[7].Width = cRequestGrid.Width / 6;

            for (int i = 1; i < cRequestGrid.Cols.Count; i++)
            {
                cRequestGrid.Cols[i].TextAlign = TextAlignEnum.LeftCenter;
            }
        }

        private void RefreshGrid()
        {
            cRequestGrid.BeginUpdate();
            cRequestGrid.Rows.Count = 1;
            IList resultList = BllToolPurchaseInfo.SearchToolPurchaseInfo(AppConfig.CabinetName, DateTime.Now.AddDays(-30), DateTime.Now);
            if (SqlDataHelper.IsDataValid(resultList))
            {
                cRequestGrid.Rows.Count = resultList.Count + 1;
                /*
                for (int i = 0; i < resultList.Count; i++)
                {
                    ToolPurchaseInfo ti = (ToolPurchaseInfo)resultList[i];
                    FillToGrid(i + 1, ti.Id, ti.ToolName, ti.ToolSpec, ti.ToolCount, FormatReqStatus(ti.RequestStatus),
                        ti.RequesterName, ti.RequestTime);
                }
                */
            }
            cRequestGrid.EndUpdate();
        }

        /*
        private string FormatReqStatus(int reqStatus)
        {
            switch (reqStatus)
            {
                case 0:
                    return "未解决";
                case 1:
                    return "解决中";
                case 2:
                    return "已完成";                       
            }
            return reqStatus.ToString();
        }

        private void FillToGrid(int targetRow, params object[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                cRequestGrid.Rows[targetRow][i + 1] = data[i];
            }
        }*/
    }
}
