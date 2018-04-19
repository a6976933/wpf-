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
using System.Windows.Shapes;
using System.ComponentModel;

namespace client_text_fresh
{
    /// <summary>
    /// loading_windows.xaml 的互動邏輯
    /// </summary>
    
    public partial class loading_windows : Window
    {
        public loading_windows()
        {
            InitializeComponent();
           
        }
       
        private void Ip_button_Click(object sender, RoutedEventArgs e)
        {
            text_windows second_windows = new text_windows(ip_address.Text);
            second_windows.Show();

            this.Close();
        }
    }
}
