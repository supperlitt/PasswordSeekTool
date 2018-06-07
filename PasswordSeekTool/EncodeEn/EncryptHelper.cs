using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordSeekTool
{
    /// <summary>
    /// DES加密/解密类。
    /// Copyright (C) weikebuluo
    /// </summary>
    public class EncryptHelper
    {
        public static string GetMD5(byte[] md5_data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] value, hash;
            value = md5_data;
            hash = md.ComputeHash(value);
            md.Clear();
            string temp = "";
            for (int i = 0, len = hash.Length; i < len; i++)
            {
                temp += hash[i].ToString("X").PadLeft(2, '0');
            }

            return temp;
        }

        public static string GetSHA1(byte[] data)
        {
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bytResult = sha.ComputeHash(data);

            //转换成字符串，32位  
            string strResult = BitConverter.ToString(bytResult);
            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉  
            strResult = strResult.Replace("-", "");

            return strResult;
        }

        public static string GetSHA256(byte[] data)
        {
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            byte[] bytResult = sha.ComputeHash(data);
            //转换成字符串，32位  
            string strResult = BitConverter.ToString(bytResult);

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉  
            strResult = strResult.Replace("-", "");

            return strResult;
        }

        public static string GetSHA384(byte[] data)
        {
            System.Security.Cryptography.SHA384 sha = new System.Security.Cryptography.SHA384CryptoServiceProvider();
            byte[] bytResult = sha.ComputeHash(data);
            //转换成字符串，32位  
            string strResult = BitConverter.ToString(bytResult);
            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉  
            strResult = strResult.Replace("-", "");

            return strResult;
        }

        public static string GetSHA512(byte[] data)
        {
            System.Security.Cryptography.SHA512 sha = new System.Security.Cryptography.SHA512CryptoServiceProvider();
            byte[] bytResult = sha.ComputeHash(data);

            //转换成字符串，32位  
            string strResult = BitConverter.ToString(bytResult);
            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉  
            strResult = strResult.Replace("-", "");

            return strResult;
        }
    }
}
