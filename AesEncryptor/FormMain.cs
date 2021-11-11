using System;
using System.Windows.Forms;
using Utilities.Encryption;

// ReSharper disable LocalizableElement

namespace AesEncryptor
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }
         
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (tbPlain.Text.Length == 0)
            {
                MessageBox.Show("请输入待加密数据");
                return;
            }
            if (tbPassword.Text.Length == 0)
            {
                MessageBox.Show("请输入加密密码");
                return;
            }
            string encKey = GenerateKey(tbPassword.Text);
            tbResult.Text = AesEncryption.Encrypt(tbPlain.Text, encKey, "1234567812345678");
            tbIniResult.Text = "ENC:" + AesEncryption.EncryptAutoCp(tbResult.Text, tbPassword.Text);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (tbPlain.Text.Length == 0)
            {
                MessageBox.Show("请输入待解密数据");
                return;
            }
            if (tbPassword.Text.Length == 0)
            {
                MessageBox.Show("请输入加密密码");
                return;
            }
            string encBase = tbPlain.Text;
            if (tbPlain.Text.StartsWith("ENC:"))
            {
                encBase = AesEncryption.DecryptAutoCp(tbPlain.Text.Substring(4), tbPassword.Text);
            }
            string decKey = GenerateKey(tbPassword.Text);
            try
            {
                tbResult.Text = AesEncryption.Decrypt(encBase, decKey, "1234567812345678");
            }
            catch (Exception)
            {
                tbResult.Text = "";
                MessageBox.Show("解密失败，请检查输入");
            }
            tbIniResult.Text = "";
        }

        private string GenerateKey(string password)
        {
            int sumSeed = 0;
            for (int i = 0; i < password.Length; i++)
            {
                sumSeed += password[i] ^ 0xE5;
            }
            Random rnd = new Random(sumSeed);
            while (true)
            {
                sumSeed = rnd.Next() % 1000000;
                if (sumSeed > 100000) break;
            }
            var encSeed = sumSeed.ToString();
            return Md5Encode.Encode(encSeed, false).Substring(0, 16);
        }
    }
}
