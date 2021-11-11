using System;
using System.Windows.Forms;
using CabinetMgr.RtVars;

// ReSharper disable LocalizableElement

namespace CabinetMgr
{
    public partial class FormInputPrompt : Form
    {
        public string RequiredItem = "";

        public FormInputPrompt()
        {
            InitializeComponent();
        }

        private void FormInoputPrompt_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            VarFormInputPrompt.InputContent = "";
            Text += RequiredItem;
            lbItemName.Text = RequiredItem + ":";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            VarFormInputPrompt.InputContent = tbItemContent.Text;
            Close();
        }
    }
}
