using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CabinetMgr.Bll;
using CabinetMgr.Bll.MeasureServiceRef;
using CabinetMgr.Config;
using CabinetMgr.RtDelegate;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using NLog;
using Utilities.Control;
using Utilities.DbHelper;
using Utilities.Json;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable FunctionNeverReturns
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormMissionInfoEx : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Panel[] _valuePanels;
        private static Label[] _valueLabels;

        private readonly FormLoading _loadingForm = new FormLoading();

        private IList<WorkingProcessEntity> _processList;
        private WorkingProcessEntity _selectedCraft;
        private WorkingStepEntity _selectedStep;

        private readonly Hashtable _taskTable = new Hashtable();
        private readonly Hashtable _stepTable = new Hashtable();

        private readonly IList<MeasureInfo> _syncList = new List<MeasureInfo>();
        private readonly object _syncLock = new object();

        private static int _maxPage = 1;
        private static int _currentPage;

        class TaskInfo
        {
            public readonly string StepId;
            public readonly string RepairId;

            public readonly string BorrowerId;

            public readonly string ToolName, StepName;

            public float MinValue, MaxValue, DefaultValue;

            public TaskInfo(string stepId, string repairId, string borrowerId, string toolName, string stepName,
                float minValue, float maxValue, float defaultValue)
            {
                StepId = stepId;
                RepairId = repairId;
                BorrowerId = borrowerId;
                ToolName = toolName;
                StepName = stepName;
                MinValue = minValue;
                MaxValue = maxValue;
                DefaultValue = defaultValue;
            }
        }

        public FormMissionInfoEx()
        {
            InitializeComponent();

            TorqueCallback.OnTorqueDataReceived = OnMagtaDataReceived;
            TorqueCallback.OnIRTorqueDataReceived = OnIRDataReceived;
            MeasuringCallback.OnMeasuringDataReceived = OnMeasuringDataReceived;

            DelegateMissionInfo.UpdateMachineCode = UpdateMachineCode;

            DelegateMissionInfo.OnToolTaken = OnToolTaken;
            DelegateMissionInfo.OnToolReturn = OnToolReturn;
        }

        private void FormMissionInfo_Load(object sender, EventArgs e)
        {
            lbItemCraft.DisplayMember = "Text";
            lbItemCraft.ValueMember = "Value";
            lbItemStep.DisplayMember = "Text";
            lbItemStep.ValueMember = "Value";

            Thread syncThread = new Thread(ExecuteSync) { IsBackground = true };
            syncThread.Start();
        }

        private delegate void UpdateValueLabelDelegate(string toolId, string result, string realValue);
        private void UpdateValueLabel(string toolId, string result, string realValue)
        {
            try
            {
                //实时更新显示数据
                if (((TaskInfo)_taskTable[toolId]).StepId == _selectedStep.Id)
                {
                    if (_valueLabels != null)
                    {
                        for (int i = 0; i < _valueLabels.Length; i++)
                        {
                            if ((int)(_valueLabels[i].Tag ?? 0) == 0)
                            {
                                if (result == "合格")
                                {
                                    _valuePanels[i].BackgroundImage = Properties.Resources.green_n;
                                }
                                else
                                {
                                    _valuePanels[i].BackgroundImage = Properties.Resources.red_n;
                                }
                                _valueLabels[i].Tag = 1;
                                _valueLabels[i].Text = realValue + " " + _selectedStep.ljMonad;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// MAGTA扭力扳手数据回传处理
        /// </summary>
        /// <param name="usageInfo"></param>
        private void OnMagtaDataReceived(TorqueDevice.TorqueInfo usageInfo)
        {
            if (usageInfo != null)
            {
                if (AppConfig.RecvLog == 1)
                {
                    Logger.Log(LogLevel.Info, ConvertJson.ObjectToJson(usageInfo));
                }
                string toolId = usageInfo.ToolId.ToString();
                if (_taskTable[toolId] == null)
                {
                    return;
                }
                TaskInfo taskInfo = (TaskInfo)_taskTable[toolId];
                string result = CheckMagtaResult(usageInfo.TargetValue, usageInfo.RealValue,
                    taskInfo.MinValue, taskInfo.MaxValue) ? "合格" : "不合格";
                MeasureInfo mi = new MeasureInfo
                {
                    ToolType = 0,
                    ToolId = toolId,
                    StepId = taskInfo.StepId,
                    RepairId = taskInfo.RepairId,
                    TargetValue = usageInfo.TargetValue.ToString(CultureInfo.InvariantCulture),
                    RealValue = usageInfo.RealValue.ToString(CultureInfo.InvariantCulture),
                    JobResult = result,
                    UserId = taskInfo.BorrowerId,
                    SyncStatus = 0
                };
                Invoke(new UpdateValueLabelDelegate(UpdateValueLabel), toolId, result, mi.RealValue);
                //加入同步队列
                lock (_syncLock)
                {
                    _syncList.Add(mi);
                }
            }
        }

        /// <summary>
        /// IR扭力扳手数据回传处理
        /// </summary>
        /// <param name="usageInfo"></param>
        private void OnIRDataReceived(TorqueDevice.TorqueInfo usageInfo)
        {
            if (usageInfo != null)
            {
                if (AppConfig.RecvLog == 1)
                {
                    Logger.Log(LogLevel.Info, ConvertJson.ObjectToJson(usageInfo));
                }
                IList tiLst = BllToolInfo.SearchToolInfo(-1, -1, "", usageInfo.IRId, "");
                string toolId = tiLst.Count > 0 ? ((ToolInfo)tiLst[0]).ToolPosition.ToString() : "-1";
                if (_taskTable[toolId] == null)
                {
                    return;
                }
                TaskInfo taskInfo = (TaskInfo)_taskTable[toolId];
                string result = CheckIRResult(usageInfo.TorqueRslt, usageInfo.AngleRslt,
                    0, 0) ? "合格" : "不合格";
                MeasureInfo mi = new MeasureInfo
                {
                    ToolType = 3,
                    ToolId = toolId,
                    StepId = taskInfo.StepId,
                    RepairId = taskInfo.RepairId,
                    TargetValue = usageInfo.PeakAngle.ToString(CultureInfo.InvariantCulture),
                    RealValue = usageInfo.PeakTorque.ToString(CultureInfo.InvariantCulture),
                    JobResult = result,
                    UserId = taskInfo.BorrowerId,
                    SyncStatus = 0
                };
                Invoke(new UpdateValueLabelDelegate(UpdateValueLabel), toolId, result, mi.RealValue);
                //加入同步队列
                lock (_syncLock)
                {
                    _syncList.Add(mi);
                }
            }
        }

        /// <summary>
        /// 千分尺数据回传处理
        /// </summary>
        /// <param name="usageInfo"></param>
        private void OnMeasuringDataReceived(MeasuringDevice.MeasuringInfo usageInfo)
        {
            if (usageInfo != null)
            {
                if (AppConfig.RecvLog == 1)
                {
                    Logger.Log(LogLevel.Info, ConvertJson.ObjectToJson(usageInfo));
                }
                string toolId = usageInfo.ToolId.ToString();
                if (_taskTable[toolId] == null)
                {
                    UpdateDisplayResult("请先领取要进行的任务");
                    return;
                }
                TaskInfo taskInfo = ((TaskInfo)_taskTable[toolId]);
                string result = CheckMeasuringResult(taskInfo.ToolName, taskInfo.StepName, "HXD3C", taskInfo.DefaultValue,
                    usageInfo.RealValue, taskInfo.MinValue, taskInfo.MaxValue, out var dataValue) ? "合格" : "不合格";
                MeasureInfo mi = new MeasureInfo
                {
                    ToolType = 2,
                    ToolId = toolId,
                    StepId = taskInfo.StepId,
                    RepairId = taskInfo.RepairId,
                    TargetValue = "",
                    RealValue = dataValue.ToString(CultureInfo.InvariantCulture),
                    JobResult = result,
                    UserId = taskInfo.BorrowerId,
                    SyncStatus = 0
                };
                Invoke(new UpdateValueLabelDelegate(UpdateValueLabel), toolId, result, mi.RealValue);
                //加入同步队列
                lock (_syncLock)
                {
                    _syncList.Add(mi);
                }
            }
        }

        private delegate void UpdateDisplayResultDelegate(string result);
        private void UpdateDisplayResult(string result)
        {
            if (lbDataResult.InvokeRequired)
            {
                lbDataResult.Invoke(new UpdateDisplayResultDelegate(UpdateDisplayResult), result);
            }
            else
            {
                lbDataResult.Text = result;
            }
        }

        private delegate void UpdateToolStatusDelegate(string status);
        private void UpdateToolStatus(string status)
        {
            if (tbToolStatus.InvokeRequired)
            {
                tbToolStatus.Invoke(new UpdateToolStatusDelegate(UpdateToolStatus), status);
            }
            else
            {
                tbToolStatus.Text = status;
            }
        }

        /// <summary>
        /// 数据上传
        /// </summary>
        private void ExecuteSync()
        {
            while (true)
            {
                IList<MeasureInfo> workList = null;
                lock (_syncLock)
                {
                    if (_syncList.Count > 0)
                    {
                        workList = new List<MeasureInfo>(_syncList.ToArray());
                    }
                }
                if (workList != null)
                {
                    for (int i = 0; i < workList.Count; i++)
                    {
                        //上传数据
                        MeasureInfo mi = workList[i];
                        if (mi.ToolType == 0)
                        {
                            if (BllMeasurementData.AddWrenchInfo(mi.ToolId, mi.TargetValue, mi.RealValue,
                                mi.UserId, mi.JobResult, DateTime.Now, DateTime.Now, "", "",
                                mi.StepId, mi.RepairId))
                            {
                                workList[i].SyncStatus = 1;
                            }
                        }
                        else
                        {
                            if (BllMeasurementData.AddQfcInfo(mi.ToolId, mi.RealValue, mi.UserId, mi.JobResult, DateTime.Now,
                                DateTime.Now, "", "", mi.StepId, mi.RepairId))
                            {
                                workList[i].SyncStatus = 1;
                            }
                        }
                    }
                    lock (_syncLock)
                    {
                        for (int i = 0; i < workList.Count; i++)
                        {
                            if (workList[i].SyncStatus == 1)
                            {
                                for (int j = 0; j < _syncList.Count; j++)
                                {
                                    if (_syncList[j].Id == workList[i].Id)
                                    {
                                        _syncList.RemoveAt(j);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(6000);
            }
        }

        /// <summary>
        /// 刷新作业动态
        /// </summary>
        /// <param name="process"></param>
        /// <param name="iconType"></param>
        /// <param name="startCount"></param>
        /// <param name="itemCount"></param>
        /// <param name="toolType"></param>
        /// <param name="stepId"></param>
        private void RefreshValueIcon(WorkingProcessEntity process, int iconType, int startCount, int itemCount,
            string toolType, string stepId)
        {
            int panelWidth = panelMagta.Width, panelHeight = panelMagta.Height;
            //重置标签
            ResetValueIcon(itemCount, panelWidth, panelHeight);
            //数据填充
            IList<WrenchInfoEntity> wrenchList = BllMeasurementData.GetWrenchInfo(stepId);
            if (wrenchList == null || wrenchList.Count == 0)
            {
                return;
            }
            if (iconType == 2)
            {
                //量尺
                string dataValue = wrenchList.OrderByDescending(a => a.JobTime).ToList()[0].QianfenchiNum;
                bool result = wrenchList.OrderByDescending(a => a.JobTime).ToList()[0].JobResult == 1;
                if (toolType != "深度尺" || (toolType == "深度尺" && lbItemStep.Text.Contains("齿轮盘齿顶面")))
                {
                    _valueLabels[0].Tag = 1;
                    _valueLabels[0].Text = dataValue;
                    _valuePanels[0].BackgroundImage = result ? Properties.Resources.green_n :
                        Properties.Resources.red_n;
                    lbDataResult.Text = result ? "合格" : "不合格";
                }
                else
                {
                    List<float> realValues = new List<float>();
                    WorkingStepEntity we = process.workingStepList.FirstOrDefault(s => s.Id == wrenchList[0].workingstepId);
                    if (we?.DefaultValue == null)
                    {
                        return;
                    }
                    for (int i = startCount; i < startCount + itemCount; i++)
                    {
                        if (!string.IsNullOrEmpty(wrenchList[i - startCount].CreatUserName))
                        {
                            tbOperator.Text = wrenchList[i - startCount].CreatUserName;
                        }

                        if (!string.IsNullOrWhiteSpace(wrenchList[i - startCount].QianfenchiNum))
                        {
                            float value = float.Parse(wrenchList[i - startCount].QianfenchiNum);
                            realValues.Add(value);
                            _valuePanels[i].BackgroundImage = Properties.Resources.green_n;
                            _valueLabels[i].Tag = 1;
                            _valueLabels[i].Text = wrenchList[i - startCount].QianfenchiNum + " " + _selectedStep.ljMonad;
                        }
                    }
                    result = CheckMeasuringResult(realValues, we.DefaultValue.Value);
                    lbDataResult.Text = result ? "合格" : "不合格";
                    for (int i = startCount; i < startCount + itemCount; i++)
                    {
                        if (result) _valuePanels[i].BackgroundImage = Properties.Resources.green_n;
                        else _valuePanels[i].BackgroundImage = Properties.Resources.red_n;
                    }
                }
            }
            else
            {
                //扳手
                for (int i = startCount; i < startCount + itemCount && i < wrenchList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(wrenchList[i - startCount].CreatUserName))
                    {
                        tbOperator.Text = wrenchList[i - startCount].CreatUserName;
                    }

                    var torqueJob = wrenchList[i - startCount].TorqueJob;
                    if (torqueJob != null && Math.Abs(torqueJob.Value) >= 0.000001)
                    {
                        WorkingStepEntity we = process.workingStepList.FirstOrDefault(s => s.Id == wrenchList[i - startCount].workingstepId);
                        if (we?.DefaultValue == null) continue;
                        float.TryParse(we.Min, out var minValue);
                        float.TryParse(we.Max, out var maxValue);
                        if (!CheckMagtaResult(we.DefaultValue.Value, torqueJob.Value, minValue, maxValue))
                        {
                            _valuePanels[i - startCount].BackgroundImage = Properties.Resources.red_n;
                        }
                        else
                        {
                            _valuePanels[i - startCount].BackgroundImage = Properties.Resources.green_n;
                        }
                        _valueLabels[i - startCount].Tag = 1;
                        _valueLabels[i - startCount].Text = torqueJob.Value.ToString(CultureInfo.InvariantCulture) + " " + _selectedStep.ljMonad;
                    }
                }
            }
        }

        /// <summary>
        /// 重建作业动态图标
        /// </summary>
        /// <param name="itemCount"></param>
        /// <param name="panelWidth"></param>
        /// <param name="panelHeight"></param>
        private void ResetValueIcon(int itemCount, int panelWidth, int panelHeight)
        {
            //清除作业动态控件
            int removeBase = 0;
            while (panelMagta.Controls.Count > 0 && (removeBase == 0 || panelMagta.Controls.Count > removeBase))
            {
                if ((panelMagta.Controls[removeBase].Tag ?? "").ToString() == "Reserved")
                {
                    removeBase++;
                    continue;
                }
                panelMagta.Controls.RemoveAt(removeBase);
            }
            lbDataResult.Text = "";
            //生成图标坐标
            int iconWidth = Properties.Resources.gray_n.Width,
                iconHeight = Properties.Resources.gray_n.Height;
            int[,] position = GetIconPosition(itemCount, panelWidth, panelHeight, iconWidth, iconHeight);
            //重建图标
            _valuePanels = new Panel[itemCount];
            _valueLabels = new Label[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                //图标图像Panel
                _valuePanels[i] = new Panel
                {
                    BackgroundImage = Properties.Resources.gray_n,
                    BackColor = Color.Transparent,
                    Left = position[i, 0],
                    Top = position[i, 1],
                    Width = Properties.Resources.gray_n.Width,
                    Height = Properties.Resources.gray_n.Height,
                    Tag = (i + 1 + (8 * _currentPage)).ToString(),
                };
                _valuePanels[i].Paint += MagtaSubPanel_Paint;
                panelMagta.Controls.Add(_valuePanels[i]);
                //图标编号
                Graphics g = _valuePanels[i].CreateGraphics();
                g.DrawString((i + 1 + (8 * _currentPage)).ToString(), new Font("微软雅黑", 10, FontStyle.Regular),
                    new SolidBrush(Color.Black),
                    (int)(_valuePanels[i].Width / 3.3), (int)(_valuePanels[i].Height / 7.0));
                //图标数值
                _valueLabels[i] = new Label();
                _valueLabels[i].Left = _valuePanels[i].Left + _valuePanels[i].Width + 5;
                _valueLabels[i].Top = _valuePanels[i].Top + 7;
                _valueLabels[i].BackColor = Color.Transparent;
                _valueLabels[i].Text = "等待检测";
                _valueLabels[i].Width = 72;
                panelMagta.Controls.Add(_valueLabels[i]);
            }
        }

        /// <summary>
        /// 摘取工具回调方法
        /// </summary>
        /// <param name="toolPosition"></param>
        private void OnToolTaken(int toolPosition)
        {
            UpdateToolStatus("离位");
            if (AppRt.CurUser == null)
            {
                UpdateDisplayResult("请使用指纹登录");
                return;
            }
            if (_selectedStep != null)
            {
                if (toolPosition + 1 == int.Parse(_selectedStep.bsHao))
                {
                    UpdateToolStatus("已拿取");
                    if (AppRt.MissionMode) DeviceLayer.CabinetDevice.SetToolLedWaitStatus(toolPosition, false);

                    //更新TaskTable
                    float.TryParse(_selectedStep.Min, out var minValue);
                    float.TryParse(_selectedStep.Max, out var maxValue);
                    float defaultValue = _selectedStep.DefaultValue ?? 0;
                    _taskTable[_selectedStep.bsHao] = new TaskInfo(_selectedStep.Id, _selectedCraft.RepairId,
                        AppRt.CurUser.Id, _selectedStep.ljName, _selectedStep.StepName, minValue, maxValue, defaultValue);

                    string toolId = (toolPosition + 1).ToString();
                    BackgroundWorker updateWorker = new BackgroundWorker();
                    updateWorker.DoWork += UpdateWorker_DoWork;
                    updateWorker.RunWorkerCompleted += UpdateWorker_RunWorkerCompleted;
                    updateWorker.RunWorkerAsync(new[] { toolId, AppRt.CurUser.Id, AppRt.CurUser.FullName });
                }
            }
        }

        /// <summary>
        /// 摘取工具后更新Step状态及操作人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string[] borrowerInfo = (string[])e.Argument;

                string toolId = borrowerInfo[0];
                string borrowerId = borrowerInfo[1];
                string borrowerName = borrowerInfo[2];
                TaskInfo ti = (TaskInfo)_taskTable[toolId];
                WorkingStepEntity stepInfo = (WorkingStepEntity)_stepTable[ti.StepId];
                if (stepInfo != null)
                {
                    stepInfo.Status = 2;
                    stepInfo.StepPeopleId = borrowerId;
                    stepInfo.StepPeopleName = borrowerName;
                    e.Result = BllMeasurementData.UpdateStepInfo(stepInfo);
                    if ((bool)e.Result)
                    {
                        _stepTable[ti.StepId] = stepInfo;
                        for (int i = 0; i < _processList.Count; i++)
                        {
                            if (_processList[i].Id == _selectedCraft.Id)
                            {
                                for (int j = 0; j < _processList[i].workingStepList.Length; j++)
                                {
                                    if (_processList[i].workingStepList[j].Id == stepInfo.Id)
                                    {
                                        _processList[i].workingStepList[j] = stepInfo;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            e.Result = false;
        }

        private void UpdateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(bool)e.Result)
            {
                MessageBox.Show("更新工序信息失败,请检查网络！");
            }
            else
            {
                LoadStepData();
            }
        }

        /// <summary>
        /// 归还工具回调方法
        /// </summary>
        /// <param name="toolPosition"></param>
        private void OnToolReturn(int toolPosition)
        {
            UpdateToolStatus("在位");
            if (_selectedStep != null)
            {
                if (toolPosition + 1 == int.Parse(_selectedStep.bsHao))
                {
                    UpdateToolStatus("已归还");
                }
            }
        }

        /// <summary>
        /// 作业动态窗口图标自绘，用于添加序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MagtaSubPanel_Paint(object sender, PaintEventArgs e)
        {
            //图标绘制
            Panel curPanel = (Panel)sender;
            e.Graphics.DrawString(curPanel.Tag.ToString(), new Font("微软雅黑", 10, FontStyle.Regular),
                new SolidBrush(Color.Black),
                (int)(curPanel.Width / 3.3), (int)(curPanel.Height / 7.0));
        }

        private void FormMissionInfo_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (_processList != null)
                {
                    LoadStepData();
                }
                TorqueCallback.OnTorqueDataReceived = OnMagtaDataReceived;
                TorqueCallback.OnIRTorqueDataReceived = OnIRDataReceived;
            }
        }

        /// <summary>
        /// 点击部件编号时弹出选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMachineCode_MouseClick(object sender, MouseEventArgs e)
        {
            tbStdCode.Text = "";
            tbTrainCode.Text = "";
            FormMachineCode mcForm = new FormMachineCode();
            mcForm.Show();
        }

        private void UpdateMachineCode(string machinecode)
        {
            tbMachineCode.Text = machinecode;
            if (AppConfig.AppType != 1)
            {
                string[] dataPart = machinecode.Split('-');
                if (dataPart.Length < 3)
                {
                    MessageBox.Show("部件编码不正确");
                    return;
                }
                bool isExist = BllMeasurementData.CheckStandardCode(machinecode);
                if (!isExist)
                {
                    string partName = "转向架";
                    if (AppConfig.AppType == 2) partName = "轮驱";
                    bool result = BllMeasurementData.AddStandardCode(machinecode, partName, dataPart[2]);
                    if (!result)
                    {
                        MessageBox.Show("添加部件编码失败");
                        return;
                    }
                }
                tbStdCode.Text = machinecode;
                tbTrainCode.Text = dataPart[0] + "-" + dataPart[1];
            }
            else
            {
                RepairProcessEntity re = BllMeasurementData.GetRepairProcessDataEx(machinecode);
                if (re == null)
                {
                    MessageBox.Show("没有找到车辆修程信息");
                    return;
                }
                string stdCode;
                if (tbStdCode.Text.Length > 0)
                {
                    stdCode = tbStdCode.Text;
                }
                else
                {
                    stdCode = re.PartsType + "-" + re.TrainNum.Split('-')[0] + "-" + re.Proces + "-" + re.TrainNum.Split('-')[1] + "-" + machinecode;
                }
                bool isExist = BllMeasurementData.CheckStandardCode(stdCode);
                if (!isExist)
                {
                    bool result = BllMeasurementData.AddStandardCode(stdCode, "牵引电机", re.Proces);
                    if (!result)
                    {
                        MessageBox.Show("添加电机编码失败");
                        return;
                    }
                }
                tbStdCode.Text = stdCode;
                tbTrainCode.Text = re.PartsType + "-" + re.TrainNum;
            }
            RefreshMission();
        }

        private void RefreshMission()
        {
            BackgroundWorker missionWorker = new BackgroundWorker();
            missionWorker.DoWork += MissionWorker_DoWork;
            missionWorker.RunWorkerCompleted += MissionWorker_RunWorkerCompleted;
            missionWorker.RunWorkerAsync();
            _loadingForm.Show();
        }

        /// <summary>
        /// 刷新量值信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MissionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //_processList = BllMeasurementData.GetProcessList(tbMachineCode.Text);
                _processList = BllMeasurementData.GetProcessList(tbStdCode.Text);
                if (!SqlDataHelper.IsDataValid(_processList))
                {
                    e.Result = false;
                    return;
                }
                e.Result = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        private void MissionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _loadingForm.Hide();
            if ((bool)e.Result == false)
            {
                MessageBox.Show("没有找到对应工序");
                return;
            }
            _stepTable.Clear();
            for (int i = 0; i < _processList.Count; i++)
            {
                for (int j = 0; j < _processList[i].workingStepList.Length; j++)
                {
                    WorkingStepEntity ws = _processList[i].workingStepList[j];
                    string stepId = ws.Id;
                    _stepTable[stepId] = ws;
                }
            }
            lbItemCraft.Items.Clear();
            lbItemStep.Items.Clear();
            foreach (var workingProcess in _processList)
            {
                ListBoxItem item = new ListBoxItem(workingProcess.WorkingName, workingProcess.Id);
                lbItemCraft.Items.Add(item);
            }
        }

        private void lbItemCraft_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbItemStep.Items.Clear();
            string selItem = ((ListBoxItem)lbItemCraft.SelectedItem).Value.ToString();
            if (string.IsNullOrWhiteSpace(selItem))
            {
                return;
            }
            _selectedCraft = _processList.FirstOrDefault(o => o.Id == selItem);
            if (_selectedCraft == null)
            {
                return;
            }
            foreach (var workingStep in _selectedCraft.workingStepList.OrderBy(o => o.Sequence))
            {
                ListBoxItem item = new ListBoxItem(workingStep.StepName, workingStep.Id);
                lbItemStep.Items.Add(item);
            }
        }

        private void LoadStepData()
        {
            if (lbItemStep.SelectedItem == null) return;
            string selText = ((ListBoxItem)lbItemStep.SelectedItem).Text;
            string selItem = ((ListBoxItem)lbItemStep.SelectedItem).Value.ToString();
            if (string.IsNullOrWhiteSpace(selItem))
            {
                return;
            }
            WorkingStepEntity ws = _selectedCraft.workingStepList.FirstOrDefault(o => o.Id == selItem);
            if (ws == null)
            {
                return;
            }
            //去掉0
            if (string.IsNullOrEmpty(ws.bsHao)) ws.bsHao = "1";
            ws.bsHao = int.Parse(ws.bsHao).ToString();
            //保存当前Step
            _selectedStep = ws;
            lock (AppRt.RequireLock)
            {
                AppRt.RequiredList.Clear();
                AppRt.RequiredList.Add(int.Parse(ws.bsHao));
            }
            //登录后，如有在位工具未取点亮指示灯
            if (AppRt.MissionMode)
            {
                if (AppRt.CurUser != null)
                {
                    List<int> waintingList = new List<int>();
                    CabinetCallback.ToolStatus[] status = DeviceLayer.CabinetDevice.GetToolStatus();
                    try
                    {
                        if (status[int.Parse(ws.bsHao) - 1].Status == 1)
                        {
                            waintingList.Add(int.Parse(ws.bsHao) - 1);
                        }
                        DeviceLayer.CabinetDevice.SetToolLedWaiting(waintingList);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("工具编号" + ws.bsHao + "可能不正确");
                    }
                }
                else
                {
                    DeviceLayer.CabinetDevice.SetToolLedWaiting(null);
                }
            }
            //任务信息
            if (ws.ProcessImage != null && ws.ProcessImage.Length > 1)
            {
                using (MemoryStream stream = new MemoryStream(ws.ProcessImage))
                {
                    Image img = Image.FromStream(stream);
                    pbInstallIndicator.Image = img;
                }
            }
            else
            {
                pbInstallIndicator.Image = null;
            }
            if (ws.SpecificationImage != null && ws.SpecificationImage.Length > 1)
            {
                using (MemoryStream stream = new MemoryStream(ws.SpecificationImage))
                {
                    Image img = Image.FromStream(stream);
                    pbSpecImage.Image = img;
                }
            }
            else
            {
                pbSpecImage.Image = null;
            }
            tbItemSpec.Text = ws.Specification;

            if (!string.IsNullOrWhiteSpace(ws.bsHao))
            {
                var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
                if (toolStatusList[int.Parse(ws.bsHao) - 1] == null || toolStatusList[int.Parse(ws.bsHao) - 1].Status == 0)
                {
                    tbToolStatus.Text = "离位";
                }
                else
                {
                    tbToolStatus.Text = "在位";
                }
            }
            else
            {
                tbToolStatus.Text = "";
            }
            tbItemStep.Text = selText;
            tbRequiredTool.Text = "使用" + ws.bsHao + "号" + ws.ljName;
            tbStdValue.Text = ws.DefaultValue.ToString();
            tbOperator.Text = ws.StepPeopleName;
            lbMeasurementUnit.Text = ws.ljMonad;
            int itemCount = Convert.ToInt32(ws.DefaultCount);
            if (itemCount > 8)
            {
                _maxPage = (int)Math.Ceiling(itemCount / 8.0);
            }
            else
            {
                _maxPage = 1;
            }
            int toolType = ws.ljName.Contains("尺") ? 2 : 0;
            int opCount = itemCount - 8 * _currentPage;
            if (opCount > 8) opCount = 8;
            RefreshValueIcon(_selectedCraft, toolType, 8 * _currentPage, opCount, ws.ljName, selItem);
        }

        private void lbItemStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPage = 0;
            LoadStepData();
        }

        /// <summary>
        /// 重置按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请使用指纹登录");
                return;
            }
            if (lbItemStep.SelectedItem == null)
            {
                MessageBox.Show("请选择工序");
                return;
            }
            DialogResult dr = MessageBox.Show("是否重置,重置当前工艺数据会清空", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr != DialogResult.OK)
            {
                return;
            }
            try
            {
                string stepId = ((ListBoxItem)lbItemStep.SelectedItem).Value.ToString();
                if (!string.IsNullOrEmpty(stepId))
                {
                    //清除待同步数据
                    lock (_syncLock)
                    {
                        IList<MeasureInfo> targetList = _syncList.Where(o => o.StepId == stepId).ToList();
                        foreach (MeasureInfo targetItem in targetList)
                        {
                            _syncList.Remove(targetItem);
                        }
                    }
                    //重置任务状态
                    _selectedStep.Status = 0;
                    _selectedStep.StepPeopleId = "";
                    _selectedStep.StepPeopleName = "";
                    BllMeasurementData.UpdateStepInfo(_selectedStep);
                    //更新列表和当前项
                    for (int i = 0; i < _processList.Count; i++)
                    {
                        if (_processList[i].Id == _selectedCraft.Id)
                        {
                            for (int j = 0; j < _processList[i].workingStepList.Length; j++)
                            {
                                if (_processList[i].workingStepList[j].Id == _selectedStep.Id)
                                {
                                    _processList[i].workingStepList[j].Status = 0;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    _selectedStep.Status = 0;
                    //删除测量信息
                    IList<WrenchInfoEntity> wrenchList = BllMeasurementData.GetWrenchInfo(stepId);
                    foreach (WrenchInfoEntity wrenchInfo in wrenchList)
                    {
                        BllMeasurementData.DeleteWrenchInfo(wrenchInfo);
                    }
                    LoadStepData();
                }
            }
            catch
            {
                MessageBox.Show("重置成功");
            }
        }

        /// <summary>
        /// 完工按钮处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请使用指纹登录");
                return;
            }
            if (lbItemStep.SelectedItem == null)
            {
                MessageBox.Show("请选择工序");
                return;
            }
            try
            {
                //更新任务状态
                _selectedStep.Status = 1;
                BllMeasurementData.UpdateStepInfo(_selectedStep);
                //更新列表和当前项
                for (int i = 0; i < _processList.Count; i++)
                {
                    if (_processList[i].Id == _selectedCraft.Id)
                    {
                        for (int j = 0; j < _processList[i].workingStepList.Length; j++)
                        {
                            if (_processList[i].workingStepList[j].Id == _selectedStep.Id)
                            {
                                _processList[i].workingStepList[j].Status = 1;
                                break;
                            }
                        }
                        break;
                    }
                }
                _selectedStep.Status = 1;
                MessageBox.Show("完工成功");
            }
            catch
            {
                MessageBox.Show("完工失败");
            }
        }

        /// <summary>
        /// 生成作业动态图标位置
        /// </summary>
        /// <param name="itemCount"></param>
        /// <param name="panelWidth"></param>
        /// <param name="panelHeight"></param>
        /// <param name="iconWidth"></param>
        /// <param name="iconHeight"></param>
        /// <returns></returns>
        private int[,] GetIconPosition(int itemCount, int panelWidth, int panelHeight,
            int iconWidth, int iconHeight)
        {
            int[,] position = new int[8, 2];
            for (int i = 0; i < itemCount; i++)
            {
                position[i, 0] = (int)(_iconPositions[itemCount - 1, i, 0] * panelWidth - iconWidth / 2.0);
                position[i, 1] = (int)(_iconPositions[itemCount - 1, i, 1] * panelHeight - iconHeight / 2.0);
            }
            return position;
        }

        private readonly float[,,] _iconPositions =
        {
            //Item=1
            { { 0.5f, 0.5f }, { 0f, 0f }, { 0f, 0f }, { 0f, 0f },
                { 0f, 0f }, { 0f, 0f }, { 0f, 0f }, { 0f, 0f }  },
            //Item=2
            { { 0.2f, 0.5f }, { 0.65f, 0.5f } , { 0f, 0f }, { 0f, 0f },
                { 0f, 0f }, { 0f, 0f }, { 0f, 0f }, { 0f, 0f } },
            //Item=3
            { { 0.45f, 0.35f }, { 0.2f, 0.7f }, { 0.7f, 0.7f }, { 0f, 0f },
                { 0f, 0f }, { 0f, 0f }, { 0f, 0f }, { 0f, 0f } },
            //Item=4
            { { 0.2f, 0.35f }, { 0.65f, 0.75f }, { 0.2f, 0.75f }, { 0.65f, 0.35f },
                { 0f, 0f }, { 0f, 0f }, { 0f, 0f }, { 0f, 0f } },
            //Item=5
            { { 0.45f, 0.25f }, { 0.25f, 0.8f }, { 0.75f, 0.5f }, { 0.15f, 0.5f },
                { 0.65f, 0.8f }, { 0f, 0f }, { 0f, 0f }, { 0f, 0f } },
            //Item=6
            { { 0.45f, 0.25f }, { 0.45f, 0.85f }, { 0.15f, 0.45f }, { 0.75f, 0.65f },
                { 0.15f, 0.65f }, { 0.75f, 0.45f }, { 0f, 0f }, { 0f, 0f } },
            //Item=7
            { { 0.45f, 0.25f }, { 0.45f, 0.85f }, { 0.15f, 0.55f }, { 0.75f, 0.55f },
                { 0.3f, 0.4f }, { 0.6f, 0.4f }, { 0.3f, 0.7f }, { 0f, 0f } },
            //Item=8
            { { 0.45f, 0.25f }, { 0.45f, 0.85f }, { 0.15f, 0.55f }, { 0.75f, 0.55f },
                { 0.3f, 0.4f }, { 0.6f, 0.7f }, { 0.3f, 0.7f }, { 0.6f, 0.4f } },
        };

        /// <summary>
        /// 验证Magta扳手数据
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="realValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private bool CheckMagtaResult(float targetValue, float realValue, float minValue, float maxValue)
        {
            if (minValue != 0 || maxValue != 0)
            {
                if (realValue >= minValue && realValue <= maxValue)
                {
                    return true;
                }
                return false;
            }
            else
            {
                float errorRate = Math.Abs((targetValue - realValue) / realValue);
                if (errorRate > 0.05)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 验证IR扳手数据
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="realValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private bool CheckIRResult(bool torqueRslt, bool angleRslt, float torqueStandard, float angleStandard)
        {
            if (torqueRslt && angleRslt)
            {
                return true;
            }
            else
            {
                return false;
            }
            //if (minValue != 0 || maxValue != 0)
            //{
            //    if (realValue >= minValue && realValue <= maxValue)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            //else
            //{
            //    float errorRate = Math.Abs((targetValue - realValue) / realValue);
            //    if (errorRate > 0.05)
            //    {
            //        return false;
            //    }
            //    return true;
            //}
        }

        /// <summary>
        /// 验证千分尺数据
        /// </summary>
        /// <param name="toolName"></param>
        /// <param name="stepName"></param>
        /// <param name="trainType"></param>
        /// <param name="targetValue"></param>
        /// <param name="realValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="finalValue"></param>
        /// <returns></returns>
        private bool CheckMeasuringResult(string toolName, string stepName, string trainType, float targetValue, float realValue,
            float minValue, float maxValue, out float finalValue)
        {
            finalValue = realValue;
            switch (toolName)
            {
                case "深度尺":
                    if (minValue != 0 || maxValue != 0)
                    {
                        return realValue >= minValue && realValue <= maxValue;
                    }
                    if (stepName.Contains("齿盘齿顶面"))
                    {
                        if (realValue <= 51.3 && realValue >= 50.65) return true;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case "内径千分尺":
                    if (trainType.Equals("HXD3C"))
                    {
                        finalValue = Math.Abs(targetValue - realValue);
                        if (minValue != 0 || maxValue != 0)
                        {
                            return realValue >= minValue && realValue <= maxValue;
                        }
                        if (finalValue <= 214.985 && finalValue >= 214.972) return true;
                        return false;
                    }
                    else
                    {
                        finalValue = 240 - realValue;
                        if (minValue != 0 || maxValue != 0)
                        {
                            return realValue >= minValue && realValue <= maxValue;
                        }
                        if (finalValue <= 239.992 && finalValue >= 239.963) return true;
                        return false;
                    }
                case "外径千分尺":
                    if (minValue != 0 || maxValue != 0)
                    {
                        return realValue >= minValue && realValue <= maxValue;
                    }
                    if (realValue <= AppConfig.MeasuringWjMax && realValue >= AppConfig.MeasuringWjMin) return true;
                    return false;
                case "游标卡尺":
                    if (minValue != 0 || maxValue != 0)
                    {
                        return realValue >= minValue && realValue <= maxValue;
                    }
                    if (trainType.Equals("HXD3C"))
                    {
                        if (realValue <= 361.5 && realValue >= 360.1) return true;
                        return false;
                    }
                    else
                    {
                        if (realValue <= 601.1 && realValue >= 599.4) return true;
                        return false;
                    }
                case "碳纤维卡尺":
                    finalValue = realValue - 24;
                    if (minValue != 0 || maxValue != 0)
                    {
                        return realValue >= minValue && realValue <= maxValue;
                    }
                    if (finalValue <= 601.1 && finalValue >= 599.4) return true;
                    return false;
                case "百分表":
                    if (minValue != 0 || maxValue != 0)
                    {
                        return realValue >= minValue && realValue <= maxValue;
                    }
                    if (trainType.Equals("HXD3C"))
                    {
                        if (stepName.Contains("跳动量"))
                        {
                            if (realValue <= 0.04) return true;
                            return false;
                        }
                        if (stepName.Contains("封环"))
                        {
                            if (realValue < 0.1) return true;
                            return false;
                        }
                        if (stepName.Contains("轴承外盖"))
                        {
                            if (realValue < 0.22) return true;
                            return false;
                        }
                        if (realValue >= 0.07 && realValue <= 0.218) return true;
                        return false;
                    }
                    else
                    {
                        if (stepName.Contains("跳动量"))
                        {
                            if (realValue <= 0.04) return true;
                            return false;
                        }
                        if (realValue >= 0.18 && realValue <= 0.38) return true;
                        return false;
                    }
            }
            return false;
        }

        private bool CheckMeasuringResult(IList<float> realValues, float targetValue)
        {
            bool result = true;
            float minValue = float.MaxValue, maxValue = float.MinValue;
            for (int i = 0; i < realValues.Count; i++)
            {
                if (Math.Abs(targetValue - realValues[i]) > 0.1)
                {
                    result = false;
                }
                if (realValues[i] < minValue) minValue = realValues[i];
                if (realValues[i] > maxValue) maxValue = realValues[i];
            }
            if (maxValue - minValue > 0.02)
            {
                result = false;
            }
            return result;
        }

        private void FormMissionInfoEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Alt && e.KeyCode == Keys.F6)
            {
                if (_selectedStep != null && !string.IsNullOrEmpty(_selectedStep.bsHao))
                {
                    int toolId = int.Parse(_selectedStep.bsHao);
                    float.TryParse(tbStdValue.Text, out var stdValue);
                    //生成模拟数据
                    MeasuringDevice.GenerateTestData(toolId, stdValue);
                }
            }
            else if (e.Shift && e.Alt && e.KeyCode == Keys.F7)
            {
                if (_selectedStep != null && !string.IsNullOrEmpty(_selectedStep.bsHao))
                {
                    int toolId = int.Parse(_selectedStep.bsHao);
                    float.TryParse(tbStdValue.Text, out var stdValue);
                    //生成模拟数据
                    MagtaDevice.GenerateTestData(toolId, stdValue);
                }
            }
            else if (e.Shift && e.Alt && e.KeyCode == Keys.T)
            {
                if (_selectedStep != null && !string.IsNullOrEmpty(_selectedStep.bsHao))
                {
                    int position = int.Parse(_selectedStep.bsHao) - 1;
                    DeviceLayer.CabinetDevice.EmulateToolTaken(position);
                }
            }
        }

        private void tbStdCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbStdCode.Text.Length == 0)
                {
                    MessageBox.Show("请扫描部件二维码");
                    return;
                }
                string[] codeInfo = tbStdCode.Text.Split('-');
                if (AppConfig.AppType != 1)
                {
                    //条码分3部分
                    if (codeInfo.Length < 3)
                    {
                        MessageBox.Show("条码信息不正确");
                        return;
                    }
                    tbTrainCode.Text = codeInfo[0] + "-" + codeInfo[1];
                    tbMachineCode.Text = tbStdCode.Text;
                }
                else
                {
                    //电机线条码5部分
                    if (codeInfo.Length < 5)
                    {
                        MessageBox.Show("条码信息不正确");
                        return;
                    }
                    tbTrainCode.Text = codeInfo[0] + "-" + codeInfo[1] + "-" + codeInfo[2];
                    tbMachineCode.Text = codeInfo[4];
                }
                UpdateMachineCode(tbMachineCode.Text);
            }
        }

        private void pbLeft_Click(object sender, EventArgs e)
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                LoadStepData();
            }
        }

        private void pbRight_Click(object sender, EventArgs e)
        {
            if (_currentPage < _maxPage - 1)
            {
                _currentPage++;
                LoadStepData();
            }
        }
    }
}

