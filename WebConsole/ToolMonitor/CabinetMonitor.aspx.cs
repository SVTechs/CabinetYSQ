using System;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Utilities.DbHelper;
using WebConsole.Bll;

namespace ToolMonitor
{
    public partial class ToolMonitor_CabinetMonitor : System.Web.UI.Page
    {
        public string CabinetName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string originName = Request.QueryString["id"];
            CabinetName = BllCabinetInfo.GetCabinetAlias(originName);

            IList<ToolInfo> toolList = BllToolInfo.SearchToolInfo(0, -1, null, null, originName);
            if (SqlDataHelper.IsDataValid(toolList))
            {
                List<object> data = new List<object>(toolList.Count);
                foreach (ToolInfo ti in toolList)
                {
                    //在位状态
                    string displayStatus = "", backColor = "";
                    if (ti.ToolStatus == 10)
                    {
                        displayStatus = "维修";
                        backColor = "#E01818";
                    }
                    else if (DateTime.Now >= ti.NextCheckTime)
                    {
                        displayStatus = "待检";
                        backColor = "#E01818";
                    }
                    else if (ti.RtStatus == 0)
                    {
                        BorrowRecord br = BllBorrowRecord.GetLastBorrowRecord(ti.Id);
                        string user = br == null ? "" : br.WorkerName ;
                        displayStatus = "离位: " + user;
                        backColor = "#F8B823";
                    }
                    else if (ti.RtStatus == 1)
                    {
                        displayStatus = "在位";
                        backColor = "#53BE38";
                    }
                    data.Add(new
                    {
                        Title = ti.ToolName,
                        ToolCode = ti.ToolCode,
                        ToolStatus = displayStatus,
                        BackColor = backColor,
                        Icon = "../../resources/images/Status.png"
                    });
                }

                List<object> groupData = new List<object>(1);
                groupData.Add(new
                {
                    Title = originName,
                    Items = data
                });
                GroupStore.DataSource = groupData;
                GroupStore.DataBind();
            }
        }

        protected void btnCabinetChooser_Click(object sender, EventArgs e)
        {
            Response.Redirect("CabinetChooser.aspx");
        }
    }
}