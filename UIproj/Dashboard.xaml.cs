using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UIproj.myclass;

namespace UIproj
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        dtba connec = new dtba();

        MySqlDataAdapter daa;
        DataSet dss;

        int mainId;

        public Dashboard()
        {
            InitializeComponent();

            GetotherData();
            GetAllProduc();

            //string imgCartoon = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString()}\\Images\\cartoon-woman-pretty.png";
            //string imgavatar = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString()}\\Images\\avatar1.jpg";
            //ImgCartoon.Source = new BitmapImage(new Uri(imgCartoon));
            //avatar1.Source = new BitmapImage(new Uri(imgavatar));
            //avatar2.Source = new BitmapImage(new Uri(imgavatar));

            mainId = login.userId;
            uname.Text = login.userName;

            TimeSpan morin1 = new TimeSpan(6, 0, 0); //6am o'clock
            TimeSpan morin2 = new TimeSpan(11, 59, 0); //11am o'clock
            TimeSpan after1 = new TimeSpan(12, 0, 0); //12pm o'clock
            TimeSpan after2 = new TimeSpan(15, 59, 0); //3pm o'clock
            TimeSpan even1 = new TimeSpan(16, 0, 0); //4pm o'clock
            TimeSpan even2 = new TimeSpan(20, 0, 0); //9pm o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > morin1) && (now < morin2))
            {
                period.Text = "Good Morning' ";
            }
            else if ((now > after1) && (now < after2))
            {
                period.Text = "Good Afternoon' ";
            }
            else if ((now > even1) && (now < even2))
            {
                period.Text = "Good Evening' ";
            }
            else
            {
                period.Text = "Hello' ";
            }
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

        private void AllOwedCust_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Customer_Name")
            {
                e.Column.Width = 180; //Resize the column width
            }
            if (e.Column.Header.ToString() == "Quantity")
            {
                e.Column.Width = 90;
            }
            if (e.Column.Header.ToString() == "Price")
            {
                e.Column.Width = 90;
            }
            if (e.Column.Header.ToString() == "Credit")
            {
                e.Column.Width = 240;
            }
        }

        private void GetotherData()
        {
            try
            {
                MySqlCommand ccommand;

                string date1 = DateTime.Now.ToString("yyyy-MM-dd");

                //Get Number of Sales                
                var aQuery = "SELECT COUNT(*) as atoplam FROM psales WHERE MDate='" + date1 + "'";
                ccommand = new MySqlCommand(aQuery, connec.connectdb);
                connec.connectdb.Open();
                MySqlDataReader readera = ccommand.ExecuteReader();
                while (readera.Read())
                {
                    noofsales.Text = readera["atoplam"].ToString();
                }
                connec.connectdb.Close();

                //Get Number of noofdispenseddrugs                
                var bQuery = "SELECT COUNT(*) as btoplam FROM product WHERE Dispensed='1'";
                ccommand = new MySqlCommand(bQuery, connec.connectdb);
                connec.connectdb.Open();
                MySqlDataReader readerb = ccommand.ExecuteReader();
                while (readerb.Read())
                {
                    noofdispenseddrugs.Text = readerb["btoplam"].ToString();
                }
                connec.connectdb.Close();

                //Get Number of noofproductsinstock                
                var cQuery = "SELECT COUNT(*) as ctoplam FROM product";
                ccommand = new MySqlCommand(cQuery, connec.connectdb);
                connec.connectdb.Open();
                MySqlDataReader readerc = ccommand.ExecuteReader();
                while (readerc.Read())
                {
                    noofproductsinstock.Text = readerc["ctoplam"].ToString();
                }
                connec.connectdb.Close();

                //Get Number of noofcategory                
                var dQuery = "SELECT COUNT(*) as dtoplam FROM catego";
                ccommand = new MySqlCommand(dQuery, connec.connectdb);
                connec.connectdb.Open();
                MySqlDataReader readerd = ccommand.ExecuteReader();
                while (readerd.Read())
                {
                    noofcategory.Text = readerd["dtoplam"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Other-Details)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connec.connectdb.Close();
            }
        }

        private void GetAllProduc()
        {
            try
            {
                string sql = "SELECT id,Customer_Name,Credit,Quantity,Price FROM psales WHERE Credit_State = '1' ORDER BY id DESC";
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sql, connec.connectdb);
                daa.Fill(dss);
                OwedCust.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Dashboard-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Remove_debt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
