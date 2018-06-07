using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(100)]
    public class Algorithm_Hex : IAlgorithm1_1
    {
        public byte[] EnPwd_UTF8(byte[] data)
        {
            StringBuilder ret = new StringBuilder();
            foreach (byte b in data)
            {
                //{0:X2} 大写
                ret.AppendFormat("{0:x2}", b);
            }

            return Encoding.UTF8.GetBytes(ret.ToString());
        }
    }
}
