using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(80)]
    public class Algorithm_RSA_PEM_2048 : IAlgorithm_RSA
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            string input_str = Encoding.UTF8.GetString(data[0]);
            string public_key = Encoding.UTF8.GetString(data[1]);
            try
            {
                RSACrypto rsaCrypto = new RSACrypto("", public_key);

                // StartWith -----BEGIN PUBLIC KEY-----  392  EndWith -----END PUBLIC KEY-----
                return Encoding.UTF8.GetBytes(rsaCrypto.Encrypt(input_str));
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
