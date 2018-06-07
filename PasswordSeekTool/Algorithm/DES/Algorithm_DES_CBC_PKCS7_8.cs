using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(90)]
    public class Algorithm_DES_CBC_PKCS7_8 : IAlgorithm2_1_Limit
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            try
            {
                return DESHelper.Encrypt(data[0], data[1], data[2], System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7, 8);
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
