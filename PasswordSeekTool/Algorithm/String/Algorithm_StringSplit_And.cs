using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(80)]
    public class Algorithm_StringSplit_And : IAlgorithm1_2
    {
        public List<byte[]> EnPwd_UTF8(byte[] data)
        {
            string[] array = Encoding.UTF8.GetString(data).Split(new char[] { '&' });
            List<byte[]> result = new List<byte[]>();
            string new_str = string.Empty;
            foreach (var str in array)
            {
                result.Add(Encoding.UTF8.GetBytes(str));
                new_str += str;
            }

            result.Add(Encoding.UTF8.GetBytes(new_str));
            return result;
        }
    }
}
