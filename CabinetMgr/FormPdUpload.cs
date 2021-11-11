using System;
using System.IO;
using System.Windows.Forms;
// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormPdUpload : Form
    {
        public FormPdUpload()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (tbPdID.Text.Length == 0)
            {
                return;
            }
            if (!File.Exists(tbImagePath.Text))
            {
                return;
            }
            byte[] buffer;
            FileStream fs = File.OpenRead(tbImagePath.Text);
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            fs.Close();
            int result = 0;//BllProcessDefinition.SetProcessDefinitionImage(tbPdID.Text, buffer);
            if (result > 0)
            {
                MessageBox.Show("Done");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
