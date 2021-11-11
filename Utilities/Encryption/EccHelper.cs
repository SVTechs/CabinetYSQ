using System;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace Utilities.Encryption
{
    /// <summary>
    /// 未完成
    /// </summary>
    public class EccHelper
    {
        private static AsymmetricCipherKeyPair _commonKey;

        public static string Encrypt(string plain, string encKey)
        {
            if (_commonKey == null)
            {
                _commonKey = GenerateKeyPair();
            }
            byte[] plainBytes = Encoding.Default.GetBytes(plain + "<|ECIEND|>");
            byte []encKeyByte = Convert.FromBase64String(encKey);
            AsymmetricKeyParameter akEncKey = PrivateKeyFactory.CreateKey(encKeyByte);

            BufferedIesCipher cipher = (BufferedIesCipher)CipherUtilities.GetCipher("ECIES");
            cipher.UpdateKey(true, akEncKey, _commonKey.Public);
            byte[] outputBytes = cipher.DoFinal(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(outputBytes);
        }

        public static string Decrypt(string enc, string decKey)
        {
            if (_commonKey == null)
            {
                _commonKey = GenerateKeyPair();
            }
            byte[] encBytes = Convert.FromBase64String(enc);
            byte[] decKeyByte = Convert.FromBase64String(decKey);
            Asn1Object decKeyObj = Asn1Object.FromByteArray(decKeyByte);
            AsymmetricKeyParameter akDecKey = PublicKeyFactory.CreateKey(SubjectPublicKeyInfo.GetInstance(decKeyObj));

            BufferedIesCipher cipher = (BufferedIesCipher)CipherUtilities.GetCipher("ECIES");
            cipher.UpdateKey(false, _commonKey.Private, akDecKey);
            byte[] outputBytes = cipher.DoFinal(encBytes, 0, encBytes.Length);
            string outputString = Encoding.Default.GetString(outputBytes);
            int endPosition = outputString.IndexOf("<|ECIEND|>");
            if (endPosition != -1)
            {
                return outputString.Substring(0, endPosition);
            }
            return "";
        }

        private static AsymmetricCipherKeyPair GenerateKeyPair()
        {
            IAsymmetricCipherKeyPairGenerator g = GeneratorUtilities.GetKeyPairGenerator("ECIES");
            X9ECParameters ecP = NistNamedCurves.GetByName("P-521");
            var ecSpec = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.H, ecP.GetSeed());
            g.Init(new ECKeyGenerationParameters(ecSpec, new SecureRandom()));
            return g.GenerateKeyPair();
        }

        /*
        private static void SaveKey()
        {
            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(_commonKey.Public);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(_commonKey.Private);

            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();
            string publicInfoByte = Convert.ToBase64String(asn1ObjectPublic.GetEncoded());
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            string privateInfoByte = Convert.ToBase64String(asn1ObjectPrivate.GetEncoded());  
        }*/
    }
}
