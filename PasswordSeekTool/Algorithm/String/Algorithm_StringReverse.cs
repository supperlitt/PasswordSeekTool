using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    /// <summary>
    /// 字符串-反向
    /// </summary>
    [Heat(70)]
    public class Algorithm_StringReverse : IAlgorithm1_1
    {
        public byte[] EnPwd_UTF8(byte[] data)
        {
            string str = Encoding.UTF8.GetString(data);
            StringBuilder newStr = new StringBuilder();
            foreach (var c in str)
            {
                newStr.Insert(0, c.ToString());
            }

            return Encoding.UTF8.GetBytes(newStr.ToString());
        }
    }
}
