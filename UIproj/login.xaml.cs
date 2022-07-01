using MySql.Data.MySqlClient;
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
using UIproj.myclass;

namespace UIproj
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        dtba connec = new dtba();

        public login()
        {
            InitializeComponent();
            username.Text = Settings1.Default.nameS;
        }

        public Button sarah;
        private void Btndrawer_MouseEnter(object sender, MouseEventArgs e)
        {
            sarah = (Button)sender;
            sarah.Foreground = Brushes.DarkGray;
        }
        private void Btndrawer_MouseLeave(object sender, MouseEventArgs e)
        {
            sarah = (Button)sender;
            sarah.Foreground = Brushes.Gray;
        }

        private void drag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BbtnEnter(object sender, MouseEventArgs e)
        {
            Button sideBbtn = (Button)sender;

            //setting hover color for sidebtn when mouse enters            
            var hover_color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF800080");
            //var hover_background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFFFFFFF");
            sideBbtn.Foreground = hover_color;
            //sideBbtn.Background = hover_background;
        }

        private void BbtnLeave(object sender, MouseEventArgs e)
        {
            Button sideBbtn = (Button)sender;
            //setting hover color for sidebtn when mouse leave
            var hover_color = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF7E467E");
            //var hover_background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#00000000");
            sideBbtn.Foreground = hover_color;
            //sideBbtn.Background = hover_background;
        }

        public static string userName;
        public static int userId;

        private void login_Click(object sender, RoutedEventArgs e)
        {
            //Subscription.count("Test Point", "2");
            if (username.Text != "" & password.Password != "")
            {
                int idd = 0, state = 0;
                string name = "";

                MySqlCommand ccommand;
                string cQuery = "SELECT * FROM users WHERE Email='" + username.Text + "'AND Password='" + password.Password + "'";
                ccommand = new MySqlCommand(cQuery, connec.connectdb);
                connec.connectdb.Open();
                try
                {
                    MySqlDataReader readerc = ccommand.ExecuteReader();
                    while (readerc.Read())
                    {
                        idd = Convert.ToInt32(readerc["id"].ToString());
                        state = Convert.ToInt32(readerc["User_State"].ToString());
                        name = readerc["UserId"].ToString();
                    }

                    Settings1.Default.nameS = name;
                    Settings1.Default.Save();

                    if (state == 1 && idd != 0)
                    {
                        userId = idd;
                        userName = name;

                        admin win = new admin();
                        win.Show();
                        this.Close();
                    }
                    else if(state == 0 && idd != 0)
                    {
                        userId = idd;
                        userName = name;

                        MainWindow win = new MainWindow();
                        win.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Login Failed, Incorrect Details");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connec.connectdb.Close();
                }
            }
            else
            {
                MessageBox.Show("Fill All Fields");
            }
        }
    }
}
