using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordSeekTool
{
    public class DESHelper
    {
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static byte[] Encrypt(byte[] Text, byte[] sKey, byte[] iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.Zeros, int ivsize = 8)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray;
                inputByteArray = Text;
                des.Key = sKey.Take(ivsize).ToArray();
                des.IV = iv.Take(ivsize).ToArray();
                des.Mode = cipherMode;
                des.Padding = paddingMode;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();

                    return ms.ToArray();
                }
            }
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static byte[] Encrypt_ECB(byte[] Text, byte[] sKey, int keysize = 8)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray;
                inputByteArray = Text;
                des.Key = sKey.Take(8).ToArray();
                des.Mode = CipherMode.ECB;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();

                    return ms.ToArray();
                }
            }
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static byte[] Decrypt(byte[] Text, byte[] sKey, byte[] iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.Zeros, int ivsize = 8)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = Text;
                des.Key = sKey.Take(ivsize).ToArray();
                des.IV = iv.Take(ivsize).ToArray();
                des.Mode = cipherMode;
                des.Padding = paddingMode;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
}
