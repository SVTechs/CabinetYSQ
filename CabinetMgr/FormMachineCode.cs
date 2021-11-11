using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CabinetMgr.Bll;
using CabinetMgr.Bll.MeasureServiceRef;
using CabinetMgr.Config;
using CabinetMgr.RtDelegate;
using Utilities.DbHelper;

namespace CabinetMgr
{
    public partial class FormMachineCode : Form
    {
        public FormMachineCode()
        {
            InitializeComponent();
        }

        private string _codeFilter = "";
        private readonly List<string> _codeList = new List<string>();

        private void FormMachineCode_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            _codeList.Clear();
            IList<RepairProcessEntity> machineList = BllMeasurementData.SearchMachineCode();
            if (SqlDataHelper.IsDataValid(machineList))
            {
                for (int i = 0; i < machineList.Count; i++)
                {
                    string code;
                    if (AppConfig.AppType != 1)
                    {
                        code = machineList[i].TrainType + "-" + machineList[i].TrainNum
                                 + "-" + machineList[i].Proces;
                    }
                    else
                    {
                        code = machineList[i].PartsId;
                    }
                    _codeList.Add(code.ToUpper());
                }
            }
            RefreshDisplayList();
        }

        private void RefreshDisplayList()
        {
            lbMachineCode.Items.Clear();
            for (int i = 0; i < _codeList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(_codeFilter) || _codeList[i].Contains(_codeFilter))
                {
                    lbMachineCode.Items.Add(_codeList[i]);
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (lbMachineCode.SelectedItem != null)
            {
                DelegateMissionInfo.UpdateMachineCode?.Invoke(lbMachineCode.SelectedItem.ToString());
                Close();
            }
            else
            {
                if (tbMachineCode.Text.Length > 0)
                {
                    DelegateMissionInfo.UpdateMachineCode?.Invoke(tbMachineCode.Text.ToUpper());
                    Close();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbMachineCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnConfirm.PerformClick();
        }

        private void tbMachineCode_TextChanged(object sender, EventArgs e)
        {
            _codeFilter = tbMachineCode.Text.ToUpper().Trim();
            RefreshDisplayList();
        }
    }
}
