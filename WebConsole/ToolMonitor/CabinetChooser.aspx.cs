using System;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Utilities.DbHelper;
using WebConsole.Bll;

namespace ToolMonitor
{
    public partial class ToolMonitor_CabinetChooser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<CabinetInfo> cabinetList = BllCabinetInfo.SearchCabinetInfo();
            if (SqlDataHelper.IsDataValid(cabinetList))
            {
                List<object> data = new List<object>(cabinetList.Count);
                foreach (CabinetInfo ci in cabinetList)
                {
                    data.Add(new
                    {
                        Id = ci.CabinetName,
                        Title = ci.CabinetAlias,
                        Icon = "../../resources/images/cabinet.png"
                    });
                }

                List<object> groupData = new List<object>(1);
                groupData.Add(new
                {
                    Title = "工具柜",
                    Items = data
                });
                GroupStore.DataSource = groupData;
                GroupStore.DataBind();
            }
        }
    }
}