using System;
using System.Drawing;
using System.Windows.Forms;
using Hardware.DeviceInterface;

// ReSharper disable LocalizableElement
// ReSharper disable ArrangeThisQualifier

namespace CabinetMgr
{
    public partial class FormFpRegister : Form
    {
        public int RegUser = -1, RegIndex = -1;

        public FormFpRegister()
        {
            InitializeComponent();
        }

        private void FormFpRegister_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            if (RegUser < 0 || RegIndex < 0)
            {
                MessageBox.Show("注册用户设置错误");
                this.Close();
            }

            FpZkDevice.StartEnroll();
            FpCallBack.OnImageReceived = ImageReceived;
            FpCallBack.OnEnrollProgressChanged = EnrollProgressChanged;
            //FpCallBack.OnEnroll = EnrollFinished;
        }

        public void ImageReceived(Bitmap fpImage)
        {
            pbFpImage.Image = fpImage;
        }

        public void EnrollProgressChanged(int pressLeft, int score)
        {
            lbRepeatCount.Text = string.Format("{0}次", pressLeft);
        }
        
        public void EnrollFinished(byte[] feature)
        {
            lbRepeatCount.Text = "0次";
            if (feature == null)
            {
                MessageBox.Show("注册失败,指纹不清晰");
                this.Close();
                return;
            }
        }
    }
}
