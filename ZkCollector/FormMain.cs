using System;
using System.Windows.Forms;
using Domain.ServerMain.Domain;
using Hardware.DeviceInterface;
using ZkCollector.Bll;

// ReSharper disable LocalizableElement

namespace ZkCollector
{
    public partial class FormMain : Form
    {
        private string _userId = "";

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            int result = FpZkDevice.Init(this);
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
            BllDataMgr.InitConn();
        }

        private void btnNameQuery_Click(object sender, EventArgs e)
        {
            if (tbQueryName.Text.Length == 0)
            {
                MessageBox.Show("请输入用户姓名");
                return;
            }
            using (FormUserList ulForm = new FormUserList { UserName = tbQueryName.Text })
            {
                ulForm.ShowDialog();
                string userId = ulForm.UserId;
                if (!string.IsNullOrEmpty(userId))
                {
                    BllUserInfo.GetUserInfo(userId, out var userInfo);
                    if (userInfo == null)
                    {
                        MessageBox.Show("网络异常");
                        return;
                    }

                    _userId = userInfo.Id;

                    tbQueryName.Text = userInfo.FullName;
                    tbQueryCode.Text = userInfo.UserName;
                    tbQueryOrg.Text = userInfo.OrgId;
                    tbUserOrg.Text = userInfo.OrgId;
                    tbUserSex.Text = userInfo.Sex;
                    tbUserAge.Text = userInfo.Age.ToString();
                    tbUserTel.Text = userInfo.Tel;

                    tbFpV91.Text = userInfo.NewLeftTemplate;
                    tbFpV92.Text = userInfo.NewRightTemplate;
                    tbFpVX1.Text = userInfo.LeftTemplateV10;
                    tbFpVX2.Text = userInfo.RightTemplateV10;
                }
            }
        }

        private void btnOrgQuery_Click(object sender, EventArgs e)
        {
            if (tbQueryOrg.Text.Length == 0)
            {
                MessageBox.Show("请输入用户班组");
                return;
            }
            using (FormUserList ulForm = new FormUserList { UserOrg = tbQueryOrg.Text })
            {
                ulForm.ShowDialog();
                string userId = ulForm.UserId;
                if (!string.IsNullOrEmpty(userId))
                {
                    BllUserInfo.GetUserInfo(userId, out var userInfo);
                    if (userInfo == null)
                    {
                        MessageBox.Show("网络异常");
                        return;
                    }

                    _userId = userInfo.Id;

                    tbQueryName.Text = userInfo.FullName;
                    tbQueryCode.Text = userInfo.UserName;
                    tbUserOrg.Text = userInfo.OrgId;
                    tbUserSex.Text = userInfo.Sex;
                    tbUserAge.Text = userInfo.Age.ToString();
                    tbUserTel.Text = userInfo.Tel;

                    tbFpV91.Text = userInfo.NewLeftTemplate;
                    tbFpV92.Text = userInfo.NewRightTemplate;
                    tbFpVX1.Text = userInfo.LeftTemplateV10;
                    tbFpVX2.Text = userInfo.RightTemplateV10;
                }
            }
        }

        private void btnCodeQuery_Click(object sender, EventArgs e)
        {
            if (tbQueryCode.Text.Length == 0)
            {
                MessageBox.Show("请输入用户工号");
                return;
            }
            UserInfo userInfo = BllUserInfo.GetUserInfoByUserName(tbQueryCode.Text);
            if (userInfo == null)
            {
                MessageBox.Show("未找到对应人员");
                return;
            }

            _userId = userInfo.Id;

            tbQueryName.Text = userInfo.FullName;
            tbQueryOrg.Text = userInfo.OrgId;
            tbUserOrg.Text = userInfo.OrgId;
            tbUserSex.Text = userInfo.Sex;
            tbUserAge.Text = userInfo.Age.ToString();
            tbUserTel.Text = userInfo.Tel;

            tbFpV91.Text = userInfo.NewLeftTemplate;
            tbFpV92.Text = userInfo.NewRightTemplate;
            tbFpVX1.Text = userInfo.LeftTemplateV10;
            tbFpVX2.Text = userInfo.RightTemplateV10;
        }

        private void btnCollectV91_Click(object sender, EventArgs e)
        {
            DoCollect(9, 0);
        }

        private void btnCollectV92_Click(object sender, EventArgs e)
        {
            DoCollect(9, 1);
        }

        private void btnCollectVX1_Click(object sender, EventArgs e)
        {
            DoCollect(10, 2);
        }

        private void btnCollectVX2_Click(object sender, EventArgs e)
        {
            DoCollect(10, 3);
        }

        private void DoCollect(int engineVersion, int type)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                MessageBox.Show("请查询目标用户");
                return;
            }
            using (FormCollect clForm = new FormCollect { EngineVersion = engineVersion })
            {
                clForm.ShowDialog();
                string feature = clForm.Feature;
                if (!string.IsNullOrEmpty(feature))
                {
                    int result = BllUserInfo.ModifyUserFeature(_userId, type, feature);
                    if (result > 0)
                    {
                        switch (type)
                        {
                            case 0:
                                tbFpV91.Text = feature;
                                break;
                            case 1:
                                tbFpV92.Text = feature;
                                break;
                            case 2:
                                tbFpVX1.Text = feature;
                                break;
                            case 3:
                                tbFpVX2.Text = feature;
                                break;
                        }    
                        MessageBox.Show("注册成功");
                    }
                    else
                    {
                        MessageBox.Show("注册失败");
                    }
                }
                else
                {
                    MessageBox.Show("指纹采集失败");
                }
            }
        }

        private void tbFullName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnNameQuery.PerformClick();
            }
        }

        private void tbUserCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCodeQuery.PerformClick();
            }
        }

        private void tbQueryOrg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOrgQuery.PerformClick();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                MessageBox.Show("请查询目标用户");
                return;
            }
            BllUserInfo.GetUserInfo(_userId, out var userInfo);
            if (userInfo == null)
            {
                MessageBox.Show("网络异常");
                return;
            }
            userInfo.FullName = tbQueryName.Text;
            userInfo.EmpName = tbQueryName.Text;
            userInfo.OrgId = tbUserOrg.Text;
            int.TryParse(tbUserAge.Text, out var age);
            userInfo.Age = age;
            userInfo.Tel = tbUserTel.Text;
            int result = BllUserInfo.UpdateUserInfo(userInfo);
            if (result > 0)
            {
                MessageBox.Show("更新成功");
            }
            else
            {
                MessageBox.Show("更新失败");
            }
        }
    }
}
