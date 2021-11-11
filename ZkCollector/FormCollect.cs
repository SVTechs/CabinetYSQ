using System;
using System.Drawing;
using System.Windows.Forms;
using Hardware.DeviceInterface;

// ReSharper disable LocalizableElement

namespace ZkCollector
{
    public partial class FormCollect : Form
    {
        public int EngineVersion = 0;
        public string Feature { get; protected set; }

        public FormCollect()
        {
            InitializeComponent();
        }

        private void FormCollect_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            if (EngineVersion == 9)
            {
                FpZkDevice.SetEngine(FpZkDevice.EngineType.V9);
            }
            else
            {
                FpZkDevice.SetEngine(FpZkDevice.EngineType.V10);
            }
            if (FpZkDevice.StartEnroll() != 1)
            {
                MessageBox.Show("启动注册失败");
                Close();
                return;
            }
            FpCallBack.OnEnrollProgressChanged += OnEnrollProgressChanged;
            FpCallBack.OnEnroll += OnEnroll;
            FpCallBack.OnImageReceived += OnImageReceived;
        }

        private void OnImageReceived(Bitmap zkImage)
        {
            pbFpImage.BackgroundImage = zkImage;
        }

        private void OnEnroll(byte[] zkFeature)
        {
            Feature = Convert.ToBase64String(zkFeature);
            Close();
        }

        private void OnEnrollProgressChanged(int pressLeft, int score)
        {
            lbProgress.Text = $"还需按压{pressLeft}次";
        }

        private void FormCollect_Shown(object sender, EventArgs e)
        {
            if (EngineVersion == 0)
            {
                MessageBox.Show("未设置引擎版本");
                Close();
                return;
            }
        }
    }
}
