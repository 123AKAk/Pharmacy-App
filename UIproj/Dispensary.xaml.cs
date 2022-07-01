using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Dispensary.xaml
    /// </summary>
    public partial class Dispensary : UserControl
    {
        dtba connec = new dtba();
        MySqlCommand insertCommand, insertCommand2;

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;

        public Dispensary()
        {
            InitializeComponent();
            GetAllProduc(0, Int32.Parse(limitnum.Text));
        }

        //        
        //echo message STARTS HERE
        DispatcherTimer timer = new DispatcherTimer();
        private void EchoMessage(String txtMessage, int msgValue)
        {
            displayMessage.IsEnabled = true;

            DoubleAnimation doubleanimation = new DoubleAnimation();
            doubleanimation.From = 0;
            doubleanimation.To = 55;
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
            doubleanimation.From = 55;
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

        //GETS ALL ADDED PRODUCTS
        private void GetAllProduc(int vala, int valb)
        {
            if (limitnum.Text == "")
            {
                valb = 10;
            }
            else
            {
                valb = Int32.Parse(limitnum.Text);
            }
            try
            {
                string sql = "SELECT * FROM product WHERE Dispensed='1' ORDER BY id DESC LIMIT " + vala + "," + valb + "";
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sql, connec.connectdb);
                daa.Fill(dss);
                allProduc.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                return;
                MessageBox.Show(ex.Message + " (Product-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        //HIDES COLUMN THAT ARE NOT SUPPOSE TO BE IN THE DATAGRID
        private void AllDispensedProduc_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Cat_Name")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Quantity")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Supplier")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Manufacturing_Date")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Expiring_Date")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Date_Bought")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Notes")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Insertion_Date")
            {                
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Dispensed")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }            
        }

        //allDispensedproduc datagrid selection
        string dispenDataCate, dispenDataDate, expringDate;
        int dispenDataQuan, dispenDataRemains;
        private void AllDispensedProduc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;

                if (rowSelected != null)
                {
                    idds.Text = rowSelected[0].ToString();
                    productName.Text = rowSelected[2].ToString();
                    priceofD.Text = rowSelected[5].ToString();
                    dispenDataQuan = Int32.Parse(rowSelected[12].ToString());
                    dispenDataRemains = Int32.Parse(rowSelected[3].ToString());
                    dispenDataCate = rowSelected[1].ToString();
                    dispenDataDate = rowSelected[10].ToString();
                    expringDate = rowSelected[7].ToString();

                    DateTime dateTimeVal = Convert.ToDateTime(expringDate);
                    validdate.Text = dateTimeVal.ToLongDateString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Product-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        //product search
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (producSearch.Text != "")
            {
                try
                {
                    string searchstr = "%" + producSearch.Text + "%";

                    string sqlQuery = "SELECT * FROM product WHERE Product_Name LIKE '" + searchstr + "' ";

                    dss = new DataSet();
                    DataTable dtt = new DataTable();
                    daa = new MySqlDataAdapter(sqlQuery, connec.connectdb);
                    daa.Fill(dss);
                    allProduc.ItemsSource = dss.Tables[0].DefaultView;
                }
                catch (Exception ex)
                {
                    EchoMessage("Error " + ex.Message, 1);
                    return;
                }
            }
            else
            {
                GetAllProduc(avala, Int32.Parse(limitnum.Text));
            }
        }

        private void ViewInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" " + productName.Text + " Info:\n\n" +
              "\t1. Price : " + priceofD.Text + "\n" +
              "\t2. Dispensed Quantity : " + dispenDataQuan + "\n" +
              "\t3. Remaining Quantity : " + dispenDataRemains + "\n" +
              "\t4. Product Category : " + dispenDataCate + "\n" +
              "\t5. Date Added : " + dispenDataDate + "");
        }

        private void RemoveFromDispensary_Click(object sender, RoutedEventArgs e)
        {
            RemoveProducFromDispensary();
        }

        //add product quantity to dispesary
        int quantity, Dispensed_Quantity;
        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (dispenDataRemains == 0)
            {
                return;
            }
            else
            {
                EchoMessage("" + dispenDataRemains-- + "" + dispenDataQuan++ + "", 2);
                quantity = dispenDataRemains--;
                Dispensed_Quantity = dispenDataQuan++;

                string insertQuery2 = "UPDATE product SET Quantity=" + quantity + ", Dispensed_Quantity=" + Dispensed_Quantity + " WHERE id =" + idds.Text + "";
                insertCommand2 = new MySqlCommand(insertQuery2, connec.connectdb);

                try
                {
                    connec.connectdb.Open();
                    int result2 = insertCommand2.ExecuteNonQuery();
                    if (result2 > 0)
                    {
                        GetAllProduc(avala, Int32.Parse(limitnum.Text));
                    }
                    else
                    {
                        EchoMessage("Error! increasing Product Quantity, Unable to process Command", 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Dispense-Add)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }
            }
        }

        //substract product quantity from dispesary
        private void SubstractClick(object sender, RoutedEventArgs e)
        {
            if(dispenDataQuan == 1)
            {
                RemoveProducFromDispensary();
            }
            else
            {
                EchoMessage("" + dispenDataRemains++ + "" + dispenDataQuan--+"", 2);
                quantity = dispenDataRemains++;
                Dispensed_Quantity = dispenDataQuan--;

                string insertQuery2 = "UPDATE product SET Quantity=" + quantity + ", Dispensed_Quantity=" + Dispensed_Quantity + " WHERE id =" + idds.Text + "";
                insertCommand2 = new MySqlCommand(insertQuery2, connec.connectdb);

                try
                {
                    connec.connectdb.Open();
                    int result2 = insertCommand2.ExecuteNonQuery();
                    if (result2 > 0)
                    {
                        GetAllProduc(avala, Int32.Parse(limitnum.Text));
                    }
                    else
                    {
                        EchoMessage("Error! decreasing Product Quantity, Unable to process Command", 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Dispense-Substract)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }
            }            
        }
        // remove product from dispennsary
        private void RemoveProducFromDispensary()
        {
            var Result = MessageBox.Show("Are You sure you want to remove Product '" + productName.Text + "' from Dispensary!?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                EchoMessage("" + dispenDataRemains + dispenDataQuan + "", 2);
                int addedquantity = dispenDataRemains + dispenDataQuan;
                //code starts here
                string insertQuery2 = "UPDATE product SET Quantity=" + addedquantity + ", Dispensed_Quantity='0', Dispensed='0'  WHERE id =" + idds.Text + "";
                insertCommand2 = new MySqlCommand(insertQuery2, connec.connectdb);

                try
                {
                    connec.connectdb.Open();
                    int result2 = insertCommand2.ExecuteNonQuery();
                    if (result2 > 0)
                    {
                        GetAllProduc(0, Int32.Parse(limitnum.Text));
                        EchoMessage("Product Removed from Dispensary", 0);
                    }
                    else
                    {
                        EchoMessage("Error! removing Product from Dispensary, Unable to process Command", 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Dispense-Remove)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }                
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }

        //PAGINATION
        int avala = 0;
        private void previous_Click(object sender, RoutedEventArgs e)
        {
            avala -= 10;
            if (avala <= 0)
            {
                avala = 0;
                GetAllProduc(avala, Int32.Parse(limitnum.Text));
            }
            else
            {
                GetAllProduc(avala, Int32.Parse(limitnum.Text));
            }
        }
        private void limitnum_TextChanged(object sender, TextChangedEventArgs e)
        {
            int parsedValue;
            if (!int.TryParse(limitnum.Text, out parsedValue))
            {
                limitnum.Text = "10";
                GetAllProduc(0, Int32.Parse(limitnum.Text));
            }
        }
        
        private void next_Click(object sender, RoutedEventArgs e)
        {
            avala += 10;
            GetAllProduc(avala, Int32.Parse(limitnum.Text));
        }


        
    }
}
