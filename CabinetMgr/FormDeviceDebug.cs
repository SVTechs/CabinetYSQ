using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using CabinetMgr.Bll;
using CabinetMgr.Config;
using Domain.Main.Domain;
using Hardware.DeviceInterface;
using Utilities.DbHelper;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormDeviceDebug : Form
    {
        public FormDeviceDebug()
        {
            InitializeComponent();
        }

        private void FormDeviceDebug_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            cbUnlockDrawerCode.SelectedIndex = 0;
            cbLockDrawerCode.SelectedIndex = 0;
        }

        private void btnOpenYellow_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedYellow(true);
        }

        private void btnOpenRed_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedRed(true);
        }

        private void btnCloseYellow_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedYellow(false);
        }

        private void btnCloseRed_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedRed(false);
        }

        private void btnOpenToolLed_Click(object sender, EventArgs e)
        {
            bool[] ledStatus = new bool[AppConfig.ToolCount];
            for (int i = 0; i < AppConfig.ToolCount; i++)
            {
                ledStatus[i] = true;
            }
            DeviceLayer.CabinetDevice.SetToolLedStatus(ledStatus);
        }

        private void btnCloseToolLed_Click(object sender, EventArgs e)
        {
            bool[] ledStatus = new bool[AppConfig.ToolCount];
            for (int i = 0; i < AppConfig.ToolCount; i++)
            {
                ledStatus[i] = false;
            }
            DeviceLayer.CabinetDevice.SetToolLedStatus(ledStatus);
        }

        private void btnUnlockDrawer_Click(object sender, EventArgs e)
        {
            bool[] status = { true, true, true, true };
            DeviceLayer.CabinetDevice.UnlockDrawer(status);
        }

        private void btnLockDrawer_Click(object sender, EventArgs e)
        {
            bool[] status = { false, false, false, false };
            DeviceLayer.CabinetDevice.UnlockDrawer(status);
        }

        private void btnUnlockDoor_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.UnlockDoor(false);
        }

        private void btnLockDoor_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.UnlockDoor(true);
        }

        private void btnUnlockSingleDrawer_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.UnlockDrawer(cbUnlockDrawerCode.SelectedIndex, true);
        }

        private void btnLockSingleDrawer_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.UnlockDrawer(cbLockDrawerCode.SelectedIndex, false);
        }

        private void btnOpenSingleLed_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.SetToolLedWaitStatus(int.Parse(tbLedNo.Text), true);
        }

        private void btnCloseSingleLed_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.SetToolLedWaitStatus(int.Parse(tbLedNo.Text), false);
        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            int result = FpZkIFace.ClearUser();
            if (result > 0)
            {
                MessageBox.Show("操作成功");
            }
            else
            {
                MessageBox.Show("操作失败");
            }
        }


        private void btnUploadIfaceUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AppConfig.AllowedGroup))
            {
                MessageBox.Show("未设置班组");
                return;
            }
            string[] group = AppConfig.AllowedGroup.Replace('，', ',').Split(',');
            IList<UserInfo> uiList = BllUserInfo.SearchUserByGroup(group);
            if (AppConfig.IfaceType != "zoko")
            {
                if (SqlDataHelper.IsDataValid(uiList))
                {
                    List<FpZkIFace.IfaceUser> ifList = new List<FpZkIFace.IfaceUser>();
                    for (int i = 0; i < uiList.Count; i++)
                    {
                        UserInfo ui = (UserInfo)uiList[i];
                        FpZkIFace.IfaceUser ifUser = new FpZkIFace.IfaceUser(ui.FullName, ui.TemplateUserId.ToString(), ui.LeftTemplateV10,
                            ui.RightTemplateV10);
                        ifList.Add(ifUser);
                    }
                    int result = FpZkIFace.UploadUserInfo(ifList);
                    if (result > 0)
                    {
                        MessageBox.Show("操作成功");
                    }
                    else
                    {
                        MessageBox.Show("操作失败");
                    }
                }
                else
                {
                    MessageBox.Show("未找到班组人员");
                }
            }
            else
            {
                
            }

        }

        private void btnExposeConfig_Click(object sender, EventArgs e)
        {
            ConfigLoader.ExposeConfig();
            MessageBox.Show("完成");
        }

        private void btnUploadUser_Click(object sender, EventArgs e)
        {
            UInt32 vEnrollNumber = 0;
            int vBackupNumber = 0;
            int vPrivilege = (int)enumMachinePrivilege.MP_NONE;
            int vnEnableFlag = 0;
            int vnResultCode, dataRsltCode;
            byte[] mbytCurEnrollData = new byte[20080];
            int mnCurPassword = 0;
            List<FKUser> lstFKUsers = new List<FKUser>();
            do
            {
                vnResultCode = FKAttendDLL.FK_GetAllUserID(FKAttendDLL.nCommHandleIndex, ref vEnrollNumber, ref vBackupNumber, ref vPrivilege, ref vnEnableFlag);
                if (vnResultCode != (int)enumErrorCode.RUN_SUCCESS)
                {
                    if (vnResultCode == (int)enumErrorCode.RUNERR_DATAARRAY_END)
                        vnResultCode = (int)enumErrorCode.RUN_SUCCESS;
                    break;
                }
                for (int i = 0; i <= 12; i++)
                {
                    dataRsltCode = FKAttendDLL.FK_GetEnrollData(
                        FKAttendDLL.nCommHandleIndex,
                        vEnrollNumber,
                        vBackupNumber,
                        ref vPrivilege,
                        mbytCurEnrollData,
                        ref mnCurPassword);
                    if (dataRsltCode == (int)enumErrorCode.RUN_SUCCESS)
                    {
                        lstFKUsers.Add(new FKUser()
                        {
                            vEnrollNumber = vEnrollNumber, vBackupNumber = vBackupNumber,
                            mbytCurEnrollData = mbytCurEnrollData, vnEnableFlag = vnEnableFlag, vPrivilege = vPrivilege
                        });
                    }
                }
            }
            while (true);
        }
    }
}
