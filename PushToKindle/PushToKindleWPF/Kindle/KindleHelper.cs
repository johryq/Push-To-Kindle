
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindleWPF.Kindle
{
    class KindleHelper
    {
        readonly string path = PulicField.AppPath;

        public void CureatHtml(List<BookContent> books)
        {
            
            string opfPath = Path.Combine(path + "../../MobiExample/bookInfo.opf");
            string tocPath = Path.Combine(path + "../../MobiExample/toc.ncx");
            string htmlPath = Path.Combine(path + "../../MobiExample/index.html");
            string cssPath = Path.Combine(path + "../../MobiExample/style.css");

            using (StreamReader reader = new StreamReader(opfPath, Encoding.UTF8))
            {
                string text = reader.ReadToEnd();
                text = text.Trim().Replace("作者", BookInfo.Auther).Replace("书名", BookInfo.BookName).Replace("时间", DateTime.Now.ToLongDateString());
                File.WriteAllText(Path.Combine(path + "../../Mobi/bookinfo.opf"), text);
            }

            using (StreamReader reader = new StreamReader(tocPath, Encoding.UTF8))
            {

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < books.Count; i++)
                {
                    sb.Append($"<navPoint id=\"navpoint -{i + 1}\" playOrder=\"{ i + 1}\"><navLabel><text> {books[i].chapter} </text></navLabel><content src=\"index.html#dir{i + 1}\"/></navPoint>\r\n");
                }
                string text = reader.ReadToEnd();
                text = text.Trim().Replace("书名", BookInfo.BookName).Replace("目录", sb.ToString());
                File.WriteAllText(Path.Combine(path + "../../Mobi/toc.ncx"), text);
            }

            using (StreamReader reader = new StreamReader(htmlPath, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                //foreach 
                for (int i = 0; i < books.Count; i++)
                {
                    sb.Append($"<h2 id = \"dir{i + 1}\">{books[i].chapter}</h2> {books[i].Content}\r\n");
                }

                string text = reader.ReadToEnd();
                text = text.Trim().Replace("书名", BookInfo.BookName).Replace("内容", sb.ToString());
                File.WriteAllText(Path.Combine(path + "../../Mobi/index.html"), text);
            }
            File.Copy(cssPath, "../../Mobi/style.css");
        }

        public bool CmdToKindleGen()
        {
            string kindleGenPath = Path.Combine(path, $"../../Kindle/KindleGen.exe");
            string infoPath = Path.Combine(path, $"../../Kindle/Mobi/{BookInfo.BookName}.opf");
            string cmdStr = $"\"{kindleGenPath}\"" + $" \"{infoPath}\"";
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false; p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                p.Start();

                p.StandardInput.WriteLine(cmdStr + "&exit");
                p.StandardInput.AutoFlush = true;
                string strOuput = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
                Console.WriteLine(strOuput);
                Console.ReadKey();
                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e + "\nKindleGen 错误");
                Console.ReadKey();
                return false;
            }
        }
    }
}
