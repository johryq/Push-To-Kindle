using System;
using System.Collections.Generic;
using System.Linq;
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

        private void BookNameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Focusable)
            {
                BookNameTxt.BorderBrush = new SolidColorBrush(Color.FromRgb(30, 144, 255));
                BookNameTxt.Text = "";
                BookNameTxt.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

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
                    BookInfo.Visibility = Visibility.Visible;
                    labInfo.Visibility = Visibility.Hidden;
                    PulicField.BookName = BookNameTxt.Text.Trim();
                    TabControl.SelectedIndex = 2;
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //MessageBox.Show(sender.ToString());
        }

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
    }
}
