using System.Drawing;
using System.Windows.Forms;

namespace CabinetMgr
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, System.EventArgs e)
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            if (height == 1920 && width == 1080)
            {
                panel1.Size = new Size(865, 1623);

            }
            if (height == 768 && width == 1024)
            {
                panel1.Size = new Size(813, 557);

            }
        }
    }
}
