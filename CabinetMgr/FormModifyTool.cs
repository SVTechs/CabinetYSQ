using System;
using System.Globalization;
using System.Windows.Forms;
using CabinetMgr.Bll;
using CabinetMgr.RtDelegate;
using CabinetMgr.RtVars;
using Domain.Main.Domain;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormModifyTool : Form
    {
        public string ToolId = "";
        private string _originCode = "";

        public FormModifyTool()
        {
            InitializeComponent();
        }

        private void FormModifyTool_Load(object sender, EventArgs e)
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

        private void FormModifyTool_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ToolId))
            {
                MessageBox.Show("工具信息无效");
                Close();
            }
            ToolInfo ti = BllToolInfo.GetToolInfoById(ToolId);
            if (ti == null)
            {
                MessageBox.Show("工具信息无效");
                Close();
                return;
            }
            tbToolName.Text = ti.ToolName;
            cbToolType.Text = ti.ToolType;
            tbHardWareId.Text = ti.HardwareId;
            tbCardId.Text = ti.CardId;
            tbDeviationPositive.Text = ti.DeviationPositive.ToString(CultureInfo.InvariantCulture);
            tbDeviationNegative.Text = ti.DeviationNegative.ToString(CultureInfo.InvariantCulture);
            tbStdValue.Text = ti.StandardRange;
            tbToolSpec.Text = ti.ToolSpec;
            cbToolPositionType.SelectedIndex = GetPositionTypeIndex(ti.ToolPositionType);
            cbToolPosition.SelectedIndex = ti.ToolPosition - 1 >= 0 ? ti.ToolPosition - 1 : 0;
            tbManager.Text = ti.ToolManager;
            tbCheckInterval.Text = ti.CheckInterval.ToString(CultureInfo.InvariantCulture);
            tbComment.Text = ti.Comment;
            cbCheckIntervalType.Text = ti.CheckIntervalType;

            _originCode = ti.ToolCode;
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
            if (cbToolPositionType.SelectedIndex == 0 && toolCode.ToString() != _originCode)
            {
                ToolInfo ti = BllToolInfo.GetToolInfo(toolCode.ToString());
                if (ti != null)
                {
                    MessageBox.Show("对应位置已有工具,请删除原有信息后再进行添加!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            int result = BllToolInfo.ModifyToolInfo(ToolId, tbToolName.Text, toolCode.ToString(), tbToolSpec.Text, cbToolType.Text,
                tbHardWareId.Text, tbCardId.Text, tbStdValue.Text, deviationPositive, deviationNegative,
                GetPositionType(cbToolPositionType.SelectedIndex), cbToolPosition.SelectedIndex + 1, "", checkInterval,
                cbCheckIntervalType.Text, tbManager.Text, tbComment.Text, AppRt.CurUser.FullName);
            if (result > 0)
            {
                DelegateToolManage.RefreshTool?.Invoke();
                MessageBox.Show("修改成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("修改失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private int GetPositionTypeIndex(int positionType)
        {
            switch (positionType)
            {
                case 0:
                    return 0;
                case 10:
                    return 1;
            }
            return -1;
        }
    }
}
