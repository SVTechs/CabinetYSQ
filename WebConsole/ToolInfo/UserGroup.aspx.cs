using System;
using System.Collections;
using System.Collections.Generic;
using Domain.ServerMain.Domain;
using Ext.Net;
using Utilities.DbHelper;
using WebConsole.Bll;

namespace ToolInfo
{
    public partial class UserGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<object> orgLst = BllUserInfo.GetOrgs();
            List<object> groupData = new List<object>(orgLst.Count);
            if (SqlDataHelper.IsDataValid(orgLst))
            {
                for (int i = 0; i < orgLst.Count; i++)
                {
                    string str = orgLst[i].ToString();

                    IList<UserInfo> lst = BllUserInfo.SearchUserByOrg(str);
                    List<object> data = new List<object>(lst.Count);
                    foreach (UserInfo u in lst)
                    {
                        bool b = BllBorrowRecord.Borrowed(u.Id);
                        if (b)
                        {
                            data.Add(new
                            {
                                Id = u.Id,
                                Title = u.FullName,
                                Icon = "../../resources/icons/Black.png"
                            });
                        }
                        else
                        {
                            data.Add(new
                            {
                                Id = u.Id,
                                Title = u.FullName,
                                Icon = "../../resources/icons/White.png"
                            });
                        }
                    }
                    groupData.Add(new
                    {
                        Title = lst[0].OrgName,
                        Items = data
                    });
                }
                GroupStore.DataSource = groupData;
                GroupStore.DataBind();
            }
        }
    }
}
