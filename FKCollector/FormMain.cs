using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FKCollector.FeatureUpdateServiceRef;
using NLog;
using Utilities.Control;
using Utilities.Encryption;
using Utilities.FileHelper;

namespace FKCollector
{
    public partial class FormMain : C1.Win.C1Ribbon.C1RibbonForm
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static string _userId = "", _deviceType, _orgId;
        private static List<FKDeviceInfo> lstZokoDevice;
        private static List<ZKDeviceInfo> lstZKDevice;
        private static List<UserInfo> lstUserInfo;
        private static Dictionary<string, string> dicOrg;
        private static readonly FeatureUpdateService FeatureUpdateService = new FeatureUpdateService();
        private static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            _deviceType = ConfigurationManager.AppSettings["deviceType"];
            _orgId =  ConfigurationManager.AppSettings["orgId"];
            lbRemain.DisplayMember = "Text";
            lbChosen.DisplayMember = "Text";
            dicOrg = DbFunction.GetAllOrg();
            if (!string.IsNullOrEmpty(_orgId))
            {
                string[] ary = _orgId.Split(',');
                foreach (string s in ary)
                {
                    if (!dicOrg.ContainsKey(s)) continue;
                    lbChosen.Items.Add(new ListBoxItem(dicOrg[s], s));
                    dicOrg.Remove(s);
                }
            }
            foreach (string key in dicOrg.Keys)
            {
                lbRemain.Items.Add(new ListBoxItem(dicOrg[key], key));
            }
            
            if (string.IsNullOrEmpty(_deviceType) || _deviceType != "zoko")
            {
                lblDeviceType.Text = "设备类型： 中控";
                int result = ZKDeviceFunction.Init();
                if (result < 0)
                {
                    MessageBox.Show("初始化指纹仪连接失败，请检查驱动是否安装");
                    Application.Exit();
                    return;
                }
                if (result == 0)
                {
                    MessageBox.Show("初始化指纹仪连接失败，请检查指纹仪是否正确连接");
                    Application.Exit();
                    return;
                }
            }
            else
            {
                lblDeviceType.Text = "设备类型：zoko";
                int result = FKDeviceFunction.Init();
                if (result < 0)
                {
                    MessageBox.Show("初始化指纹仪连接失败，请检查驱动是否安装");
                    Application.Exit();
                    return;
                }
                if (result == 0)
                {
                    MessageBox.Show("初始化指纹仪连接失败，请检查指纹仪是否正确连接");
                    Application.Exit();
                    return;
                }
            }

