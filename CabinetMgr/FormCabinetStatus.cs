using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using CabinetMgr.RtDelegate;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using NLog;
using Utilities.DbHelper;
using Utilities.FileHelper;

// ReSharper disable FunctionNeverReturns
// ReSharper disable LocalizableElement
// ReSharper disable ArrangeThisQualifier

namespace CabinetMgr
{
    public partial class FormCabinetStatus : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public bool PanelMode = false;
        private int _historyRow = 14, _pageCount = 1;
        private int _toolRowCount = 4;

        private int _toolColWidth, _toolRowHeight, _drawerColWidth, _drawerRowHeight;
        private CellStyle _ctRed, _ctYellow, _ctGreen, _ctBlue, _ctBlack, _ctSplit, _ctNormal,
            _ctConfirm, _ctComment;
        private bool _initDone = true;
        private string _offDutyTime;
        private int _alertCtrl;
        private IList<ToolInfo> lstToolInfos;
        private Dictionary<int, int> addRowPointDic = new Dictionary<int, int>();
        
        public FormCabinetStatus()
        {
            InitializeComponent();

            CabinetCallback.OnToolTaken = OnToolTaken;
            CabinetCallback.OnToolReturn = OnToolReturn;
            UhfCallback.OnCardTaken = OnCardTaken;
            UhfCallback.OnCardReturn = OnCardReturn;
        }

        private void FormCabinetStatus_Load(object sender, EventArgs e)
        {
            _offDutyTime = AppConfig.OffDutyTime;

            //加载抽屉工具信息
            IList toolList = BllToolInfo.SearchDrawerToolInfo();
            if (SqlDataHelper.IsDataValid(toolList))
            {
                for (int i = 0; i < toolList.Count; i++)
                {
                    ToolInfo ti = (ToolInfo)toolList[i];
                    UhfDevice.AddCardToDevice(ti.ToolPosition - 1, ti.Id, ti.CardId);
                }
            }

            InitGrid();
            RefreshHistoryGrid();
            Thread alertThd = new Thread(AlertCtrl) { IsBackground = true };
            alertThd.Start();
            Thread errorScanner = new Thread(ScanForError) { IsBackground = true };
            //errorScanner.Start();
            lstToolInfos = BllToolInfo.SearchToolInfo(-1, -1, "", "", "").Cast<ToolInfo>().ToList();
            PropertyInfo spotPI = typeof(AppConfig).GetProperty($"AddRowPoint");
            List<string> tmpList = new List<string>();
            if (spotPI != null)
            {
                tmpList = spotPI.GetValue(null, null).ToString().Split('|').ToList();
            }
            for (int i = 0; i < tmpList.Count; i++)
            {
                if (string.IsNullOrEmpty(tmpList[i])) continue;
                addRowPointDic.Add(int.Parse(tmpList[i].Split(',')[0]), int.Parse(tmpList[i].Split(',')[1]));
            }
        }

