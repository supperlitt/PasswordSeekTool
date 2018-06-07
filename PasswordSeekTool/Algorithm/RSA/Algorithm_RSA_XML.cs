using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(90)]
    public class Algorithm_RSA_XML : IAlgorithm_RSA
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            try
            {
                // StartWith <RSAKeyValue> EndWith </RSAKeyValue>
                return RSAHelper.Encrypt_XML(data[0], Encoding.UTF8.GetString(data[1]));
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
