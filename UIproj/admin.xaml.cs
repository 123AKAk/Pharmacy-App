using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace UIproj
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection LastHourSeries { get; set; }
        public SeriesCollection LastHourSeries1 { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public admin()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //RenderPages.Children.Clear();
            //RenderPages.Children.Add(new Dashboard());
            adminStats objOpen = new adminStats();
            amainframe.NavigationService.Navigate(objOpen);
            btn1.Foreground = Brushes.White;

            //automatically click the stats button on window load
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(btn1, new object[0]);
        }
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void minimise(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void maximise(object sender, RoutedEventArgs e)
        {
            //if (this.WindowState == WindowState.Normal)
            //{
            //    this.WindowState = WindowState.Maximized;

            //    maxia.Visibility = Visibility.Hidden;
            //    resto.Visibility = Visibility.Visible;
            //    maxi.ToolTip = "Restore Down";
            //}
            //else if (this.WindowState == WindowState.Maximized)
            //{
            //    this.WindowState = WindowState.Normal;

            //    resto.Visibility = Visibility.Hidden;
            //    maxia.Visibility = Visibility.Visible;
            //    maxi.ToolTip = "Maximise";
            //}
        }
        private void closea(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        string clcikedB;
        bool btnIsClicked;
        private void topBtnEnter(object sender, MouseEventArgs e)
        {
            Button topBbtn = (Button)sender;            
            var hover_color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");            
            topBbtn.Foreground = hover_color;            
        }

        private void topBtnLeave(object sender, MouseEventArgs e)
        {
            Button topBbtn = (Button)sender;
            if (topBbtn.Name == clcikedB & btnIsClicked == true)
            {

            }
            else
            {
                //setting hover color for sidebtn when mouse leave
                var hover_color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFD3D3D3");                
                topBbtn.Foreground = hover_color;                
            }
        }
        
        public Button navigatebtn;        
        private void navigate_click(object sender, RoutedEventArgs e)
        {
            navigatebtn = (Button)sender;
            var default_color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFD3D3D3");
            btn1.Foreground = default_color;
            btn2.Foreground = default_color;
            btn3.Foreground = default_color;
            btn4.Foreground = default_color;
            btn5.Foreground = default_color;
            btn6.Foreground = default_color;
            btn7.Foreground = default_color;            

            var m_color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
            switch (navigatebtn.Name.ToString())
            {
                case "btn1":
                    adminStats objOpen = new adminStats();
                    amainframe.NavigationService.Navigate(objOpen);                    
                    break;
                case "btn2":
                    adminsales objOpen1 = new adminsales();
                    amainframe.NavigationService.Navigate(objOpen1);                    
                    break;
                case "btn3":
                    adminusers objOpen2 = new adminusers();
                    amainframe.NavigationService.Navigate(objOpen2);                    
                    break;
                case "btn4":
                    adminuseractivity objOpen3 = new adminuseractivity();
                    amainframe.NavigationService.Navigate(objOpen3);                    
                    break;
                case "btn5":
                    admincategory objOpen4 = new admincategory();
                    amainframe.NavigationService.Navigate(objOpen4);                    
                    break;
                case "btn6":
                    adminstock objOpen5 = new adminstock();
                    amainframe.NavigationService.Navigate(objOpen5);
                    break;
                case "btn7":
                    login objOpen6 = new login();
                    objOpen6.Show();
                    this.Close();
                    break;
            }
            clcikedB = navigatebtn.Name;
            navigatebtn.Foreground = m_color;
            btnIsClicked = true;
        }

        public Button sarah;
        private void Btndrawer_MouseEnter(object sender, MouseEventArgs e)
        {
            sarah = (Button)sender;
            if (sarah.Name == "mini" || sarah.Name == "clo")
            {
                sarah.Foreground = Brushes.DarkGray;
            }
            else
            {
                sarah.Foreground = Brushes.DarkGray;
            }
        }

        private void Btndrawer_MouseLeave(object sender, MouseEventArgs e)
        {
            sarah = (Button)sender;
            if (sarah.Name == "mini" || sarah.Name == "clo")
            {
                sarah.Foreground = Brushes.Black;
            }
            else
            {
                sarah.Foreground = Brushes.Gray;
            }
        }
    }
}