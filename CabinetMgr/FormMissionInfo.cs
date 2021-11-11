using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.RtDelegate;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Domain.Qcshkf.Domain;
using Domain.ShangHaiMeasure.Domain;
using Hardware.DeviceInterface;
using Utilities.DbHelper;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormMissionInfo : Form
    {
        CellStyle _btnStyle;

        private static Label[] _valueLabels;
        private string _toolCode="";

        public FormMissionInfo()
        {
            InitializeComponent();

            DelegateMissionInfo.RefreshMission = RefreshMissionInfo;
            DelegateMissionInfo.OnTorqueDataReceived = OnMagtaDataReceived;
            InitGrid();
        }

        private void FormMissionInfo_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// MAGTA扭力扳手数据回传处理
        /// </summary>
        /// <param name="usageInfo"></param>
        private void OnMagtaDataReceived(TorqueDevice.TorqueInfo usageInfo)
        {
            if (usageInfo != null)
            {
                string toolId = usageInfo.ToolId.ToString();
                ToolInfo ti = BllToolInfo.GetToolInfoByHardwareId(toolId);
                if (ti != null)
                {
                    int result = BllMeasurementData.SaveResult(ti.ToolCode, usageInfo.RealValue);
                    if (result > 0)
                    {
                        RefreshMagtaData();
                    }
                }             
            }
        }

        private void RefreshMagtaData()
        {
            if (string.IsNullOrEmpty(_toolCode))
            {
                for (int i = 0; i < _valueLabels.Length; i++)
                {
                    _valueLabels[i].Text = "";
                }
                return;
            }
            MeasurementData md = BllMeasurementData.GetMeasurementDataByToolCode(_toolCode);
            if (md != null && _valueLabels != null)
            {
                string[] data = md.DataValue?.TrimEnd(',').Split(',');
                if (data == null) return;
                for (int i = 0; i < data.Length && i < _valueLabels.Length; i++)
                {
                    _valueLabels[i].Text = data[i] + "N·M";
                }
            }
        }


        private void InitGrid()
        {
            cProcedureGrid.Rows.Count = 1;
            cProcedureGrid.Cols.Count = 9;
            cProcedureGrid.Cols[1].Visible = false;
            cProcedureGrid.Cols[2].Caption = "部件工艺";
            cProcedureGrid.Cols[2].Width = cProcedureGrid.Width / 7;
            cProcedureGrid.Cols[3].Caption = "部件工序";
            cProcedureGrid.Cols[3].Width = cProcedureGrid.Width / 4;
            cProcedureGrid.Cols[4].Caption = "所需工具编号";
            cProcedureGrid.Cols[4].Width = cProcedureGrid.Width / 8;
            cProcedureGrid.Cols[5].Caption = "所需工具名称";
            cProcedureGrid.Cols[5].Width = cProcedureGrid.Width / 5;
            cProcedureGrid.Cols[6].Caption = "工艺标准值";
            cProcedureGrid.Cols[6].Width = cProcedureGrid.Width / 8;
            cProcedureGrid.Cols[7].Caption = "是否领取";
            cProcedureGrid.Cols[7].Width = cProcedureGrid.Width / 10;
            cProcedureGrid.Cols[8].Caption = "ProcessId";
            cProcedureGrid.Cols[8].Visible = false;
            for (int i = 1; i < cProcedureGrid.Cols.Count; i++)
            {
                cProcedureGrid.Cols[i].TextAlign = TextAlignEnum.LeftCenter;
            }

            _btnStyle = cProcedureGrid.Styles.Add("BtnStyle");
            _btnStyle.BackgroundImage = Properties.Resources.aquire;
            _btnStyle.BackgroundImageLayout = ImageAlignEnum.Stretch;
        }

        private void btnAquireAll_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!AppRt.MissionMode)
            {
                MessageBox.Show("当前为自由领取模式，如需领取任务，请重新登录并确认领取任务!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int errCount = 0;
            List<string> idList = new List<string>();
            for (int i = 1; i < cProcedureGrid.Rows.Count; i++)
            {
                string wid = cProcedureGrid.Rows[i][1].ToString();
                string toolCode = cProcedureGrid.Rows[i][4].ToString();
                //检查工具是否在位
                int position = int.Parse(toolCode);
                var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
                if (toolStatusList[position - 1].Status == 0)
                {
                    errCount++;
                    continue;
                }
                idList.Add(wid);
            }
            if (idList.Count == 0)
            {
                MessageBox.Show("未发现可领取工具或所需工具均不在位");
                return;
            }
            int result = BllWorkUserInfo.SetAsAquired(idList.ToArray(), AppRt.CurUser.FullName);
            if (result < 0)
            {
                MessageBox.Show("任务领取失败，请检查网络");
                return;
            }
            if (errCount > 0)
            {
                MessageBox.Show($"共{errCount}件工具不在位，对应任务无法领取");
            }
            RefreshMissionInfo();
        }

        private void cProcedureGrid_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestInfo ht = cProcedureGrid.HitTest(e.X, e.Y);
            if (ht.Row > 0 && ht.Column == 7)
            {
                if (!Equals(cProcedureGrid.GetCellStyle(ht.Row, 7), _btnStyle))
                {
                    return;
                }
                if (AppRt.CurUser == null)
                {
                    MessageBox.Show("请先登录再进行操作!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!AppRt.MissionMode)
                {
                    MessageBox.Show("当前为自由领取模式，如需领取任务，请重新登录并确认领取任务!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //领取按钮点击处理
                int aquireMode = 0;
                string wid = cProcedureGrid.Rows[ht.Row][1].ToString();
                string toolCode = cProcedureGrid.Rows[ht.Row][4].ToString();
                //检查工具是否在位
                int position = int.Parse(toolCode);
                var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
                if (toolStatusList[position - 1].Status == 0)
                {
                    MessageBox.Show("所需工具当前不在位");
                    return;
                }
                if (toolStatusList[position - 1].RepairStatus == 1)
                {
                    MessageBox.Show("所需工具正在维修");
                    return;
                }
                //检查其他任务是否使用相同工具
                for (int i = 1; i < cProcedureGrid.Rows.Count; i++)
                {
                    if (i == ht.Row) continue;
                    if (toolCode.Equals(cProcedureGrid.Rows[i][4].ToString()))
                    {
                        DialogResult dr = MessageBox.Show("存在其他工序使用同一工具，是否一同领取？", "提示", MessageBoxButtons.OKCancel);
                        if (dr == DialogResult.OK)
                        {
                            aquireMode = 1;
                        }
                        else
                        {
                            aquireMode = 2;
                        }
                        break;
                    }
                }
                if (aquireMode == 1)
                {
                    //领取全部同工具任务
                    List<string> idList = new List<string>();
                    for (int i = 1; i < cProcedureGrid.Rows.Count; i++)
                    {
                        if (toolCode.Equals(cProcedureGrid.Rows[i][4].ToString()))
                        {
                            idList.Add(cProcedureGrid.Rows[i][1].ToString());
                        }
                    }
                    int result = BllWorkUserInfo.SetAsAquired(idList.ToArray(), AppRt.CurUser.FullName);
                    if (result <= 0)
                    {
                        MessageBox.Show("任务领取失败，请检查网络");
                        return;
                    }
                }
                else if (aquireMode == 2)
                {
                    //领取同工具最小编号任务
                    for (int i = 1; i < cProcedureGrid.Rows.Count; i++)
                    {
                        if (toolCode.Equals(cProcedureGrid.Rows[i][4].ToString()))
                        {
                            wid = cProcedureGrid.Rows[i][1].ToString();
                            break;
                        }
                    }
                    int result = BllWorkUserInfo.SetAsAquired(wid, AppRt.CurUser.FullName);
                    if (result <= 0)
                    {
                        MessageBox.Show("任务领取失败，请检查网络");
                        return;
                    }
                }
                else
                {
                    //领取点击的任务
                    int result = BllWorkUserInfo.SetAsAquired(wid, AppRt.CurUser.FullName);
                    if (result <= 0)
                    {
                        MessageBox.Show("任务领取失败，请检查网络");
                        return;
                    }
                }
                RefreshMissionInfo();
            }
            else if (ht.Row > 0)
            {
                ShowMissionDetail(ht.Row);
            }
        }

        private void ShowMissionDetail(int row)
        {
            if (row < 0)
            {
                _toolCode = "";
                tbItemSpec.Text = "";
                pbInstallIndicator.Image = null;
                RefreshMagtaIcon(null);
                RefreshMagtaData();
                return;
            }
            //显示任务信息
            _toolCode = cProcedureGrid.Rows[row][4].ToString();
            string processId = cProcedureGrid.Rows[row][8].ToString();
            ProcessDefinition pd = BllProcessDefinition.GetProcessDefinition(processId);
            if (pd != null)
            {
                byte[] imagebytes = pd.ProcessImage;
                if (imagebytes != null)
                {
                    using (MemoryStream ms = new MemoryStream(imagebytes))
                    {
                        Bitmap bmpt = new Bitmap(ms);
                        pbInstallIndicator.Image = bmpt;
                    }
                }
                else
                {
                    pbInstallIndicator.Image = null;
                }
                tbItemSpec.Text = pd.Specification;

                IList magtaList = BllProcessMagta.GetProcessMagta(pd.Id);
                RefreshMagtaIcon(magtaList);
                RefreshMagtaData();
            }
        }

        private void RefreshMagtaIcon(IList magtaList)
        {
            while (panelMagta.Controls.Count > 0)
            {
                panelMagta.Controls.RemoveAt(0);
            }
            if (magtaList == null) return;
            int panelWidth = panelMagta.Width, panelHeight = panelMagta.Height;
            if (SqlDataHelper.IsDataValid(magtaList))
            {
                _valueLabels = new Label[magtaList.Count];
                for (int i = 0; i < magtaList.Count; i++)
                {
                    ProcessMagta magtaInfo = (ProcessMagta)magtaList[i];
                    string[] xy = magtaInfo.DisplayLocation.Split(',');
                    Panel pb = new Panel
                    {
                        BackgroundImage = Properties.Resources.bolt,
                        BackColor = Color.Transparent,
                        Left = (int)((float)panelWidth * int.Parse(xy[0]) / 100),
                        Top = (int)((float)panelHeight * int.Parse(xy[1]) / 100),
                        Width = Properties.Resources.bolt.Width,
                        Height = Properties.Resources.bolt.Height,
                        Tag = (i + 1).ToString(),
                    };
                    pb.Paint += MagtaSubPanel_Paint;
                    panelMagta.Controls.Add(pb);

                    Graphics g = pb.CreateGraphics();
                    g.DrawString((i + 1).ToString(), new Font("微软雅黑", 10, FontStyle.Regular),
                        new SolidBrush(Color.White),
                        (int)(pb.Width / 3.0), (int)(pb.Height / 10.0));

                    _valueLabels[i] = new Label();
                    _valueLabels[i].Left = pb.Left + pb.Width + 5;
                    _valueLabels[i].Top = pb.Top + 7;
                    _valueLabels[i].BackColor = Color.Transparent;
                    _valueLabels[i].Text = "等待检测";
                    panelMagta.Controls.Add(_valueLabels[i]);
                }
            }
        }

        private void MagtaSubPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel curPanel = (Panel)sender;
            e.Graphics.DrawString(curPanel.Tag.ToString(), new Font("微软雅黑", 10, FontStyle.Regular),
                new SolidBrush(Color.White),
                (int)(curPanel.Width / 3.0), (int)(curPanel.Height / 10.0));
        }

        private void RefreshMissionInfo()
        {
            try
            {
                Invoke(new DelegateMissionInfo.RefreshMissionDelegate(RefreshMissionInfoMt));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void RefreshMissionInfoMt()
        {
            try
            {
                cProcedureGrid.Rows.Count = 1;
                if (AppRt.CurUser == null)
                {
                    tbTrainCode.Text = "";
                    tbMachineCode.Text = "";
                    ShowMissionDetail(-1);
                    return;
                }
                string workUserId = AppRt.CurUser.Id;
                IList workList = BllWorkUserInfo.SearchWorkUserInfo(workUserId);
                if (!SqlDataHelper.IsDataValid(workList))
                {
                    tbTrainCode.Text = "";
                    tbMachineCode.Text = "";
                    return;
                }
                //填充机车、电机编号
                WorkUserInfo wui = (WorkUserInfo)workList[0];
                tbTrainCode.Text = string.Format("{0}-{1}-{2}", wui.TrainType, wui.TrainNum, wui.LevelNum);
                tbMachineCode.Text = wui.DeviceCode;

                cProcedureGrid.BeginUpdate();
                cProcedureGrid.Rows.Count = workList.Count + 1;
                lock (AppRt.RequireLock)
                {
                    AppRt.RequiredList.Clear();
                    for (int i = 0; i < workList.Count; i++)
                    {
                        WorkUserInfo cui = (WorkUserInfo)workList[i];
                        ToolInfo ti = BllToolInfo.GetToolInfo(cui.ToolCode);
                        FillToProcedureGrid(i + 1, cui.Id, cui.ProcessTemplateName,
                            cui.ProcessDefinitionName,
                            cui.ToolCode, ti != null ? ti.ToolName : "", cui.DefaultJobValue, "",
                            cui.ProcessDefinitionId);

                        cProcedureGrid.Rows[i + 1].Height = 30;
                        if (cui.WorkStatus == 0)
                        {
                            //待领取任务
                            cProcedureGrid.SetCellStyle(i + 1, 7, _btnStyle);
                        }
                        else
                        {
                            if (cui.WorkStatus == 10)
                            {
                                //任务已接受,设置对应工具为待领取状态(亮灯)
                                DeviceLayer.CabinetDevice.SetToolLedWaitStatus(int.Parse(cui.ToolCode) - 1, true);
                                cProcedureGrid.Rows[i + 1][7] = "待领工具";
                                //缓存待领工具
                                //AppRt.RequiredList.Add(cui);
                            }
                            else if (cui.WorkStatus == 11)
                            {
                                //工具已领取,设置对应工具为灭灯状态
                                DeviceLayer.CabinetDevice.SetToolLedWaitStatus(int.Parse(cui.ToolCode) - 1, false);
                                cProcedureGrid.Rows[i + 1][7] = "已领取";
                            }
                            else if (cui.WorkStatus == 15)
                            {
                                cProcedureGrid.Rows[i + 1][7] = "已归还";
                            }
                        }
                    }
                }
                
                cProcedureGrid.EndUpdate();
                ShowMissionDetail(1);
            }
            catch (Exception)
            {
                cProcedureGrid.EndUpdate();
            }
        }

        private void FillToProcedureGrid(int targetRow, params object[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                cProcedureGrid.Rows[targetRow][i + 1] = data[i];
            }
        }

        private void FormMissionInfo_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                DelegateMissionInfo.RefreshMission?.Invoke();
                TorqueCallback.OnTorqueDataReceived = OnMagtaDataReceived;
            }
        }
    }
}