        private void AlertCtrl()
        {
            while (true)
            {
                try
                {
                    Invoke(new UpdateGridDelegate(UpdateGridMt), null);
                    if (++_alertCtrl >= _pageCount * 10)
                    {
                        _alertCtrl = 0;
                    }
                    Thread.Sleep(500);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private delegate void UpdateGridDelegate();
        private void UpdateGridMt()
        {
            if (_alertCtrl % 3 == 0)
            {
                var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
                RefreshUpperGrid(toolStatusList);
            }
            cUpperGrid.Invalidate();
        }

        private void FormCabinetStatus_Shown(object sender, EventArgs e)
        {
            //_initDone = true;
        }


        private void OnToolTaken(int toolPosition)
        {
            try
            {
                Invoke(new OnToolTakenDelegate(OnToolTakenMt), toolPosition);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public delegate void OnToolTakenDelegate(int toolPosition);
        /// <summary>
        /// 工具借出
        /// </summary>
        /// <param name="toolPosition"></param>
        public void OnToolTakenMt(int toolPosition)
        {
            toolPosition++;
            BllToolInfo.SetRtStatus(toolPosition.ToString(), 0);
            if (AppRt.IsInit) return;
            if (AppRt.CurUser == null) return;
            lock (AppRt.RequireLock)
            {
                if (AppRt.MissionMode)
                {
                    if (!AppRt.RequiredList.Contains(toolPosition))
                    {
                        //错拿工具,闪烁提示
                        DeviceLayer.CabinetDevice.StartToolAlert(toolPosition - 1, DateTime.Now.AddDays(1));
                    }
                    else
                    {
                        DelegateMissionInfo.OnToolTaken?.Invoke(toolPosition - 1);
                        //清除可能存在的异常指示
                        DeviceLayer.CabinetDevice.EndToolAlert(toolPosition - 1);
                    }
                }
                else
                {
                    //清除可能存在的异常指示
                    DeviceLayer.CabinetDevice.EndToolAlert(toolPosition - 1);
                }
            }
            //刷新任务列表
            DelegateMissionInfo.RefreshMission?.Invoke();
            //保存借出记录
            ToolInfo ti = BllToolInfo.GetUpperToolInfo(toolPosition);
            if (ti != null)
            {
                if (BllBorrowRecord.AddBorrowRecord(ti.Id, ti.ToolName, toolPosition, AppRt.CurUser.Id,
                        AppRt.CurUser.FullName, int.Parse(ti.HardwareId)) <= 0)
                {
                    MessageBox.Show("借取记录保存失败");
                }
            }
            //更新流水显示
            RefreshHistoryGrid();
        }

        private void OnToolReturn(int toolPosition)
        {
            try
            {
                this.Invoke(new OnToolReturnDelegate(OnToolReturnMt), toolPosition);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public delegate void OnToolReturnDelegate(int toolPosition);
        /// <summary>
        /// 工具归还
        /// </summary>
        /// <param name="toolPosition"></param>
        public void OnToolReturnMt(int toolPosition)
        {
            DelegateMissionInfo.OnToolReturn?.Invoke(toolPosition);
            toolPosition++;
            BllToolInfo.SetRtStatus(toolPosition.ToString(), 1);
            if (AppRt.IsInit) return;
            if (AppRt.CurUser == null) return;
            //清除工具异常状态
            DeviceLayer.CabinetDevice.EndToolAlert(toolPosition - 1);
            //更新流程记录
            /*
            string workUserId = AppRt.CurUser.Id;
            ReturnInfo ri = new ReturnInfo()
            {
                ToolPosition = toolPosition,
                WorkUserId = workUserId
            };
            Thread uiReturnThd = new Thread(WorkUiReturn) {IsBackground = true};
            uiReturnThd.Start(ri);*/
            //保存归还记录
            ToolInfo ti = BllToolInfo.GetUpperToolInfo(toolPosition);
            if (ti != null)
            {
                if (BllReturnRecord.AddReturnRecord(toolPosition, AppRt.CurUser.Id, AppRt.CurUser.FullName) <= 0)
                {
                    MessageBox.Show("还回记录保存失败");
                }
            }
            //更新流水显示
            RefreshHistoryGrid();
        }

        /*
class ReturnInfo
{
    public string WorkUserId;
    public int ToolPosition;
}


private void WorkUiReturn(object retInfo)
{
    ReturnInfo ri = (ReturnInfo) retInfo;
    List<string> returnList = new List<string>();
    IList workList = BllWorkUserInfo.SearchWorkUserInfo(ri.WorkUserId);
    if (SqlDataHelper.IsDataValid(workList))
    {
        for (int i = 0; i < workList.Count; i++)
        {
            WorkUserInfo wui = (WorkUserInfo)workList[i];
            if (wui.WorkStatus == 11 && ri.ToolPosition.ToString().Equals(wui.ToolCode))
            {
                returnList.Add(wui.Id);
            }
        }
        if (returnList.Count > 0)
        {
            if (BllWorkUserInfo.SetAsReturned(returnList.ToArray()) <= 0)
            {
                MessageBox.Show("还回状态保存失败");
                return;
            }
        }
        //刷新任务列表
        DelegateMissionInfo.RefreshMission?.Invoke();
    }
}*/

        /// <summary>
        /// 抽屉工具借出
        /// </summary>
        /// <param name="comIndex"></param>
        /// <param name="cardId"></param>
        public void OnCardTaken(int comIndex, string cardId)
        {
            if (AppRt.IsInit) return;
            if (AppRt.CurUser == null) return;
            //保存借出记录
            ToolInfo ti = BllToolInfo.GetToolInfoByCard(cardId);
            if (ti != null)
            {
                if (BllBorrowRecord.AddBorrowRecord(ti.Id, ti.ToolName, 10000 + comIndex, AppRt.CurUser.Id, AppRt.CurUser.FullName, int.Parse(ti.HardwareId)) <= 0)
                {
                    MessageBox.Show("借取记录保存失败");
                }
            }
            //更新流水显示
            RefreshHistoryGrid();
        }

        /// <summary>
        /// 抽屉工具归还
        /// </summary>
        /// <param name="comIndex"></param>
        /// <param name="cardId"></param>
        public void OnCardReturn(int comIndex, string cardId)
        {
            if (AppRt.IsInit) return;
            if (AppRt.CurUser == null) return;
            //更新流程记录
            string workUserId = AppRt.CurUser.Id;
            IList workList = BllWorkUserInfo.SearchWorkUserInfo(workUserId);
            if (SqlDataHelper.IsDataValid(workList))
            {
                //下层是否绑定任务待定
            }
            //保存归还记录
            ToolInfo ti = BllToolInfo.GetToolInfoByCard(cardId);
            if (ti != null)
            {
                if (BllReturnRecord.AddReturnRecord(10000 + comIndex, AppRt.CurUser.Id, AppRt.CurUser.FullName) <= 0)
                {
                    MessageBox.Show("还回记录保存失败,请核对工具是否为该工人借取");
                }
            }
            //更新流水显示
            RefreshHistoryGrid();
        }

        private void ScanForError()
        {
            bool checkEnabled = false;
            while (true)
            {
                if (DateTime.Now >= DateTime.Parse(_offDutyTime))
                {
                    checkEnabled = true;
                    bool hasError = false;
                    //上层扫描
                    var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
                    for (int i = 0; i < AppConfig.ToolCount; i++)
                    {
                        if (toolStatusList[i].Status == 0)
                        {
                            BorrowRecord br = BllBorrowRecord.GetBorrowRecord(i + 1);
                            if (br != null && br.Status == 0)
                            {
                                //工具不在位且有对应借出记录
                                hasError = true;
                                //开启工具闪灯
                                DeviceLayer.CabinetDevice.StartToolAlert(i, DateTime.Now.AddDays(1));
                            }
                            else
                            {
                                DeviceLayer.CabinetDevice.EndToolAlert(i);
                            }
                        }
                        else
                        {
                            DeviceLayer.CabinetDevice.EndToolAlert(i);
                        }
                    }
                    //下层扫描
                    for (int d = 0; d < AppConfig.DrawerCount; d++)
                    {
                        var deviceList = UhfDevice.GetCardList(d);
                        if (deviceList != null)
                        {
                            int existCount = 0;
                            for (int i = 0; i < deviceList.Length; i++)
                            {
                                if (deviceList[i].IsExist) existCount++;
                            }
                            //判断是否存在未还工具
                            if (existCount != deviceList.Length)
                            {
                                //hasError = true;
                            }
                        }
                    }
                    RefreshHistoryGrid();

                    //扫描上层超时工具
                    IList toolList = BllToolInfo.SearchToolInfo(0, -1, null, null, null);
                    if (SqlDataHelper.IsDataValid(toolList))
                    {
                        for (int i = 0; i < toolList.Count && i < AppConfig.ToolCount; i++)
                        {
                            ToolInfo ti = (ToolInfo)toolList[i];
                            if (DateTime.Now >= ti.NextCheckTime)
                            {
                                //开启工具闪灯
                                DeviceLayer.CabinetDevice.SetToolLedCheckStatus(ti.ToolPosition - 1, true);
                                hasError = true;
                            }
                            else
                            {
                                //关闭闪灯
                                DeviceLayer.CabinetDevice.SetToolLedCheckStatus(ti.ToolPosition - 1, false);
                            }
                        }
                    }

                    if (hasError)
                    {
                        //开启红色报警灯
                        DeviceLayer.CabinetDevice.OpenLedRed(true);
                    }
                    else
                    {
                        DeviceLayer.CabinetDevice.OpenLedRed(false);
                    }
                }
                else
                {
                    if (checkEnabled)
                    {
                        DeviceLayer.CabinetDevice.OpenLedRed(false);
                        for (int i = 0; i < AppConfig.ToolCount; i++)
                        {
                            DeviceLayer.CabinetDevice.EndToolAlert(i);
                        }
                        checkEnabled = false;
                    }
                }
                Thread.Sleep(2000);
            }
        }

        private delegate void RefreshHistoryGridDelegate();
        private void RefreshHistoryGrid()
        {
            if (cHistoryGrid.InvokeRequired)
            {
                RefreshHistoryGridDelegate d = RefreshHistoryGrid;
                cHistoryGrid.Invoke(d);
            }
            else
            {
                IList borrowList = BllBorrowRecord.SearchBorrowRecord(DateTime.Now.AddDays(-1), DateTime.Now);
                if (SqlDataHelper.IsDataValid(borrowList))
                {
                    try
                    {
                        cHistoryGrid.BeginUpdate();
                        int i;
                        for (i = 0; i < _historyRow && i < borrowList.Count; i++)
                        {
                            BorrowRecord br = (BorrowRecord)borrowList[i];
                            string comment = "";
                            switch (br.Status)
                            {
                                case 20:
                                    comment = br.ReturnTime.ToString("MM-dd HH:mm");
                                    break;
                                case 11:
                                    comment = "备注:" + br.ExpireComment;
                                    break;
                            }
                            ReturnRecord rr = BllReturnRecord.GetReturnRecord(br.Id);
                            string rrUser = rr == null ? "" : rr.WorkerName;
                            FillToHistoryGrid(i + 1, br.Id, br.EventTime.ToString("MM-dd HH:mm"), br.ToolPosition.ToString("D2"),
                                br.ToolName, br.WorkerName, FormatHistoryStatus(br.Status), comment, rrUser);
                        }
                        for (; i < _historyRow; i++)
                        {
                            FillToHistoryGrid(i + 1, -1, "", "", "", "", "", "", ""); //Kane,原来少个"",导致原来数据行数据多，变为行数少时，会有还工具人清除不掉，2020-12-17 21:39
                        }
                        cHistoryGrid.EndUpdate();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        cHistoryGrid.EndUpdate();
                    }
                }
            }
            //同时刷新借还窗口
            DelegateReturnRecord.RefreshHistoryGrid?.Invoke();
        }

        private void FormCabinetStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Alt && e.KeyCode == Keys.K)
            {
                AppRt.IsInit = false;
                int iToolNo = 0;
                try
                {
                    string toolNo = "2";// ConfigurationManager.AppSettings["ToolNo"];
                    iToolNo = Convert.ToInt16(toolNo) - 1;
                }
                catch
                {
                }
                //CabinetDevice.EmulateToolTaken(iToolNo);
                DeviceLayer.CabinetDevice.EmulateToolTaken(iToolNo);
            }
            if (e.Shift && e.Alt && e.KeyCode == Keys.L)
            {
                AppRt.IsInit = false;
                int iToolNo = 0;
                try
                {
                    string toolNo = "2";// ConfigurationManager.AppSettings["ToolNo"];
                    iToolNo = Convert.ToInt16(toolNo) - 1;
                }
                catch
                {
                }
                DeviceLayer.CabinetDevice.EmulateToolReturn(iToolNo);
            }
        }

        private void tmRtRefresher_Tick(object sender, EventArgs e)
        {
            var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
            if (toolStatusList != null)
            {
                tmRtRefresher.Interval = 60000;
                BllToolInfo.UpdateRtStatus(toolStatusList);
            }
        }

        private string FormatHistoryStatus(int status)
        {
            switch (status)
            {
                case 0:
                    return "未归还";
                case 10:
                    return "超时未还";
                case 11:
                    return "超时未还";
                case 20:
                    return "已归还";
            }
            return "";
        }

        private void FillToHistoryGrid(int targetRow, params object[] data)
        {
            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    cHistoryGrid.Rows[targetRow][i + 1] = data[i];
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 刷新上层工具柜状态
        /// </summary>
        private void RefreshUpperGrid(CabinetCallback.ToolStatus[] toolStatusList)
        {
            //未初始化时不进行刷新
            if (_initDone == false) return;

            try
            {
                cUpperGrid.BeginUpdate();
                cUpperGrid.Rows.Count = 1;

                int curRow = 1, curCol = 1;
                //预设显示行数
                cUpperGrid.Rows.Count = 200;
                for (int i = 1; i < cUpperGrid.Rows.Count; i++)
                {
                    cUpperGrid.Rows[i].Height = _toolRowHeight;
                }
                int skipLength = 0, takeLength = AppConfig.ToolCount,
                    toolCount = AppConfig.ToolCount;
                int pageCapacity = AppConfig.ToolCol * _toolRowCount;
                if (false)
                {
                    _pageCount = addRowPointDic.Count;
                    int page = _alertCtrl / 10;
                    takeLength = addRowPointDic[page];
                    for (int i = 0; i < page; i++)
                    {
                        skipLength += addRowPointDic[i];
                    }
                    for (int i = skipLength; i < toolCount && i < skipLength + takeLength; i++)
                    {
                        //每列格数限制
                        if ((i - skipLength) % AppConfig.ToolCol == 0 && i != skipLength)
                        {
                            curRow++;
                            curCol = 1;
                        }
                        if (cUpperGrid.Rows[curRow][0] == null)
                        {
                            cUpperGrid.SetCellStyle(curRow, 0, cUpperGrid.GetCellStyle(curRow - 1, 0));
                        }
                        var gridContent = (i + 1).ToString("D2") + "|";
                        if (i >= AppConfig.ToolCount)
                        {
                            //模拟数据
                            gridContent += "在位";
                        }
                        else if (toolStatusList[i].RepairStatus == 1)
                        {
                            gridContent += "维修";
                        }
                        else if (toolStatusList[i].AlertStatus == 1)
                        {
                            gridContent += "异常";
                        }
                        else if (toolStatusList[i].CheckStatus == 1)
                        {
                            gridContent += "待检";
                        }
                        else
                        {
                            gridContent += toolStatusList[i].Status == 0 ? "离位" : "在位";
                        }
                        ToolInfo ti = lstToolInfos.FirstOrDefault(x => x.HardwareId == (i + 1).ToString());
                        if (ti != null)
                        {
                            gridContent += "|" + ti.ToolName;
                        }
                        else
                        {
                            gridContent += "|";
                        }
                        cUpperGrid.Rows[curRow][curCol] = gridContent;
                        curCol++;
                    }
                    UpdateText(string.Format("当前层：{0}", page + 1));
                }
                else
                {
                    if (toolCount > pageCapacity)
                    {
                        //分页模式相关计算
                        if (_pageCount == 1)
                        {
                            //计算页数
                            _pageCount = (int)Math.Ceiling((float)toolCount / pageCapacity);
                        }
                        skipLength = _alertCtrl / 10 * pageCapacity;
                        takeLength = toolCount - skipLength >= pageCapacity ? pageCapacity : toolCount - skipLength;
                    }
                    //填充工具箱状态表
                    for (int i = skipLength; i < toolCount && i < skipLength + takeLength; i++)
                    {
                        //每列格数限制
                        if ((i - skipLength) % AppConfig.ToolCol == 0 && i != skipLength)
                        {
                            curRow++;
                            curCol = 1;
                        }
                        if (cUpperGrid.Rows[curRow][0] == null)
                        {
                            cUpperGrid.SetCellStyle(curRow, 0, cUpperGrid.GetCellStyle(curRow - 1, 0));
                        }
                        var gridContent = (i + 1).ToString("D2") + "|";
                        if (i >= AppConfig.ToolCount)
                        {
                            //模拟数据
                            gridContent += "在位";
                        }
                        else if (toolStatusList[i].RepairStatus == 1)
                        {
                            gridContent += "维修";
                        }
                        else if (toolStatusList[i].AlertStatus == 1)
                        {
                            gridContent += "异常";
                        }
                        else if (toolStatusList[i].CheckStatus == 1)
                        {
                            gridContent += "待检";
                        }
                        else
                        {
                            //string user = BllBorrowRecord.GetLastUnReturnedBorrowRecord(i)?.WorkerName;
                            gridContent += toolStatusList[i].Status == 0 ? "离位:" : "在位";
                        }
                        ToolInfo ti = lstToolInfos.FirstOrDefault(x => x.HardwareId == (i + 1).ToString());
                        if (ti != null)
                        {
                            gridContent += "|" + ti.ToolName;
                        }
                        else
                        {
                            gridContent += "|";
                        }
                        //太急 以后优化
                        switch (addRowPointDic.Count)
                        {
                            default:
                                gridContent += "|0";
                                break;
                            case 2:
                                if (i + 1 > addRowPointDic[0] && i + 1 <= addRowPointDic[0] + addRowPointDic[1])
                                {
                                    gridContent += "|1";
                                }
                                else
                                {
                                    gridContent += "|0";
                                }
                                break;
                            case 3:
                                if (i + 1 > addRowPointDic[0] && i + 1 <= addRowPointDic[0] + addRowPointDic[1])
                                {
                                    gridContent += "|1";
                                }
                                else if (i + 1 > addRowPointDic[0] + addRowPointDic[1] && i + 1 <= addRowPointDic[0] + addRowPointDic[1] + addRowPointDic[2])
                                {
                                    gridContent += "|2";
                                }
                                else
                                {
                                    gridContent += "|0";
                                }
                                break;
                            case 4:
                                if (i + 1 > addRowPointDic[0] && i + 1 <= addRowPointDic[0] + addRowPointDic[1])
                                {
                                    gridContent += "|1";
                                }
                                else if (i + 1 > addRowPointDic[0] + addRowPointDic[1] && i + 1 <= addRowPointDic[0] + addRowPointDic[1] + addRowPointDic[2])
                                {
                                    gridContent += "|2";
                                }
                                else if (i + 1 > addRowPointDic[0] + addRowPointDic[1] + addRowPointDic[2] && i + 1 <= addRowPointDic[0] + addRowPointDic[1] + addRowPointDic[2] + addRowPointDic[3])
                                {
                                    gridContent += "|3";
                                }
                                else
                                {
                                    gridContent += "|0";
                                }
                                break;
                        }
                        cUpperGrid.Rows[curRow][curCol] = gridContent;
                        curCol++;
                    }
                
                }
                cUpperGrid.Rows.Count = curRow + 1;
                cUpperGrid.EndUpdate();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                cUpperGrid.EndUpdate();

            }
        }

        private delegate void UpdateTextDelegte(string text);

        private void UpdateText(string text)
        {
            if (lbl_Floor.InvokeRequired)
            {
                UpdateTextDelegte d = UpdateText;
                lbl_Floor.Invoke(d, text);
            }
            else
            {
                lbl_Floor.Text = text;
                Application.DoEvents();
            }
        }


        /// <summary>
        /// 刷新下层工具柜状态
        /// </summary>
        private void RefreshBottomGrid()
        {
            //未初始化时不进行刷新
            if (_initDone == false) return;

            try
            {
                cBottomGrid.BeginUpdate();
                cBottomGrid.Rows.Count = 1;

                int curRow = 1, curCol = 1;
                //预设显示行数
                cBottomGrid.Rows.Count = 200;
                for (int i = 1; i < cBottomGrid.Rows.Count; i++)
                {
                    cBottomGrid.Rows[i].Height = _drawerRowHeight;
                }
                //填充工具箱状态表
                for (int d = 0; d < AppConfig.DrawerCount; d++)
                {
                    if (d % AppConfig.DrawerCol == 0 && d != 0)
                    {
                        curRow++;
                        curCol = 1;
                    }
                    if (cBottomGrid.Rows[curRow][0] == null)
                    {
                        cBottomGrid.SetCellStyle(curRow, 0, cBottomGrid.GetCellStyle(curRow - 1, 0));
                    }
                    //处理箱内工具状态
                    var deviceList = UhfDevice.GetCardList(d);
                    if (deviceList != null)
                    {
                        int existCount = 0;
                        for (int i = 0; i < deviceList.Length; i++)
                        {
                            if (deviceList[i].IsExist) existCount++;
                        }
                        cBottomGrid.Rows[curRow][curCol] = (d + 1).ToString("D2") + "|" + existCount + "/" + deviceList.Length;
                    }
                    curCol++;
                }
                cBottomGrid.Rows.Count = curRow + 1;
                cBottomGrid.EndUpdate();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void InitGrid()
        {
            GenGridLine();

            _ctRed = cUpperGrid.Styles.Add("CtRed");
            _ctRed.BackColor = Color.HotPink;
            _ctYellow = cUpperGrid.Styles.Add("CtYellow");
            _ctYellow.BackColor = Color.Gold;
            _ctGreen = cUpperGrid.Styles.Add("CtGreen");
            _ctGreen.BackColor = Color.FromArgb(207, 221, 238);
            _ctBlue = cUpperGrid.Styles.Add("CtBlue");
            _ctBlue.BackColor = Color.SteelBlue;
            _ctBlack = cUpperGrid.Styles.Add("CtBlack");
            _ctBlack.BackColor = Color.DarkGray;
            _ctSplit = cUpperGrid.Styles.Add("CtSplit");
            _ctSplit.BackColor = Color.FromArgb(207, 221, 238);
            _ctNormal = cUpperGrid.Styles.Add("CtNormal");
            _ctNormal.BackColor = Color.FromArgb(225, 225, 225);
            cUpperGrid.HighLight = HighLightEnum.Never;
            cUpperGrid.Width = _toolColWidth * AppConfig.ToolCol;

            _ctConfirm = cHistoryGrid.Styles.Add("CtConfirm");
            _ctConfirm.BackgroundImage = Properties.Resources.confirm;
            _ctConfirm.BackgroundImageLayout = ImageAlignEnum.Stretch;
            _ctComment = cHistoryGrid.Styles.Add("CtComment");
            _ctComment.BackgroundImage = Properties.Resources.comment;
            _ctComment.BackgroundImageLayout = ImageAlignEnum.Stretch;

            cBottomGrid.HighLight = HighLightEnum.Never;
            cBottomGrid.Width = _drawerColWidth * AppConfig.DrawerCol;
        }

        private void FormCabinetStatus_Resize(object sender, EventArgs e)
        {

        }

        private void GenGridLine()
        {
            //上部Grid设置
            /*
            int toolCount = AppConfig.ToolCount > AppConfig.EmuToolCount ? AppConfig.ToolCount : AppConfig.EmuToolCount;
            int toolRowCount = (int)Math.Ceiling((double)toolCount / AppConfig.ToolCol);
            if (toolRowCount > 4) toolRowCount = 4;
            */
            _toolColWidth = cUpperGrid.Width / AppConfig.ToolCol;
            _toolRowHeight = cUpperGrid.Height / _toolRowCount;
            cUpperGrid.Cols[0].Visible = false;
            cUpperGrid.Cols.Count = AppConfig.ToolCol + 1;
            cUpperGrid.Rows[0].Visible = false;
            for (int i = 1; i <= AppConfig.ToolCol; i++)
            {
                cUpperGrid.Cols[i].Width = _toolColWidth;
                cUpperGrid.Cols[i].TextAlign = TextAlignEnum.CenterCenter;
            }
            for (int i = 1; i < cUpperGrid.Rows.Count; i++)
            {
                cUpperGrid.Rows[i].Height = _toolRowHeight;
            }

            //下部Grid设置
            int drawerRowCount = (int)Math.Ceiling((double)AppConfig.DrawerCount / AppConfig.DrawerCol);
            if (drawerRowCount == 0) drawerRowCount = 2;
            _drawerColWidth = cBottomGrid.Width / AppConfig.DrawerCol;
            _drawerRowHeight = cBottomGrid.Height / drawerRowCount;
            cBottomGrid.Cols[0].Visible = false;
            cBottomGrid.Cols.Count = AppConfig.DrawerCol + 1;
            cBottomGrid.Rows[0].Visible = false;
            for (int i = 1; i <= AppConfig.DrawerCol; i++)
            {
                cBottomGrid.Cols[i].Width = _drawerColWidth;
                cBottomGrid.Cols[i].TextAlign = TextAlignEnum.CenterCenter;
            }
            for (int i = 1; i < cBottomGrid.Rows.Count; i++)
            {
                cBottomGrid.Rows[i].Height = _drawerRowHeight;
            }
            //历史Grid设置
            cHistoryGrid.Cols.Count = 9;
            cHistoryGrid.Cols[3].Caption = "工具编号";
            cHistoryGrid.Cols[3].Width = cHistoryGrid.Width / 8;
            cHistoryGrid.Cols[3].TextAlign = TextAlignEnum.LeftCenter;
            cHistoryGrid.Cols[3].Style.Font = new Font("宋体", 16);
            cHistoryGrid.Cols[1].Caption = "Id";
            cHistoryGrid.Cols[1].Visible = false;
            cHistoryGrid.Cols[2].Caption = "领取时间";
            cHistoryGrid.Cols[2].Width = cHistoryGrid.Width / 7;
            cHistoryGrid.Cols[2].Style = cHistoryGrid.Cols[3].Style.Clone();
            cHistoryGrid.Cols[2].Style.Font = new Font("宋体", 16);
            cHistoryGrid.Cols[4].Caption = "工具名称";
            cHistoryGrid.Cols[4].Width = cHistoryGrid.Width / 7;
            cHistoryGrid.Cols[4].Style = cHistoryGrid.Cols[3].Style.Clone();
            cHistoryGrid.Cols[4].Style.Font = new Font("宋体", 16);
            cHistoryGrid.Cols[4].TextAlign = TextAlignEnum.LeftCenter;
            cHistoryGrid.Cols[5].Caption = "领取人";
            cHistoryGrid.Cols[5].Width = cHistoryGrid.Width / 8;
            cHistoryGrid.Cols[5].Style = cHistoryGrid.Cols[3].Style.Clone();
            cHistoryGrid.Cols[5].Style.Font = new Font("宋体", 16);
            cHistoryGrid.Cols[5].TextAlign = TextAlignEnum.LeftCenter;
            cHistoryGrid.Cols[6].Caption = "归还状态";
            cHistoryGrid.Cols[6].Style = cHistoryGrid.Cols[3].Style.Clone();
            cHistoryGrid.Cols[6].Style.Font = new Font("宋体", 16);
            cHistoryGrid.Cols[6].Width = cHistoryGrid.Width / 8;
            cHistoryGrid.Cols[7].Caption = "归还时间";
            cHistoryGrid.Cols[7].Style = cHistoryGrid.Cols[3].Style.Clone();
            cHistoryGrid.Cols[7].Style.Font = new Font("宋体", 16);
            cHistoryGrid.Cols[7].Width = cHistoryGrid.Width / 7;
            cHistoryGrid.Cols[8].Caption = "归还人";
            cHistoryGrid.Cols[8].Style = cHistoryGrid.Cols[3].Style.Clone();
            cHistoryGrid.Cols[8].Style.Font = new Font("宋体", 16);
            cHistoryGrid.Cols[8].Width = cHistoryGrid.Width / 7;
            cHistoryGrid.Rows.Count = _historyRow + 1;
            for (int i = 1; i < _historyRow; i++)
            {
                cHistoryGrid.Rows[i][0] = i.ToString();
                cHistoryGrid.Rows[i].Height = 25;
            }
        }

        #region FlexGrid单元格重绘

        private void cUpperGrid_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            //防止自带样式覆盖
            if (e.Col == 0)
            {
                e.DrawCell();
                e.Handled = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(e.Text))
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    Rectangle textBound = e.Bounds;
                    int baseX = textBound.X, baseY = textBound.Y;
                    textBound.X = baseX + AppConfig.ToolCodeX;
                    textBound.Y = baseY + AppConfig.ToolCodeY;
                    #region 图片设置（写死

                    string toolName = e.Text.Split('|')[2];
                    switch (toolName)
                    {
                        case "电钻":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.电钻离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.电钻0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.电钻1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.电钻2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.电钻3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "电动螺丝刀":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.电钻离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.电钻0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.电钻1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.电钻2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.电钻3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "风扳机":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.风扳机离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.风扳机0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.风扳机1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.风扳机2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.风扳机3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "万用表":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.万用表离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.万用表0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.万用表1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.万用表2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.万用表3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "重型套筒":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.套筒离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.套筒0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.套筒1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.套筒2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.套筒3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "电磨":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.电磨离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.电磨0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.电磨1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.电磨2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.电磨3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "断线钳":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.断线钳离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.断线钳0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.断线钳1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.断线钳2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.断线钳3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "铆钉枪":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.铆钉枪离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.铆钉枪0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.铆钉枪1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.铆钉枪2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.铆钉枪3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "深度游标卡尺":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.尺子类离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.尺子类0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.尺子类1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.尺子类2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.尺子类3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "冲击螺丝刀":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.冲击螺丝刀离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.冲击螺丝刀0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.冲击螺丝刀1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.冲击螺丝刀2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.冲击螺丝刀3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "液压压线钳":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.压线钳离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "针形压线钳":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.压线钳离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "管钳":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.管钳离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.管钳0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.管钳1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.管钳2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.管钳3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "角磨机":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.角磨机离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.角磨机0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.角磨机1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.角磨机2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.角磨机3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "冲击钻":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.冲击钻离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.冲击钻0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.冲击钻1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.冲击钻2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.冲击钻3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "活动扳手":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.活动扳手离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.活动扳手0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.活动扳手1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.活动扳手2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.活动扳手3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "手枪钻":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.电钻离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.电钻0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.电钻1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.电钻2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.电钻3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "退针器":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.退针器离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.退针器0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.退针器1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.退针器2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.退针器3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "扭力扳手":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.扭力扳手离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.扭力扳手0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.扭力扳手1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.扭力扳手2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.扭力扳手3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "螺丝刀式套筒":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.套筒螺丝刀离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.套筒螺丝刀0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.套筒螺丝刀1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.套筒螺丝刀2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.套筒螺丝刀3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "自动过分相磁铁":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.磁铁离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.磁铁0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.磁铁1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.磁铁2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.磁铁3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "棘轮短接杆":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.棘轮接杆离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "棘轮加长杆":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.棘轮接杆离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮接杆3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "棘轮扳手":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.棘轮扳手离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "棘轮头":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.棘轮头离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "风扳机头":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.棘轮头离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮头3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "棘轮开口扳手":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.棘轮扳手离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.棘轮扳手3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "双头棘轮扳手":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.双头棘轮扳手离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.双头棘轮扳手0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.双头棘轮扳手1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.双头棘轮扳手2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.双头棘轮扳手3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "摇把":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.摇把离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.摇把0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.摇把1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.摇把2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.摇把3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "锯弓":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.锯弓离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.锯弓0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.锯弓1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.锯弓2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.锯弓3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "快速液压钳":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.液压钳离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.液压钳0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.液压钳1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.液压钳2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.液压钳3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "梅花开口扳手":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.扳手离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.扳手0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.扳手1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.扳手2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.扳手3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "丁字扳手":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.丁字扳手离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.丁字扳手0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.丁字扳手1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.丁字扳手2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.丁字扳手3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "压线钳":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.压线钳离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.压线钳3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "十字螺丝刀":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.十字螺丝刀离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.十字螺丝刀0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.十字螺丝刀1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.十字螺丝刀2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.十字螺丝刀3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        case "一字螺丝刀":
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.一字螺丝刀离位, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources.一字螺丝刀0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources.一字螺丝刀1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources.一字螺丝刀2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources.一字螺丝刀3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                        default:
                            if (toolName.Contains("磁铁"))
                            {
                                if (e.Text.Contains("离位"))
                                {
                                    e.Graphics.DrawImage(Properties.Resources.磁铁离位, e.Bounds);
                                }
                                else
                                {
                                    switch (e.Text.Split('|')[3])
                                    {
                                        case "0":
                                            e.Graphics.DrawImage(Properties.Resources.磁铁0, e.Bounds);
                                            break;
                                        case "1":
                                            e.Graphics.DrawImage(Properties.Resources.磁铁1, e.Bounds);
                                            break;
                                        case "2":
                                            e.Graphics.DrawImage(Properties.Resources.磁铁2, e.Bounds);
                                            break;
                                        case "3":
                                            e.Graphics.DrawImage(Properties.Resources.磁铁3, e.Bounds);
                                            break;
                                    }
                                }
                                break;
                            }
                            if (toolName.Contains("尺"))
                            {
                                if (e.Text.Contains("离位"))
                                {
                                    e.Graphics.DrawImage(Properties.Resources.尺子类离位, e.Bounds);
                                }
                                else
                                {
                                    switch (e.Text.Split('|')[3])
                                    {
                                        case "0":
                                            e.Graphics.DrawImage(Properties.Resources.尺子类0, e.Bounds);
                                            break;
                                        case "1":
                                            e.Graphics.DrawImage(Properties.Resources.尺子类1, e.Bounds);
                                            break;
                                        case "2":
                                            e.Graphics.DrawImage(Properties.Resources.尺子类2, e.Bounds);
                                            break;
                                        case "3":
                                            e.Graphics.DrawImage(Properties.Resources.尺子类3, e.Bounds);
                                            break;
                                    }
                                }
                                break;
                            }
                            if (toolName.Contains("螺丝刀"))
                            {
                                if (e.Text.Contains("离位"))
                                {
                                    e.Graphics.DrawImage(Properties.Resources.十字螺丝刀离位, e.Bounds);
                                }
                                else
                                {
                                    switch (e.Text.Split('|')[3])
                                    {
                                        case "0":
                                            e.Graphics.DrawImage(Properties.Resources.十字螺丝刀0, e.Bounds);
                                            break;
                                        case "1":
                                            e.Graphics.DrawImage(Properties.Resources.十字螺丝刀1, e.Bounds);
                                            break;
                                        case "2":
                                            e.Graphics.DrawImage(Properties.Resources.十字螺丝刀2, e.Bounds);
                                            break;
                                        case "3":
                                            e.Graphics.DrawImage(Properties.Resources.十字螺丝刀3, e.Bounds);
                                            break;
                                    }
                                }
                                break;
                            }
                            if (toolName.Contains("锤"))
                            {
                                if (e.Text.Contains("离位"))
                                {
                                    e.Graphics.DrawImage(Properties.Resources.锤离位, e.Bounds);
                                }
                                else
                                {
                                    switch (e.Text.Split('|')[3])
                                    {
                                        case "0":
                                            e.Graphics.DrawImage(Properties.Resources.锤0, e.Bounds);
                                            break;
                                        case "1":
                                            e.Graphics.DrawImage(Properties.Resources.锤1, e.Bounds);
                                            break;
                                        case "2":
                                            e.Graphics.DrawImage(Properties.Resources.锤2, e.Bounds);
                                            break;
                                        case "3":
                                            e.Graphics.DrawImage(Properties.Resources.锤3, e.Bounds);
                                            break;
                                    }
                                }
                                break;
                            }
                            if (toolName.Contains("钳"))
                            {
                                if (e.Text.Contains("离位"))
                                {
                                    e.Graphics.DrawImage(Properties.Resources.钳离位, e.Bounds);
                                }
                                else
                                {
                                    switch (e.Text.Split('|')[3])
                                    {
                                        case "0":
                                            e.Graphics.DrawImage(Properties.Resources.钳0, e.Bounds);
                                            break;
                                        case "1":
                                            e.Graphics.DrawImage(Properties.Resources.钳1, e.Bounds);
                                            break;
                                        case "2":
                                            e.Graphics.DrawImage(Properties.Resources.钳2, e.Bounds);
                                            break;
                                        case "3":
                                            e.Graphics.DrawImage(Properties.Resources.钳3, e.Bounds);
                                            break;
                                    }
                                }
                                break;
                            }
                            if (e.Text.Contains("离位"))
                            {
                                e.Graphics.DrawImage(Properties.Resources.yellow, e.Bounds);
                            }
                            else
                            {
                                switch (e.Text.Split('|')[3])
                                {
                                    case "0":
                                        e.Graphics.DrawImage(Properties.Resources._0, e.Bounds);
                                        break;
                                    case "1":
                                        e.Graphics.DrawImage(Properties.Resources._1, e.Bounds);
                                        break;
                                    case "2":
                                        e.Graphics.DrawImage(Properties.Resources._2, e.Bounds);
                                        break;
                                    case "3":
                                        e.Graphics.DrawImage(Properties.Resources._3, e.Bounds);
                                        break;
                                }
                            }
                            break;
                    }

                    //if (e.Text.Contains("在位"))
                    //{
                    //    e.Graphics.DrawImage(Properties.Resources.Status3, e.Bounds);
                    //}
                    //else if (e.Text.Contains("离位"))
                    //{
                    //    e.Graphics.DrawImage(Properties.Resources.Status4, e.Bounds);
                    //}
                    //else
                    //{
                    //    if (_alertCtrl % 2 == 0)
                    //    {
                    //        e.Graphics.DrawImage(Properties.Resources.Status2, e.Bounds);
                    //    }
                    //    else
                    //    {
                    //        e.Graphics.DrawImage(Properties.Resources.Status4, e.Bounds);
                    //    }
                    //}

                    #endregion

                    string[] dpArray = e.Text.Split('|');
                    if (dpArray[0].Length >= 3) textBound.X -= 5;
                    e.Graphics.DrawString(dpArray[0], new Font("微软雅黑", 11, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
                    textBound.X = baseX + AppConfig.ToolStatusX;
                    textBound.Y = baseY + AppConfig.ToolStatusY;
                    e.Graphics.DrawString(dpArray[2], new Font("微软雅黑", 12, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
                    e.Handled = true;
                }
            }
        }

        private void cBottomGrid_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
        {
            //防止自带样式覆盖
            if (e.Col == 0)
            {
                e.DrawCell();
                e.Handled = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(e.Text))
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    string[] dpArray = e.Text.Split('|');
                    string[] existStatus = dpArray[1].Split('/');
                    if (existStatus[0].Equals(existStatus[1]))
                    {
                        e.Graphics.DrawImage(Properties.Resources.bstatus3, e.Bounds);
                    }
                    else
                    {
                        if (existStatus[0].Equals("0"))
                        {
                            e.Graphics.DrawImage(Properties.Resources.bstatus1, e.Bounds);
                        }
                        else
                        {
                            e.Graphics.DrawImage(Properties.Resources.bstatus2, e.Bounds);
                        }
                    }
                    Rectangle textBound = e.Bounds;
                    int baseX = textBound.X, baseY = textBound.Y;
                    textBound.X = baseX + 65;
                    textBound.Y = baseY + 12;
                    e.Graphics.DrawString(dpArray[0], new Font("微软雅黑", 12, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
                    textBound.X = baseX + 65;
                    textBound.Y = baseY + 102;
                    e.Graphics.DrawString(dpArray[1], new Font("微软雅黑", 11, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
                    e.Handled = true;
                }
            }
        }

        #endregion

        private void tmGridUpdater_Tick(object sender, EventArgs e)
        {
            if (DeviceLayer.CabinetDevice.IsInitDone())
            {
                var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
                RefreshUpperGrid(toolStatusList);
                RefreshBottomGrid();
            }
            else
            {
                tmGridUpdater.Interval = 3000;
            }
        }

        private void FormCabinetStatus_VisibleChanged(object sender, EventArgs e)
        {
            if (PanelMode)
            {
                panelBottom.Left = panelUpper.Left;
                panelBottom.Top = panelUpper.Top + panelUpper.Height + 5;
                panelBottom.Height = this.Height / 3;

                cHistoryGrid.Left = panelUpper.Left + panelUpper.Width + 5;
                cHistoryGrid.Height = this.Height - 100;
            }
        }


    }
}







