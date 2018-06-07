using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(70)]
    public class Algorithm_DES_ECB_Zeros_8 : IAlgorithm2_1_Limit
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            try
            {
                return DESHelper.Encrypt_ECB(data[0], data[1], 8);
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
