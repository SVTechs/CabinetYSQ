using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.RtVars;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using Utilities.DbHelper;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormDrawerStatus : Form
    {
        public bool SelectionMode = false;
        public int DrawerPosition = -1;

        private CellStyle _ctRed, _ctYellow, _ctGreen, _ctBlue, _ctGray, _ctSplit, _ctNormal;

        public FormDrawerStatus()
        {
            InitializeComponent();
        }

        private void tmStatusUpdater_Tick(object sender, EventArgs e)
        {
            RefreshTool();
        }

        private void FormDrawerStatus_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            InitGrid();
            RefreshTool();

            if (SelectionMode)
            {
                VarFormDrawerStatus.ToolId = "";
            }
        }

        private void FormDrawerStatus_Shown(object sender, EventArgs e)
        {
            if (DrawerPosition < 0)
            {
                MessageBox.Show("数据异常");
                Close();
            }
        }

        private void InitGrid()
        {
            cDrawerGrid.Cols.Count = 5;
            cDrawerGrid.Rows.Count = 1;
            cDrawerGrid.Cols[1].Caption = "Id";
            cDrawerGrid.Cols[1].Visible = false;
            cDrawerGrid.Cols[2].Caption = "工具名称";
            cDrawerGrid.Cols[2].Width = cDrawerGrid.Width / 3;
            cDrawerGrid.Cols[3].Caption = "工具规格";
            cDrawerGrid.Cols[3].Width = cDrawerGrid.Width / 3;
            cDrawerGrid.Cols[4].Caption = "工具状态";
            cDrawerGrid.Cols[4].Width = cDrawerGrid.Width / 4;

            _ctRed = cDrawerGrid.Styles.Add("CtRed");
            _ctRed.BackColor = Color.HotPink;
            _ctYellow = cDrawerGrid.Styles.Add("CtYellow");
            _ctYellow.BackColor = Color.Gold;
            _ctGreen = cDrawerGrid.Styles.Add("CtGreen");
            _ctGreen.BackColor = Color.FromArgb(207, 221, 238);
            _ctBlue = cDrawerGrid.Styles.Add("CtBlue");
            _ctBlue.BackColor = Color.SteelBlue;
            _ctGray = cDrawerGrid.Styles.Add("CtGray");
            _ctGray.BackColor = Color.Gray;
            _ctSplit = cDrawerGrid.Styles.Add("CtSplit");
            _ctSplit.BackColor = Color.FromArgb(207, 221, 238);
            _ctNormal = cDrawerGrid.Styles.Add("CtNormal");
            _ctNormal.BackColor = Color.FromArgb(225, 225, 225);

            for (int i = 1; i < cDrawerGrid.Cols.Count; i++)
            {
                cDrawerGrid.Cols[i].TextAlign = TextAlignEnum.LeftCenter;
            }
        }

        private void RefreshTool()
        {
            cDrawerGrid.BeginUpdate();
            cDrawerGrid.Rows.Count = 1;
            var cardList = UhfDevice.GetCardList(DrawerPosition - 1);
            IList toolList = BllToolInfo.SearchToolInfo(10, DrawerPosition, null, null, null);
            if (SqlDataHelper.IsDataValid(toolList))
            {
                cDrawerGrid.Rows.Count = toolList.Count + 1;
                for (int i = 0; i < toolList.Count; i++)
                {
                    ToolInfo ti = (ToolInfo)toolList[i];
                    string toolStatus = "";
                    if (ti.ToolStatus == 10)
                    {
                        toolStatus = "维修";
                        cDrawerGrid.SetCellStyle(i+1, 4, _ctRed);
                    }
                    else
                    {
                        var toolInfo = from ci in cardList where ci.ToolId == ti.Id select ci;
                        foreach (var info in toolInfo)
                        {
                            if (info.IsExist)
                            {
                                toolStatus = "在位";
                                cDrawerGrid.SetCellStyle(i + 1, 4, _ctGreen);
                            }
                            else
                            {
                                toolStatus = "离位";
                                cDrawerGrid.SetCellStyle(i + 1, 4, _ctGray);
                            }
                            break;
                        }
                    }
                    FillToHistoryGrid(i + 1, ti.Id, ti.ToolName, ti.ToolSpec, toolStatus);
                    cDrawerGrid.Rows[i + 1].Height = 30;
                }
            }
            cDrawerGrid.EndUpdate();
        }

        private void FillToHistoryGrid(int targetRow, params object[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                cDrawerGrid.Rows[targetRow][i + 1] = data[i];
            }
        }

        private void cDrawerGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (!SelectionMode) return;
            HitTestInfo ht = cDrawerGrid.HitTest(e.X, e.Y);
            if (ht.Row > 0)
            {
                VarFormDrawerStatus.ToolId = (string)cDrawerGrid.Rows[ht.Row][1];
                Close();
            }
        }
    }
}
