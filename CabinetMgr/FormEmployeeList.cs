using System;
using System.Windows.Forms;

namespace CabinetMgr
{
    public partial class FormEmployeeList : Form
    {
        public FormEmployeeList()
        {
            InitializeComponent();
        }

        private void FormEmployeeList_Load(object sender, EventArgs e)
        {
            /*
            IList userList = BllUserInfo.SearchUser();
            if (SqlDataHelper.IsDataValid(userList))
            {
                cEmployeeGrid.DataSource = userList;
            }*/
        }

        private void btnFpRegister_Click(object sender, EventArgs e)
        {
            FormFpRegister frForm = new FormFpRegister();
            frForm.RegUser = 1;
            frForm.RegIndex = 1;
            frForm.ShowDialog();
        }
    }
}
