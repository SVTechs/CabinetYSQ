using System;
using System.IO;
using System.Text;

namespace CabinetMgr.Updater
{
    public class FileHelper
    {
        public static string GetMd5HashFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                return null;
            }
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}