using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(100)]
    public class Algorithm_MD5 : IAlgorithm1_1
    {
        public byte[] EnPwd_UTF8(byte[] data)
        {
            return Encoding.UTF8.GetBytes(EncryptHelper.GetMD5(data));
        }
    }
}
