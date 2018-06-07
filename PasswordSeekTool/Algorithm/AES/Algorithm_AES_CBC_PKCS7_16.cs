using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(90)]
    public class Algorithm_AES_CBC_PKCS7_16 : IAlgorithm_AES
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            try
            {
                return AESHelper.AESEncrypt(data[0], data[1], data[2]);
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
