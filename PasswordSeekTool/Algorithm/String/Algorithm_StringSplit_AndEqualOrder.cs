using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool.Algorithm
{
    [Heat(80)]
    public class Algorithm_StringSplit_AndEqualOrder : IAlgorithm1_2
    {
        public List<byte[]> EnPwd_UTF8(byte[] data)
        {
            string[] array = Encoding.UTF8.GetString(data).Split(new char[] { '&' });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var str in array)
            {
                string[] strArray = str.Split(new char[] { '=' }, StringSplitOptions.None);
                if (strArray.Length == 2)
                {
                    if (!dic.ContainsKey(strArray[0]))
                    {
                        dic.Add(strArray[0], strArray[1]);
                    }
                }
                else
                {
                    if (!dic.ContainsKey(strArray[0]))
                    {
                        dic.Add(strArray[0], "");
                    }
                }
            }

            string new_str_asc = string.Empty;
            string new_str_desc = string.Empty;
            string[] keyArray = dic.Keys.ToArray();
            Array.Sort(keyArray, string.CompareOrdinal);
            foreach (var item in keyArray)
            {
                new_str_asc = new_str_asc + dic[item];
                new_str_desc = dic[item] + new_str_desc;
            }

            List<byte[]> result = new List<byte[]>();
            result.Add(Encoding.UTF8.GetBytes(new_str_asc));
            result.Add(Encoding.UTF8.GetBytes(new_str_desc));

            return result;
        }
    }
}
