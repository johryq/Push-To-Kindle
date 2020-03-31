using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindle
{
    class HtmlToKindle
    {
        public void CureatHtml(string html,List<BookContent> books)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string opfPath = Path.Combine(path + "../../KindleGen/bookInfo.opf");
            string tocPath = Path.Combine(path + "../../KindleGen/toc.ncx");
            string htmlPath = Path.Combine(path + "../../KindleGen/index.html");
            
            using (StreamReader reader = new StreamReader(opfPath, Encoding.UTF8))
            {
                string text = reader.ReadToEnd();
                text = text.Trim().Replace("作者", BookInfo.Auther).Replace("书名", BookInfo.BookName).Replace("时间", DateTime.Now.ToLongDateString());
                File.WriteAllText(Path.Combine(path + "../../Kindle/bookinfo.opf"), text);
            }

            using (StreamReader reader = new StreamReader(tocPath, Encoding.UTF8))
            {
                
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < books.Count; i++)
                {
                    sb.Append("<navPoint id=\"navpoint - " + i.ToString() + "\"><navLabel><text>" + books[i].chapter + "</text></navLabel><content src=\"index.html#dir" + i.ToString() + "\"/></navPoint>\r\n");
                }
                string text = reader.ReadToEnd();
                text = text.Trim().Replace("书名",BookInfo.BookName).Replace("目录", sb.ToString());
                File.WriteAllText(Path.Combine(path + "../../Kindle/toc.ncx"), text);
            }

            using (StreamReader reader = new StreamReader(htmlPath, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                //foreach 
                string text = reader.ReadToEnd();
                text = text.Trim().Replace("作者", sb.ToString());
                File.WriteAllText(Path.Combine(path + "../../Kindle/idnex.html"),text);
            }
        }
    }
}
