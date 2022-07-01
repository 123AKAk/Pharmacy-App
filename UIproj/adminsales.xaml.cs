using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data;
using System.Collections;

namespace UIproj
{
    /// <summary>
    /// Interaction logic for adminsales.xaml
    /// </summary>
    public partial class adminsales : UserControl
    {
        dtba connec = new dtba();

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;

        string queryState = "SState='1' AND Credit = ''";
        string querySize = "LIMIT 25";
        public adminsales()
        {
            InitializeComponent();
            GetallProductSales();
            GridSize.Text = "25";
            SalesType.Text = "Show Completed Sales";
        }

        //        
        //echo message STARTS HERE
        DispatcherTimer timer = new DispatcherTimer();
        private void EchoMessage(String txtMessage, int msgValue)
        {
            displayMessage.IsEnabled = true;

            DoubleAnimation doubleanimation = new DoubleAnimation();
            doubleanimation.From = 0;
            doubleanimation.To = 35;
            doubleanimation.Duration = TimeSpan.FromSeconds(1);
            doubleanimation.EasingFunction = new QuarticEase();

            displaText.Text = txtMessage;
            if (msgValue == 0)
            { displayMessage.Background = Brushes.DimGray; }
            else if (msgValue == 1)
            { displayMessage.Background = Brushes.IndianRed; }
            else
            { return; }

            displayMessage.BeginAnimation(HeightProperty, doubleanimation);
            timer.Tick += Timer_tick;
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Start();
        }

        //stops the mesasge timer and returns it back to default
        private void Timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            DoubleAnimation doubleanimation = new DoubleAnimation();
            doubleanimation.From = 35;
            doubleanimation.To = 0;
            doubleanimation.Duration = TimeSpan.FromSeconds(1);

            doubleanimation.EasingFunction = new QuarticEase();
            displayMessage.BeginAnimation(HeightProperty, doubleanimation);
        }
        //echo message ENDS HERE        
        //on mouse hover keeps the message display visible        
        private void displayMessage_MouseEnter(object sender, MouseEventArgs e)
        {
            timer.Stop();
        }
        private void displayMessage_MouseLeave(object sender, MouseEventArgs e)
        {
            displayMessage.IsEnabled = false;
            timer.Tick += Timer_tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        //echo message ends here

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


        //All sales
        private void GetallProductSales()
        {
            string aquery = "SELECT id, Product_Id, Product_Name, Customer_Name, Quantity, Price, Insertion_Date, Prescription_Status, Credit FROM psales WHERE " + queryState + " ORDER BY id DESC " + querySize + "";
            try
            {
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(aquery, connec.connectdb);
                daa.Fill(dss);
                allProductSales.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (All Sales-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //HIDES COLUMN THAT ARE NOT SUPPOSE TO BE IN THE DATAGRID
        private void AllProductSales_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Product_Id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Product_Name")
            {
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Quantity")
            {
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Price")
            {
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Insertion_Date")
            {
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        private void AllProductSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;

                if (rowSelected != null)
                {
                    idds.Text = rowSelected[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Product-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchSale_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchSale.Text != "")
            {
                try
                {
                    string asearchstr = "%" + searchSale.Text + "%";
                    string sqlQuery = "SELECT id,Product_Name,Customer_Name,Quantity,Price,Insertion_Date,Prescription_Status,Credit FROM psales WHERE Insertion_Date LIKE '%" + asearchstr + "%' AND "+ queryState + " ORDER BY id DESC";
                    dss = new DataSet();
                    DataTable dtt = new DataTable();
                    daa = new MySqlDataAdapter(sqlQuery, connec.connectdb);
                    daa.Fill(dss);
                    allProductSales.ItemsSource = dss.Tables[0].DefaultView;
                }
                catch (Exception ex)
                {
                    EchoMessage("Error " + ex.Message, 1);
                    return;
                }
            }
            else
            {
                GetallProductSales();
            }
        }

        //save data
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                try
                {
                    scmbl = new MySqlCommandBuilder(daa);
                    daa.Update(dss);
                    GetallProductSales();
                    EchoMessage("Saved", 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Product-Save)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                EchoMessage("Select Product to Proceed", 1);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (GridSize.Text.Equals("Show All"))
            {
                querySize = "";
            }
            else
            {
                int size = Int32.Parse(GridSize.Text);
                querySize = "LIMIT " + size + "";
            }
            if (SalesType.Text.Equals("Show Deleted Sales"))
            {
                queryState = "SState='0' AND Credit = ''";
            }
            if (SalesType.Text.Equals("Show Completed Sales"))
            {
                queryState = "SState='1' AND Credit = ''";
            }
            if (SalesType.Text.Equals("Show Credited Sales"))
            {
                queryState = "SState='1' AND Credit != ''";
            }
            GetallProductSales();
        }
    }
}
