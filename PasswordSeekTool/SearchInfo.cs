using PasswordSeekTool.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool
{
    public class SearchInfo
    {
        private List<InputInfo> inputList = new List<InputInfo>();

        public List<InputInfo> InputList
        {
            get { return inputList; }
            set { inputList = value; }
        }

        private OutputInfo outInfo = new OutputInfo();

        public OutputInfo OutInfo
        {
            get { return outInfo; }
            set { outInfo = value; }
        }

        public int Deep { get; set; }
    }

    public class InputInfo
    {
        public InputInfo(string content)
        {
            this.pid = 0;
            this.id = CacheIndex.GetIndex();
            this.content = content;
        }

        public InputInfo(string content, int pid, Type ial)
        {
            this.content = content;
            this.pid = pid;
            this.alg = ial;
            this.id = CacheIndex.GetIndex();
        }

        public int id { get; set; }

        public int pid { get; set; }

        public Type alg { get; set; }

        public string algkey { get; set; }

        /// <summary>
        /// 原始数据
        /// 全小写
        /// 全大写
        /// 三中数据，一个字符串有三个任务的情况
        /// </summary>
        public string content { get; set; }

        public override string ToString()
        {
            string str = string.Format("id:{0};pid:{1};alg:{2};content:{3}", id, pid, alg, content);

            return str;
        }
    }

    public class OutputInfo
    {
        public int OutType = 0;

        private string outString = string.Empty;

        public string OutString
        {
            get { return outString; }
            set { outString = value; }
        }

        public List<string> CheckStringList = new List<string>();

        private bool isInit = false;

        public void InitCheckList()
        {
            if (!isInit)
            {
                isInit = true;
                CheckStringList.Add(outString);
                try
                {
                    string base64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(outString));
                    CheckStringList.Add(base64Str);
                }
                catch { }

                try
                {
                    string hexStr = HexUtil.Byte2Hex(Encoding.UTF8.GetBytes(outString));
                    CheckStringList.Add(hexStr);
                }
                catch { }

                try
                {
                    var data = Convert.FromBase64String(outString);
                    string hexStr2 = HexUtil.Byte2Hex(data);
                    string real_data = Encoding.UTF8.GetString(data);
                    CheckStringList.Add(hexStr2);
                    CheckStringList.Add(real_data);
                }
                catch { }

                try
                {
                    var data = HexUtil.Read16Byte(outString);
                    string real_data = Encoding.UTF8.GetString(data);
                    CheckStringList.Add(real_data);
                }
                catch { }
            }
        }
    }

    public class CacheIndex
    {
        private static int index = 0;

        private static object lockObj = new object();

        public static int GetIndex()
        {
            lock (lockObj)
            {
                index++;

                return index;
            }
        }
    }
}
