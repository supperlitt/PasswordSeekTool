using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(80)]
    public class Algorithm_StringSplit_AndEqual : IAlgorithm1_2
    {
        public List<byte[]> EnPwd_UTF8(byte[] data)
        {
            string[] array = Encoding.UTF8.GetString(data).Split(new char[] { '&' });
            List<byte[]> result = new List<byte[]>();
            string new_str_key = string.Empty;
            string new_str_value = string.Empty;
            foreach (var str in array)
            {
                string[] strArray = str.Split(new char[] { '=' }, StringSplitOptions.None);
                if (strArray.Length == 2)
                {
                    result.Add(Encoding.UTF8.GetBytes(str));
                    result.Add(Encoding.UTF8.GetBytes(strArray[0]));
                    result.Add(Encoding.UTF8.GetBytes(strArray[1]));

                    new_str_key += strArray[0];
                    new_str_value += strArray[1];
                }
                else
                {
                    new_str_key += strArray[0];
                }
            }

            result.Add(Encoding.UTF8.GetBytes(new_str_key));
            result.Add(Encoding.UTF8.GetBytes(new_str_value));
            return result;
        }
    }
}
