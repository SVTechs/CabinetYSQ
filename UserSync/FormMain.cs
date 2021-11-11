//using Domain.Qcshkf.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;

namespace UserSync
{
    public partial class FormMain : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            Thread syncThread = new Thread(ScanAndSync) { IsBackground = true };
            syncThread.Start();
        }

        private void ScanAndSync()
        {
            while (true)
            {
                try
                {
                    int add, update;
                    Exception e;
                    IList<People> lstPeoples = GetPeoples();
                    int i = UpdateUserInfo(lstPeoples, out add, out update, out e);
                    if (i == 1)
                    {
                        AddLog("时间:" + DateTime.Now + " 新增:" + add + " 更新:" + update);
                    }
                    else
                    {
                        Logger.Error(i);
                        AddLog("时间:" + DateTime.Now + "人员更新失败，请查看日志。错误代码：" + i);
                    }
                }
                catch (Exception e)
                {
                    AddLog("发生错误: " + e.Message);
                    Logger.Error(e);
                }
                Thread.Sleep(1000 * 60 * 60);
            }
        }

        private delegate void AddLogDelegate(string log);
        private void AddLog(string log)
        {
            if (lbSysLog.InvokeRequired)
            {
                AddLogDelegate delg = AddLog;
                lbSysLog.Invoke(delg, log);
            }
            else
            {
                lbSysLog.Items.Add(log + DateTime.Now.ToString("  (yyyy-MM-dd HH:mm:ss)"));
                if (lbSysLog.Items.Count > 500)
                {
                    lbSysLog.Items.RemoveAt(0);
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("您确定要停止同步吗？", "提示", MessageBoxButtons.OKCancel);
            if (dr != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void mainNotify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private IList<People> GetPeoples()
        {
            string zwConnStr = ConfigurationManager.ConnectionStrings["zwStock"].ConnectionString;
            //string cabinetConnStr = ConfigurationManager.ConnectionStrings["cabinet"].ConnectionString;
            try
            {
                IList<People> lst = new List<People>();
                SqlConnection sqlc = new SqlConnection(zwConnStr);
                sqlc.Open();
                DataSet ds = SqlHelper.GetDataSet(zwConnStr, CommandType.Text,
                    "select Id, JobNum, PassWord, Job, Name from People", null);
                if (!SqlDataHelper.IsDataValid(ds)) return null;
                DataTable dtUser = ds.Tables[0];
                foreach (DataRow dr in dtUser.Rows)
                {
                    lst.Add(new People() { Id = dr["Id"]?.ToString(),
                        JobNum = dr["JobNum"]?.ToString(),
                        Password = dr["password"]?.ToString(),
                        Job = dr["Job"]?.ToString(),
                        Name = dr["Name"]?.ToString() });
                }
                if (!SqlDataHelper.IsDataValid(lst)) return null;
                ds = SqlHelper.GetDataSet(sqlc, CommandType.Text, "select * from PersonJobRoles", null);
                DataTable dtPersonJobRoles = ds.Tables[0];
                ds = SqlHelper.GetDataSet(sqlc, CommandType.Text,
                    "select Id, RoleGroupId, OrganizationId, Name from JobRoles", null);
                DataTable dtJobRoles = ds.Tables[0];
                ds = SqlHelper.GetDataSet(sqlc, CommandType.Text,
                    "select Id, Description,Name OrganizationName from Organizations", null);
                DataTable dtOrganizations = ds.Tables[0];
                DataRow[] aryPersonJobRole,aryJobRole, aryOrganization;
                for (int i = 0; i < lst.Count; i++)
                {
                    aryPersonJobRole = dtPersonJobRoles.Select("Person_Id ='" + lst[i].Id + "'");
                    if (aryPersonJobRole.Length > 0)
                    {
                        aryJobRole = dtJobRoles.Select("Id = '" + aryPersonJobRole[0]["JobRole_Id"] + "'");
                        aryOrganization = dtOrganizations.Select("Id = '" + aryJobRole[0]["OrganizationId"] + "'");
                        lst[i].SetJobRole_Id(aryJobRole[0]["Id"]?.ToString(),
                            aryJobRole[0]["RoleGroupId"]?.ToString(),
                            aryJobRole[0]["OrganizationId"]?.ToString(),
                            aryOrganization[0]["OrganizationName"]?.ToString());
                    }
                }
                sqlc.Close();
                return lst;

            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                Logger.Error(ex);
                return null;
            }
        }

        private int UpdateUserInfo(IList<People> lst, out int add, out int update, out Exception e)
        {
            try
            {
                add = 0;
                update = 0;
                e = null;
                string cabinetConnStr = ConfigurationManager.ConnectionStrings["cabinetServer"].ConnectionString;
                SqlConnection sqlc = new SqlConnection(cabinetConnStr);
                sqlc.Open();
                DataTable dtUser = SqlHelper.GetDataTable(cabinetConnStr, CommandType.Text,
                    "select ID, UserName, Password, FullName, OrgId from Userinfo ", null);
                //if (!SqlDataHelper.IsDataValid(ds))
                //{
                //    return -200;
                //}
                //DataTable dtUser = ds.Tables[0];
                DataRow[] ary;
                People p;
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Id", ""),
                    new SqlParameter("@UserName", ""),
                    new SqlParameter("@Password", ""),
                    new SqlParameter("@FullName", ""),
                    new SqlParameter("@OrgId", ""),
                    new SqlParameter("@EnrollId", ""),
                    new SqlParameter("@OrgName", ""),
                    new SqlParameter("@Createtime", DateTime.Now),
                    new SqlParameter("@Updatetime", DateTime.Now),
                    new SqlParameter("@LEFTTEMPLATE", new byte[0]),
                    new SqlParameter("@RIGHTTEMPLATE", new byte[0]),
                    new SqlParameter("@FACETEMPLATE", new byte[0]),
                    new SqlParameter("@LeftTemplateV10", ""),
                    new SqlParameter("@RightTemplateV10", ""),
                    new SqlParameter("@FaceTemplateV10", ""),

                };
                for (int i = 0; i < lst.Count; i++)
                {
                    p = lst[i];
                    para[0].Value = p.Id ?? "";
                    para[1].Value = p.JobNum ?? "";
                    para[2].Value = p.Password ?? "";
                    para[3].Value = p.Name ?? "";
                    para[4].Value = p.OrganizationId ?? "";
                    para[5].Value = p.JobNum ?? "";
                    para[6].Value = p.OrganizationName ?? "";
                    ary = dtUser.Select("Id ='" + p.Id + "'");
                    if (ary.Length == 0)
                    {
                        add += SqlHelper.ExecuteNonQuery(sqlc, CommandType.Text,
                            "insert into UserInfo(Id, UserName, Password, FullName, OrgId, EnrollId, OrgName, Createtime, " +
                            "LEFTTEMPLATE, RIGHTTEMPLATE, FACETEMPLATE, LeftTemplateV10, RightTemplateV10, FaceTemplateV10)" +
                            " values (@Id, @UserName, @Password, @FullName, @OrgId, @EnrollId, @OrgName, @Createtime, " +
                            "@LEFTTEMPLATE, @RIGHTTEMPLATE, @FACETEMPLATE, @LeftTemplateV10, @RightTemplateV10, @FaceTemplateV10)", para);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(p.OrganizationId)) continue;
                        if (ary[0]["UserName"]?.ToString() == p.JobNum && ary[0]["Password"]?.ToString() == p.Password &&
                            ary[0]["OrgId"]?.ToString() == p.OrganizationId) continue;
                        update += SqlHelper.ExecuteNonQuery(sqlc, CommandType.Text,
                            "update UserInfo set UserName = @UserName, Password = @Password, OrgId = @OrgId, EnrollId = @EnrollId, OrgName = @OrgName, Updatetime = @Updatetime where Id = @Id" , para);
                    }
                }
                Logger.Log(LogLevel.Info, "时间:" + DateTime.Now + " 新增:" + add + " 更新:" + update);
                return 1;
            }
            catch (Exception ex)
            {
                AddLog(ex.Message);
                Logger.Error(ex);
                add = 0;
                update = 0;
                e = ex;
                return -100;
            }
        }

        class People
        {
            public string Id;
            public string JobNum;
            public string Password;
            public string Job;
            public string Name;
            public string JobRole_Id;
            public string RoleGroupId;
            public string OrganizationId;
            public string OrganizationName;

            public void Init(string id, string jobNum, string password, string job, string name)
            {
                Id = id;
                JobNum = jobNum;
                Password = password;
                Job = job;
                Name = name;
            }

            public void SetJobRole_Id(string jobRole_Id, string roleGroupId, string organizationId, string organizationName)
            {
                JobRole_Id = jobRole_Id;
                RoleGroupId = roleGroupId;
                OrganizationId = organizationId;
                OrganizationName = organizationName;
            }
        }
    }
}