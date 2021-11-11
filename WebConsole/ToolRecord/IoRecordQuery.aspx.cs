using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using Domain.ServerMain.Domain;
using Ext.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Utilities.DbHelper;
using WebConsole.Bll;
using WebConsole.Config;

namespace ToolRecord
{
    public partial class ToolRecord_IoRecordQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                Session["TimeStart"] = "";
                Session["TimeEnd"] = "";
                Session["UserName"] = "";
                Session["ToolStatus"] = "";
                Session["Cabinet"] = "";
                Session["WorkerId"] = "";
                if (!string.IsNullOrEmpty(Request.Params["workId"]))
                {
                    string workId = Request.Params["workId"];
                    Session["WorkerId"] = workId;
                    UserInfo ui = BllUserInfo.GetUserInfo(workId);
                    lblUser.Text = ui.FullName;
                    lblUser.Visible = true;
                    btnReturn.Visible = true;
                    btnIoRate.Visible = false;
                }
                else
                {
                    lblUser.Text = "";
                    lblUser.Visible = false;
                    btnReturn.Visible = false;
                    btnIoRate.Visible = true;
                }


                IList<CabinetInfo> cabinetList = BllCabinetInfo.SearchCabinetInfo();
                if (SqlDataHelper.IsDataValid(cabinetList))
                {
                    cbCabinet.Items.Add(new ListItem("全部", ""));
                    for (int i = 0; i < cabinetList.Count; i++)
                    {
                        ListItem li = new ListItem
                        {
                            Text = cabinetList[i].CabinetAlias,
                            Value = cabinetList[i].CabinetName
                        };
                        cbCabinet.Items.Add(li);
                    }
                }
                cbToolStatus.Items.Add(new ListItem("全部", -1));
                cbToolStatus.Items.Add(new ListItem("已还", 20));
                cbToolStatus.Items.Add(new ListItem("未还", 0));

