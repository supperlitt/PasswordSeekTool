using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(60)]
    public class Algorithm_SHA512 : IAlgorithm1_1
    {
        public byte[] EnPwd_UTF8(byte[] data)
        {
            return Encoding.UTF8.GetBytes(EncryptHelper.GetSHA512(data));
        }
    }
}
