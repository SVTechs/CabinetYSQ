using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using NLog;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormToolReport : Form
    {
        private int _pageCount = 1;
        private int _reportToolColWidth, _toolRowHeight, _reportDrawerColWidth, _drawerRowHeight, _toolRowCount = 5;
        private bool _initDone;
        private int _alertCtrl;
        private IList<ToolInfo> lstToolInfos;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Dictionary<int, int> addRowPointDic = new Dictionary<int, int>();

        public FormToolReport()
        {
            InitializeComponent();
        }

        private void FormToolReport_Load(object sender, EventArgs e)
        {

            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            if (height == 1920 && width == 1080)
            {

                panelUpper.Size = new Size(872, 1217);
                panelUpper.BackgroundImageLayout = ImageLayout.Tile;
                cUpperGrid.Location = new Point(0, 60);
                cUpperGrid.Size = new Size(872, 1155);

                panelBottom.Location = new Point(3, 1181);
                panelBottom.Size = new Size(872, 450);
                panelBottom.BackgroundImageLayout = ImageLayout.Tile;
                cBottomGrid.Size = new Size(872, 388);
                cBottomGrid.Location = new Point(0, 60);
                _toolRowCount = 12;
                AppConfig.ReportDrawerCol = 1;
            }
            if (height == 768 && width == 1024)
            {

                panelUpper.Size = new Size(817, 556);
                //panelUpper.BackgroundImageLayout = ImageLayout.Tile;
                cUpperGrid.Location = new Point(0, 52);
                cUpperGrid.Size = new Size(812, 501);

                //panelBottom.Location = new Point(3, 1181);
                //panelBottom.Size = new Size(872, 450);
                //panelBottom.BackgroundImageLayout = ImageLayout.Tile;
                //cBottomGrid.Size = new Size(872, 388);
                //cBottomGrid.Location = new Point(0, 60);
                //_toolRowCount = 12;
                AppConfig.ReportDrawerCol = 1;
            }
            InitGrid();


            Thread alertThd = new Thread(AlertCtrl) { IsBackground = true };
            alertThd.Start();
            lstToolInfos = BllToolInfo.SearchToolInfo(-1,-1,"","","").Cast<ToolInfo>().ToList();
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
            // ReSharper disable once FunctionNeverReturns
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

        private void InitGrid()
        {
            GenGridLine();

            cUpperGrid.HighLight = HighLightEnum.Never;
            cUpperGrid.Width = _reportToolColWidth * AppConfig.ReportToolCol;

            cBottomGrid.HighLight = HighLightEnum.Never;
            cBottomGrid.Width = _reportDrawerColWidth * AppConfig.ReportDrawerCol;
        }

        private void GenGridLine()
        {
            //上部Grid设置
            _reportToolColWidth = cUpperGrid.Width / AppConfig.ReportToolCol;
            _toolRowHeight = cUpperGrid.Height / _toolRowCount;
            cUpperGrid.Cols[0].Visible = false;
            cUpperGrid.Cols.Count = AppConfig.ReportToolCol + 1;
            cUpperGrid.Rows[0].Visible = false;
            for (int i = 1; i <= AppConfig.ReportToolCol; i++)
            {
                cUpperGrid.Cols[i].Width = _reportToolColWidth;
                cUpperGrid.Cols[i].TextAlign = TextAlignEnum.CenterCenter;
            }
            for (int i = 1; i < cUpperGrid.Rows.Count; i++)
            {
                cUpperGrid.Rows[i].Height = _toolRowHeight;
            }

            //下部Grid设置
            int drawerRowCount = (int)Math.Ceiling((double)AppConfig.DrawerCount / AppConfig.ReportDrawerCol);
            if (drawerRowCount == 0) drawerRowCount = 1;
            _reportDrawerColWidth = cBottomGrid.Width / AppConfig.ReportDrawerCol;
            _drawerRowHeight = cBottomGrid.Height / drawerRowCount;
            cBottomGrid.Cols[0].Visible = false;
            cBottomGrid.Cols.Count = AppConfig.ReportDrawerCol + 1;
            cBottomGrid.Rows[0].Visible = false;
            for (int i = 1; i <= AppConfig.ReportDrawerCol; i++)
            {
                cBottomGrid.Cols[i].Width = _reportDrawerColWidth;
                cBottomGrid.Cols[i].TextAlign = TextAlignEnum.CenterCenter;
            }
            for (int i = 1; i < cBottomGrid.Rows.Count; i++)
            {
                cBottomGrid.Rows[i].Height = _drawerRowHeight;
            }
        }

        private void UpdateRepairStatus(int toolPositionType)
        {
            IList toolList = BllToolInfo.SearchToolInfo(toolPositionType, -1, null, null, null);
            for (int i = 0; i < toolList.Count; i++)
            {
                ToolInfo ti = (ToolInfo)toolList[i];
                if (ti.ToolStatus == 10)
                {
                    DeviceLayer.CabinetDevice.SetToolLedRepairStatus(int.Parse(ti.ToolCode) - 1, true);
                }
                else
                {
                    DeviceLayer.CabinetDevice.SetToolLedRepairStatus(int.Parse(ti.ToolCode) - 1, false);
                }
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
                UpdateRepairStatus(0);

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
                            gridContent += toolStatusList[i].Status == 0 ? "离位" : "在位";
                        }
                        ToolInfo ti = lstToolInfos.FirstOrDefault(x => x.ToolPosition == (i + 1));
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
                    if (d % AppConfig.ReportDrawerCol == 0 && d != 0)
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
                cBottomGrid.EndUpdate();
            }
        }

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

        private void FormToolReport_Shown(object sender, EventArgs e)
        {
            _initDone = true;
        }

        private void cUpperGrid_Click(object sender, EventArgs e)
        {

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
                float scale = e.Graphics.DpiX / 92;
                if (!string.IsNullOrEmpty(e.Text))
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    Rectangle textBound = e.Bounds;
                    int baseX = textBound.X, baseY = textBound.Y;
                    textBound.X = (int)(baseX + AppConfig.ReportToolCodeX * scale);
                    textBound.Y = (int)(baseY + AppConfig.ReportToolCodeY * scale);
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
                    if (dpArray[0].Length >= 3) textBound.X -= (int)(5 * scale);
                    e.Graphics.DrawString(dpArray[0], new Font("微软雅黑", 11, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
                    textBound.X = (int) (baseX);// + AppConfig.ReportToolStatusX);// * scale);
                    textBound.Y = (int)(baseY + AppConfig.ReportToolStatusY * scale);
                    e.Graphics.DrawString(dpArray[2], new Font("微软雅黑", 11, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
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
                    textBound.X = baseX + 88;
                    textBound.Y = baseY + 14;
                    e.Graphics.DrawString(dpArray[0], new Font("微软雅黑", 12, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
                    textBound.X = baseX + 85;
                    textBound.Y = baseY + 115;
                    e.Graphics.DrawString(dpArray[1], new Font("微软雅黑", 11, FontStyle.Regular), new SolidBrush(Color.Black), textBound);
                    e.Handled = true;
                }
            }
        }

        #endregion

        private void cUpperGrid_MouseClick(object sender, MouseEventArgs e)
        {
            return;
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HitTestInfo ht = cUpperGrid.HitTest(e.X, e.Y);
            if (ht.Row > 0)
            {
                string gridContent = cUpperGrid.Rows[ht.Row][ht.Column]?.ToString();
                if (cUpperGrid.Rows[ht.Row][ht.Column] == null ||
                    string.IsNullOrWhiteSpace(gridContent))
                {
                    return;
                }
                int selIndex = int.Parse(gridContent.Split('|')[0]);
                if (!gridContent.Contains("维修"))
                {
                    ToolInfo ti = BllToolInfo.GetToolInfo(selIndex.ToString());
                    if (ti == null)
                    {
                        MessageBox.Show("没有找到该位置工具信息");
                        return;
                    }
                    string alertMsg = $"您确定要报修{selIndex}号箱内工具吗?";
                    DialogResult dr = MessageBox.Show(alertMsg, "提示", MessageBoxButtons.OKCancel);
                    if (dr != DialogResult.OK) return;
                    if (selIndex <= AppConfig.ToolCount)
                    {
                        int result = BllToolRepairRequest.AddToolRepairRequest(AppRt.CurUser.UserName, ti.ServerIdent, ti.ToolName,
                            AppConfig.CabinetPosition, AppConfig.CabinetName);
                        if (result >= 0)
                        {
                            BllToolInfo.SetToolStatus(selIndex.ToString(), 10, AppRt.CurUser.FullName);
                            DeviceLayer.CabinetDevice.SetToolLedRepairStatus(selIndex - 1, true);
                            MessageBox.Show("报修成功");
                        }
                        else
                        {
                            MessageBox.Show("报修失败");
                        }
                    }
                    else
                    {
                        //模拟数据
                        MessageBox.Show("报修成功");
                    }
                }
                else
                {
                    string alertMsg = $"您确定{selIndex}号箱内工具已维修完毕吗?";
                    DialogResult dr = MessageBox.Show(alertMsg, "提示", MessageBoxButtons.OKCancel);
                    if (dr != DialogResult.OK) return;
                    if (selIndex <= AppConfig.ToolCount)
                    {
                        ToolInfo ti = BllToolInfo.GetToolInfo(selIndex.ToString());
                        if (ti == null)
                        {
                            MessageBox.Show("没有找到该位置工具信息");
                            return;
                        }
                        BllToolInfo.SetToolStatus(selIndex.ToString(), 0, AppRt.CurUser.FullName);
                        DeviceLayer.CabinetDevice.SetToolLedRepairStatus(selIndex - 1, false);
                        MessageBox.Show("提交成功");
                    }
                    else
                    {
                        //模拟数据
                        MessageBox.Show("提交成功");
                    }
                }
            }
        }

        private void cBottomGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (AppRt.CurUser == null)
            {
                MessageBox.Show("请先登录再进行操作!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HitTestInfo ht = cBottomGrid.HitTest(e.X, e.Y);
            if (ht.Row > 0)
            {
                if (cBottomGrid.Rows[ht.Row][ht.Column] == null ||
                    string.IsNullOrWhiteSpace(cBottomGrid.Rows[ht.Row][ht.Column].ToString()))
                {
                    return;
                }
                int selIndex = ht.Column;
                FormDrawerStatus dsForm = new FormDrawerStatus { DrawerPosition = selIndex, SelectionMode = true };
                dsForm.ShowDialog();
                if (!string.IsNullOrEmpty(VarFormDrawerStatus.ToolId))
                {
                    ToolInfo ti = BllToolInfo.GetToolInfoById(VarFormDrawerStatus.ToolId);
                    if (ti.ToolStatus == 0)
                    {
                        string alertMsg = $"您确定要报修工具 {ti.ToolName} 吗?";
                        DialogResult dr = MessageBox.Show(alertMsg, "提示", MessageBoxButtons.OKCancel);
                        if (dr != DialogResult.OK) return;
                        int result = BllToolRepairRequest.AddToolRepairRequest(AppRt.CurUser.UserName, ti.ServerIdent, ti.ToolName,
                            AppConfig.CabinetPosition, AppConfig.CabinetName);
                        if (result >= 0)
                        {
                            BllToolInfo.SetToolStatusById(ti.Id, 10, AppRt.CurUser.FullName);
                            MessageBox.Show("报修成功");
                        }
                        else
                        {
                            MessageBox.Show("报修失败");
                        }
                    }
                    else if (ti.ToolStatus == 10)
                    {
                        string alertMsg = $"您确定工具 {ti.ToolName} 已维修完毕吗?";
                        DialogResult dr = MessageBox.Show(alertMsg, "提示", MessageBoxButtons.OKCancel);
                        if (dr != DialogResult.OK) return;
                        var toolStatusList = DeviceLayer.CabinetDevice.GetToolStatus();
                        BllToolInfo.SetToolStatusById(ti.Id, toolStatusList[ti.ToolPosition - 1].Status, AppRt.CurUser.FullName);
                        MessageBox.Show("提交成功");
                    }
                }
            }
        }
    }
}