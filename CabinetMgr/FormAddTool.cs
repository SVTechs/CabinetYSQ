using System;
using System.Windows.Forms;
using CabinetMgr.Bll;
using CabinetMgr.RtDelegate;
using CabinetMgr.RtVars;
using Domain.Main.Domain;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormAddTool : Form
    {
        public FormAddTool()
        {
            InitializeComponent();
        }

        private void FormAddTool_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            for (int i = 1; i <= 64; i++)
            {
                cbToolPosition.Items.Add(i);
            }

            cbToolPosition.SelectedIndex = 0;
            cbToolPositionType.SelectedIndex = 0;
            cbToolType.SelectedIndex = 0;
            cbCheckIntervalType.SelectedIndex = 0;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbToolName.Text.Trim() == "")
            {
                MessageBox.Show("工具名称不能为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbToolName.Focus();
                return;
            }
            if (tbToolSpec.Text.Trim() == "")
            {
                MessageBox.Show("工具规格不能为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbToolSpec.Focus();
                return;
            }
            float deviationPositive;
            if (!float.TryParse(tbDeviationPositive.Text.Trim(), out deviationPositive))
            {
                MessageBox.Show("误差值应为数字!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbDeviationPositive.Focus();
                return;
            }
            float deviationNegative;
            if (!float.TryParse(tbDeviationNegative.Text.Trim(), out deviationNegative))
            {
                MessageBox.Show("误差值应为数字!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbDeviationNegative.Focus();
                return;
            }
            float checkInterval;
            if (!float.TryParse(tbCheckInterval.Text.Trim(), out checkInterval))
            {
                MessageBox.Show("有效期应为数字!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbStdValue.Focus();
                return;
            }
            int toolCode = cbToolPosition.SelectedIndex + 1;
            if (cbToolPositionType.SelectedIndex == 1) toolCode += 100000;
            if (cbToolPositionType.SelectedIndex == 0)
            {
                ToolInfo ti = BllToolInfo.GetToolInfo(toolCode.ToString());
                if (ti != null)
                {
                    MessageBox.Show("对应位置已有工具,请删除原有信息后再进行添加!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            int result = BllToolInfo.AddToolInfo(tbToolName.Text, toolCode.ToString(), tbToolSpec.Text, cbToolType.Text,
                tbHardWareId.Text, tbCardId.Text, tbStdValue.Text, deviationPositive, deviationNegative,
                GetPositionType(cbToolPositionType.SelectedIndex), cbToolPosition.SelectedIndex + 1, "", checkInterval, 
                cbCheckIntervalType.Text, tbManager.Text, tbComment.Text, AppRt.CurUser.FullName);
            if (result > 0)
            {
                foreach (var control in panelMain.Controls)
                {
                    var textBox = control as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = "";
                        continue;
                    }
                    var comboBox = control as ComboBox;
                    if (comboBox != null)
                    {
                        comboBox.SelectedIndex = 0;
                    }
                }
                DelegateToolManage.RefreshTool?.Invoke();
                MessageBox.Show("添加成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("添加失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetPositionType(int positionType)
        {
            switch (positionType)
            {
                case 0:
                    return 0;
                case 1:
                    return 10;
            }
            return -1;
        }
    }
}
