using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using Utilities.Encryption;

namespace FKCollector
{
    /// <summary>
    /// INI�ļ���д�ࡣ
    /// </summary>
	public class IniHelper
	{
        // ��дINI�ļ���ء�
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);


        /// <summary>
        /// ��INIд�����ݡ�
        /// </summary>
        /// <PARAM name="Section">�ڵ�����</PARAM>
        /// <PARAM name="Key">������</PARAM>
        /// <PARAM name="Value">ֵ����</PARAM>
        public static void Write(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        public static void WriteEx(string section, string key, string value, string seed, string path)
        {
            string encValue = AesEncryption.EncryptAutoCp(value, seed);
            WritePrivateProfileString(section, key, "ENC:" + encValue, path);
        }


        /// <summary>
        /// ��ȡINI���ݡ�
        /// </summary>
        /// <PARAM name="Section">�ڵ�����</PARAM>
        /// <PARAM name="Key">������</PARAM>
        /// <PARAM name="Path">ֵ����</PARAM>
        /// <returns>��Ӧ��ֵ��</returns>
        public static string Read(string section, string key, string path)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();
        }

        public static string ReadEx(string section, string key, string seed, string path)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, path);
            string decValue = AesEncryption.DecryptAutoCp(temp.ToString(), seed);
            return decValue.TrimEnd('\0');
        }

        /// <summary>
        /// ��ȡһ��ini�������еĽ�
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetAllSectionNames(out string[] sections, string path)
        {
            int maxBuffer = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(maxBuffer);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, maxBuffer, path);
            if (bytesReturned == 0)
            {
                sections = null;
                return -1;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, bytesReturned);
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return 0;
        }

        /// <summary>
        /// �õ�ĳ���ڵ��������е�key��value���
        /// </summary>
        /// <param name="section"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetAllKeyValues(string section, out string[] keys, out string[] values, string path)
        {
            byte[] b = new byte[65535];

            GetPrivateProfileSection(section, b, b.Length, path);
            string s = Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            ArrayList result = new ArrayList();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            keys = new string[result.Count];
            values = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].ToString().Split(new char[] { '=' });
                if (item.Length == 2)
                {
                    keys[i] = item[0].Trim();
                    values[i] = item[1].Trim();
                }
                else if (item.Length == 1)
                {
                    keys[i] = item[0].Trim();
                    values[i] = "";
                }
                else if (item.Length == 0)
                {
                    keys[i] = "";
                    values[i] = "";
                }
            }

            return 0;
        }
	}
}
