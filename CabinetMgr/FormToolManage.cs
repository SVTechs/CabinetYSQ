using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.RtDelegate;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Utilities.DbHelper;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormToolManage : Form
    {
        public FormToolManage()
        {
            InitializeComponent();

            DelegateToolManage.RefreshTool = RefreshTool;
        }

        private void FormToolManage_Load(object sender, EventArgs e)
        {
            InitGrid();
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            if (height == 1920 && width == 1080)
            {
                panel1.Size = new Size(860, 1585);
                cToolGrid.Size = new Size(852, 1531);
            }
            if (height == 768 && width == 1024)
            {
                panel1.Size = new Size(809, 514);
                cToolGrid.Size = new Size(809, 476);
                cToolGrid.Location = new Point(0, 38);
                tbToolName.Location = new Point(505, 3);
                tbToolSpec.Location = new Point(619, 3);
                cbToolType.Location = new Point(721, 4);
            }


            cbToolType.Items.Add("不限制");
            IList cateList = BllToolInfo.GetToolCategory();
            if (SqlDataHelper.IsDataValid(cateList))
            {
                for (int i = 0; i < cateList.Count; i++)
                {
                    object[] objArray = (object[]) cateList[i];
                    if (objArray[1] == null) continue;
                    cbToolType.Items.Add(objArray[1]);
                }
            }
            cbToolType.SelectedIndex = 0;
        }

        private void InitGrid()
        {
            cToolGrid.Cols.Count = 16;
            cToolGrid.Cols[1].Caption = "Id";
            cToolGrid.Cols[1].Visible = false;
            cToolGrid.Cols[2].Caption = "所在层";
            cToolGrid.Cols[2].Width = cToolGrid.Width / 15;
            cToolGrid.Cols[3].Caption = "位置";
            cToolGrid.Cols[3].Width = cToolGrid.Width / 15;
            cToolGrid.Cols[4].Caption = "名称";
            cToolGrid.Cols[5].Caption = "规格";
            cToolGrid.Cols[6].Caption = "类型";
            cToolGrid.Cols[7].Caption = "标准值";
            cToolGrid.Cols[8].Caption = "正误差";
            cToolGrid.Cols[9].Caption = "负误差";
            cToolGrid.Cols[10].Caption = "检查间隔";
            cToolGrid.Cols[11].Caption = "下次检查";
            cToolGrid.Cols[12].Caption = "保管人";
            cToolGrid.Cols[13].Caption = "操作人";
            cToolGrid.Cols[14].Caption = "操作时间";
            cToolGrid.Cols[15].Caption = "备注";

            for (int i = 1; i < cToolGrid.Cols.Count; i++)
            {
                cToolGrid.Cols[i].TextAlign = TextAlignEnum.LeftCenter;
            }
        }

        private void RefreshTool()
        {
            cToolGrid.BeginUpdate();
            cToolGrid.Rows.Count = 1;
            string toolType = cbToolType.SelectedIndex == 0 ? "" : cbToolType.Text;
            IList toolList = BllToolInfo.SearchToolInfo(-1, -1, tbToolName.Text.Trim(), tbToolSpec.Text.Trim(), toolType);
            if (SqlDataHelper.IsDataValid(toolList))
            {
                cToolGrid.Rows.Count = toolList.Count + 1;
                for (int i = 0; i < toolList.Count; i++)
                {
                    ToolInfo ti = (ToolInfo)toolList[i];
                    FillToGrid(i + 1, ti.Id, FormatToolPositionType(ti.ToolPositionType), ti.ToolPosition, 
                        ti.ToolName, ti.ToolSpec, ti.ToolType, ti.StandardRange, ti.DeviationPositive,
                        ti.DeviationNegative, ti.CheckInterval + ti.CheckIntervalType, ti.NextCheckTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ti.ToolManager, ti.Operator, ti.OperateTime, ti.Comment);
                }
            }
            cToolGrid.EndUpdate();
        }

        private string FormatToolPositionType(int toolType)
        {
            switch (toolType)
            {
                case 0:
                    return "上层";
                case 10:
                    return "下层";
            }
            return toolType.ToString();
        }

        private void FillToGrid(int targetRow, params object[] data)
        {
            cToolGrid.Rows[targetRow][0] = targetRow;
            for (int i = 0; i < data.Length; i++)
            {
                cToolGrid.Rows[targetRow][i + 1] = data[i];
            }
        }

        private void btnAddTool_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作");
                return;
            }
            FormAddTool adForm = new FormAddTool();
            adForm.Show();
        }

        private void btnDelTool_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本功能已被禁用");
            /*
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作");
                return;
            }
            int selRow = cToolGrid.Selection.r1;
            if (selRow <= 0)
            {
                MessageBox.Show("请先选择要删除的工具");
                return;
            }
            string itemName = cToolGrid.Rows[selRow][4].ToString();
            string alertMsg = string.Format("您确定要删除工具{0}吗", itemName);
            DialogResult dr = MessageBox.Show(alertMsg, "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                int result = BllToolInfo.DeleteToolInfo((string)cToolGrid.Rows[selRow][1]);
                if (result > 0)
                {
                    RefreshTool();
                    MessageBox.Show("删除成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("删除失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }*/
        }

        private void btnModifyTool_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作");
                return;
            }
            int selRow = cToolGrid.Selection.r1;
            if (selRow <= 0)
            {
                MessageBox.Show("请先选择要修改的工具");
                return;
            }
            FormModifyTool mtForm = new FormModifyTool();
            mtForm.ToolId = (string) cToolGrid.Rows[selRow][1];
            mtForm.Show();
        }

        private void tbToolName_TextChanged(object sender, EventArgs e)
        {
            RefreshTool();
        }

        private void tbToolSpec_TextChanged(object sender, EventArgs e)
        {
            RefreshTool();
        }

        private void FormToolManage_VisibleChanged(object sender, EventArgs e)
        {
            RefreshTool();
        }

        private void cbToolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTool();
        }

        private void FormToolManage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.I && e.Alt)
            {
                OpenFileDialog ofDlg = new OpenFileDialog();
                DialogResult dr = ofDlg.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                string filePath = ofDlg.FileName;
                int result = BllToolInfo.AddToolInfoFromFile(filePath);
                MessageBox.Show(result.ToString());
            }
        }
    }
}
