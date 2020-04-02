using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO.Compression;
using System.IO;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace PushToKindle
{
    class Program
    {
        static void Main(string[] args)
        {
            BookInfo.FilePath = AppDomain.CurrentDomain.BaseDirectory;
            HtmlToKindle ht = new HtmlToKindle();
            //ht.CureatHtml(html,books);
            EmailHelper email = new EmailHelper();
            email.SendMail(new List<string> { "@kindle.com" }, BookInfo.BookName, "", new Dictionary<string, string> { { BookInfo.BookName,BookInfo.FilePath} }) ;
            //email.SendEmail();
            #region old
            //RequestOptions options = new RequestOptions();
            //options.Uri = new Uri("https://www.biquge5200.cc/130_130510/");
            //IWebProxy proxy = GetProxy();
            //string result = string.Empty;
            ////IWebProxy proxy =
            //var request = (HttpWebRequest)WebRequest.Create(options.Uri);
            ////POST大于1024字节时,POST请求分成两步
            ////发送一个请求询问Server是否接受 Exprct:100 -continue
            ////不是所有Server都会响应 100 -continue; 此时就会出现错误
            //request.ServicePoint.Expect100Continue = false;
            //request.ServicePoint.UseNagleAlgorithm = false;
            //if (!string.IsNullOrEmpty(options.XHRParams))
            //{
            //    request.AllowWriteStreamBuffering = true;
            //}
            //else
            //{
            //    request.AllowWriteStreamBuffering = false;
            //}
            //request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            //request.ContentType = options.ContentType;
            //request.AllowAutoRedirect = options.AllowAutoRedirect;
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
            //request.Timeout = options.Timeout;
            //request.KeepAlive = options.KeepAlive;
            //if (!string.IsNullOrEmpty(options.Referer)) request.Referer = options.Referer;//返回上一级历史链接
            //request.Method = options.Method="GET";
            //if (proxy != null) request.Proxy = proxy;//设置代理服务器IP，伪装请求地址
            //if (!string.IsNullOrEmpty(options.RequestCookies)) request.Headers[HttpRequestHeader.Cookie] = options.RequestCookies;
            //request.ServicePoint.ConnectionLimit = options.ConnectionLimit;//定义最大连接数
            //if (options.WebHeader != null && options.WebHeader.Count > 0) request.Headers.Add(options.WebHeader);//添加头部信息
            //if (!string.IsNullOrEmpty(options.XHRParams))//如果是POST请求，加入POST数据
            //{
            //    byte[] buffer = Encoding.UTF8.GetBytes(options.XHRParams);
            //    if (buffer != null)
            //    {
            //        request.ContentLength = buffer.Length;
            //        request.GetRequestStream().Write(buffer, 0, buffer.Length);
            //    }
            //}
            #endregion
            string url = "https://www.biquge5200.cc/130_130510/";
            //url = "https://www.biquge5200.cc/130_130510/171445544.html";
            string result = string.Empty;
            List<BookContent> books = new List<BookContent>();
            var request = IniRequest(url);
            //获取网页
            result = GetUzipResponse(request);
            //获取目录URL和章节名
            books = GetNextUrl(result);
            //Thread thread = new Thread();
            //thread.IsBackground = true;
            //thread.Start();
            Stopwatch sp = new Stopwatch();
            sp.Start();
            int x = 0;
            foreach (BookContent book in books)
            {
                if (!string.IsNullOrEmpty(book.Content))
                {
                    Console.WriteLine(book.chapter);
                    book.Content = GetContent(GetUzipResponse(IniRequest(book.Content)));
                    Thread.Sleep(300);
                    x++;
                }
                //if (x > 10)
                //{
                //    break;
                //}
            }
            ht.CureatHtml(books);
            Console.WriteLine($"加载{books.Count}耗时{sp.Elapsed.TotalSeconds}");
            Console.ReadKey();
        }


        /// <summary>
        /// 代理访问
        /// </summary>
        /// <returns></returns>
        private static System.Net.WebProxy GetProxy()
        {
            System.Net.WebProxy webProxy = null;
            //try
            //{
            //    // 代理链接地址加端口
            //    string proxyHost = "192.168.1.1";
            //    string proxyPort = "9030";

            //    // 代理身份验证的帐号跟密码
            //    //string proxyUser = "xxx";
            //    //string proxyPass = "xxx";

            //    // 设置代理服务器
            //    webProxy = new System.Net.WebProxy();
            //    // 设置代理地址加端口
            //    webProxy.Address = new Uri(string.Format("{0}:{1}", proxyHost, proxyPort));
            //    // 如果只是设置代理IP加端口，例如192.168.1.1:80，这里直接注释该段代码，则不需要设置提交给代理服务器进行身份验证的帐号跟密码。
            //    //webProxy.Credentials = new System.Net.NetworkCredential(proxyUser, proxyPass);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("获取代理信息异常", DateTime.Now.ToString(), ex.Message);
            //}
            return webProxy;
        }

        /// <summary>
        /// 初始化请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebRequest IniRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.UseNagleAlgorithm = false;
            request.Method = "get";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
            request.Timeout = 5000;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = int.MaxValue;
            return request;
        }

        /// <summary>
        /// 获取目录和章节URL
        /// </summary>
        /// <param name="htmlValue"></param>
        /// <returns></returns>
        public static List<BookContent>  GetNextUrl(string htmlValue)
        {
            List<BookContent> bookContents = new List<BookContent>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlValue);

            BookInfo.BookName = doc.DocumentNode.SelectSingleNode("//div[@id='info']/h1").InnerText;
            BookInfo.Auther = doc.DocumentNode.SelectSingleNode("//div[@id='info']/p").InnerText.Replace("作&nbsp;&nbsp;&nbsp;&nbsp;者：", "");

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='list']/dl/dd/a");
            int num = 2;
            foreach(HtmlNode node in nodes)
            {
                if (num > 10)
                {
                    bookContents.Add(new BookContent { 
                        chapter = node.InnerText ,
                        Content= node.Attributes["href"].Value
                    }) ;
                }
                num++;
            }
            return bookContents;
        }

        /// <summary>
        /// 获取小说章节内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetContent(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='content']/p");
            StringBuilder sb = new StringBuilder();
            foreach(HtmlNode node in nodes)
            {
                sb.Append($"<p> {node.InnerHtml.Trim()} </p>");
            }

            return sb.ToString() ;
        }

        /// <summary>
        /// 返回响应的内容(解压后的压)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetUzipResponse(HttpWebRequest request)
        {
            string result = string.Empty;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))//解压
                {
                    using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("GBK")))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                {
                    using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (Stream stream = response.GetResponseStream())//原始
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            request.Abort();
            return result;
        }


        /// <summary>
        /// 通过网页chaset获取编码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        static string GetHtml(string url, Encoding encoding)
        {
            byte[] buf = new WebClient().DownloadData(url);
            if (encoding != null) return encoding.GetString(buf);
            string html = Encoding.UTF8.GetString(buf);
            encoding = GetEncoding(html);
            if (encoding == null || encoding == Encoding.UTF8) return html;
            return encoding.GetString(buf);
        }
        // 根据网页的HTML内容提取网页的Encoding
        static Encoding GetEncoding(string html)
        {
            string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
            string charset = Regex.Match(html, pattern).Groups["charset"].Value;
            try { return Encoding.GetEncoding(charset); }
            catch (ArgumentException) { return null; }
        }
    }
}
