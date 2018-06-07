using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(60)]
    public class Algorithm_StringAdd_Equal : IAlgorithm2_1
    {
        public byte[] EnPwd_UTF8(List<byte[]> data)
        {
            return Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(data[0]) + Encoding.UTF8.GetString(data[1]));
        }
    }
}
