using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    public interface IAlgorithm1_2
    {
        List<byte[]> EnPwd_UTF8(byte[] data);
    }
}
