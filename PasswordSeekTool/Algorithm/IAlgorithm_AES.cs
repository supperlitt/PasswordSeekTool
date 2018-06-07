using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    public interface IAlgorithm_AES
    {
        byte[] EnPwd_UTF8(List<byte[]> data);
    }
}
