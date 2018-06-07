using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(80)]
    public class Algorithm_XXTEA_Normal : IAlgorithm2_1
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            string input_str = Encoding.UTF8.GetString(data[0]);
            string public_key = Encoding.UTF8.GetString(data[1]);
            try
            {
                string result = XXTEA_CSDN.Encrypt(input_str, public_key);

                return Encoding.UTF8.GetBytes(result);
            }
            catch (Exception)
            {
                return Encoding.UTF8.GetBytes("");
            }
        }
    }
}
