using System;
using System.Collections;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace Utilities.Encryption
{
    public class JRsaHelper
    {
        public static Hashtable GenerateKeyPair(int keyLength)
        {
            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            RsaKeyGenerationParameters rsaKeyGenerationParameters = new RsaKeyGenerationParameters(BigInteger.ValueOf(10001), new SecureRandom(), keyLength, 25);
            rsaKeyPairGenerator.Init(rsaKeyGenerationParameters);//初始化参数   
            AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();
            AsymmetricKeyParameter publicKey = keyPair.Public;//公钥   
            AsymmetricKeyParameter privateKey = keyPair.Private;//私钥   

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();
            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded();
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded();

            Hashtable ht = new Hashtable();
            ht["PrivateKey"] = Convert.ToBase64String(privateInfoByte);
            ht["PublicKey"] = Convert.ToBase64String(publicInfoByte);
            return ht;
        }

        /// <summary>  
        /// 字符串加密  
        /// </summary>  
        /// <returns>加密遇到错误将会返回空字符串</returns>  
        public static string EncryptString(string source, string pubKey, int keyLength = 2048)
        {
            byte[] publicInfoByte = Convert.FromBase64String(pubKey);
            Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte);//这里也可以从流中读取，从本地导入   
            AsymmetricKeyParameter pubKeyPara = PublicKeyFactory.CreateKey(SubjectPublicKeyInfo.GetInstance(pubKeyObj));
            IAsymmetricBlockCipher cipher = new RsaEngine();
            cipher.Init(true, pubKeyPara);//true表示加密   
            byte[] plainData = Encoding.Unicode.GetBytes(source);
            StringBuilder output = new StringBuilder();
            int dataLength = plainData.Length;
            int encLength = keyLength / 16;
            for (int i = 0; i < dataLength; i += encLength)
            {
                int takeLength = dataLength - i < encLength ? dataLength - i : encLength;
                byte[] encryptData = cipher.ProcessBlock(plainData.Skip(i).Take(takeLength).ToArray(), 0, takeLength);
                output.Append(Convert.ToBase64String(encryptData) + "|");
            }
            return output.ToString().TrimEnd('|');
        }

        /// <summary>  
        /// 字符串解密  
        /// </summary>  
        /// <returns>遇到解密失败将会返回空字符串</returns>  
        public static string DecryptString(string encryptedString, string priKey)
        {
            byte[] privateInfoByte = Convert.FromBase64String(priKey);
            AsymmetricKeyParameter priKeyPara = PrivateKeyFactory.CreateKey(privateInfoByte);
            IAsymmetricBlockCipher cipher = new RsaEngine();
            cipher.Init(false, priKeyPara);
            string[] blockList = encryptedString.Split('|');
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < blockList.Length; i++)
            {
                byte[] encryptedData = Convert.FromBase64String(blockList[i]);
                output.Append(Encoding.Unicode.GetString(cipher.ProcessBlock(encryptedData, 0, encryptedData.Length)));
            }
            return output.ToString();
        }

        /// <summary>  
        /// 字符串加密  
        /// </summary>  
        /// <returns>加密遇到错误将会返回空字符串</returns>  
        public static string EncryptStringRx(string source, string priKey, int keyLength = 2048)
        {
            byte[] privateInfoByte = Convert.FromBase64String(priKey);
            AsymmetricKeyParameter priKeyPara = PrivateKeyFactory.CreateKey(privateInfoByte);
            IAsymmetricBlockCipher cipher = new RsaEngine();
            cipher.Init(true, priKeyPara);
            byte[] plainData = Encoding.Unicode.GetBytes(source);
            StringBuilder output = new StringBuilder();
            int dataLength = plainData.Length;
            int encLength = keyLength / 16;
            for (int i = 0; i < dataLength; i += encLength)
            {
                int takeLength = dataLength - i < encLength ? dataLength - i : encLength;
                byte[] encryptData = cipher.ProcessBlock(plainData.Skip(i).Take(takeLength).ToArray(), 0, takeLength);
                output.Append(Convert.ToBase64String(encryptData) + "|");
            }
            return output.ToString().TrimEnd('|');
        }

        /// <summary>  
        /// 字符串解密  
        /// </summary>  
        /// <returns>遇到解密失败将会返回空字符串</returns>  
        public static string DecryptStringRx(string encryptedString, string pubKey, int inputFormat)
        {
            byte[] publicInfoByte = Convert.FromBase64String(pubKey);
            Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte);//这里也可以从流中读取，从本地导入   
            AsymmetricKeyParameter pubKeyPara = PublicKeyFactory.CreateKey(SubjectPublicKeyInfo.GetInstance(pubKeyObj));
            IAsymmetricBlockCipher cipher = new RsaEngine();
            cipher.Init(false, pubKeyPara);
            string[] blockList = encryptedString.Split('|');
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < blockList.Length; i++)
            {
                byte[] encryptedData = Convert.FromBase64String(blockList[i]);
                output.Append(Encoding.Unicode.GetString(cipher.ProcessBlock(encryptedData, 0, encryptedData.Length)));
            }
            return output.ToString();
        }

        public static string ByteToString(byte[] inBytes)
        {
            string stringOut = "";
            foreach (byte inByte in inBytes)
            {
                stringOut = stringOut + string.Format("{0:X2} ", inByte);
            }
            return stringOut;
        }

        public string ByteToString(byte[] inBytes, int len)
        {
            string stringOut = "";
            for (int i = 0; i < len; i++)
            {
                stringOut = stringOut + string.Format("{0:X2} ", inBytes[i]);
            }
            return stringOut;
        }

        // 把十六进制字符串转换成字节型  
        public static byte[] StringToByte(string inString)
        {
            var byteStrings = inString.Split(" ".ToCharArray());
            var byteOut = new byte[byteStrings.Length - 1];
            for (int i = 0; i == byteStrings.Length - 1; i++)
            {
                byteOut[i] = Convert.ToByte(("0x" + byteStrings[i]));
            }
            return byteOut;
        }
    }
}
