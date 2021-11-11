using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using CabinetMgr.RtDelegate;
using Domain.Main.Domain;
using Utilities.DbHelper;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormBorrowRecord : Form
    {
        private int _historyRow = 30;

        private CellStyle _ctRed, _ctYellow, _ctGreen, _ctBlue, _ctBlack, _ctSplit, _ctNormal,
            _ctConfirm, _ctComment;
        private readonly string _offDutyTime;

        public FormBorrowRecord()
        {
            InitializeComponent();

            _offDutyTime = AppConfig.OffDutyTime;

            InitGrid();
            RefreshHistoryGrid();

            DelegateReturnRecord.RefreshHistoryGrid = RefreshHistoryGrid;
        }

        private void FormReturnRecord_Load(object sender, EventArgs e)
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            if (height == 1920 && width == 1080)
            {
                Size = new Size(868, 1583);
                panel1.Size = new Size(854, 1569);
                cHistoryGrid.Size = new Size(850, 1561);
                //Size = new Size(908, 1766);
                //Height = 1766;
            }
            if (height == 768 && width == 1024)
            {
                Size = new Size(815, 517);
                cHistoryGrid.Size = new Size(792, 495);
                //Size = new Size(908, 1766);
                //Height = 1766;
            }
        }

        private void InitGrid()
        {
            GenGridLine();

            _ctRed = cHistoryGrid.Styles.Add("CtRed");
            _ctRed.BackColor = Color.HotPink;
            _ctYellow = cHistoryGrid.Styles.Add("CtYellow");
            _ctYellow.BackColor = Color.Gold;
            _ctGreen = cHistoryGrid.Styles.Add("CtGreen");
            _ctGreen.BackColor = Color.FromArgb(207, 221, 238);
            _ctBlue = cHistoryGrid.Styles.Add("CtBlue");
            _ctBlue.BackColor = Color.SteelBlue;
            _ctBlack = cHistoryGrid.Styles.Add("CtBlack");
            _ctBlack.BackColor = Color.DarkGray;
            _ctSplit = cHistoryGrid.Styles.Add("CtSplit");
            _ctSplit.BackColor = Color.FromArgb(207, 221, 238);
            _ctNormal = cHistoryGrid.Styles.Add("CtNormal");
            _ctNormal.BackColor = Color.FromArgb(225, 225, 225);

            _ctConfirm = cHistoryGrid.Styles.Add("CtConfirm");
            _ctConfirm.BackgroundImage = Properties.Resources.confirm;
            _ctConfirm.BackgroundImageLayout = ImageAlignEnum.Stretch;
            _ctComment = cHistoryGrid.Styles.Add("CtComment");
            _ctComment.BackgroundImage = Properties.Resources.comment;
            _ctComment.BackgroundImageLayout = ImageAlignEnum.Stretch;
        }

        private void GenGridLine()
        {
            //历史Grid设置
            cHistoryGrid.Cols.Count = 8;
            cHistoryGrid.Cols[1].Caption = "Id";
            cHistoryGrid.Cols[1].Visible = false;
            cHistoryGrid.Cols[2].Caption = "领取时间";
            cHistoryGrid.Cols[2].Width = cHistoryGrid.Width / 6;
            cHistoryGrid.Cols[3].Caption = "工具编号";
            cHistoryGrid.Cols[3].Width = cHistoryGrid.Width / 7;
            cHistoryGrid.Cols[3].TextAlign = TextAlignEnum.LeftCenter;
            cHistoryGrid.Cols[4].Caption = "工具名称";
            cHistoryGrid.Cols[4].Width = cHistoryGrid.Width / 6;
            cHistoryGrid.Cols[5].Caption = "领取人";
            cHistoryGrid.Cols[5].Width = cHistoryGrid.Width / 7;
            cHistoryGrid.Cols[6].Caption = "归还状态";
            cHistoryGrid.Cols[6].Width = cHistoryGrid.Width / 7;
            cHistoryGrid.Cols[7].Caption = "归还时间";
            cHistoryGrid.Cols[7].Width = cHistoryGrid.Width / 6;
            cHistoryGrid.Rows.Count = _historyRow + 1;
            for (int i = 1; i < cHistoryGrid.Cols.Count; i++)
            {
                cHistoryGrid.Cols[i].TextAlign = TextAlignEnum.LeftCenter;
            }
            for (int i = 1; i < _historyRow; i++)
            {
                cHistoryGrid.Rows[i][0] = i.ToString();
                cHistoryGrid.Rows[i].Height = 35;
            }
        }

        private void RefreshHistoryGrid()
        {
            if (cHistoryGrid.InvokeRequired)
            {
                DelegateReturnRecord.RefreshHistoryGridDelegate d = RefreshHistoryGrid;
                cHistoryGrid.Invoke(d);
            }
            else
            {
                IList borrowList = BllBorrowRecord.SearchBorrowRecord(DateTime.Now.AddDays(-1), DateTime.Now);
                if (SqlDataHelper.IsDataValid(borrowList))
                {
                    bool expireCheck = DateTime.Now >= DateTime.Parse(_offDutyTime);
                    cHistoryGrid.BeginUpdate();
                    int i;
                    for (i = 0; i < borrowList.Count && i < _historyRow - 1; i++)
                    {
                        BorrowRecord br = (BorrowRecord) borrowList[i];
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
                        FillToHistoryGrid(i + 1, br.Id, br.EventTime.ToString("MM-dd HH:mm"),
                            br.HardwareId.ToString("D2"),
                            br.ToolName, br.WorkerName, FormatHistoryStatus(br.Status), comment);
                        for (int j = 1; j < 7; j++)
                        {
                            cHistoryGrid.SetCellStyle(i + 1, j, _ctNormal);
                        }
                        if (expireCheck)
                        {
                            //工具超时未还情况处理
                            if (br.Status < 20)
                            {
                                if (br.Status == 0)
                                {
                                    //加红色标记
                                    for (int j = 1; j < 7; j++)
                                    {
                                        cHistoryGrid.SetCellStyle(i + 1, j, _ctRed);
                                    }
                                    //显示确定按钮
                                    cHistoryGrid.SetCellStyle(i + 1, 7, _ctConfirm);
                                }
                                else if (br.Status == 10)
                                {
                                    //显示备注按钮
                                    cHistoryGrid.SetCellStyle(i + 1, 7, _ctComment);
                                }
                                else
                                {
                                    //清空显示
                                    cHistoryGrid.SetCellStyle(i + 1, 7, _ctNormal);
                                }
                            }
                            else
                            {
                                cHistoryGrid.SetCellStyle(i + 1, 7, _ctNormal);
                            }
                        }
                        else
                        {
                            cHistoryGrid.SetCellStyle(i + 1, 7, _ctNormal);
                        }
                    }
                    for (; i < _historyRow; i++)
                    {
                        FillToHistoryGrid(i + 1, -1, "", "", "", "", "", "");
                        for (int j = 1; j < 7; j++)
                        {
                            cHistoryGrid.SetCellStyle(i + 1, j, _ctNormal);
                        }
                    }
                    cHistoryGrid.EndUpdate();
                }
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
            for (int i = 0; i < data.Length; i++)
            {
                cHistoryGrid.Rows[targetRow][i + 1] = data[i];
            }
        }

        private void cHistoryGrid_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestInfo ht = cHistoryGrid.HitTest(e.X, e.Y);
            if (ht.Row > 0 && ht.Column == 7)
            {
                if (Equals(cHistoryGrid.GetCellStyle(ht.Row, 7), _ctConfirm))
                {
                    //确认按钮
                    string alert = string.Format("您确定暂不归还 [{0}] 借取的 [{1}] 吗?",
                        cHistoryGrid.Rows[ht.Row][5], cHistoryGrid.Rows[ht.Row][4]);
                    DialogResult dr = MessageBox.Show(alert, "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        string brId = (string)cHistoryGrid.Rows[ht.Row][1];
                        int result = BllBorrowRecord.SetAsConfirmed(brId);
                        if (result <= 0)
                        {
                            MessageBox.Show("确认失败");
                        }
                    }
                    RefreshHistoryGrid();
                }
                else if (Equals(cHistoryGrid.GetCellStyle(ht.Row, 7), _ctComment))
                {
                    //备注按钮
                    FormInputComment icForm = new FormInputComment();
                    icForm.RecordId = (string)cHistoryGrid.Rows[ht.Row][1];
                    icForm.ShowDialog();
                    RefreshHistoryGrid();
                }
            }
        }
    }
}
