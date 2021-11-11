using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable UnusedMember.Local
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace Utilities.Encryption
{
    public static class RsaHelper
    {
        /// <summary>  
        /// RSA的容器 可以解密的源字符串长度为 DWKEYSIZE/8-11   
        /// </summary>  
        public const int DwKeysize = 1024;
        private static string GenericIV = "1234567812345678";

        /// <summary>  
        /// RSA加密的密匙结构  公钥和私匙  
        /// </summary>  
        //public struct RSAKey  
        //{  
        //    public string PublicKey { get; set; }  
        //    public string PrivateKey { get; set; }  
        //}  

        public static string[] GenerateRsaKeys()
        {
            string[] rsaKeys = new string[2];
            RSACryptoServiceProvider rsaInst = new RSACryptoServiceProvider();
            rsaKeys[0] = rsaInst.ToXmlString(false);
            rsaKeys[1] = rsaInst.ToXmlString(true);
            return rsaKeys;
        }

        public static bool IsDataValid(string inputText)
        {
            for (int i = 0; i < inputText.Length; i++)
            {
                if (inputText[i] >= 'a' && inputText[i] <= 'z') continue;
                if (inputText[i] >= 'A' && inputText[i] <= 'Z') continue;
                if (char.IsDigit(inputText[i])) continue;
                if (char.IsWhiteSpace(inputText[i])) continue;
                return false;
            }
            return true;
        }

        #region 得到RSA的解谜的密匙对
        /// <summary>  
        /// 得到RSA的解谜的密匙对  
        /// </summary>  
        /// <returns></returns>  
        //public static RSAKey GetRASKey()  
        //{  
        //    RSACryptoServiceProvider.UseMachineKeyStore = true;  
        //    //声明一个指定大小的RSA容器  
        //    RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(DWKEYSIZE);  
        //    //取得RSA容易里的各种参数  
        //    RSAParameters p = rsaProvider.ExportParameters(true);  
        //    return new RSAKey()  
        //    {  
        //        PublicKey = ComponentKey(p.Exponent, p.Modulus),  
        //        PrivateKey = ComponentKey(p.D, p.Modulus)  
        //    };  
        //}  
        #endregion

        #region 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符
        /// <summary>  
        /// 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符  
        /// </summary>  
        /// <param name="source"></param>  
        /// <returns></returns>  
        public static bool CheckSourceValidate(string source)
        {
            return (DwKeysize / 8 - 11) >= source.Length;
        }
        #endregion

        #region 组合解析密匙
        /// <summary>  
        /// 组合成密匙字符串  
        /// </summary>  
        /// <param name="b1"></param>  
        /// <param name="b2"></param>  
        /// <returns></returns>  
        public static string ComponentKey(byte[] b1, byte[] b2)
        {
            List<byte> list = new List<byte>();
            //在前端加上第一个数组的长度值 这样今后可以根据这个值分别取出来两个数组  
            list.Add((byte)b1.Length);
            list.AddRange(b1);
            list.AddRange(b2);
            byte[] b = list.ToArray<byte>();
            return Convert.ToBase64String(b);
        }

        /// <summary>  
        /// 解析密匙  
        /// </summary>  
        /// <param name="key">密匙</param>  
        /// <param name="b1">RSA的相应参数1</param>  
        /// <param name="b2">RSA的相应参数2</param>  
        private static void ResolveKey(string key, out byte[] b1, out byte[] b2)
        {
            //从base64字符串 解析成原来的字节数组  
            byte[] b = Convert.FromBase64String(key);
            //初始化参数的数组长度  
            b1 = new byte[b[0]];
            b2 = new byte[b.Length - b[0] - 1];
            //将相应位置是值放进相应的数组  
            for (int n = 1, i = 0, j = 0; n < b.Length; n++)
            {
                if (n <= b[0])
                {
                    b1[i++] = b[n];
                }
                else
                {
                    b2[j++] = b[n];
                }
            }
        }
        #endregion

        private static string DeModPx(string encMod, string pubExp)
        {
            encMod = DecodeBase64("UTF-8", encMod);
            string baseHash = encMod.Substring(0, 16);
            string spHash = GetMd5(baseHash).Substring(0, 16);
            int spIndex = encMod.IndexOf(spHash);
            if (spIndex == -1) return "";
            string extFactor = GetMd5(pubExp).Substring(0, 24);
            string part2 = encMod.Substring(16, spIndex - 16);
            string part1 = encMod.Substring(spIndex + 16);
            string originMod = AxDecrypt(part1, extFactor, GenericIV) + AxDecrypt(part2, extFactor, GenericIV);
            if (!GetMd5(originMod).Substring(0, 16).Equals(baseHash)) return "";
            return originMod;
        }

        #region 字符串加密解密 公开方法

        /// <summary>  
        /// 字符串加密  
        /// </summary>  
        /// <param name="source">源字符串 明文</param>
        /// <param name="d"></param>
        /// <param name="modulus"></param>
        /// <returns>加密遇到错误将会返回空字符串</returns>  
        public static string EncryptString(string source, string d, string modulus)
        {
            try
            {
                if (!CheckSourceValidate(source))
                {
                    throw new Exception("source string too long");
                }
                BigInteger biN = BigInteger.Parse(modulus, NumberStyles.AllowHexSpecifier);
                BigInteger biD = BigInteger.Parse(d, NumberStyles.AllowHexSpecifier);
                return EncryptString(source, biD, biN);
            }
            catch
            {
                // ignored
            }
            return "";
        }

        /// <summary>
        /// 通过加密后的密钥解密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="pubExpPx"></param>
        /// <param name="modPx1"></param>
        /// <param name="modPx2"></param>
        /// <param name="modPx3"></param>
        /// <param name="modPx4"></param>
        /// <returns></returns>
        public static string DecryptStringPx(string encryptString, string pubExpPx, string modPx1, string modPx2,
            string modPx3, string modPx4)
        {
            try
            {
                string originExp = DecodeBase64("UTF-8", pubExpPx);
                if (originExp.Length < 16) return "";
                if (!GetMd5(originExp.Substring(16)).Substring(0, 16).Equals(originExp.Substring(0, 16))) return "";
                originExp = originExp.Substring(16);
                string modulus = DeModPx(modPx1, originExp) + DeModPx(modPx2, originExp) + DeModPx(modPx3, originExp)
                    + DeModPx(modPx4, originExp);
                BigInteger biE = BigInteger.Parse(originExp, NumberStyles.AllowHexSpecifier);
                BigInteger biN = BigInteger.Parse(modulus, NumberStyles.AllowHexSpecifier);
                string resStr = DecryptString(encryptString, biE, biN);
                int chkLength = resStr.Length > 16 ? 16 : resStr.Length;
                if (IsDataValid(resStr.Substring(0, chkLength)))
                {
                    return resStr;
                }
                return "";
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }

        /// <summary>  
        /// 字符串解密  
        /// </summary>  
        /// <param name="encryptString">密文</param>  
        /// <param name="exponent">密钥</param>
        /// <param name="modulus"></param>
        /// <returns>遇到解密失败将会返回空字符串</returns>  
        public static string DecryptString(string encryptString, string exponent, string modulus)
        {
            try
            {
                BigInteger biE = BigInteger.Parse(exponent, NumberStyles.AllowHexSpecifier);
                BigInteger biN = BigInteger.Parse(modulus, NumberStyles.AllowHexSpecifier);
                return DecryptString(encryptString, biE, biN);
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }
        #endregion

        #region 字符串加密解密 私有  实现加解密的实现方法
        /// <summary>  
        /// 用指定的密匙加密   
        /// </summary>  
        /// <param name="source">明文</param>  
        /// <param name="d">可以是RSACryptoServiceProvider生成的D</param>  
        /// <param name="n">可以是RSACryptoServiceProvider生成的Modulus</param>  
        /// <returns>返回密文</returns>  
        private static string EncryptString(string source, BigInteger d, BigInteger n)
        {
            int len = source.Length;
            int len1;
            int blockLen;
            if ((len % 128) == 0)
                len1 = len / 128;
            else
                len1 = len / 128 + 1;
            string block;
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < len1; i++)
            {
                if (len >= 128)
                    blockLen = 128;
                else
                    blockLen = len;
                block = source.Substring(i * 128, blockLen);
                byte[] oText = Encoding.Default.GetBytes(block);
                BigInteger biText = new BigInteger(oText);
                BigInteger biEnText = BigInteger.ModPow(biText, d, n);
                string temp = biEnText.ToString("X");
                result.Append(temp).Append("@");
                len -= blockLen;
            }
            return result.ToString().TrimEnd('@');
        }

        /// <summary>  
        /// 用指定的密匙加密   
        /// </summary>  
        /// <param name="encryptString"></param>
        /// <param name="e">可以是RSACryptoServiceProvider生成的Exponent</param>  
        /// <param name="n">可以是RSACryptoServiceProvider生成的Modulus</param>  
        /// <returns>返回明文</returns>  
        private static string DecryptString(string encryptString, BigInteger e, BigInteger n)
        {
            StringBuilder result = new StringBuilder();
            string[] strarr1 = encryptString.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strarr1.Length; i++)
            {
                string block = strarr1[i];
                BigInteger biText = BigInteger.Parse(block, NumberStyles.AllowHexSpecifier);
                BigInteger biEnText = BigInteger.ModPow(biText, e, n);
                string temp = Encoding.Default.GetString(biEnText.ToByteArray());
                result.Append(temp);
            }
            return result.ToString();
        }
        #endregion

        /// <summary>  
        /// 将字符串使用base64算法加密  
        /// </summary>  
        /// <param name="codeType">编码类型</param>  
        /// <param name="code">待加密的字符串</param>  
        /// <returns>加密后的字符串</returns>        
        public static string EncodeBase64(string codeType, string code)
        {
            string encode;
            byte[] bytes = Encoding.GetEncoding(codeType).GetBytes(code); //将一组字符编码为一个字节序列.  
            try
            {
                encode = Convert.ToBase64String(bytes); //将8位无符号整数数组的子集转换为其等效的,以64为基的数字编码的字符串形式.  
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>  
        /// 将字符串使用base64算法解密  
        /// </summary>  
        /// <param name="codeType">编码类型</param>  
        /// <param name="code">已用base64算法加密的字符串</param>  
        /// <returns>解密后的字符串</returns>  
        public static string DecodeBase64(string codeType, string code)
        {
            string decode;
            byte[] bytes = Convert.FromBase64String(code); //将2进制编码转换为8位无符号整数数组.  
            try
            {
                decode = Encoding.GetEncoding(codeType).GetString(bytes); //将指定字节数组中的一个字节序列解码为一个字符串。  
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        /// <summary>  
        /// 读取公钥或私钥  
        /// </summary>  
        /// <param name="includePrivateparameters">为True则包含私钥</param>  
        /// <param name="path">Xml格式保存的完整公/私钥路径</param>  
        /// <returns>公钥或私钥参数形式 </returns>  
        public static RSAParameters ReadKey(bool includePrivateparameters, string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string publickey = reader.ReadToEnd();
                RSACryptoServiceProvider rcp = new RSACryptoServiceProvider();
                rcp.FromXmlString(publickey);
                return rcp.ExportParameters(includePrivateparameters);
            }
        }

        public static string AxEncrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AxDecrypt(string toDecrypt, string key, string iv)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        public static string GetMd5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = "";

            for (int i = 0; i < targetData.Length; i++)
            {
                string hexStr = targetData[i].ToString("X");
                if (hexStr.Length == 1) hexStr = "0" + hexStr;
                byte2String += hexStr;
            }
            return byte2String;
        }
    }
}
