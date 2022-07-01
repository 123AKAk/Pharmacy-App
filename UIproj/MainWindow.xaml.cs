using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIproj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ToDoList tdl = new ToDoList();

        int mainId;

        public MainWindow()
        {
            InitializeComponent();

            mainId = login.userId;
            username.Text = login.userName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //RenderPages.Children.Clear();
            //RenderPages.Children.Add(new Dashboard());
            Dashboard objOpen = new Dashboard();
            mainframe.NavigationService.Navigate(objOpen);
            calculatorPage calOPen = new calculatorPage();
            calFrame.NavigationService.Navigate(calOPen);
            list4.IsSelected = true;
        }
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch(Exception ex)
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
            login objOpen6 = new login();
            objOpen6.Show();
            this.Close();
        }

        public Button navigatebtn;
        private void navigate_click(object sender, RoutedEventArgs e)
        {
            list1.IsSelected = false;
            list2.IsSelected = false;
            list3.IsSelected = false;
            list4.IsSelected = false;
            list5.IsSelected = false;
            list6.IsSelected = false;

            navigatebtn = (Button)sender;
            switch (navigatebtn.Name.ToString())
            {
                case "btn1":
                    DrugStock objOpen = new DrugStock();
                    mainframe.NavigationService.Navigate(objOpen);
                    list1.IsSelected = true;
                    break;
                case "btn2":
                    Dispensary objOpen1 = new Dispensary();
                    mainframe.NavigationService.Navigate(objOpen1);
                    list2.IsSelected = true;
                    break;
                case "btn3":
                    makeSale objOpen2 = new makeSale();
                    mainframe.NavigationService.Navigate(objOpen2);
                    list3.IsSelected = true;
                    break;
                case "btn4":
                    Dashboard objOpen3 = new Dashboard();
                    mainframe.NavigationService.Navigate(objOpen3);
                    list4.IsSelected = true;
                    break;
                case "btn5":
                    category objOpen4 = new category();
                    mainframe.NavigationService.Navigate(objOpen4);
                    //objOpen4.Show();
                    list5.IsSelected = true;
                    break;
                case "btn6":
                    this.Close();
                    break;
            }
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
            if(sarah.Name == "mini" || sarah.Name == "clo")
            {
                sarah.Foreground = Brushes.Black;                
            }
            else
            {
                sarah.Foreground = Brushes.Gray;
            }
        }

        private void updates_Click(object sender, RoutedEventArgs e)
        {
            news obj = new news();
            obj.ShowDialog();
        }

        private void notifi_Click(object sender, RoutedEventArgs e)
        {
            notification Win1Open = new notification();
            otherframe.NavigationService.Navigate(Win1Open);
            showWinx.IsOpen = true;
            showing.Text = "Notifications";
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            asettings Win2Open = new asettings();
            otherframe.NavigationService.Navigate(Win2Open);
            showWinx.IsOpen = true;
            showing.Text = "Settings";
        }

        public void func()
        {
            showWinx.IsOpen = false;
            asettings Win2Open = new asettings();
            otherframe.NavigationService.Navigate(Win2Open);
            showWinx.IsOpen = true;
            showing.Text = "Settings";
            MessageBox.Show("System Overload..../// Incorrect Path....");
        }
    }
}