                rDay.Checked = true;
            }
        }

        [DirectMethod]
        public void SaveInput()
        {
            string timeStart = dtTimeStart.Text ?? "", timeEnd = dtTimeEnd.Text ?? "";
            if (timeStart.StartsWith("000")) timeStart = "";
            if (timeEnd.StartsWith("000")) timeEnd = "";
            Session["TimeStart"] = timeStart;
            Session["TimeEnd"] = timeEnd;
            Session["UserName"] = tbSearchUserName.Text;
            Session["ToolStatus"] = cbToolStatus.Value;
            Session["Cabinet"] = cbCabinet.Value.ToString() == "全部" ? "" : cbCabinet.Value;
            int userCount = 0;
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                //Session["WorkerId"] = "";
            }
            else
            {
                IList<UserInfo> lstUi = BllUserInfo.SearchUser(Session["UserName"].ToString(), "", 0, 1);
                UserInfo ui = new UserInfo();
                if (lstUi.Count > 0)
                {
                    ui = lstUi[0];
                    Session["WorkerId"] = ui.Id;
                }
            }
        }

        protected void IoGridData_OnReadData_Refresh(object sender, StoreReadDataEventArgs e)
        {
            string timeStart = (Session["TimeStart"] ?? "").ToString(), timeEnd = (Session["TimeEnd"] ?? "").ToString();
            if (string.IsNullOrEmpty(timeStart)) timeStart = Env.MinTime.ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(timeEnd)) timeEnd = Env.MaxTime.ToString("yyyy-MM-dd");
            string workId = (Session["WorkerId"] ?? "").ToString();
            string cabinetName = (Session["Cabinet"] ?? "").ToString();
            int tmpStatus;
            int toolStatus = int.TryParse(Session["ToolStatus"].ToString(), out tmpStatus) ? int.Parse(Session["ToolStatus"].ToString()) : -1;
            e.Total = BllBorrowRecord.GetBorrowRecordCount(DateTime.Parse(timeStart), DateTime.Parse(timeEnd),
                workId, cabinetName, toolStatus);
            IList<BorrowRecord> recordList = BllBorrowRecord.SearchBorrowRecord(DateTime.Parse(timeStart),
                DateTime.Parse(timeEnd), workId, cabinetName,  toolStatus,
            e.Start, e.Limit);
            IList<ReturnRecord> rrList = BllReturnRecord.SearchReturnRecord(DateTime.Parse(timeStart), DateTime.Parse(timeEnd));
            if (SqlDataHelper.IsDataValid(recordList))
            {
                List<object[]> listData = GetBorrowData(recordList, rrList);
                ((Store) sender).DataSource = listData;
            }
        }

        protected void btnExport_OnDirectClick(object sender, DirectEventArgs e)
        {
            string timeStart = (Session["TimeStart"] ?? "").ToString(), timeEnd = (Session["TimeEnd"] ?? "").ToString();
            if (string.IsNullOrEmpty(timeStart)) timeStart = Env.MinTime.ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(timeEnd)) timeEnd = Env.MaxTime.ToString("yyyy-MM-dd");
            string workId = (Session["WorkerId"] ?? "").ToString();
            string cabinetName = (Session["Cabinet"] ?? "").ToString();
            int tmpStatus;
            int toolStatus = int.TryParse(Session["ToolStatus"].ToString(), out tmpStatus) ? int.Parse(Session["ToolStatus"].ToString()) : -1;
            IList<BorrowRecord> recordList = BllBorrowRecord.SearchBorrowRecord(DateTime.Parse(timeStart),
                DateTime.Parse(timeEnd), workId, cabinetName, toolStatus,  0, -1);
            IList<ReturnRecord> rrList = BllReturnRecord.SearchReturnRecord(DateTime.Parse(timeStart), DateTime.Parse(timeEnd));
            if (SqlDataHelper.IsDataValid(recordList))
            {
                List<object[]> listData = GetBorrowData(recordList, rrList);
                string fileBase = MapPath("/") + "ExcelFile";
                if (!Directory.Exists(fileBase)) Directory.CreateDirectory(fileBase);
                string filePath = fileBase + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                Export2003(listData, filePath, "test");
            }
        }

        private List<object[]> GetBorrowData(IList<BorrowRecord> recordList, IList<ReturnRecord> rrList)
        {
            Hashtable aliasTable = new Hashtable();
            List<object[]> listData = new List<object[]>();
            for (int i = 0; i < recordList.Count; i++)
            {
                string returnTime = "";
                if (recordList[i].Status != 0)
                {
                    returnTime = recordList[i].ReturnTime != null
                        ? recordList[i].ReturnTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "";
                }
                string originName = recordList[i].DataOwner;
                if (aliasTable[originName] == null)
                {
                    aliasTable[originName] = BllCabinetInfo.GetCabinetAlias(originName);
                }
                ReturnRecord rr = rrList.FirstOrDefault(x => x.BorrowRecord == recordList[i].Id);
                listData.Add(new object[]
                {
                    recordList[i].Id,
                    recordList[i].EventTime != null
                        ? recordList[i].EventTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "",
                    recordList[i].ToolPosition, recordList[i].ToolName,
                    recordList[i].WorkerName, aliasTable[originName], recordList[i].Status < 20 ? "未还" : "已还",
                    returnTime,
                    rr == null ? "" : rr.WorkerName,
                });
            }
            return listData;
        }

        public void Export2003(List<object[]> lstData, string filePath, string fileName)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet(fileName);
            IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("借取时间");
            row.CreateCell(1).SetCellValue("工具编号");
            row.CreateCell(2).SetCellValue("工具名称");
            row.CreateCell(3).SetCellValue("借取人");
            row.CreateCell(4).SetCellValue("工具柜");
            row.CreateCell(5).SetCellValue("归还状态");
            row.CreateCell(6).SetCellValue("归还时间");
            row.CreateCell(7).SetCellValue("归还人");
            for (int i = 0; i < lstData.Count; i++)
            {
                IRow row2 = sheet.CreateRow(i + 1);
                object[] o = lstData[i];
                for (int j = 0; j < 8; j++)
                {
                    if (o[j + 1] == null) continue;
                    row2.CreateCell(j).SetCellValue(o[j + 1].ToString());
                }
            }
            // 写入到客户端  
            using (MemoryStream ms = new MemoryStream())
            {
                book.Write(ms);
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
                book = null;
            }
            Response.Redirect("~/ExcelFile/" + Path.GetFileName(filePath));
        }

        protected void btnIoRate_OnDirectClick(object sender, DirectEventArgs e)
        {
            IoRateWindow.Hidden = false;
        }

        protected void IoRateStore_OnReadData(object sender, StoreReadDataEventArgs e)
        {

        }

        protected void btnIoRateQuery_OnDirectClick(object sender, DirectEventArgs e)
        {
            if (dfStartDate.IsEmpty || dfEndDate.IsEmpty)
            {
                X.MessageBox.Alert("", "请选择开始日期及结束日期");
                return;
            }

            DateTime startDate = dfStartDate.SelectedDate;
            DateTime endDate = dfEndDate.SelectedDate;
            string workId = (Session["WorkerId"] ?? "").ToString();
            string cabinetName = (Session["Cabinet"] ?? "").ToString();
            int tmpStatus;
            int toolStatus = int.TryParse(Session["ToolStatus"].ToString(), out tmpStatus) ? int.Parse(Session["ToolStatus"].ToString()) : -1;
            IList<BorrowRecord> recordList = BllBorrowRecord.SearchBorrowRecord(startDate,
                endDate, "", cabinetName, toolStatus, 0, -1);
            BindChartData(recordList);
        }

        private void BindChartData(IList<BorrowRecord> recordList)
        {
            IList<DataItem> rslt = new List<DataItem>();
            for (int i = 0; i < recordList.Count; i++)
            {
                BorrowRecord br = recordList[i];
                DataItem di = rslt.FirstOrDefault(x => x.ToolName == br.ToolName);
                if (di == null)
                {
                    rslt.Add(new DataItem()
                    {
                        ToolName = br.ToolName,
                        Rate = 1,
                    });
                }
                else
                {
                    di.Rate += 1;
                }
            }
            IoRateStore.DataSource = rslt.OrderByDescending(x => x.Rate).Take(10);
            IoRateStore.DataBind();
        }

        protected void rgDate_OnDirectChange(object sender, DirectEventArgs e)
        {
            RadioGroup rg = sender as RadioGroup;
            IList<BorrowRecord> recordList = new List<BorrowRecord>();
            string workId = (Session["WorkerId"] ?? "").ToString();
            string cabinetName = (Session["Cabinet"] ?? "").ToString();
            int tmpStatus;
            int toolStatus = int.TryParse(Session["ToolStatus"].ToString(), out tmpStatus) ? int.Parse(Session["ToolStatus"].ToString()) : -1;
            DateTime startDate = Env.MinTime;
            DateTime endDate = Env.MaxTime;
            string defaultType = rg.CheckedItems[0].InputValue;
            switch (defaultType)
            {
                case "Day":
                    startDate = DateTime.Today.AddDays(-1);
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "Month":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "Year":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    endDate = new DateTime(DateTime.Today.Year, 12, 31);
                    break;
            }
            recordList = BllBorrowRecord.SearchBorrowRecord(startDate,
                endDate, "", cabinetName, toolStatus, 0, -1);
            BindChartData(recordList);
        }
    }

    public class DataItem
    {
        public string ToolName { get; set; }

        public int Rate { get; set; }


    }

}