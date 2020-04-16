using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindleWPF.Request
{
    public static class ResqestHelper
    {
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
            //request.Referer = "https://www.biquge5200.cc/";
            return request;
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
    }
}
