using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Domain.ZWStock.Domain;
using Domain.ServerMain.Domain;
using Ext.Net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Utilities.DbHelper;
using WebConsole.Bll;
using ListItem = Ext.Net.ListItem;

namespace ToolInfo
{
    public partial class ToolInfo_ToolInfo : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                Session["SearchName"] = "";
                Session["SearchSpec"] = "";
                Session["SearchCabinet"] = "";
                Session["ToolId"] = "";

                var ulli = new ListItem
                {
                    Text = "全部",
                    Value = ""
                };
                cbCabinet.Items.Add(ulli);
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
                    }
                }

                IList<string> toolNameList = BllToolInfo.GetToolNameList();
                if (SqlDataHelper.IsDataValid(toolNameList))
                {
                    for (int i = 0; i < toolNameList.Count; i++)
                    {
                        var li = new ListItem
                        {
                            Text = toolNameList[i],
                            Value = toolNameList[i]
                        };
                        cbSearchName.Items.Add(li);
                    }
                }

                IList<string> typeList = BllToolInfo.GetToolType();
                if (SqlDataHelper.IsDataValid(typeList))
                {
                    for (int i = 0; i < typeList.Count; i++)
                    {
                        var li = new ListItem
                        {
                            Text = typeList[i],
                            Value = typeList[i]
                        };
                        cbToolType.Items.Add(li);
                    }
                }
            }
        }

        [DirectMethod]
        public void SaveInput()
        {
            //Session["SearchName"] = tbSearchName.Text;
            Session["SearchName"] = cbSearchName.Text;
            Session["SearchSpec"] = tbSearchSpec.Text;
            Session["SearchCabinet"] = cbCabinet.Text == "全部" ? "" : cbCabinet.Text;
        }

        protected void ToolGridData_OnReadData_Refresh(object sender, StoreReadDataEventArgs e)
        {
            string toolName = (Session["SearchName"] ?? "").ToString();
            string toolSpec = (Session["SearchSpec"] ?? "").ToString();
            string dataOwner = (Session["SearchCabinet"] ?? "").ToString();
            e.Total = BllToolInfo.GetToolInfoCount(toolName, toolSpec, dataOwner);
            IList<Domain.ServerMain.Domain.ToolInfo> recordList = BllToolInfo.SearchToolInfo(toolName, toolSpec, dataOwner, e.Start, e.Limit);

            //e.Total = BllItemMasters.GetItemMastersCount((Session["SearchBarCode"] ?? "").ToString(),
            //    (Session["SearchName"] ?? "").ToString(), (Session["SearchSpec"] ?? "").ToString(), (Session["SearchCabinet"] ?? "").ToString());
            //IList<ItemMasters> recordList = BllItemMasters.SearchItemMasters((Session["SearchBarCode"] ?? "").ToString(),
            //    (Session["SearchName"] ?? "").ToString(), (Session["SearchSpec"] ?? "").ToString(), (Session["SearchCabinet"] ?? "").ToString(), e.Start, e.Limit);

            //foreach (ItemMasters im in recordList)
            //{
            //    im.StoreroomBinItemMastersesList = BllStoreroomBinItemMasters.SearchStoreroomBinItemMasterses(im.Id);
            //    if (im.StoreroomBinItemMastersesList.Count == 0) continue;
            //    im.Position = "";
            //    List<StoreroomBins> tmp = new List<StoreroomBins>();
            //    foreach (StoreroomBinItemMasters s in im.StoreroomBinItemMastersesList)
            //    {
            //        StoreroomBins sb = lstStoreroomBins.First(x => x.Id == s.StoreroomBin_Id);
            //        tmp.Add(sb);
            //    }
            //    tmp.OrderBy(x => x.depth);
            //    for (int i = 0; i < tmp.Count; i++)
            //    {
            //        im.Position += tmp[i].Name + "/";
            //    }

            //}
                ((Store)sender).DataSource = recordList;
        }

        [DirectMethod]
        public void OnAddToolClick()
        {
            //if (!PermissionControl.CheckPermission(Session["UserPerm"], "949F6125-4E62-4B53-8469-E8E4D3632C92"))
            //{
            //    X.Msg.Alert("提示", "权限不足").Show();
            //    return;
            //}
            //if (tbToolCode.Text.Length == 0)
            //{
            //    X.Msg.Alert("提示", "请输入工具编号").Show();
            //    return;
            //}
            //if (tbToolName.Text.Length == 0)
            //{
            //    X.Msg.Alert("提示", "请输入工具名称").Show();
            //    return;
            //}
            //if (cbToolCate.Text.Length == 0)
            //{
            //    X.Msg.Alert("提示", "请选择工具分类").Show();
            //    return;
            //}
            //string cabinetName = cbCabinetName.Value.ToString() == "-" ? "" : cbCabinetName.Value.ToString();
            //string toolCode = tbToolCode.Text;
            //string toolName = tbToolName.Text;
            //string toolSpec = tbToolSpec.Text;
            //int toolCate = int.Parse(cbToolCate.Value.ToString());
            //string toolBrand = tbToolBrand.Text;
            //int result = BllMtToolInfo.AddMtToolInfo(toolCode, 1, toolName, toolSpec, toolCate, toolBrand,
            //    0, "", "", "", 0, 1, DateTime.Now, 0, 0, 0, cabinetName);
            //if (result < 0)
            //{
            //    X.Msg.Alert("提示", "添加失败").Show();
            //    return;
            //}
            //X.Msg.Alert("提示", "添加成功").Show();
        }

        [DirectMethod]
        public void OnEdtToolClick()
        {
            if (!PermissionControl.CheckPermission(Session["UserPerm"], "6C66BB29-965D-447C-8241-24B3FC75E8CB"))
            {
                X.Msg.Alert("提示", "权限不足").Show();
                return;
            }
            //if (tbToolCode.Text.Length == 0)
            //{
            //    X.Msg.Alert("提示", "请输入工具编号").Show();
            //    return;
            //}
            if (tbToolName.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请输入工具名称").Show();
                return;
            }
            if (cbToolType.Text.Length == 0)
            {
                X.Msg.Alert("提示", "请选择工具分类").Show();
                return;
            }
            string toolId = tbToolId.Text;
            //string cabinetName = cbCabinetName.Value.ToString() == "-" ? "" : cbCabinetName.Value.ToString();
            string toolName = tbToolName.Text;
            string toolSpec = tbToolSpec.Text;
            string toolType = cbToolType.Text;
            Domain.ServerMain.Domain.ToolInfo ti = BllToolInfo.GetToolInfo(toolId);
            ItemMasters im = BllItemMasters.GetItemMastersById(toolId);
            im.Name = toolName;
            im.Spec = toolSpec;
            BllItemMasters.UpdateItemMasters(im);
            
            ti.ToolName = toolName;
            ti.ToolSpec = toolSpec;
            ti.ToolType = toolType;
            //ti.DataOwner = cabinetName;
            int result = BllToolInfo.UpdateToolInfo(ti);
            if (result < 0)
            {
                X.Msg.Alert("提示", "修改失败").Show();
                return;
            }
            X.Msg.Alert("提示", "修改成功").Show();
        }

        [DirectMethod]
        public void ToolInfo(string toolId)
        {
            Session["ToolId"] = toolId;
            IoGridData.Reload();
        }

        protected void IoGridData_OnReadData_Refresh(object sender, StoreReadDataEventArgs e)
        {
            string toolId = Session["ToolId"].ToString();
            e.Total = BllBorrowRecord.GetBorrowRecordCount(toolId);
            IList<BorrowRecord> recordList = BllBorrowRecord.SearchBorrowRecord(toolId, e.Start, e.Limit);

            if (SqlDataHelper.IsDataValid(recordList))
            {
                List<object[]> listData = GetBorrowData(recordList);
                ((Store)sender).DataSource = listData;
            }
        }

        private List<object[]> GetBorrowData(IList<BorrowRecord> recordList)
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
                ReturnRecord rr = BllReturnRecord.GetReturnRecord(recordList[i].Id);
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

        [DirectMethod]
        public void DeleteToolInfo(string toolId)
        {
            //if (!PermissionControl.CheckPermission(Session["UserPerm"], "71742929-87AE-4D73-9D2F-07E674B6A102"))
            //{
            //    X.Msg.Alert("提示", "权限不足").Show();
            //    return;
            //}
            //int result = BllMtToolInfo.DeleteMtToolInfo(int.Parse(toolId));
            //if (result > 0)
            //{
            //    X.Msg.Alert("提示", "删除成功").Show();
            //}
            //else
            //{
            //    X.Msg.Alert("提示", "删除失败，请检查网络").Show();
            //}
        }

        [DirectMethod]
        public void FillEdtInfo(string toolId)
        {
            Domain.ServerMain.Domain.ToolInfo ti = BllToolInfo.GetToolInfo(toolId);
            if (ti != null)
            {
                tbToolId.Text = ti.Id;
                tbToolName.Text = ti.ToolName;
                tbToolSpec.Text = ti.ToolSpec;
                cbToolType.Select(ti.ToolType);
                //cbCabinetName.Select(ti.DataOwner);
            }
        }

        protected void btnExport_OnDirectClick(object sender, DirectEventArgs e)
        {
            string toolId = Session["ToolId"].ToString();
            IList<BorrowRecord> recordList = BllBorrowRecord.SearchBorrowRecord(toolId, 0, 10000);
            Hashtable aliasTable = new Hashtable();
            if (SqlDataHelper.IsDataValid(recordList))
            {
                List<object[]> listData = GetBorrowData(recordList);
                string fileBase = MapPath("/") + "ExcelFile";
                if (!Directory.Exists(fileBase)) Directory.CreateDirectory(fileBase);
                string filePath = fileBase + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                Export2003(listData, filePath, "test");
            }

            
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
    }
}
