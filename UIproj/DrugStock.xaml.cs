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
    /// Interaction logic for DrugStock.xaml
    /// </summary>
    public partial class DrugStock : UserControl
    {
        dtba connec = new dtba();
        MySqlCommand insertCommand, insertCommand2;

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;

        int whichWin;
        public DrugStock()
        {
            InitializeComponent();
            GetprodCate();
            GetAllProduc(0, Int32.Parse(limitnum.Text));

            whichWin = Settings1.Default.onWhichWindow;
            if(whichWin == 1)
            {
                drugdta.Opacity = 0;
                akwax.Opacity = 0;
                drugdta.Visibility = Visibility.Visible;
                dta.Visibility = Visibility.Collapsed;
                akway.Visibility = Visibility.Collapsed;
                akwax.Visibility = Visibility.Visible;

                asarah.Kind = MaterialDesignThemes.Wpf.PackIconKind.PaperAdd;
                asarah.ToolTip = "Add Drugs";

                DoubleAnimation badass = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    AutoReverse = false
                };
                akwax.BeginAnimation(OpacityProperty, badass);
                drugdta.BeginAnimation(OpacityProperty, badass);
                akwax.Opacity = 100;
                drugdta.Opacity = 100;
            }
            else if (whichWin == 0)
            {
                dta.Opacity = 0;
                akway.Opacity = 0;
                dta.Visibility = Visibility.Visible;
                drugdta.Visibility = Visibility.Collapsed;
                akwax.Visibility = Visibility.Collapsed;
                akway.Visibility = Visibility.Visible;

                asarah.Kind = MaterialDesignThemes.Wpf.PackIconKind.PaperEdit;
                asarah.ToolTip = "Edit Drugs";

                DoubleAnimation badass = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    AutoReverse = false
                };
                akway.BeginAnimation(OpacityProperty, badass);
                dta.BeginAnimation(OpacityProperty, badass);
                akway.Opacity = 100;
                dta.Opacity = 100;
            }
            else
            {

            }
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
            {displayMessage.Background = Brushes.DimGray;}
            else if(msgValue == 1)
            {displayMessage.Background = Brushes.IndianRed;}
            else
            {return;}
            
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

        private void Btndrawer_Click(object sender, RoutedEventArgs e)
        {
            if (drugdta.Visibility == Visibility.Visible)
            {
                dta.Opacity = 0;
                akway.Opacity = 0;
                dta.Visibility = Visibility.Visible;
                drugdta.Visibility = Visibility.Collapsed;
                akwax.Visibility = Visibility.Collapsed;
                akway.Visibility = Visibility.Visible;                

                asarah.Kind = MaterialDesignThemes.Wpf.PackIconKind.PaperEdit;
                asarah.ToolTip = "Edit Drugs";

                DoubleAnimation badass = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    AutoReverse = false
                };
                akway.BeginAnimation(OpacityProperty, badass);
                dta.BeginAnimation(OpacityProperty, badass);
                akway.Opacity = 100;
                dta.Opacity = 100;

                Settings1.Default.onWhichWindow = 0;
            }
            else
            {
                drugdta.Opacity = 0;
                akwax.Opacity = 0;
                drugdta.Visibility = Visibility.Visible;
                dta.Visibility = Visibility.Collapsed;
                akway.Visibility = Visibility.Collapsed;
                akwax.Visibility = Visibility.Visible;

                asarah.Kind = MaterialDesignThemes.Wpf.PackIconKind.PaperAdd;
                asarah.ToolTip = "Add Drugs";

                DoubleAnimation badass = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    AutoReverse = false
                };
                akwax.BeginAnimation(OpacityProperty, badass);
                drugdta.BeginAnimation(OpacityProperty, badass);
                akwax.Opacity = 100;
                drugdta.Opacity = 100;

                Settings1.Default.onWhichWindow = 1;
            }            
            Settings1.Default.Save();
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

        //Get category into combo box
        private void GetprodCate()
        {
            MySqlCommand ccommand;

            var cQuery = "SELECT Category_Name FROM catego ORDER BY id DESC";

            ccommand = new MySqlCommand(cQuery, connec.connectdb);

            try
            {
                //for ecommandc
                connec.connectdb.Open();
                MySqlDataReader readerc = ccommand.ExecuteReader();
                while (readerc.Read())
                {
                    productCate.Items.Add(readerc["Category_Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connec.connectdb.Close();
            }
        }

        //cleardata in textbox
        private void cleardata_click(object sender, RoutedEventArgs e)
        {
            clearProduct();
        }

        //add product
        string[] richiea;
        string madda;
        private void insertProduc()
        {
            MySqlCommand ccommand;
            var cQuery = "SELECT Product_Name FROM product ORDER BY id DESC";
            ccommand = new MySqlCommand(cQuery, connec.connectdb);
            try
            {
                connec.connectdb.Open();
                MySqlDataReader readerc = ccommand.ExecuteReader();
                while (readerc.Read())
                {
                    madda += readerc["Product_Name"].ToString() + ",";
                    richiea = madda.Split(',');
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connec.connectdb.Close();
            }
            int pos = Array.IndexOf(richiea, producname.Text.ToUpper());
            if (pos > -1)
            {
                EchoMessage("Product Already Exist", 1);
                return;
            }
            else
            {
                DateTime now = DateTime.Now;
                string date = now.ToString();

                string insertQuery = "INSERT INTO product(Cat_Name,Product_Name,Quantity,Supplier,Price,Manufacturing_Date,Expiring_Date,Date_Bought,Notes,Insertion_Date)values(@dCat_Name,@dProduct_Name,@dQuantity,@dSupplier,@dPrice,@dManufacturing_Date,@dExpiring_Date,@dDate_Bought,@dNotes,@dInsertion_Date)";
                insertCommand = new MySqlCommand(insertQuery, connec.connectdb);

                insertCommand.Parameters.AddWithValue("@dCat_Name", productCate.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dProduct_Name", producname.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dQuantity", quantity.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dSupplier", supplier.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dPrice", price.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dManufacturing_Date", manuDate.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dExpiring_Date", expSDate.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dDate_Bought", dateBoug.Text.ToUpper());
                insertCommand.Parameters.AddWithValue("@dNotes", specialNotes.Text);
                insertCommand.Parameters.AddWithValue("@dInsertion_Date", date);
                try
                {
                    connec.connectdb.Open();
                    int result = insertCommand.ExecuteNonQuery();
                    if (result > 0)
                    {
                        GetAllProduc(0, Int32.Parse(limitnum.Text));
                        EchoMessage("Success: Product Added", 0);                                                
                    }
                    else
                    {
                        EchoMessage("Error Unable to Proceed", 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }
                clearProduct();
            }
        }

        //clear all fields
        private void clearProduct()
        {
            productCate.Text = "";
            producname.Clear();
            supplier.Clear();
            quantity.Clear();
            price.Clear();
            manuDate.Text = "";
            expSDate.Text = "";
            dateBoug.Text = "";
        }
        
        //validate input fields
        private void btn_add_click(object sender, RoutedEventArgs e)
        {            
            if (productCate.Text == "")
            {
                EchoMessage("Select Product Category, If there is no Category, go to Category Page and Add", 1);                
            }
            else if (producname.Text == "")
            {
                EchoMessage("Product Name Feild cannot be Empty", 1);                
            }
            else if (supplier.Text == "")
            {
                EchoMessage("Supplier/Company Feild cannot be Empty", 1);                
            }
            else if (quantity.Text == "")
            {
                EchoMessage("Quantity Feild cannot be Empty", 1);                
            }
            else if (price.Text == "")
            {
                EchoMessage("Price Feild cannot be Empty", 1);                
            }
            else if (manuDate.Text == "")
            {
                EchoMessage("Manufacturing Date Feild cannot be Empty", 1);                
            }
            else if (expSDate.Text == "")
            {
                EchoMessage("Expiring Date Feild cannot be Empty", 1);                
            }
            else if (dateBoug.Text == "")
            {
                EchoMessage("Date Bought Feild cannot be Empty", 1);                
            }
            else
            {
                insertProduc();
            }
        }

        //checks if text is an integer
        public TextBox checTxt;
        private void check_TextChanged(object sender, TextChangedEventArgs e)
        {
            checTxt = (TextBox)sender;
            int parsedValue;
            if (!int.TryParse(checTxt.Text, out parsedValue))
            {
                checTxt.Clear();
                return;
            }
        }

        //GETS ALL ADDED PRODUCTS
        private void GetAllProduc(int vala, int valb)
        {            
            if(limitnum.Text == "")
            {
                valb = 8;
            }
            else
            {
                valb = Int32.Parse(limitnum.Text);
            }
            try
            {
                string sql = "SELECT * FROM product ORDER BY id DESC LIMIT "+vala+","+valb+"";
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


                        selecteddis = allProduc.SelectedIndex;
                        productName.Foreground = Brushes.DodgerBlue;
                        allProduc.Columns[3].IsReadOnly = true;

                        //DataGridRow dataGridRow2 = allProduc.ItemContainerGenerator.ContainerFromIndex(allProduc.SelectedIndex) as DataGridRow;
                        //dataGridRow2.Foreground = Brushes.DarkBlue;
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
                    GetAllProduc(avala, Int32.Parse(limitnum.Text));
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
                            GetAllProduc(avala, Int32.Parse(limitnum.Text));
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

                    string sqlQuery = "SELECT * FROM product WHERE "+saerchType.Text+ " LIKE '" + searchstr + "' ";

                    dss = new DataSet();
                    DataTable dtt = new DataTable();
                    daa = new MySqlDataAdapter(sqlQuery, connec.connectdb);
                    daa.Fill(dss);
                    allProduc.ItemsSource = dss.Tables[0].DefaultView;
                }
                catch (Exception ex)
                {
                    EchoMessage("Error "+ex.Message, 1);
                    return;
                }
            }
            else
            {
                GetAllProduc(avala, Int32.Parse(limitnum.Text));
            }
        }

        //validate to move dispensary
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                var Result = MessageBox.Show("Are You sure you want to move Product '"+productName.Text+"' to Dispensary!?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
                        GetAllProduc(avala, Int32.Parse(limitnum.Text));
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


        int avala = 0;
        private void previous_Click(object sender, RoutedEventArgs e)
        {
            avala -= 8;            
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
                limitnum.Text = "8";
                GetAllProduc(0, Int32.Parse(limitnum.Text));
            }
        }

        private void allProduc_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //var row = e.Row;
            //var v = e.Row.Item.ToString();

            ////if (e.Row.Item.ToString() == "Dispesed")
            //if (v.Contains("Price"))
            //{
            //    row.Foreground = Brushes.DarkBlue;
            //}
            //else
            //{

            //}
        }        

        private void next_Click(object sender, RoutedEventArgs e)
        {
            avala += 8;            
            GetAllProduc(avala, Int32.Parse(limitnum.Text));
        }
    }
}
