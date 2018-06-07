using PasswordSeekTool.Algorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PasswordSeekTool
{
    public partial class MainFrm : Form
    {
        private List<Type> class_list1_1 = new List<Type>();
        private List<Type> class_list1_2 = new List<Type>();
        private List<Type> class_list2_1 = new List<Type>();
        private List<Type> class_list2_1_Limit = new List<Type>();
        private List<Type> class_list_AES = new List<Type>();
        private List<Type> class_list_RSA = new List<Type>();
        private int init_deep = 0;
        private long base_calc = 0;

        private bool isContainsBig = false;
        private bool isContainsSmall = false;
        private int minHeatSize = 1;

        public MainFrm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            base_calc = 0;
            this.lblCalc.Text = "0";
            this.btnSearch.Enabled = false;
            this.isContainsBig = this.chkBig.Checked;
            this.isContainsSmall = this.chkSmall.Checked;
            this.minHeatSize = this.trackBar1.Value;
            SearchInfo searchInfo = new SearchInfo();
            searchInfo.OutInfo.OutType = rbtnAll.Checked ? 0 : 1;
            searchInfo.OutInfo.OutString = this.txtOutArg1.Text.Trim();
            searchInfo.OutInfo.InitCheckList();
            init_deep = int.Parse(this.txtDeep.Text);
            searchInfo.Deep = init_deep;
            for (int i = 1; i <= 18; i++)
            {
                if (this.tabControl.Controls.Find("txtArg" + i, true).Length > 0)
                {
                    if (this.tabControl.Controls.Find("txtArg" + i, true)[0] is TextBox)
                    {
                        string text = (this.tabControl.Controls.Find("txtArg" + i, true)[0] as TextBox).Text;
                        if (!string.IsNullOrEmpty(text))
                        {
                            searchInfo.InputList.Add(new InputInfo(text));
                            if (this.isContainsBig)
                            {
                                searchInfo.InputList.Add(new InputInfo(text.ToUpper()));
                            }

                            if (this.isContainsSmall)
                            {
                                searchInfo.InputList.Add(new InputInfo(text.ToLower()));
                            }
                        }
                    }
                }
            }

            // 增加空字符串
            searchInfo.InputList.Add(new InputInfo(""));
            Thread t = new Thread(SearchExecute);
            t.IsBackground = true;
            t.Start(searchInfo);
        }

        private void SearchExecute(object obj)
        {
            SearchInfo searchInfo = obj as SearchInfo;
            try
            {
                SetProcess(5);
                if (searchInfo.Deep > 0)
                {
                    // 算法修改了，需要计算次数还没有修改。。。。
                    string msg = string.Format("需要计算次数:（{0} * {1} + {2} * {1} * {1}）= {3}", class_list1_1.Count, searchInfo.InputList.Count, class_list2_1.Count, (class_list1_1.Count * searchInfo.InputList.Count + class_list2_1.Count * searchInfo.InputList.Count * searchInfo.InputList.Count));
                    this.ShowMsg(msg);

                    // 2、算法拼凑字符串，改成了，把字符串处理当成一种算法来对待。可以几个字符串生成一个，可以一个字符串生成多个，...
                    // 经过算法处理后的字符串，再次加入集合中。进行处理
                    List<InputInfo> needAddList = new List<InputInfo>();

                    bool result = SearchInterface1_1(searchInfo, ref needAddList);
                    needAddList.Distinct(new EqualityComparer());
                    SetProcess(10);
                    if (result)
                    {
                        this.btnSearch.Enabled = true;

                        return;
                    }

                    result = SearchInterface2_1(searchInfo, ref needAddList);
                    needAddList.Distinct(new EqualityComparer());
                    SetProcess(20);
                    if (result)
                    {
                        this.btnSearch.Enabled = true;

                        return;
                    }

                    result = SearchInterface2_1_Limit(searchInfo, ref needAddList);
                    needAddList.Distinct(new EqualityComparer());
                    SetProcess(30);
                    if (result)
                    {
                        this.btnSearch.Enabled = true;

                        return;
                    }

                    result = SearchInterface_AES(searchInfo, ref needAddList);
                    needAddList.Distinct(new EqualityComparer());
                    SetProcess(40);
                    if (result)
                    {
                        this.btnSearch.Enabled = true;

                        return;
                    }

                    result = SearchInterface_RSA(searchInfo, ref needAddList);
                    needAddList.Distinct(new EqualityComparer());
                    SetProcess(40);
                    if (result)
                    {
                        this.btnSearch.Enabled = true;

                        return;
                    }

                    result = SearchInterface1_2(searchInfo, ref needAddList);
                    needAddList.Distinct(new EqualityComparer());
                    SetProcess(50);
                    if (result)
                    {
                        this.btnSearch.Enabled = true;

                        return;
                    }

                    // TODO:数据量太大的情况，需要把数据，写入文件。
                    // TODO:写入文件数据量太大的时候，需要把数据写入文件，并压缩
                    needAddList.RemoveAll(p => p.content.Length == 0);
                    foreach (var item in needAddList)
                    {
                        if (!searchInfo.InputList.Exists(p => p.content == item.content))
                        {
                            searchInfo.InputList.Add(item);
                        }
                    }

                    SetProcess(90);

                    searchInfo.Deep--;
                    this.ShowMsg("当前层完毕，下一层 字符串个数 " + searchInfo.InputList.Count);

                    Thread t = new Thread(SearchExecute);
                    t.IsBackground = true;
                    t.Start(searchInfo);
                }
                else
                {
                    this.btnSearch.Enabled = true;
                    this.ShowMsg("查找完毕，未找到算法");
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
        }

        private bool SearchInterface1_1(SearchInfo searchInfo, ref List<InputInfo> needAddList)
        {
            foreach (var class_item in class_list1_1)
            {
                int hot = Convert.ToInt32(class_item.GetCustomAttributesData()[0].ConstructorArguments[0].Value);
                if (hot > minHeatSize)
                {
                    ConstructorInfo c = class_item.GetConstructor(new Type[] { });
                    IAlgorithm1_1 ial = (IAlgorithm1_1)c.Invoke(new object[] { });
                    foreach (var item in searchInfo.InputList)
                    {
                        byte[] result = ial.EnPwd_UTF8(Encoding.UTF8.GetBytes(item.content));
                        Interlocked.Increment(ref base_calc);
                        if (base_calc % 10000 == 0)
                        {
                            this.Invoke(new Action<Label>(p => p.Text = base_calc.ToString()), this.lblCalc);
                        }

                        string result_str = Encoding.UTF8.GetString(result);
                        needAddList.Add(new InputInfo(result_str, item.id, ial.GetType()));
                        if (this.isContainsSmall)
                        {
                            needAddList.Add(new InputInfo(result_str.ToLower(), item.id, ial.GetType()));
                        }

                        if (this.isContainsBig)
                        {
                            needAddList.Add(new InputInfo(result_str.ToUpper(), item.id, ial.GetType()));
                        }

                        if (searchInfo.OutInfo.CheckStringList.Contains(result_str.ToLower()))
                        {
                            // TODO:输出算法
                            this.ShowMsg("找到结果：逆序加密方式");
                            this.ShowMsg("最后加密方式:" + ial + ";content:" + result_str);
                            this.ShowMsg(item.ToString());
                            int pid = item.pid;
                            for (int i = searchInfo.Deep; i <= init_deep; i++)
                            {
                                var model = searchInfo.InputList.Find(a => a.id == pid);
                                if (model != null)
                                {
                                    this.ShowMsg(model.ToString());
                                    pid = model.pid;
                                }
                            }

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool SearchInterface2_1(SearchInfo searchInfo, ref List<InputInfo> needAddList)
        {
            foreach (var class_item in class_list2_1)
            {
                int hot = Convert.ToInt32(class_item.GetCustomAttributesData()[0].ConstructorArguments[0].Value);
                if (hot > minHeatSize)
                {
                    ConstructorInfo c = class_item.GetConstructor(new Type[] { });
                    IAlgorithm2_1 ial = (IAlgorithm2_1)c.Invoke(new object[] { });
                    foreach (var item in searchInfo.InputList)
                    {
                        var list = (from f in searchInfo.InputList select f.content).ToList();
                        foreach (var extend_str in list)
                        {
                            byte[] result = ial.EnPwd_UTF8(new List<byte[]>() { Encoding.UTF8.GetBytes(item.content), Encoding.UTF8.GetBytes(extend_str) });
                            Interlocked.Increment(ref base_calc);
                            if (base_calc % 10000 == 0)
                            {
                                this.Invoke(new Action<Label>(p => p.Text = base_calc.ToString()), this.lblCalc);
                            }

                            string result_str = Encoding.UTF8.GetString(result);
                            needAddList.Add(new InputInfo(result_str, item.id, ial.GetType()));
                            if (this.isContainsSmall)
                            {
                                needAddList.Add(new InputInfo(result_str.ToLower(), item.id, ial.GetType()));
                            }

                            if (this.isContainsBig)
                            {
                                needAddList.Add(new InputInfo(result_str.ToUpper(), item.id, ial.GetType()));
                            }

                            if (searchInfo.OutInfo.CheckStringList.Contains(result_str.ToLower()))
                            {
                                // TODO:输出算法
                                this.ShowMsg("找到结果：逆序加密方式");
                                this.ShowMsg("最后加密方式:" + ial + ";content:" + result_str);
                                this.ShowMsg(item.ToString());
                                int pid = item.pid;
                                for (int i = searchInfo.Deep; i <= init_deep; i++)
                                {
                                    var model = searchInfo.InputList.Find(a => a.id == pid);
                                    if (model != null)
                                    {
                                        this.ShowMsg(model.ToString());
                                        pid = model.pid;
                                    }
                                }

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool SearchInterface2_1_Limit(SearchInfo searchInfo, ref List<InputInfo> needAddList)
        {
            foreach (var class_item in class_list2_1_Limit)
            {
                int hot = Convert.ToInt32(class_item.GetCustomAttributesData()[0].ConstructorArguments[0].Value);
                if (hot > minHeatSize)
                {
                    ConstructorInfo c = class_item.GetConstructor(new Type[] { });
                    IAlgorithm2_1_Limit ial = (IAlgorithm2_1_Limit)c.Invoke(new object[] { });
                    foreach (var item in searchInfo.InputList)
                    {
                        var list = new List<string>();
                        if (ial.ToString().Contains("8"))
                        {
                            list = (from f in searchInfo.InputList where f.content.Length == 8 select f.content).ToList();
                        }

                        // f=first s=second
                        for (int f = 0; f < list.Count; f++)
                        {
                            for (int s = 0; s < list.Count; s++)
                            {
                                byte[] result = ial.EnPwd_UTF8(new List<byte[]>() { Encoding.UTF8.GetBytes(item.content), Encoding.UTF8.GetBytes(list[f]), Encoding.UTF8.GetBytes(list[s]) });
                                Interlocked.Increment(ref base_calc);
                                if (base_calc % 10000 == 0)
                                {
                                    this.Invoke(new Action<Label>(p => p.Text = base_calc.ToString()), this.lblCalc);
                                }

                                string result_str = Encoding.UTF8.GetString(result);
                                needAddList.Add(new InputInfo(result_str, item.id, ial.GetType()));
                                if (this.isContainsSmall)
                                {
                                    needAddList.Add(new InputInfo(result_str.ToLower(), item.id, ial.GetType()));
                                }

                                if (this.isContainsBig)
                                {
                                    needAddList.Add(new InputInfo(result_str.ToUpper(), item.id, ial.GetType()));
                                }

                                if (searchInfo.OutInfo.CheckStringList.Contains(result_str.ToLower()))
                                {
                                    // TODO:输出算法
                                    this.ShowMsg("找到结果：逆序加密方式");
                                    this.ShowMsg("最后加密方式:" + ial + ";content:" + result_str);
                                    this.ShowMsg(item.ToString());
                                    int pid = item.pid;
                                    for (int i = searchInfo.Deep; i <= init_deep; i++)
                                    {
                                        var model = searchInfo.InputList.Find(a => a.id == pid);
                                        if (model != null)
                                        {
                                            this.ShowMsg(model.ToString());
                                            pid = model.pid;
                                        }
                                    }

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool SearchInterface_AES(SearchInfo searchInfo, ref List<InputInfo> needAddList)
        {
            foreach (var class_item in class_list_AES)
            {
                int hot = Convert.ToInt32(class_item.GetCustomAttributesData()[0].ConstructorArguments[0].Value);
                if (hot > minHeatSize)
                {
                    ConstructorInfo c = class_item.GetConstructor(new Type[] { });
                    IAlgorithm_AES ial = (IAlgorithm_AES)c.Invoke(new object[] { });
                    foreach (var item in searchInfo.InputList)
                    {
                        var list = new List<string>();
                        if (ial.ToString().Contains("16"))
                        {
                            list = (from f in searchInfo.InputList where f.content.Length == 16 select f.content).ToList();
                        }

                        // f=first s=second
                        for (int f = 0; f < searchInfo.InputList.Count; f++)
                        {
                            for (int s = 0; s < list.Count; s++)
                            {
                                byte[] result = ial.EnPwd_UTF8(new List<byte[]>() { Encoding.UTF8.GetBytes(item.content), Encoding.UTF8.GetBytes(searchInfo.InputList[f].content), Encoding.UTF8.GetBytes(list[s]) });
                                Interlocked.Increment(ref base_calc);
                                if (base_calc % 10000 == 0)
                                {
                                    this.Invoke(new Action<Label>(p => p.Text = base_calc.ToString()), this.lblCalc);
                                }

                                string result_str = Encoding.UTF8.GetString(result);
                                needAddList.Add(new InputInfo(result_str, item.id, ial.GetType()));
                                if (this.isContainsSmall)
                                {
                                    needAddList.Add(new InputInfo(result_str.ToLower(), item.id, ial.GetType()));
                                }

                                if (this.isContainsBig)
                                {
                                    needAddList.Add(new InputInfo(result_str.ToUpper(), item.id, ial.GetType()));
                                }

                                if (searchInfo.OutInfo.CheckStringList.Contains(result_str.ToLower()))
                                {
                                    // TODO:输出算法
                                    this.ShowMsg("找到结果：逆序加密方式");
                                    this.ShowMsg("最后加密方式:" + ial + ";content:" + result_str);
                                    this.ShowMsg(item.ToString());
                                    int pid = item.pid;
                                    for (int i = searchInfo.Deep; i <= init_deep; i++)
                                    {
                                        var model = searchInfo.InputList.Find(a => a.id == pid);
                                        if (model != null)
                                        {
                                            this.ShowMsg(model.ToString());
                                            pid = model.pid;
                                        }
                                    }

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool SearchInterface_RSA(SearchInfo searchInfo, ref List<InputInfo> needAddList)
        {
            foreach (var class_item in class_list_RSA)
            {
                int hot = Convert.ToInt32(class_item.GetCustomAttributesData()[0].ConstructorArguments[0].Value);
                if (hot > minHeatSize)
                {
                    ConstructorInfo c = class_item.GetConstructor(new Type[] { });
                    IAlgorithm_RSA ial = (IAlgorithm_RSA)c.Invoke(new object[] { });
                    foreach (var item in searchInfo.InputList)
                    {
                        var list = new List<string>();
                        if (ial.ToString().Contains("1024"))
                        {
                            if (Encoding.UTF8.GetBytes(item.content).Length > 117)
                            {
                                continue;
                            }

                            list = (from f in searchInfo.InputList
                                    where ((f.content.StartsWith("-----BEGIN PUBLIC KEY-----") && f.content.EndsWith("-----END PUBLIC KEY-----") && f.content.Length < 300) ||
                                    f.content.Replace("\r", "").Replace("\n", "").Length == 216)
                                    select f.content).ToList();
                        }
                        else if (ial.ToString().Contains("2048"))
                        {
                            if (Encoding.UTF8.GetBytes(item.content).Length > 245)
                            {
                                continue;
                            }

                            list = (from f in searchInfo.InputList
                                    where (f.content.Length > 300 && (f.content.StartsWith("-----BEGIN PUBLIC KEY-----") && f.content.EndsWith("-----END PUBLIC KEY-----")) ||
                                    f.content.Replace("\r", "").Replace("\n", "").Length == 392)
                                    select f.content).ToList();
                        }
                        else if (ial.ToString().Contains("100"))
                        {
                            list = (from f in searchInfo.InputList
                                    where new Regex("^a-eA-E0-9$").IsMatch(f.content) && f.content.Length % 2 == 0
                                    select f.content).ToList();
                        }
                        else
                        {
                            list = (from f in searchInfo.InputList
                                    where f.content.StartsWith("<RSAKeyValue>") && f.content.EndsWith("</RSAKeyValue>")
                                    select f.content).ToList();
                        }

                        foreach (string p_key in list)
                        {
                            string used_key = p_key.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\r\n", "");
                            byte[] result = ial.EnPwd_UTF8(new List<byte[]>() { Encoding.UTF8.GetBytes(item.content), Encoding.UTF8.GetBytes(used_key) });
                            Interlocked.Increment(ref base_calc);
                            if (base_calc % 10000 == 0)
                            {
                                this.Invoke(new Action<Label>(p => p.Text = base_calc.ToString()), this.lblCalc);
                            }

                            string result_str = Encoding.UTF8.GetString(result);
                            needAddList.Add(new InputInfo(result_str, item.id, ial.GetType()));
                            if (this.isContainsSmall)
                            {
                                needAddList.Add(new InputInfo(result_str.ToLower(), item.id, ial.GetType()));
                            }

                            if (this.isContainsBig)
                            {
                                needAddList.Add(new InputInfo(result_str.ToUpper(), item.id, ial.GetType()));
                            }

                            if (searchInfo.OutInfo.CheckStringList.Contains(result_str.ToLower()))
                            {
                                // TODO:输出算法
                                this.ShowMsg("找到结果：逆序加密方式");
                                this.ShowMsg("最后加密方式:" + ial + ";content:" + result_str);
                                this.ShowMsg(item.ToString());
                                int pid = item.pid;
                                for (int i = searchInfo.Deep; i <= init_deep; i++)
                                {
                                    var model = searchInfo.InputList.Find(a => a.id == pid);
                                    if (model != null)
                                    {
                                        this.ShowMsg(model.ToString());
                                        pid = model.pid;
                                    }
                                }

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool SearchInterface1_2(SearchInfo searchInfo, ref List<InputInfo> needAddList)
        {
            foreach (var class_item in class_list1_2)
            {
                int hot = Convert.ToInt32(class_item.GetCustomAttributesData()[0].ConstructorArguments[0].Value);
                if (hot > minHeatSize)
                {
                    ConstructorInfo c = class_item.GetConstructor(new Type[] { });
                    IAlgorithm1_2 ial = (IAlgorithm1_2)c.Invoke(new object[] { });
                    foreach (var item in searchInfo.InputList)
                    {
                        List<byte[]> result = ial.EnPwd_UTF8(Encoding.UTF8.GetBytes(item.content));
                        Interlocked.Increment(ref base_calc);
                        if (base_calc % 10000 == 0)
                        {
                            this.Invoke(new Action<Label>(p => p.Text = base_calc.ToString()), this.lblCalc);
                        }

                        foreach (var result_data in result)
                        {
                            string result_str = Encoding.UTF8.GetString(result_data);
                            if (string.IsNullOrEmpty(result_str))
                            {
                                continue;
                            }

                            needAddList.Add(new InputInfo(result_str, item.id, ial.GetType()));
                            if (this.isContainsSmall)
                            {
                                needAddList.Add(new InputInfo(result_str.ToLower(), item.id, ial.GetType()));
                            }

                            if (this.isContainsBig)
                            {
                                needAddList.Add(new InputInfo(result_str.ToUpper(), item.id, ial.GetType()));
                            }

                            if (searchInfo.OutInfo.CheckStringList.Contains(result_str.ToLower()))
                            {
                                // TODO:输出算法
                                this.ShowMsg("找到结果：逆序加密方式");
                                this.ShowMsg("最后加密方式:" + ial + ";content:" + result_str);
                                this.ShowMsg(item.ToString());
                                int pid = item.pid;
                                for (int i = searchInfo.Deep; i <= init_deep; i++)
                                {
                                    var model = searchInfo.InputList.Find(a => a.id == pid);
                                    if (model != null)
                                    {
                                        this.ShowMsg(model.ToString());
                                        pid = model.pid;
                                    }
                                }

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void ShowMsg(string msg)
        {
            this.Invoke(new Action<TextBox>(p =>
            {
                if (p.TextLength > 30000)
                {
                    p.Clear();
                }

                p.AppendText(msg + "\r\n");
            }), this.txtResult);
        }

        private void SetProcess(int value)
        {
            this.Invoke(new Action<ProgressBar>(p => p.Value = value), this.progressBar1);
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            class_list1_1 = ClassTool.GetType(typeof(IAlgorithm1_1));
            class_list1_2 = ClassTool.GetType(typeof(IAlgorithm1_2));
            class_list2_1 = ClassTool.GetType(typeof(IAlgorithm2_1));
            class_list2_1_Limit = ClassTool.GetType(typeof(IAlgorithm2_1_Limit));
            class_list_AES = ClassTool.GetType(typeof(IAlgorithm_AES));
            class_list_RSA = ClassTool.GetType(typeof(IAlgorithm_RSA));
        }

        class EqualityComparer : IEqualityComparer<InputInfo>
        {
            public bool Equals(InputInfo x, InputInfo y)
            {
                if (x.content == y.content)
                {
                    return true;
                }

                return false;
            }

            public int GetHashCode(InputInfo obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