            //BllDataMgr.InitConn();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(string.IsNullOrEmpty(_deviceType) || _deviceType != "zoko")
            {
                ZKDeviceFunction.DisConnect();
            }
            else
            {
                FKAttendDLL.FK_DisConnect(FKAttendDLL.nCommHandleIndex);
            }
        }



        private void btnCodeQuery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbQueryCode.Text))
            {
                MessageBox.Show("工号不能为空");
                return;
            }
            string userName = tbQueryCode.Text;
            UserInfo ui = DbFunction.SearchUserInfo(userName);
            if (ui != null)
            {
                tbName.Text = ui.FullName;
                tbOrgId.Text = ui.OrgId;
                tbEnrollId.Text = ui.EnrollId.ToString();
                tbLEFTTEMPLATE.Text = ui.LEFTTEMPLATE.ToString();
                tbRIGHTTEMPLATE.Text = ui.RIGHTTEMPLATE.ToString();
                tbFACETEMPLATE.Text = ui.FACETEMPLATE.ToString();
                tbLeftTemplateV10.Text = ui.LeftTemplateV10;
                tbRightTemplateV10.Text = ui.RightTemplateV10;
                tbFaceTemplateV10.Text = ui.FaceTemplateV10.ToString();
                btnSaveUser.Visible = false;
            }
            else
            {
                MessageBox.Show("未找到该用户");
                //if (MessageBox.Show("没有此工号，是否要创建该用户?", "" ,MessageBoxButtons.OKCancel) == DialogResult.OK)
                //{
                //    btnSaveUser.Visible = true;
                //}
            }
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("姓名不能为空");
                return;
            }
            if (string.IsNullOrEmpty(tbQueryCode.Text))
            {
                MessageBox.Show("工号不能为空");
                return;
            }
            //int rslt = DbFunction.AddUserInfo(tbQueryCode.Text.Trim(),tbName.Text.Trim(),tbOrgId.Text.Trim());
            //if (rslt  == 1)
            //{
            //    MessageBox.Show("保存成功");
            //    btnSaveUser.Visible = false;
            //}
        }

        private void btnConnStr_Click(object sender, EventArgs e)
        {
            FormConnectString fcs = new FormConnectString();
            fcs.ShowDialog();
        }

        private void c1Button1_Click(object sender, EventArgs e)
        {
            ListBoxItem lb = (ListBoxItem)lbRemain.SelectedItem;
            if (lb != null )
            {
                lbChosen.Items.Add(lb);
                lbRemain.Items.Remove(lb);
            }
        }

        private void c1Button2_Click(object sender, EventArgs e)
        {
            ListBoxItem lb = (ListBoxItem)lbChosen.SelectedItem;
            if (lb != null)
            {
                lbRemain.Items.Add(lb);
                lbChosen.Items.Remove(lb);
            }
        }

        private void btnSaveOrg_Click(object sender, EventArgs e)
        {
            string orgId = "";
            foreach (ListBoxItem i in lbChosen.Items)
            {
                orgId += i.Value + ",";
            }
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
            string basicPath = di.Parent.FullName + "\\CabinetMgr\\Config.ini";
            IniHelper.Write("AppConfig", "AllowedGroup", orgId, basicPath);
            config.AppSettings.Settings["OrgId"].Value = orgId;
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbDB.Checked)
                {
                    int i = DbFunction.GetCount();
                    if (i < 0)
                    {
                        MessageBox.Show("数据库连接失败");
                        return;
                    }
                    if (!string.IsNullOrEmpty(_orgId))
                    {
                        lstUserInfo = DbFunction.UserByOrgId();
                    }
                    else
                    {
                        lstUserInfo = DbFunction.GetAllUser();
                    }

                    if (lstUserInfo != null)
                    {
                        lblDataCount.Text = "数据共" + lstUserInfo.Count + "条";
                    }
                    else
                    {
                        lblDataCount.Text = "数据库未获取到数据";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_deviceType) || _deviceType != "zoko")
                    {
                        lstZKDevice = ZKDeviceFunction.GetAllInfo();
                        if (lstZKDevice != null)
                        {
                            lblDataCount.Text = "数据共" + lstZKDevice.Count + "条";
                        }
                        else
                        {
                            lblDataCount.Text = "考勤机未获取到数据";
                        }
                    }
                    else
                    {
                        lstZokoDevice = FKDeviceFunction.GetAllInfo();
                        if (lstZokoDevice != null)
                        {
                            lblDataCount.Text = "数据共" + lstZokoDevice.Count + "条";
                        }
                        else
                        {
                            lblDataCount.Text = "考勤机未获取到数据";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("下载数据出错");
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbDB.Checked)
                {
                    if (lstUserInfo == null || lstUserInfo.Count == 0) return;
                    if (string.IsNullOrEmpty(_deviceType) || _deviceType != "zoko")
                    {
                        bool b = ZKDeviceFunction.SetDeviceInfo(lstUserInfo);
                        if (b)
                        {
                            MessageBox.Show("上传成功");
                        }
                        else
                        {
                            MessageBox.Show("上传失败");
                        }
                    }
                    else
                    {
                        List<UInt32> lstFailed;
                        List<UInt32> lstSucceed;
                        FKDeviceFunction.SetDeviceInfo(lstUserInfo, out lstFailed, out lstSucceed);
                        if (lstSucceed.Count > 0)
                        {
                            DbFunction.SetSucceed(lstSucceed);
                        }
                        if (lstFailed.Count > 0)
                        {
                            DbFunction.SetFailed(lstFailed);
                        }
                    }
                }

                if (rbDevice.Checked)
                {
                    int rslt = -1;
                    if (string.IsNullOrEmpty(_deviceType) || _deviceType != "zoko")
                    {
                        if (lstZKDevice == null || lstZKDevice.Count == 0) return;
                        for (int i = 0; i < lstZKDevice.Count; i++)
                        {
                            ZKDeviceInfo zkDi = lstZKDevice[i];
                            UserInfo ui = DbFunction.SearchUserInfo(zkDi.EnrollId.ToString());
                            if (ui == null)
                            {
                                string name = ZKDeviceFunction.GetName(zkDi.EnrollId);
                                Logger.Warn("数据库中无该人员信息" + name);
                                continue;
                            }
                            if (zkDi.BackupNum == 50)
                            {
                                ui.FaceTemplateV10 = zkDi.StrData;
                                DbFunction.ModifyFeature(ui);
                                continue;
                            }
                            if (zkDi.BackupNum % 2 == 1)
                            {
                                ui.LeftTemplateV10 = zkDi.StrData;
                                DbFunction.ModifyFeature(ui);
                            }
                            else
                            {
                                ui.RightTemplateV10 = zkDi.StrData;
                                DbFunction.ModifyFeature(ui);
                            }
                        }
                        DataSet dsZk = DbFunction.GetAllUserDataSet();
                        rslt = FeatureUpdateService.UpdateFeature(dsZk, "");
                    }
                    else
                    {
                        if (lstZokoDevice == null || lstZokoDevice.Count == 0) return;
                        for (int i = 0; i < lstZokoDevice.Count; i++)
                        {
                            FKDeviceInfo fkDi = lstZokoDevice[i];
                            UserInfo ui = DbFunction.SearchUserInfo(fkDi.EnrollId.ToString(), "");
                            if (ui == null)
                            {
                                string name = FKDeviceFunction.GetName(fkDi.EnrollId);
                                Logger.Warn("数据库中无该人员信息" + name);
                                continue;
                            }
                            if (fkDi.BackupNum == 12)
                            {
                                ui.FACETEMPLATE = fkDi.ByteData;
                                DbFunction.ModifyFeature(ui);
                                continue;
                            }
                            if (fkDi.BackupNum % 2 == 1)
                            {
                                ui.LEFTTEMPLATE = fkDi.ByteData;
                                DbFunction.ModifyFeature(ui);
                            }
                            else
                            {
                                ui.RIGHTTEMPLATE = fkDi.ByteData;
                                DbFunction.ModifyFeature(ui);
                            }
                        }
                        DataSet ds = DbFunction.GetAllUserDataSet();
                        rslt = FeatureUpdateService.UpdateFeature(ds, "zoko");
                    }
                    if (rslt > 0)
                    {
                        MessageBox.Show("更新成功");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("上传数据出错");
            }



        }
    }

    public class Item
    {
        public string Id;
        public string Text;
    }
}
