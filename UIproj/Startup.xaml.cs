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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UIproj
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        public Startup()
        {
            InitializeComponent();
        }

        private DispatcherTimer htimer;
        Random _random = new Random();
        int numbers = 0;

        private void drag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //page loaded display pictures        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(@"\images\" + 0 + ".jpg", UriKind.Relative));
            sliderImageLogin.Source = bitmapImage;

            htimer = new DispatcherTimer();
            htimer.Interval = new TimeSpan(0, 0, 5);
            htimer.Tick += timer_tick;
            htimer.Start();
        }
        // all timing starts here        
        private void timer_tick(object sender, EventArgs e)
        {
            numbers++;
            DoubleAnimation badass = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0)),
                AutoReverse = false
            };
            sliderImageLogin.BeginAnimation(OpacityProperty, badass);
            sliderImageLogin.Opacity = 100;

            if (numbers == 10)
            {
                numbers = 0;
                BitmapImage bitmapImage = new BitmapImage(new Uri(@"\images\" + numbers + ".jpg", UriKind.Relative));
                sliderImageLogin.Source = bitmapImage;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(@"\images\" + numbers + ".jpg", UriKind.Relative));
                sliderImageLogin.Source = bitmapImage;
            }
            htimer.Stop();
            htimer.Start();
        }
    }
}
