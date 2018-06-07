using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(80)]
    public class Algorithm_RSA_PEM_Hex_100_Min : IAlgorithm_RSA
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            string input_str = Encoding.UTF8.GetString(data[0]);
            string public_key = Encoding.UTF8.GetString(data[1]);
            try
            {
                // StartWith -----BEGIN PUBLIC KEY-----  216  EndWith -----END PUBLIC KEY-----
                return RSAHelper.Encrypt_PEM_Hex_Min(data[0], Encoding.UTF8.GetString(data[1]));
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
