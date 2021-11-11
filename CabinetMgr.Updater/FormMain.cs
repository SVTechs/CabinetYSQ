using System;
using System.Threading;
using System.Windows.Forms;

// ReSharper disable LocalizableElement

namespace CabinetMgr.Updater
{
    public partial class FormMain : Form
    {
        private int _msgCount = 1;

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            UpdateManager.ShowMsg = AddMsgToList;
            UpdateManager.SwitchToDl = SetListToDlMode;
            UpdateManager.SwitchToMsg = SetListToMsgMode;
            UpdateManager.ClearMsg = ClearList;
            UpdateManager.UpdateMsg = UpdateProgressByTargetCol;
            UpdateManager.CleanUp();

            if (AppConfig.LoadConfig() == 0)
            {
                AddMsgToList("错误", "获取配置信息失败", "");
                UpdateManager.RestartApp();
                return;
            }
            Text = string.Format("自动更新 ({0}-{1})", AppConfig.AppUnit, AppConfig.AppName);
            Thread updThread = new Thread(UpdateManager.UpdateDispatcher);
            updThread.IsBackground = true;
            updThread.Start();
        }

        private string GetMsgCount()
        {
            string msgStr = _msgCount.ToString();
            _msgCount++;
            return msgStr;
        }

        private void ClearList()
        {
            if (lv_StatusList.InvokeRequired)
            {
                UpdateManager.MsgSwitchDelegate d = ClearList;
                lv_StatusList.Invoke(d);
            }
            else
            {
                while (lv_StatusList.Items.Count != 0)
                {
                    lv_StatusList.Items[0].Remove();
                }
                _msgCount = 1;
            }
        }

        private void SetListToMsgMode()
        {
            if (lv_StatusList.InvokeRequired)
            {
                UpdateManager.MsgSwitchDelegate d = SetListToMsgMode;
                lv_StatusList.Invoke(d);
            }
            else
            {
                lv_StatusList.Columns[1].Text = "状态";
                lv_StatusList.Columns[2].Text = "详情";
                lv_StatusList.Columns[3].Text = "备注";
            }
        }

        private void SetListToDlMode()
        {
            if (lv_StatusList.InvokeRequired)
            {
                UpdateManager.MsgSwitchDelegate d = SetListToDlMode;
                lv_StatusList.Invoke(d);
            }
            else
            {
                lv_StatusList.Columns[1].Text = "文件";
                lv_StatusList.Columns[2].Text = "路径";
                lv_StatusList.Columns[3].Text = "进度";
            }
        }

        private void UpdateProgressByTargetCol(int sigColNo, string sigColInfo, int prgPosition, string newPrg,
            int curCount, int fullCount)
        {
            if (lv_StatusList.InvokeRequired)
            {
                UpdateManager.UpdateMsgDelegate d = UpdateProgressByTargetCol;
                lv_StatusList.Invoke(d, sigColNo, sigColInfo, prgPosition, newPrg, curCount, fullCount);
            }
            else
            {
                for (int i = 0; i < lv_StatusList.Items.Count; i++)
                {
                    if (lv_StatusList.Items[i].SubItems[sigColNo].Text.Equals(sigColInfo))
                    {
                        if (!lv_StatusList.Items[i].SubItems[prgPosition].Text.Equals(newPrg))
                        {
                            lv_StatusList.Items[i].SubItems[prgPosition].Text = newPrg;
                        }
                        UpdateCurProgress(curCount);
                        UpdateFullProgress(fullCount);
                        return;
                    }
                }
            }
        }

        private void AddMsgToList(string col2, string col3, string col4)
        {
            if (lv_StatusList.InvokeRequired)
            {
                UpdateManager.AddMsgDelegate d = AddMsgToList;
                lv_StatusList.Invoke(d, col2, col3, col4);
            }
            else
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = GetMsgCount();
                lvi.SubItems.Add(col2);
                lvi.SubItems.Add(col3);
                lvi.SubItems.Add(col4);
                lv_StatusList.Items.Add(lvi);
                lv_StatusList.EnsureVisible(lv_StatusList.Items.Count - 1);
            }
        }

        public delegate void ProgressDelegate(int percent);
        public void UpdateCurProgress(int percent)
        {
            if (m_CurProgress.InvokeRequired)
            {
                ProgressDelegate d = UpdateCurProgress;
                m_CurProgress.Invoke(d, percent);
            }
            else
            {
                m_CurProgress.Value = percent;
            }
        }

        public void UpdateFullProgress(int percent)
        {
            if (m_FullProgress.InvokeRequired)
            {
                ProgressDelegate d = UpdateFullProgress;
                m_FullProgress.Invoke(d, percent);
            }
            else
            {
                m_FullProgress.Value = percent;
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AppConfig.AllowClose)
            {
                e.Cancel = true;
                return;
            }
            DialogResult dr = MessageBox.Show("中断升级可能造成程序运行异常,您确定吗?", "警告", MessageBoxButtons.OKCancel);
            if (dr != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
