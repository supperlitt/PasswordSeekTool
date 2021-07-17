using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PasswordSeekTool
{
    public class RSATool
    {
        public RSATool()
        {

        }
        /// <summary>
        /// KEY 结构体
        /// </summary>
        public struct RSAKEY
        {
            /// <summary>
            /// 公钥
            /// </summary>
            public string PublicKey
            {
                get;
                set;
            }
            /// <summary>
            /// 私钥
            /// </summary>
            public string PrivateKey
            {
                get;
                set;
            }
        }
        public RSAKEY GetKey()
        {
            //RSA密钥对的构造器  
            RsaKeyPairGenerator keyGenerator = new RsaKeyPairGenerator();

            //RSA密钥构造器的参数  
            RsaKeyGenerationParameters param = new RsaKeyGenerationParameters(
                Org.BouncyCastle.Math.BigInteger.ValueOf(3),
                new Org.BouncyCastle.Security.SecureRandom(),
                1024,   //密钥长度  
                25);
            //用参数初始化密钥构造器  
            keyGenerator.Init(param);
            //产生密钥对  
            AsymmetricCipherKeyPair keyPair = keyGenerator.GenerateKeyPair();
            //获取公钥和密钥  
            AsymmetricKeyParameter publicKey = keyPair.Public;
            AsymmetricKeyParameter privateKey = keyPair.Private;

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);


            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();

            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded("UTF-8");
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

            RSAKEY item = new RSAKEY()
            {
                PublicKey = Convert.ToBase64String(publicInfoByte),
                PrivateKey = Convert.ToBase64String(privateInfoByte)
            };
            return item;
        }
        private AsymmetricKeyParameter GetPublicKeyParameter(string s)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] publicInfoByte = Convert.FromBase64String(s);
            Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte);//这里也可以从流中读取，从本地导入   
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);
            return pubKey;
        }
        private AsymmetricKeyParameter GetPrivateKeyParameter(string s)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] privateInfoByte = Convert.FromBase64String(s);
            // Asn1Object priKeyObj = Asn1Object.FromByteArray(privateInfoByte);//这里也可以从流中读取，从本地导入   
            // PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);
            return priKey;
        }
        public string EncryptByKey(string s, string key, bool isPublic)
        {
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
            //加密  
            try
            {
                engine.Init(true, isPublic ? GetPublicKeyParameter(key) : GetPrivateKeyParameter(key));
                byte[] byteData = System.Text.Encoding.UTF8.GetBytes(s);

                int inputLen = byteData.Length;
                MemoryStream ms = new MemoryStream();
                int offSet = 0;
                byte[] cache;
                int i = 0;
                // 对数据分段加密
                while (inputLen - offSet > 0)
                {
                    if (inputLen - offSet > 117)
                    {
                        cache = engine.ProcessBlock(byteData, offSet, 117);
                    }
                    else
                    {
                        cache = engine.ProcessBlock(byteData, offSet, inputLen - offSet);
                    }
                    ms.Write(cache, 0, cache.Length);
                    i++;
                    offSet = i * 117;
                }
                byte[] encryptedData = ms.ToArray();

                //var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Convert.ToBase64String(encryptedData);
                //Console.WriteLine("密文（base64编码）:" + Convert.ToBase64String(testData) + Environment.NewLine);
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public string DecryptByPublicKey(string s, string key, bool isPublic)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());


            //加密  

            try
            {
                engine.Init(false, isPublic ? GetPublicKeyParameter(key) : GetPrivateKeyParameter(key));
                byte[] byteData = Convert.FromBase64String(s);

                int inputLen = byteData.Length;
                MemoryStream ms = new MemoryStream();
                int offSet = 0;
                byte[] cache;
                int i = 0;
                // 对数据分段加密
                while (inputLen - offSet > 0)
                {
                    if (inputLen - offSet > 128)
                    {
                        cache = engine.ProcessBlock(byteData, offSet, 128);
                    }
                    else
                    {
                        cache = engine.ProcessBlock(byteData, offSet, inputLen - offSet);
                    }
                    ms.Write(cache, 0, cache.Length);
                    i++;
                    offSet = i * 128;
                }
                byte[] encryptedData = ms.ToArray();

                //var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Encoding.UTF8.GetString(ms.ToArray());
                //Console.WriteLine("密文（base64编码）:" + Convert.ToBase64String(testData) + Environment.NewLine);
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">密匙</param>
        /// <returns></returns>
        public string SignByPrivateKey(string data, string key)
        {
            AsymmetricKeyParameter priKey = GetPrivateKeyParameter(key);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(data);

            ISigner normalSig = SignerUtilities.GetSigner("SHA1WithRSA");
            normalSig.Init(true, priKey);
            normalSig.BlockUpdate(byteData, 0, data.Length);
            byte[] normalResult = normalSig.GenerateSignature(); //签名结果
            return Convert.ToBase64String(normalResult);
            //return System.Text.Encoding.UTF8.GetString(normalResult);
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="plainData">验证数据</param>
        /// <param name="sign">签名</param>
        /// <param name="key">公匙</param>
        /// <returns></returns>
        public bool ValidationPublicKey(string plainData, string sign, string key)
        {
            AsymmetricKeyParameter priKey = GetPublicKeyParameter(key);

            byte[] signBytes = Convert.FromBase64String(sign);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainData);


            ISigner verifier = SignerUtilities.GetSigner("SHA1WithRSA");
            verifier.Init(false, priKey);
            verifier.BlockUpdate(plainBytes, 0, plainBytes.Length);

            return verifier.VerifySignature(signBytes); //验签结果
        }
    }
}
