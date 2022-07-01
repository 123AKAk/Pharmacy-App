using System;
using System.Collections.Generic;
using System.IO;
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

namespace UIproj
{
    /// <summary>
    /// Interaction logic for news.xaml
    /// </summary>
    public partial class news : Window
    {
        public news()
        {
            InitializeComponent();

            try
            {
                //myweb.Source = new Uri("http://" + "localhost:8081/magazine/index.html");
                string exportDir = @Directory.GetCurrentDirectory() + "\\Blog\\HTMLPage1.html";
                //string exportDir = System.IO.Path.GetFullPath(@"..\..\") + "\\HTMLPage1.html";
                //xframe.NavigationService.Navigate(exportDir);
                //xframe.Source = new Uri("http://" + "google.com", UriKind.Absolute);
                xframe.Source = new Uri(exportDir, UriKind.RelativeOrAbsolute);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeWin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
