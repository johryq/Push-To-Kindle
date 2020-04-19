using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindleWPF.Kindle
{
    /// <summary>
    /// 制作mobi的HTML文件所需信息
    /// </summary>
    public static class BookInfo
    {
        public static string BookName { get; set; }
        public static string Auther { get; set; }
        public static string CureateTime { get; set; }
        //public static int TocNumber { get; set; }
    }
}
