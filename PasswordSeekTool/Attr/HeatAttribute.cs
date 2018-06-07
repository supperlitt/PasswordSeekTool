using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordSeekTool
{
    /// <summary>
    /// 热度
    /// </summary>
    public class HeatAttribute : Attribute
    {
        public int count = 0;

        public HeatAttribute(int count)
        {
            // 0 - 100
            // 100就是高频率
            this.count = count;
        }
    }
}
