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
    /// Interaction logic for adminstock.xaml
    /// </summary>
    public partial class adminstock : UserControl
    {
        dtba connec = new dtba();
        MySqlCommand insertCommand, insertCommand2;

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;

        string querySize = "LIMIT 25";

        public adminstock()
        {
            InitializeComponent();
            GetAllProduc();
            GridSize.Text = "25";
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
        //echo message ends

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
        private void GetAllProduc()
        {
            string aquery = "SELECT * FROM product ORDER BY id DESC " + querySize + "";
            try
            {
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(aquery, connec.connectdb);
                daa.Fill(dss);
                allProduc.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Product-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //HIDES COLUMN THAT ARE NOT SUPPOSE TO BE IN THE DATAGRID
        private void allProduc_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Quantity")
            {
                int parsedValue;
                if (!int.TryParse(e.Column.ToString(), out parsedValue))
                {
                    e.Column.Equals("");
                    return;
                }
            }
            if (e.Column.Header.ToString() == "Insertion_Date")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only                                              
            }
            if (e.Column.Header.ToString() == "Dispensed")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only                                              
            }
            if (e.Column.Header.ToString() == "Dispensed_Quantity")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only                                              
            }
        }

        //allproduc datagrid selection        
        int dispense = 0;
        int aquantis;
        private int selecteddis;
        private void allProduc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dispense = 0;
            allProduc.Columns[3].IsReadOnly = false;
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;

                if (rowSelected != null)
                {
                    idds.Text = rowSelected[0].ToString();
                    productName.Text = rowSelected[2].ToString();
                    aquantis = Int32.Parse(rowSelected[3].ToString());
                    dispense = Int32.Parse(rowSelected[11].ToString());

                    //giving color to the dispensed items
                    if (dispense.Equals(1))
                    {
                        //Style cellStyle = new Style(typeof(DataGridCell));                        
                        //Setter setcolor = new Setter(DataGridCell.ForegroundProperty, Foreground=Brushes.Gray);
                        //cellStyle.Setters.Add(setcolor);
                        //allProduc.Columns[3].CellStyle = cellStyle;

                        allProduc.Columns[3].IsReadOnly = true;
                        selecteddis = allProduc.SelectedIndex;
                        productName.Foreground = Brushes.DarkRed;
                        
                        //DataGridRow dataGridRow2 = allProduc.ItemContainerGenerator.ContainerFromIndex(allProduc.SelectedIndex) as DataGridRow;
                        //dataGridRow2.Foreground = Brushes.DarkRed;
                    }
                    else
                    {
                        productName.Foreground = Brushes.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Product-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //saves edit
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                try
                {
                    scmbl = new MySqlCommandBuilder(daa);
                    daa.Update(dss);
                    GetAllProduc();
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

        //deletes product
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                var Result = MessageBox.Show("Are You sure you want to Delete Product '" + productName.Text + "'?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {

                    string sQuery = "DELETE FROM `product` WHERE id='" + idds.Text + "'";
                    insertCommand = new MySqlCommand(sQuery, connec.connectdb);
                    try
                    {
                        connec.connectdb.Open();
                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            GetAllProduc();
                            EchoMessage("Success: Product '" + productName.Text + "' Deleted", 0);
                        }
                        else
                        {
                            EchoMessage("Error Unable to Delete Product '" + productName.Text + "'", 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " (Product-Del)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
            else
            {
                EchoMessage("Select Product to Proceed", 1);
            }
        }

        //product search
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (saerchType.Text != "")
            {
                try
                {
                    string searchstr = "%" + producSearch.Text + "%";

                    string sqlQuery = "SELECT * FROM product WHERE " + saerchType.Text + " LIKE '" + searchstr + "' ";

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
                GetAllProduc();
            }
        }

        //validate to move dispensary
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                var Result = MessageBox.Show("Are You sure you want to move Product '" + productName.Text + "' to Dispensary!?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    //checks if product is dispensed already                   
                    if (dispense == 1)
                    {
                        EchoMessage("Product is Already Dispensed", 1);
                        return;
                    }
                    //end
                    else
                    {
                        dispensedQuantityError.Visibility = Visibility.Hidden;
                        dispenseOpen.IsOpen = true;
                        dispensedQuantity.Text = "1";
                    }
                }
                else if (Result == MessageBoxResult.No)
                {
                    return;
                }
            }
            else
            {
                EchoMessage("Select Product to Proceed", 1);
            }
        }

        int aboutDispensedVal;
        private void SubstractDispenClick(object sender, RoutedEventArgs e)
        {
            aboutDispensedVal = Int32.Parse(dispensedQuantity.Text);
            aboutDispensedVal--;
            dispensedQuantity.Text = aboutDispensedVal.ToString();
            if (Int32.Parse(dispensedQuantity.Text) < 1)
            {
                aboutDispensedVal++;
                dispensedQuantity.Text = aboutDispensedVal.ToString();
            }
            else
            {
                dispensedQuantityError.Visibility = Visibility.Hidden;
                dispensednext.IsEnabled = true;
            }
        }

        private void AddDispenClick(object sender, RoutedEventArgs e)
        {
            aboutDispensedVal = Int32.Parse(dispensedQuantity.Text);
            aboutDispensedVal++;
            dispensedQuantity.Text = aboutDispensedVal.ToString();
            if (Int32.Parse(dispensedQuantity.Text) > aquantis)
            {
                dispensedQuantityError.Text = "Quantity Limit reached";
                dispensedQuantityError.Visibility = Visibility.Visible;
                aboutDispensedVal--;
                dispensedQuantity.Text = aboutDispensedVal.ToString();
            }
            else
            {
                dispensedQuantityError.Visibility = Visibility.Hidden;
                dispensednext.IsEnabled = true;
            }
        }

        //move to dispensary
        private void nextDispense(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(dispensedQuantity.Text) != 0)
            {
                DateTime now = DateTime.Now;
                string adate = now.ToString();

                EchoMessage("" + aquantis + "-" + dispensedQuantity.Text + "", 2);
                int newquantity = aquantis - Int32.Parse(dispensedQuantity.Text);

                string insertQuery2 = "UPDATE product SET Quantity=" + newquantity + ", Dispensed='1', Dispensed_Quantity='" + dispensedQuantity.Text + "' WHERE id =" + idds.Text + "";
                insertCommand2 = new MySqlCommand(insertQuery2, connec.connectdb);

                try
                {
                    connec.connectdb.Open();
                    int result2 = insertCommand2.ExecuteNonQuery();
                    if (result2 > 0)
                    {
                        GetAllProduc();
                        EchoMessage("Product '" + productName.Text + "' has been Moved to Dispensary", 0);
                    }
                    else
                    {
                        EchoMessage("Error! moving Product, Unable to process Command", 1);
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
            else
            {
                EchoMessage("Quantity cannot be Zero", 1);
                return;
            }
        }
        
        //refresh datagrid
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
            GetAllProduc();
        }
    }
}
