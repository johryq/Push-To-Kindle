using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using HtmlAgilityPack;

namespace PushToKindleWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Object.SearchInfo_Biquge5200Cc> searchInfos_Biquge5200Ccs = new List<Object.SearchInfo_Biquge5200Cc>();
        List<Kindle.BookContent> bookContents = new List<Kindle.BookContent>();
        Object.EmailObj emailObj = new Object.EmailObj();
        Date.ConfigurationManagement configuration = new Date.ConfigurationManagement();

        /// <summary>
        /// 搜索网页返回数据
        /// </summary>
        /// <param name="searchBook"></param>
        /// <param name="searchInfos_Biquge5200Cc"></param>
        /// <returns></returns>
        private string Search_Biquge5200Cc(string searchBook, ref List<Object.SearchInfo_Biquge5200Cc> searchInfos_Biquge5200Cc)
        {
            string html = string.Empty;
            HttpWebRequest searchRequest = Request.ResqestHelper.IniRequest("https://www.biquge5200.cc/modules/article/search.php?searchkey=" + searchBook);
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    html = Request.ResqestHelper.GetUzipResponse(searchRequest);
                    if (!string.IsNullOrWhiteSpace(html))
                    {
                        break;
                    }
                    i++;
                }
                if (string.IsNullOrWhiteSpace(html))
                {
                    return "请求失败";
                }
            }
            catch (Exception e)
            {
                labInfo.Visibility = Visibility.Visible;
                labInfo.Content = e.Message + "\n搜索失败请稍后再试!";
                return "请求失败";
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection bookInfoNodes = doc.DocumentNode.SelectNodes("//table[@class='grid']/tr");

            int num = 0;
            searchInfos_Biquge5200Cc.Clear();

            //2
            foreach (HtmlNode node in bookInfoNodes)
            {
                if (num > 0)
                {
                    Object.SearchInfo_Biquge5200Cc info = new Object.SearchInfo_Biquge5200Cc();
                    info.BookName = node.SelectSingleNode("td[@class='odd']/a").InnerText;
                    info.BookLink = node.SelectSingleNode("td[@class='odd']/a").Attributes["href"].Value;
                    info.LatestChapter = node.SelectSingleNode("td[@class='even']/a").InnerText;
                    info.ChapterLink = node.SelectSingleNode("td[@class='even']/a").Attributes["href"].Value;
                    info.Date = node.SelectNodes("td[@class='odd']")[2].InnerText;
                    info.Auther = node.SelectNodes("td[@class='odd']")[1].InnerText;
                    info.WordCount = node.SelectNodes("td[@class='even']")[1].InnerText;
                    searchInfos_Biquge5200Cc.Add(info);
                }
                num++;
            }
            #region
            /////1
            //HtmlNodeCollection odd = doc.DocumentNode.SelectNodes("//table[@class='grid']/tr/td[@class='odd']");
            //HtmlNodeCollection even = doc.DocumentNode.SelectNodes("//table[@class='grid']/tr/td[@class='even']");

            //for (int i= 0; i < bookInfoNodes.Count+1; i++)
            //{
            //    info.BookName = odd[i+i*2].ChildNodes[0].InnerText;
            //    info.BookLink = odd[i+i*2].ChildNodes[0].Attributes["href"].Value;
            //    info.LatestChapter = even[i + i * 2].ChildNodes[0].InnerText;
            //    info.ChapterLink = even[i + i * 2].ChildNodes[0].Attributes["href"].Value;
            //    info.Date = odd[i + 2 + i * 2].InnerText;
            //    info.Auther = odd[i + 1 + i * 2].InnerText;
            //    searchInfos_Biquge5200Cc.Add(info);
            //}

            ////3
            //for(int i= 1; i < bookInfoNodes.Count; i++)
            //{
            //    info.BookName = bookInfoNodes[i].SelectSingleNode("td[@class='odd']/a").InnerText;
            //    info.BookLink = bookInfoNodes[i].SelectSingleNode("td[@class='odd']/a").Attributes["href"].Value;
            //    info.LatestChapter = bookInfoNodes[i].SelectSingleNode("td[@class='even']/a").InnerText;
            //    info.ChapterLink = bookInfoNodes[i].SelectSingleNode("td[@class='even']/a").Attributes["href"].Value;
            //    info.Date = bookInfoNodes[i].SelectNodes("td[@class='odd']")[2].InnerText;
            //    info.Auther = bookInfoNodes[i].SelectNodes("td[@class='odd']")[1].InnerText;
            //    searchInfos_Biquge5200Cc.Add(info);
            //    i++;
            //}
            #endregion
            return "ok";
        }

        /// <summary>
        /// 获取每章的链接
        /// </summary>
        /// <param name="htmlValue"></param>
        /// <returns></returns>
        private List<Kindle.BookContent> GetNextUrl(string htmlValue)
        {
            List<Kindle.BookContent> bookContents = new List<Kindle.BookContent>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlValue);

            Kindle.BookInfo.BookName = doc.DocumentNode.SelectSingleNode("//div[@id='info']/h1").InnerText;
            Kindle.BookInfo.Auther = doc.DocumentNode.SelectSingleNode("//div[@id='info']/p").InnerText.Replace("作&nbsp;&nbsp;&nbsp;&nbsp;者：", "");

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='list']/dl/dd/a");
            int num = 2;
            foreach (HtmlNode node in nodes)
            {
                if (num > 10)
                {
                    bookContents.Add(new Kindle.BookContent
                    {
                        chapter = node.InnerText,
                        Content = node.Attributes["href"].Value
                    });
                }
                num++;
            }
            return bookContents;
        }

        /// <summary>
        /// 获取每章内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetContent(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='content']/p");
            StringBuilder sb = new StringBuilder();
            foreach (HtmlNode node in nodes)
            {
                sb.Append($"<p> {node.InnerHtml.Trim()} </p>");
            }

            return sb.ToString();
        }

        public string ReplaceBadCharOfFileName(string fileName)
        {
            StringBuilder sb = new StringBuilder(fileName);
            
            sb = sb.Replace("\\", string.Empty);
            sb = sb.Replace("/", string.Empty);
            sb = sb.Replace(":", string.Empty);
            sb = sb.Replace("*", string.Empty);
            sb = sb.Replace("?", string.Empty);
            sb = sb.Replace("\"", string.Empty);
            sb = sb.Replace("<", string.Empty);
            sb = sb.Replace(">", string.Empty);
            sb = sb.Replace("|", string.Empty);
            sb = sb.Replace(" ", string.Empty);    //前面的替换会产生空格,最后将其一并替换掉
            return sb.ToString();
        }
        public string DownBook()
        {
            
            bookContents.Clear();
            Object.SearchInfo_Biquge5200Cc SearchInfo_Biquge5200Cc = DateGridBookInfo.SelectedValue as Object.SearchInfo_Biquge5200Cc;
            //初始化小说信息
            Kindle.BookInfo.BookName = ReplaceBadCharOfFileName(SearchInfo_Biquge5200Cc.BookName);
            Kindle.BookInfo.Auther = SearchInfo_Biquge5200Cc.Auther;
            //初始化请求
            HttpWebRequest listReq = Request.ResqestHelper.IniRequest(SearchInfo_Biquge5200Cc.BookLink);
            //获取每章的名字和链接
            string listHtml = Request.ResqestHelper.GetUzipResponse(listReq);
            //下载内容
            try
            {
                bookContents = GetNextUrl(listHtml);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n目录获取失败,请稍后再试");
                return "error";
            }
            if (bookContents.Count != 0)
            {
                //初始化进度条
                grid1.Visibility = Visibility.Visible;
                progressBar1.Maximum = bookContents.Count + 1;
                progressBar1.Minimum = 0;
                UpdateProgressBarDelegate updateProgressBaDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
                //获取小说内容
                int num = 0;
                int error = 0;
                int val = 0;
                foreach (Kindle.BookContent bT in bookContents)
                {
                    if (!string.IsNullOrWhiteSpace(bT.Content))
                    {
                        Dispatcher.Invoke(updateProgressBaDelegate, DispatcherPriority.Background, new object[] { RangeBase.ValueProperty, Convert.ToDouble(num) });
                        labDown.Content = $"已下载完成: {num}/{bookContents.Count}";
                        try
                        {
                            bT.Content = GetContent(Request.ResqestHelper.GetUzipResponse(Request.ResqestHelper.IniRequest(bT.Content)));
                            Thread.Sleep(300);
                        }
                        catch
                        {
                            bT.Content = $"<a href=\"{bT.Content}\">{bT.chapter}</a>";
                            error++;
                        }
                        num++;
                    }
                    if (error > 10 && val == 0)
                    {
                        MessageBoxResult result = MessageBox.Show("获取小说章节失败超过10章,是否结束下载!", "提示", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            return "error";
                        }
                        val = 1;
                    }
                }
            }
            //显示进度条
            return "ok";
        }

        //********************************窗体事件*******************************

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 搜索框按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BookNameTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(BookNameTxt.Text))
                {
                    labInfo.Visibility = Visibility.Visible;
                }
                else
                {
                    string searchBook = BookNameTxt.Text.Trim();
                    BookInfo.Visibility = Visibility.Visible;
                    labInfo.Visibility = Visibility.Hidden;
                    //清空绑定
                    DateGridBookInfo.ItemsSource = null;
                    DateGridBookInfo.Items.Clear();
                    //搜索请求,获取数据
                    string result = Search_Biquge5200Cc(searchBook, ref searchInfos_Biquge5200Ccs);
                    if (result == "ok" && searchInfos_Biquge5200Ccs.Count != 0)
                    {
                        //Datagrid数据绑定
                        (this.FindName("DateGridBookInfo") as DataGrid).ItemsSource = searchInfos_Biquge5200Ccs;
                        TabControl.SelectedIndex = 2;
                    }
                    else
                    {
                        MessageBox.Show("没有找到任何内容");
                    }
                }
            }
        }

        /// <summary>
        /// 向kindle邮箱推送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPush_Click(object sender, RoutedEventArgs e)
        {
            if (labinfo3.Content.ToString() == "请选中您要下载的书籍")
            {
                MessageBox.Show("您未选则任何书籍");

            }
            else
            {
                if (emailObj.SendEmail == null)
                {
                    MessageBox.Show("请先设置邮箱信息");
                    return;
                }
                //Object.SearchInfo_Biquge5200Cc SearchInfo_Biquge5200Cc = DateGridBookInfo.SelectedValue as Object.SearchInfo_Biquge5200Cc;
                //string fielP = System.IO.Path.GetFullPath($".. / .. / Kindle / Mobi / Data /{ ReplaceBadCharOfFileName(SearchInfo_Biquge5200Cc.BookName)}.mobi");
                //if (File.Exists(fielP ))
                //    //if (File.Exists(System.IO.Path.Combine(PulicField.AppPath, $".. / .. / Kindle / Mobi / Data /{ ReplaceBadCharOfFileName(SearchInfo_Biquge5200Cc.BookName)}.mobi")))
                // {
                //    PulicField.MobiPath= System.IO.Path.Combine(PulicField.AppPath, $"~/ Kindle / Mobi / Data /{ ReplaceBadCharOfFileName(SearchInfo_Biquge5200Cc.BookName)}.mobi");
                //}
                btnDown_Click(sender, e);
                //发送邮件
                Kindle.EmailHelper email = new Kindle.EmailHelper();
                string errorInfo = string.Empty;
                email.SendMail(emailObj.SendEmail,emailObj.PassWorld,new List<string> { emailObj.ReceiveEmail }, Kindle.BookInfo.BookName, "", new Dictionary<string, string> { { Kindle.BookInfo.BookName, PulicField.MobiPath } },ref errorInfo);
                if (string.IsNullOrEmpty(errorInfo))
                {
                    MessageBox.Show("发送成功");
                }
                else
                {
                    MessageBox.Show(errorInfo);
                }
                //labDown.Content = "发送成功!";

            }
        }

        private delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);
        /// <summary>
        /// 制作mobi文件至本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (labinfo3.Content.ToString() == "请选中您要下载的书籍")
            {
                MessageBox.Show("您未选则任何书籍");

            }
            else
            {
                grid1.Visibility = Visibility.Visible;
                if (DownBook() == "ok")
                {
                    Kindle.KindleHelper kindleHelper = new Kindle.KindleHelper();
                    labDown.Content = "生成文件中,请稍后!";
                    kindleHelper.CureatHtml(bookContents);
                    kindleHelper.CmdToKindleGen();
                    labDown.Content = "生成完成!";
                    //btnDownInfo.Visibility = Visibility.Visible;
                }
                btnHiddenGrid.Visibility = Visibility.Visible;
            }
        }

        private void btnSaveSite_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmailPwd.Password.Trim()) && !string.IsNullOrWhiteSpace(txtReceiveEmail.Text.Trim()) && !string.IsNullOrWhiteSpace(txtSendEmail.Text.Trim()))
            {
                if (configuration.HaveKey())
                {
                    emailObj.SendEmail = txtSendEmail.Text.Trim();
                    emailObj.PassWorld = txtEmailPwd.Password.Trim();
                    emailObj.ReceiveEmail = txtReceiveEmail.Text.Trim();
                    configuration.UpdataValue(emailObj);
                }
                else
                {
                    emailObj.SendEmail = txtSendEmail.Text.Trim();
                    emailObj.PassWorld = txtEmailPwd.Password.Trim();
                    emailObj.ReceiveEmail = txtReceiveEmail.Text.Trim();
                    configuration.AppendKey(emailObj);
                }
                labSiteInfo.Visibility = Visibility.Visible;
                labSiteInfo.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                labSiteInfo.Content = "保存成功";
            }
            else
            {
                labSiteInfo.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                labSiteInfo.Content = "输入不能为空!!!";
                labSiteInfo.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 加载邮箱信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void window1_Loaded(object sender, RoutedEventArgs e)
        {
            if (configuration.HaveKey())
            {
                emailObj = configuration.GetVal();
                txtSendEmail.Text = emailObj.SendEmail;
                txtReceiveEmail.Text = emailObj.ReceiveEmail;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            configuration.DelInfo();
            txtEmailPwd.Password = string.Empty;
            txtSendEmail.Text = string.Empty;
            txtReceiveEmail.Text = string.Empty;
        }
        //****************************辅助事件********************************
        #region 辅助事件

        /// <summary>
        /// 搜索框检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BookNameTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            BookNameTxt.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            if (string.IsNullOrWhiteSpace(BookNameTxt.Text))
            {
                BookNameTxt.Text = "请输入书名,然后回车";
                BookNameTxt.Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
        }

        /// <summary>
        /// 更改选中的书名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateGridBookInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                labinfo3.Content = (DateGridBookInfo.SelectedValue as Object.SearchInfo_Biquge5200Cc).BookName;
            }
            catch
            {
                labinfo3.Content = "请选中您要下载的书籍";

            }
        }

        /// <summary>
        /// 隐藏进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHiddenGrid_Click(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Hidden;
            btnHiddenGrid.Visibility = Visibility.Hidden;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //MessageBox.Show(sender.ToString());
        }

        /// <summary>
        /// 搜索框获取焦点后,清空输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BookNameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Focusable)
            {
                BookNameTxt.BorderBrush = new SolidColorBrush(Color.FromRgb(30, 144, 255));
                BookNameTxt.Text = "";
                BookNameTxt.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        #endregion
        //*****************************未开始**************************
        #region 未开始
        /// <summary>
        /// 选择下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// 查看详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labBookName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Book.Visibility = Visibility.Visible;
            //if(DateGridBookInfo.SelectedCells.Count > 0)
            //{
            //    labinfo3.Content = DateGridBookInfo.SelectedItem.ToString();
            //}
        }

        #endregion

      
    }
}
