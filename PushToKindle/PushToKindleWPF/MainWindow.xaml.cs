using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;

namespace PushToKindleWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            double DGCol = window1.Width - 200;
        }

        /// <summary>
        /// 搜索框获取焦点
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
                    //搜索请求
                    Search_Biquge5200Cc(searchBook);
                    
                    
                    

                    //Kindle.BookInfo.BookName = BookNameTxt.Text.Trim();
                    TabControl.SelectedIndex = 2;
                }
            }
        }
        private void Search_Biquge5200Cc(string searchBook)
        {
            HttpWebRequest searchRequest = Request.ResqestHelper.IniRequest("https://www.biquge5200.cc/modules/article/search.php?searchkey=" + searchBook);
            string html = Request.ResqestHelper.GetUzipResponse(searchRequest);
            List<Object.SearchInfo_Biquge5200Cc> searchInfos_Biquge5200Cc = new List<Object.SearchInfo_Biquge5200Cc>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection bookNodes = doc.DocumentNode.SelectNodes("/td[@class=odd]");
            HtmlNodeCollection chapterNodes = doc.DocumentNode.SelectNodes("/td[@class=even]");
            HtmlNodeCollection autherNodes = doc.DocumentNode.SelectNodes("/td[@class=odd]");
            HtmlNodeCollection dataNodes = doc.DocumentNode.SelectNodes("/td[@class=odd]");
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //MessageBox.Show(sender.ToString());
        }

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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Book.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 查看详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labBookName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        /// <summary>
        /// 向kindle邮箱推送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPush_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 制作mobi文件至本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 选择下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveSite_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmailPwd.Text.Trim()) && !string.IsNullOrWhiteSpace(txtReceiveEmail.Text.Trim()) && !string.IsNullOrWhiteSpace(txtSendEmail.Text.Trim()))
            {
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
    }
}
