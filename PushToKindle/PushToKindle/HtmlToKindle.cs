using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindleWPF
{
    class HtmlToKindle
    {
        public void CureatHtml(List<BookContent> books)
        {
            string path = BookInfo.FilePath;
            string opfPath = Path.Combine(path + "../../KindleGen/bookInfo.opf");
            string tocPath = Path.Combine(path + "../../KindleGen/toc.ncx");
            string htmlPath = Path.Combine(path + "../../KindleGen/index.html");
            string cssPath = Path.Combine(path + "../../KindleGen/style.css");

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
                    sb.Append($"<navPoint id=\"navpoint -{i+1}\" playOrder=\"{ i + 1}\"><navLabel><text> {books[i].chapter} </text></navLabel><content src=\"index.html#dir{i+1}\"/></navPoint>\r\n");
                }
                string text = reader.ReadToEnd();
                text = text.Trim().Replace("书名",BookInfo.BookName).Replace("目录", sb.ToString());
                File.WriteAllText(Path.Combine(path + "../../Kindle/toc.ncx"), text);
            }

            using (StreamReader reader = new StreamReader(htmlPath, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                //foreach 
                for(int i = 0; i < books.Count; i++)
                {
                    sb.Append($"<h2 id = \"dir{i+1}\">{books[i].chapter}</h2> {books[i].Content}\r\n");
                }
                
                string text = reader.ReadToEnd();
                text = text.Trim().Replace("书名",BookInfo.BookName).Replace("内容", sb.ToString());
                File.WriteAllText(Path.Combine(path + "../../Kindle/index.html"),text);
            }
            File.Copy(cssPath, "../../Kindle/style.css");
        }
    }
}
