using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CabinetMgr.Config;
using Hardware.DeviceInterface;
using Hardware.NCanBus;

// ReSharper disable LocalizableElement

namespace Hardware.DeviceDebug
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            CabinetCallback.OnInitDone = OnCabinetInitDone;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            ConfigLoader.LoadConfig();

            if (AppConfig.DeviceType == 1)
            {
                DeviceLayer.CabinetDevice = new CabinetDeviceRx1();
            }
            else if (AppConfig.DeviceType == 2)
            {
                DeviceLayer.CabinetDevice = new CabinetDeviceRx2();
            }
            else if (AppConfig.DeviceType == 3)
            {
                DeviceLayer.CabinetDevice = new CabinetDeviceRx3();
            }
            else if (AppConfig.DeviceType == 4)
            {
                DeviceLayer.CabinetDevice = new CabinetDeviceRx4();
            }

            //配置Modbus模块信息
            Type configType = typeof(AppConfig);
            for (int i = 1; i <= 9; i++)
            {
                string ip = "";
                int port = 0, capacity = 0, startIndex = 0;
                //状态检测设备
                PropertyInfo property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}Ip");
                if (property != null)
                {
                    ip = (string)property.GetValue(null, null);
                }
                if (!string.IsNullOrEmpty(ip))
                {
                    property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}Port");
                    if (property != null)
                    {
                        port = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}Capacity");
                    if (property != null)
                    {
                        capacity = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolChecker{(i == 1 ? "" : i.ToString())}StartIndex");
                    if (property != null)
                    {
                        startIndex = (int)property.GetValue(null, null);
                    }
                    DeviceLayer.CabinetDevice.AddToolCheckerDevice(ip, port, (ushort)capacity, (ushort)startIndex);
                }
                //灯控设备
                property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}Ip");
                if (property != null)
                {
                    ip = (string)property.GetValue(null, null);
                }
                if (!string.IsNullOrEmpty(ip))
                {
                    property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}Port");
                    if (property != null)
                    {
                        port = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}Capacity");
                    if (property != null)
                    {
                        capacity = (int)property.GetValue(null, null);
                    }
                    property = configType.GetProperty($"ToolLedOperator{(i == 1 ? "" : i.ToString())}StartIndex");
                    if (property != null)
                    {
                        startIndex = (int)property.GetValue(null, null);
                    }
                    DeviceLayer.CabinetDevice.AddToolLedOperatorDevice(ip, port, (ushort)capacity, (ushort)startIndex);
                }
            }
            for (int i = 1; i <= 9; i++)
            {
                PropertyInfo mProperty = configType.GetProperty($"BrokenCheckerMapping{i}");
                if (mProperty != null)
                {
                    string mapping = (string)mProperty.GetValue(null, null);
                    if (!string.IsNullOrEmpty(mapping))
                    {
                        string[] data = mapping.Split(',');
                        int origin, dest;
                        if (int.TryParse(data[0], out origin) && int.TryParse(data[1], out dest))
                        {
                            DeviceLayer.CabinetDevice.AddCheckerMapping(origin - 1, dest - 1);
                        }
                    }
                }
                mProperty = configType.GetProperty($"BrokenOperatorMapping{i}");
                if (mProperty != null)
                {
                    string mapping = (string)mProperty.GetValue(null, null);
                    if (!string.IsNullOrEmpty(mapping))
                    {
                        string[] data = mapping.Split(',');
                        int origin, dest;
                        if (int.TryParse(data[0], out origin) && int.TryParse(data[1], out dest))
                        {
                            DeviceLayer.CabinetDevice.AddOperatorMapping(origin - 1, dest - 1);
                        }
                    }
                }
            }
            if (AppConfig.ReserveToolCount == 0) AppConfig.ReserveToolCount = AppConfig.ToolCount;

            DeviceLayer.CabinetDevice.InitDevicePure((ushort)AppConfig.ToolCount, (ushort)AppConfig.ReserveToolCount, (ushort)AppConfig.DrawerCount,
                AppConfig.CabinetCheckerIp, AppConfig.CabinetOperatorIp);
        }

        private delegate void CabinetInitDoneDelegate(string result);
        public void OnCabinetInitDone(string result)
        {
            try
            {
                if (InvokeRequired)
                {
                    CabinetInitDoneDelegate d = OnCabinetInitDone;
                    Invoke(d, result);
                }
                else
                {
                    if (!string.IsNullOrEmpty(result)) Text = result;
                    else Text = "初始化完成";
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void btnOpenLed_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tbLedCount.Text, out var toolCount))
            {
                MessageBox.Show("灯数量无效");
                return;
            }
            bool []status = new bool[toolCount];
            for (int i = 0; i < toolCount; i++)
            {
                status[i] = true;
            }
            DeviceLayer.CabinetDevice.SetToolLedStatus(status);
        }

        private void btnOpenYellow_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedYellow(true);
        }

        private void btnOpenRed_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedRed(true);
        }

        private void btnCloseLed_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tbLedCount.Text, out var toolCount))
            {
                MessageBox.Show("灯数量无效");
                return;
            }
            bool[] status = new bool[toolCount];
            for (int i = 0; i < toolCount; i++)
            {
                status[i] = false;
            }
            DeviceLayer.CabinetDevice.SetToolLedStatus(status);
        }

        private void btnCloseYellow_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedYellow(false);
        }

        private void btnCloseRed_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenLedRed(false);
        }

        private void btnUnlockDrawer_Click(object sender, EventArgs e)
        {
            bool[] status = {true, true, true, true};
            DeviceLayer.CabinetDevice.UnlockDrawer(status);
        }

        private void btnLockDrawer_Click(object sender, EventArgs e)
        {
            bool[] status = { false, false, false, false };
            DeviceLayer.CabinetDevice.UnlockDrawer(status);
        }

        private void btnUnlockDoor_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.UnlockDoor(true);
            Thread.Sleep(1000);
            DeviceLayer.CabinetDevice.UnlockDoor(false);
        }

        private void btnLockDoor_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.LockDoor(true);
            Thread.Sleep(1000);
            DeviceLayer.CabinetDevice.LockDoor(false);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnOpenLight_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenAllLight(true);
        }

        private void btnCloseLight_Click(object sender, EventArgs e)
        {
            DeviceLayer.CabinetDevice.OpenAllLight(false);
        }
    }
}
