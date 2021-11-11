using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using CabinetMgr.RtDelegate;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using Utilities.DbHelper;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormToolCheck : Form
    {
        private ToolInfo _toolInfo;
        private int _storePosition = -1;

        private CellStyle _ctRed, _ctYellow, _ctGreen, _ctBlue, _ctBlack, _ctSplit, _ctNormal, _ctComment;

        private string _mutalUser = "李刚|张国辉";

        public FormToolCheck()
        {
            InitializeComponent();

            DelegateToolCheck.RefreshToolCheckInfo = RefreshToolInfo;
        }

        private void FormToolCheck_Load(object sender, EventArgs e)
        {
            InitGrid();

            cbToolPositionType.SelectedIndex = 0;
        }

        private void RefreshToolInfo()
        {
            try
            {
                this.Invoke(new DelegateToolCheck.RefreshToolCheckDelegate(RefreshToolInfoMt));
            }
            catch (Exception)
            {
                // ignored
            }           
        }

        private void RefreshToolInfoMt()
        {
            _toolInfo = null;
            AppRt.CheckingTool = "";
            int toolPositionType = cbToolPositionType.SelectedIndex, toolPosition = cbToolPosition.SelectedIndex;
            if (toolPositionType >= 0 && toolPosition >= 0)
            {
                if (toolPositionType == 0)
                {
                    //上层工具信息
                    ToolInfo ti = BllToolInfo.GetToolInfo((toolPosition + 1).ToString());
                    if (ti != null)
                    {
                        //显示工具信息
                        _toolInfo = ti;
                        tbToolName.Text = ti.ToolName;
                        if (Math.Abs(ti.DeviationPositive - ti.DeviationNegative) < 0.00001)
                        {
                            tbToolStandard.Text = string.Format("{0}N·M (±{1}%)", ti.StandardRange, ti.DeviationPositive * 100);
                        }
                        else
                        {
                            tbToolStandard.Text = string.Format("{0}N·M (+{1}%/-{2}%)", ti.StandardRange, ti.DeviationPositive * 100, ti.DeviationNegative * 100);
                        }
                        //显示检查历史
                        cCheckGrid.BeginUpdate();
                        cCheckGrid.Rows.Count = 1;
                        IList chkHistory =
                            BllToolCheckRecord.SearchToolCheckRecord(ti.Id, DateTime.Now.AddDays(-30), DateTime.Now);
                        if (SqlDataHelper.IsDataValid(chkHistory))
                        {
                            cCheckGrid.Rows.Count = chkHistory.Count + 1;
                            ToolCheckRecord tcRecord;
                            for (int i = 0; i < chkHistory.Count; i++)
                            {
                                tcRecord = (ToolCheckRecord)chkHistory[i];
                                FillToCheckGrid(i + 1, tcRecord.Id, tcRecord.ChkTime.ToString("yy.MM.dd"), tcRecord.ChkValue1,
                                    tcRecord.ChkValue2, tcRecord.ChkValue3, tcRecord.AvgValue, tcRecord.StdValue, tcRecord.DeviationRate, tcRecord.ChkResult,
                                    tcRecord.ChkUser, tcRecord.ChkUserMutual, tcRecord.Comment);
                                cCheckGrid.Rows[i + 1].Height = 30;
                                if (string.IsNullOrEmpty(tcRecord.Comment))
                                {
                                    cCheckGrid.SetCellStyle(i + 1, 12, _ctComment);
                                }
                                else
                                {
                                    cCheckGrid.SetCellStyle(i + 1, 12, _ctNormal);
                                }
                                cCheckGrid.Rows[i + 1].AllowEditing = false;
                            }
                            //待测数据检查
                            tcRecord = (ToolCheckRecord)chkHistory[0];
                            if (tcRecord.ChkTime.ToString("yy.MM.dd").Equals(DateTime.Now.ToString("yy.MM.dd")))
                            {
                                for (int j = 1; j < cCheckGrid.Cols.Count - 1; j++)
                                {
                                    cCheckGrid.SetCellStyle(1, j, _ctNormal);
                                }
                                if (Math.Abs(tcRecord.ChkValue1) < 0.00001)
                                {
                                    AppRt.CheckingTool = ti.ToolCode;
                                    _storePosition = 1;
                                    cCheckGrid.SetCellStyle(1, 3, _ctYellow);
                                }
                                else if (tcRecord.ChkValue2 < 0.00001)
                                {
                                    AppRt.CheckingTool = ti.ToolCode;
                                    _storePosition = 2;
                                    cCheckGrid.SetCellStyle(1, 4, _ctYellow);
                                }
                                else if (tcRecord.ChkValue3 < 0.00001)
                                {
                                    AppRt.CheckingTool = ti.ToolCode;
                                    _storePosition = 3;
                                    cCheckGrid.SetCellStyle(1, 5, _ctYellow);
                                }
                                else
                                {
                                    _storePosition = -1;
                                }
                                btnExecCheck.Visible = false;
                                //if (string.IsNullOrEmpty(tcRecord.ChkUserMutual))
                                {
                                    //允许当日修改
                                    cCheckGrid.Rows[1].AllowEditing = true;
                                }
                            }
                            else
                            {
                                _storePosition = -1;
                                btnExecCheck.Visible = true;
                            }
                        }
                        else
                        {
                            _storePosition = -1;
                            btnExecCheck.Visible = true;
                        }
                        cCheckGrid.EndUpdate();
                    }
                    else
                    {
                        tbToolName.Text = "";
                        tbToolStandard.Text = "";
                    }
                }
            }
        }

        private void FillToCheckGrid(int targetRow, params object[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                cCheckGrid.Rows[targetRow][i + 1] = data[i];
            }
        }

        private void InitGrid()
        {
            cCheckGrid.Cols.Count = 13;
            cCheckGrid.Cols[1].Caption = "Id";
            cCheckGrid.Cols[1].Visible = false;
            cCheckGrid.Cols[2].Caption = "日期";
            cCheckGrid.Cols[2].AllowEditing = false;
            cCheckGrid.Cols[2].Width = cCheckGrid.Width / 10;
            cCheckGrid.Cols[3].Caption = "测试1";
            cCheckGrid.Cols[3].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[2].AllowEditing = false;
            cCheckGrid.Cols[4].Caption = "测试2";
            cCheckGrid.Cols[4].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[4].AllowEditing = false;
            cCheckGrid.Cols[5].Caption = "测试3";
            cCheckGrid.Cols[5].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[5].AllowEditing = false;
            cCheckGrid.Cols[6].Caption = "均值";
            cCheckGrid.Cols[6].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[6].AllowEditing = false;
            cCheckGrid.Cols[7].Caption = "目标值";
            cCheckGrid.Cols[7].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[7].AllowEditing = false;
            cCheckGrid.Cols[8].Caption = "偏差";
            cCheckGrid.Cols[8].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[8].AllowEditing = false;
            cCheckGrid.Cols[9].Caption = "结论";
            cCheckGrid.Cols[9].Width = cCheckGrid.Width / 13;
            cCheckGrid.Cols[9].AllowEditing = false;
            cCheckGrid.Cols[10].Caption = "测试人";
            cCheckGrid.Cols[10].AllowEditing = false;
            cCheckGrid.Cols[10].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[11].Caption = "互检人";
            cCheckGrid.Cols[11].ComboList = _mutalUser;
            cCheckGrid.Cols[11].Width = cCheckGrid.Width / 12;
            cCheckGrid.Cols[12].Caption = "备注";
            cCheckGrid.Cols[12].AllowEditing = false;
            for (int i = 1; i < cCheckGrid.Cols.Count; i++)
            {
                cCheckGrid.Cols[i].TextAlign = TextAlignEnum.LeftCenter;
            }

            _ctRed = cCheckGrid.Styles.Add("CtRed");
            _ctRed.BackColor = Color.HotPink;
            _ctYellow = cCheckGrid.Styles.Add("CtYellow");
            _ctYellow.BackColor = Color.Gold;
            _ctGreen = cCheckGrid.Styles.Add("CtGreen");
            _ctGreen.BackColor = Color.FromArgb(207, 221, 238);
            _ctBlue = cCheckGrid.Styles.Add("CtBlue");
            _ctBlue.BackColor = Color.SteelBlue;
            _ctBlack = cCheckGrid.Styles.Add("CtBlack");
            _ctBlack.BackColor = Color.DarkGray;
            _ctSplit = cCheckGrid.Styles.Add("CtSplit");
            _ctSplit.BackColor = Color.FromArgb(207, 221, 238);
            _ctNormal = cCheckGrid.Styles.Add("CtNormal");
            _ctNormal.BackColor = Color.FromArgb(225, 225, 225);

            _ctComment = cCheckGrid.Styles.Add("CtComment");
            _ctComment.BackgroundImage = Properties.Resources.comment;
            _ctComment.BackgroundImageLayout = ImageAlignEnum.Stretch;
        }

        private void cbToolPositionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbToolPosition.Items.Clear();
            switch (cbToolPositionType.SelectedIndex)
            {
                case 0:
                    for (int i = 1; i <= AppConfig.ToolCount; i++)
                    {
                        cbToolPosition.Items.Add(i);
                    }
                    cbToolPosition.SelectedIndex = 0;
                    break;
                case 1:
                    for (int i = 1; i <= AppConfig.DrawerCount; i++)
                    {
                        cbToolPosition.Items.Add(i);
                    }
                    cbToolPosition.SelectedIndex = 0;
                    break;
            }
        }

        private void cbToolPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshToolInfo();
        }

        private void btnExecCheck_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cCheckGrid.Rows.Count == 1 || !cCheckGrid.Rows[1][2].Equals(DateTime.Now.ToString("yy.MM.dd")))
            {
                FormInputPrompt icForm = new FormInputPrompt();
                icForm.RequiredItem = "校验目标值";
                icForm.ShowDialog();
                if (string.IsNullOrEmpty(VarFormInputPrompt.InputContent))
                {
                    return;
                }
                float stdValue;
                if (!float.TryParse(VarFormInputPrompt.InputContent, out stdValue))
                {
                    MessageBox.Show("目标值必须为数字");
                    return;
                }
                //添加当日记录
                int result = BllToolCheckRecord.AddToolCheckRecord(_toolInfo.Id, _toolInfo.ToolName, _toolInfo.ToolSpec,
                    stdValue, _toolInfo.DeviationPositive, _toolInfo.DeviationNegative, AppRt.CurUser.FullName);
                if (result > 0)
                {
                    RefreshToolInfo();
                }
                else
                {
                    MessageBox.Show("保存记录失败");
                }
            }
            else
            {
                RefreshToolInfo();
            }
        }

        private void FormToolCheck_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshToolInfo();
            }
        }

        private void OnMagtaDataReceived(TorqueDevice.TorqueInfo usageInfo)
        {
            if (usageInfo != null)
            {
                if (_storePosition < 0)
                {
                    //转发为工具使用数据
                    DelegateMissionInfo.OnTorqueDataReceived?.Invoke(usageInfo);
                    return;
                }
                int recordId = (int)cCheckGrid.Rows[1][1];
                ToolInfo ti = BllToolInfo.GetToolInfoByHardwareId(usageInfo.ToolId.ToString());
                if (ti != null && ti.ToolCode != AppRt.CheckingTool)
                {
                    //工具使用数据
                    DelegateMissionInfo.OnTorqueDataReceived?.Invoke(usageInfo);
                }
                else
                {
                    //工具检查
                    BllToolCheckRecord.SaveChkValue(recordId, _storePosition, usageInfo.RealValue);
                    RefreshToolInfo();
                }
            }
        }

        private void cCheckGrid_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestInfo ht = cCheckGrid.HitTest(e.X, e.Y);
            if (ht.Row > 0 && ht.Column == 12)
            {
                if (Equals(cCheckGrid.GetCellStyle(ht.Row, 12), _ctComment))
                {
                    if (AppRt.CurUser == null)
                    {
                        MessageBox.Show("请先登录再进行操作!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //备注按钮
                    FormToolCheckComment icForm = new FormToolCheckComment();
                    icForm.RecordId = (int)cCheckGrid.Rows[ht.Row][1];
                    icForm.Show();
                }
            }
        }

        private void cCheckGrid_CellChanged(object sender, RowColEventArgs e)
        {
            if (e.Row == 1 && e.Col == 11)
            {
                int recordId = (int)cCheckGrid.Rows[1][1];
                if (cCheckGrid.Rows[1][11] != null)
                {
                    int result = BllToolCheckRecord.SetChkUserMutal(recordId, cCheckGrid.Rows[1][11].ToString());
                    if (result <= 0)
                    {
                        MessageBox.Show("互检人设置失败");
                    }
                }
            }
        }
    }
}
