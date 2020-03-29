﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindle
{
    class RequestOptions
    {
        /// <summary>
        /// 请求方式，GET或POST
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// 上一级历史记录链接
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// 超时时间（毫秒）
        /// </summary>
        public int Timeout = 15000;
        /// <summary>
        /// 启用长连接
        /// </summary>
        public bool KeepAlive = true;
        /// <summary>
        /// 禁止自动跳转
        /// </summary>
        public bool AllowAutoRedirect = false;
        /// <summary>
        /// 定义最大连接数
        /// </summary>
        public int ConnectionLimit = int.MaxValue;
        /// <summary>
        /// 请求次数
        /// </summary>
        public int RequestNum = 3;
        /// <summary>
        /// 可通过文件上传提交的文件类型
        /// </summary>
        public string Accept = "*/*";
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType = "application/x-www-form-urlencoded";
        /// <summary>
        /// 实例化头部信息
        /// </summary>
        private WebHeaderCollection header = new WebHeaderCollection();
        /// <summary>
        /// 头部信息
        /// </summary>
        public WebHeaderCollection WebHeader
        {
            get { return header; }
            set { header = value; }
        }
        /// <summary>
        /// 定义请求Cookie字符串
        /// </summary>
        public string RequestCookies { get; set; }
        /// <summary>
        /// 异步参数数据
        /// </summary>
        public string XHRParams { get; set; }
    }
}
