using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using Domain.ServerMain.Domain;
using Domain.ZWStock.Domain;
using Ext.Net;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Utilities.DbHelper;
using WebConsole.Bll;
using WebConsole.Config;
using ListItem = Ext.Net.ListItem;

namespace TorqueWrenchCheck
{
    public partial class TorqueWrenchCheck_WrenchInfo : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                Session["SearchCode"] = "";
                Session["SearchSpec"] = "";
                Session["SearchCabinet"] = "";
                Session["WrenchId"] = "";
                //Session["ToolId"] = "";

                var ulli = new ListItem
                {
                    Text = "全部",
                    Value = ""
                };
                cbCabinet.Items.Add(ulli);
                //cbEditWindowCabinet.Items.Add(ulli);
                IList<CabinetInfo> cabinetList = BllCabinetInfo.SearchCabinetInfo();
                if (SqlDataHelper.IsDataValid(cabinetList))
                {
                    for (int i = 0; i < cabinetList.Count; i++)
                    {
                        var li = new ListItem
                        {
                            Text = cabinetList[i].CabinetAlias,
                            Value = cabinetList[i].CabinetName
                        };
                        cbCabinet.Items.Add(li);
                        cbEditWindowCabinet.Items.Add(li);
                    }
                }
            }
        }

        [DirectMethod]
        public void SaveInput()
        {
            //Session["SearchName"] = tbSearchName.Text;
            Session["SearchCode"] = tbSearchCode.Text;
            Session["SearchSpec"] = tbSearchSpec.Text;
            Session["SearchCabinet"] = cbCabinet.Text == "全部" ? "" : cbCabinet.Text;
        }

        protected void WrenchGridData_OnReadData(object sender, StoreReadDataEventArgs e)
        {
            string wrenchCode = (Session["SearchCode"] ?? "").ToString();
            string wrenchSpec = (Session["SearchSpec"] ?? "").ToString();
            string dataOwner = (Session["SearchCabinet"] ?? "").ToString();
            Exception ex;
            e.Total = BllWrenchInfo.GetWrenchInfoCount(wrenchCode, wrenchSpec, dataOwner, out ex);
            IList<WrenchInfo> recordList = BllWrenchInfo.SearchWrenchInfo(wrenchCode, wrenchSpec, dataOwner, e.Start, e.Limit, out ex);
            ((Store)sender).DataSource = recordList;
        }

        [DirectMethod]
        public void ShowWrenchCheckRecord(string wrenchId)
        {
            Session["WrenchId"] = wrenchId;
            CheckGridData.LoadPage(1);
        }

        protected void CheckGridData_OnReadData(object sender, StoreReadDataEventArgs e)
        {
            string wrenchId = Session["WrenchId"].ToString();
            Exception ex;
            e.Total = BllWrenchCheckRecord.GetWrenchCheckRecordCount(wrenchId, out ex);
            IList<WrenchCheckRecord> recordList = BllWrenchCheckRecord.SearchWrenchCheckRecord(wrenchId, e.Start, e.Limit, out ex);
            ((Store)sender).DataSource = recordList;
        }

        [DirectMethod]
        public void DeleteWrenchInfo(string wrenchId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "378DFAB7-0E56-4A2D-A12C-3F4EB4774EBC"))
            {
                X.Msg.Alert("提示", "权限不足").Show();
                return;
            }
            Exception ex;
            int result = BllWrenchInfo.DeleteWrenchInfo(wrenchId, out ex);
            if (result > 0)
            {
                X.Msg.Alert("提示", "删除成功").Show();
            }
            else
            {
                X.Msg.Alert("提示", string.Format("删除失败，原因为{0}", ex.Message)).Show();
            }
        }

        [DirectMethod]
        public void FillEdtInfo(string wrenchId)
        {
            Exception ex;
            WrenchInfo wi = BllWrenchInfo.GetWrenchInfo(wrenchId, out ex);
            if (wi != null)
            {
                tbWrenchId.Text = wi.Id;
                tbWrenchName.Text = wi.WrenchName;
                tbWrenchCode.Text = wi.WrenchCode;
                tbWrenchSpec.Text = wi.WrenchSpec;
                tbStandardRange.Text = wi.StandardRange;
                nbWrenchPosition.Value = wi.WrenchPosition;
                cbEditWindowCabinet.Select(wi.DataOwner);
                nbCheckInterval.Text = wi.CheckInterval.ToString();
                cbCheckIntervalType.Select(wi.CheckIntervalType);
            }

            EditWindow.Title = "编辑信息";
            EditWindow.Hidden = false;
        }

        protected void btnAddWrenchInfo_OnDirectClick(object sender, DirectEventArgs e)
        {
            tbWrenchId.Text = "";
            tbWrenchName.Text = "";
            tbWrenchCode.Text = "";
            tbWrenchSpec.Text = "";
            tbStandardRange.Text = "";
            nbWrenchPosition.Value = 0;
            cbEditWindowCabinet.Select(-1);
            nbCheckInterval.Value = 0;
            cbCheckIntervalType.Select(-1);

            EditWindow.Title = "添加信息";
            EditWindow.Hidden = false;
        }

        [DirectMethod]
        public void OnAddWrenchClick()
        {
            int tmp;
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "B48222A8-186E-45E4-AEC3-D40BC5BC9A09"))
            {
                X.Msg.Alert("提示", "权限不足").Show();
                return;
            }
            if (tbWrenchName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入扳手名称").Show();
                return;
            }
            if (tbWrenchCode.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请选择扳手编号").Show();
                return;
            }
            if (nbWrenchPosition.Text.Length <= 0)
            {
                X.Msg.Alert("提示", "请填写数字扳手位置").Show();
                return;
            }
            if (cbEditWindowCabinet.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请选择扳手所在柜子").Show();
                return;
            }
            if (nbCheckInterval.Text.Length > 0)
            {
                if (cbCheckIntervalType.Value.ToString().Length <= 0)
                {
                    X.Msg.Alert("提示", "请填写正确的校验间隔类型").Show();
                    return;
                }
            }
            string wrenchName = tbWrenchName.Text;
            string wrenchCode = tbWrenchCode.Text;
            string wrenchSpec = tbWrenchSpec.Text;
            string standardRange = tbStandardRange.Text;
            int wrenchPosition = int.Parse(nbWrenchPosition.Value.ToString());
            string dataOwner = cbEditWindowCabinet.Value.ToString();
            int checkInterval = int.Parse(nbCheckInterval.Value.ToString());
            string checkIntervalType = cbCheckIntervalType.Value.ToString();
            Exception ex;
            int result = BllWrenchInfo.AddWrenchInfo(wrenchName, wrenchCode, wrenchSpec, standardRange, wrenchPosition, 
                checkInterval, checkIntervalType, dataOwner, out ex);
            if (result < 0)
            {
                X.Msg.Alert("提示", string.Format("添加失败,原因为{0}", ex.Message)).Show();
                return;
            }
            X.Msg.Alert("提示", "添加成功").Show();
            EditWindow.Hidden = true;
        }

        [DirectMethod]
        public void OnEdtWrenchClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "AA0BEBFD-A409-485A-955C-10005CA897A3"))
            {
                X.Msg.Alert("提示", "权限不足").Show();
                return;
            }
            if (tbWrenchName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入扳手名称").Show();
                return;
            }
            if (tbWrenchCode.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请选择扳手编号").Show();
                return;
            }
            if (nbWrenchPosition.Text.Length <= 0)
            {
                X.Msg.Alert("提示", "请填写数字扳手位置").Show();
                return;
            }
            if (cbEditWindowCabinet.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请选择扳手所在柜子").Show();
                return;
            }
            if (nbCheckInterval.Text.Length > 0)
            {
                if (cbCheckIntervalType.Value.ToString().Length <= 0)
                {
                    X.Msg.Alert("提示", "请填写正确的校验间隔类型").Show();
                    return;
                }
            }

            string id = tbWrenchId.Text;
            string wrenchName = tbWrenchName.Text;
            string wrenchCode = tbWrenchCode.Text;
            string wrenchSpec = tbWrenchSpec.Text;
            string standardRange = tbStandardRange.Text;
            int wrenchPosition = int.Parse(nbWrenchPosition.Value.ToString());
            string dataOwner = cbEditWindowCabinet.Value.ToString();
            int checkInterval = int.Parse(nbCheckInterval.Value.ToString());
            string checkIntervalType = cbCheckIntervalType.Value.ToString();
            Exception ex;

            int result = BllWrenchInfo.ModifyWrenchInfo(id, wrenchName, wrenchCode, wrenchSpec, standardRange, wrenchPosition, checkInterval, checkIntervalType, dataOwner, out ex);
            if (result < 0)
            {
                X.Msg.Alert("提示", string.Format("修改失败,原因为{0}", ex.Message)).Show();
                return;
            }
            X.Msg.Alert("提示", "修改成功").Show();
        }


        [DirectMethod]
        public string ShowWrenchCheckPdf(string dataId)
        {
            Exception ex;
            WrenchCheckRecord wcr = BllWrenchCheckRecord.GetWrenchCheckRecord(dataId, out ex);
            return wcr.PdfFile;
        }

        [DirectMethod]
        public void DeleteWrenchCheckPdf(string dataId)
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "2241CA44-CF43-423C-9C7A-E3B713E6D2E2"))
            {
                X.Msg.Alert("提示", "权限不足").Show();
                return;
            }
            Exception ex;
            int result = BllWrenchCheckRecord.DeleteWrenchCheckRecord(dataId, out ex);
            if (result < 0)
            {
                X.Msg.Alert("提示", string.Format("删除失败,原因为{0}", ex.Message)).Show();
                return;
            }
            X.Msg.Alert("提示", "删除成功").Show();
            CheckGridData.LoadPage(1);
        }


        [DirectMethod]
        public void FileUpLoad()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "468071E1-82FD-432B-A8C5-0CC303C45B90"))
            {
                X.Msg.Alert("提示", "权限不足").Show();
                return;
            }
            if (!fuFile.HasFile) return;
            Exception ex;
            string fileName = fuFile.PostedFile.FileName;
            string caseFile = "";
            string strFileName = System.IO.Path.GetFileName(fileName);
            string basePath = "../WrenchCheckFile/";
            string url = AbsoluteWebRoot.AbsoluteUri;
            //if (BllWrenchInfo.SearchWrenchInfoFile(strFileName, out ex).Count > 0 )
            //{
            //    X.Msg.Alert("提示", "该文件已上传").Show();
            //}
            bool saveOnly = false;
            if (fuFile.HasFile)
            {
                fuFile.PostedFile.SaveAs(Server.MapPath(basePath) + strFileName);
                string localName = DocExtractor.ConvertFile(Server.MapPath(basePath), strFileName, StreamToBytes(fuFile.PostedFile.InputStream), saveOnly);
                caseFile = (basePath + localName).Replace("../", url);

                UserInfo ui = Session["UserInfo"] as UserInfo;
                string wrenchId = Session["WrenchId"].ToString();
                WrenchInfo wi = BllWrenchInfo.GetWrenchInfo(wrenchId, out ex);
                wi.CheckTime = DateTime.Now;
                switch (wi.CheckIntervalType)
                {
                    case "Day":
                        wi.NextCheckTime = DateTime.Now.AddDays(wi.CheckInterval);
                        break;
                    case "Month":
                        wi.NextCheckTime = DateTime.Now.AddMonths(wi.CheckInterval);
                        break;
                    case "Year":
                        wi.NextCheckTime = DateTime.Now.AddYears(wi.CheckInterval);
                        break;
                }
                BllWrenchInfo.UpdateWrenchInfo(wi, out ex);
                string status = GetCellValue(Server.MapPath(basePath) + strFileName, 6, 7);
                int result = BllWrenchCheckRecord.AddWrenchCheckRecord(wrenchId, wi.WrenchName, wi.WrenchPosition, ui.Id, ui.FullName, DateTime.Now, status, wi.DataOwner, caseFile, out ex);
                if (result < 0)
                {
                    X.Msg.Alert("提示", string.Format("上传失败,原因为{0}", ex.Message)).Show();
                    return;
                }
                X.Msg.Alert("提示", "上传成功").Show();
                CheckGridData.LoadPage(1);
                WrenchGrid.Refresh();
            }


        }

        public byte[] StreamToBytes(Stream stream)

        {

            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            stream.Seek(0, SeekOrigin.Begin);

            return bytes;

        }

        public static string GetCellValue(string filePath, int rowNum, int cellNum)
        {
            //DataTable dt = new DataTable();
            string str = "";
            if (Path.GetExtension(filePath).ToLower() == ".xls")
                str = GetValueForXLS(filePath, rowNum, cellNum);
            else if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                str = GetValueForXLSX(filePath, rowNum, cellNum);
            return str;
        }

        public static string GetValueForXLS(string filePath, int rowNum, int cellNum, string sheetName = "Sheet1")
        {
            string str = "";
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                ISheet sheet = hssfworkbook.GetSheetAt(0);
                HSSFCell c = sheet.GetRow(rowNum).GetCell(cellNum) as HSSFCell;
                str = GetValueType(c).ToString();
                return str;
            }
      
        }

        public static string GetValueForXLSX(string filePath, int rowNum, int cellNum, string sheetName = "Sheet1")
        {
            string str = "";
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                ISheet sheet = xssfworkbook.GetSheetAt(0);
                XSSFCell c = sheet.GetRow(rowNum).GetCell(cellNum) as XSSFCell;
                str = GetValueType(c).ToString();
                return str;
            }
        }

        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    short format = cell.CellStyle.DataFormat;
                    if (format == 14 || format == 31 || format == 57 || format == 58)
                    {
                        DateTime date = cell.DateCellValue;
                        return date.ToString("yyyy-MM-dd");
                    }
                    else
                        return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }

        public static Uri AbsoluteWebRoot
        {
            get
            {
                var context = HttpContext.Current;
                UriBuilder uri = new UriBuilder();
                uri.Host = context.Request.Url.Host;
                if (!context.Request.Url.IsDefaultPort)
                {
                    uri.Port = context.Request.Url.Port;
                }
                uri.Path = VirtualPathUtility.ToAbsolute("~/");
                Uri absoluteWebRoot = uri.Uri;
                return absoluteWebRoot;
            }
        }


        protected void WrenchGridData_OnDataBound(object sender, EventArgs e)
        {
            
        }

        protected void btnExport_OnDirectClick(object sender, DirectEventArgs e)
        {
            string wrenchId = Session["WrenchId"].ToString();
            Exception ex;
            IList<WrenchCheckRecord> recordList = BllWrenchCheckRecord.SearchWrenchCheckRecord(wrenchId, 0, -1, out ex);
            if (SqlDataHelper.IsDataValid(recordList))
            {
                List<object[]> listData = new List<object[]>();
                foreach (WrenchCheckRecord wcr in recordList)
                {
                    listData.Add(new object[]
                    {
                        wcr.WrenchName,
                        wcr.WrenchPosition,
                        wcr.DataOwner,
                        wcr.WorkerName,
                        wcr.EventTime,
                        wcr.Status,
                        wcr.PdfFile
                    });
                }
                string fileBase = MapPath("/") + "ExcelFile";
                if (!Directory.Exists(fileBase)) Directory.CreateDirectory(fileBase);
                string filePath = fileBase + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                Export2003(listData, filePath, "检验结果");
            }
        }

        public void Export2003(List<object[]> lstData, string filePath, string fileName)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            IFont font = book.CreateFont();
            font.Color = HSSFColor.Blue.Index;
            font.Underline = FontUnderlineType.Single;
            ICellStyle style = book.CreateCellStyle();
            style.SetFont(font);
            ISheet sheet = book.CreateSheet(fileName);
            IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("扳手名称");
            row.CreateCell(1).SetCellValue("扳手位置");
            row.CreateCell(2).SetCellValue("所在工具柜");
            row.CreateCell(3).SetCellValue("校验人");
            row.CreateCell(4).SetCellValue("上传时间");
            row.CreateCell(5).SetCellValue("校验结果");
            row.CreateCell(6).SetCellValue("校验文档");
            for (int i = 0; i < lstData.Count; i++)
            {
                IRow row2 = sheet.CreateRow(i + 1);
                object[] o = lstData[i];
                for (int j = 0; j < 7; j++)
                {
                    if (o[j] == null) continue;
                    row2.CreateCell(j).SetCellValue(o[j].ToString());
                    if (j == 6)
                    {
                        ICell cell = row2.CreateCell(j);
                        cell.SetCellValue("校验报告");
                        cell.CellStyle = style;
                        HSSFHyperlink link = new HSSFHyperlink(HyperlinkType.Url);
                        link.Address = o[j].ToString().Replace("193.168.0.249","10.215.91.115");
                        cell.Hyperlink = link;
                    }
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
    }
}
