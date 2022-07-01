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
    /// Interaction logic for makeSale.xaml
    /// </summary>
    public partial class makeSale : UserControl
    {
        dtba connec = new dtba();
        MySqlCommand insertCommand, insertCommand2, insertCommand3;

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;

        public makeSale()
        {
            InitializeComponent();
            GetAllProduc();
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

        //GETS ALL ADDED PRODUCTS
        private void GetAllProduc()
        {
            try
            {
                string sql = "SELECT id, Product_Name, Price, Quantity, Dispensed_Quantity FROM product WHERE Dispensed='1' ORDER BY id DESC";
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sql, connec.connectdb);
                daa.Fill(dss);
                allProduc.ItemsSource = dss.Tables[0].DefaultView;
                replaceProduc.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Product-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            GetallProductSales();
        }
        //HIDES COLUMN THAT ARE NOT SUPPOSE TO BE IN THE DATAGRID
        private void AllDispensedProduc_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Quantity")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Price")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        int aquantis;
        private void AllDispensedProduc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;

                if (rowSelected != null)
                {
                    idds.Text = rowSelected[0].ToString();
                    drugName.Text = rowSelected[1].ToString();
                    price.Text = rowSelected[2].ToString();
                    aquantis = Int32.Parse(rowSelected[4].ToString());
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

                    string sqlQuery = "SELECT id, Product_Name, Price, Quantity FROM product WHERE Product_Name LIKE '" + searchstr + "' ";

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

        private void credit_Click(object sender, RoutedEventArgs e)
        {
            if (credit.IsChecked == true)
            {
                creditNotes.Visibility = Visibility.Visible;
            }
            else if (credit.IsChecked == false)
            {
                creditNotes.Visibility = Visibility.Collapsed;
            }
            else
            {

            }
        }

        private void nextGrid_Click(object sender, RoutedEventArgs e)
        {
            int toprice = 0;

            gridTwo.Visibility = Visibility.Visible;
            DoubleAnimation badass = new DoubleAnimation
            {
                From = 0,
                To = 720,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                AutoReverse = false
            };
            gridTwo.BeginAnimation(WidthProperty, badass);

            //adds the prices together
            foreach (int price in saleprice)
            {
                toprice = toprice + price;
            }

            totalprice.Text = "N"+toprice.ToString();
        }
        
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            gridOne.Visibility = Visibility.Visible;
            DoubleAnimation badass = new DoubleAnimation
            {
                From = 720,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                AutoReverse = false
            };
            gridTwo.BeginAnimation(WidthProperty, badass);
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

        private void openAllSales_Click(object sender, RoutedEventArgs e)
        {
            if(makeSales.Visibility == Visibility.Visible)
            {
                itk.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                allSales.Visibility = Visibility.Visible;
                makeSales.Visibility = Visibility.Collapsed;
                asarah.Kind = MaterialDesignThemes.Wpf.PackIconKind.Medicine;
                asarah.ToolTip = "Make Sale";
                btnSarah.Margin = new Thickness(0, 5, 47, 0);
            }
            else
            {
                itk.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                makeSales.Visibility = Visibility.Visible;
                allSales.Visibility = Visibility.Collapsed; 
                asarah.Kind = MaterialDesignThemes.Wpf.PackIconKind.Purse;
                asarah.ToolTip = "View All Sales";
                btnSarah.Margin = new Thickness(0, 5, 30, 0);
            }
        }
       
        int seriNo = 0;
        string aName;
        ArrayList productList = new ArrayList();
        ArrayList productListQuantity = new ArrayList();
        ArrayList aquantisQuantity = new ArrayList();
        ArrayList idQuantity = new ArrayList();
        ArrayList saleprice = new ArrayList();
        private void addprod_Click(object sender, RoutedEventArgs e)
        {
            aName = patientName.Text;
            int creditstate = 0;

            if (drugName.Text == "")
            {
                EchoMessage("Select Product to Proceed", 1);
            }
            else if (totalQuantity.Text == "")
            {
                EchoMessage("Insert Quantity of Product to be Bought", 1);
            }
            else
            {
                foreach (var item in lvSdrugs.Items)
                {
                    if (item.ToString().Contains(drugName.Text))
                    {
                        EchoMessage("Product is already in Order List", 1);
                        return;
                }   }
                if (seriNo == 5)
                {
                    EchoMessage("Maximum Sale Ammount is 5", 1);
                }
                if (Int32.Parse(totalQuantity.Text) > aquantis)
                {
                    EchoMessage("Your Quantity is Higer than what's in Dispensary", 1);
                    return;
                }
                if (lvSdrugs.Items.Contains(drugName.Text))
                {
                    EchoMessage("That Product is already in Order", 1);
                }
                else
                {
                    if (creditNotes.Text != "")
                    {
                        creditstate = 1;
                    }

                    seriNo++;
                    string sseNo = seriNo.ToString();
                    var row = new { serialNo = sseNo, patientNamea = aName, drugNamea = drugName.Text, pricea = price.Text, quantitya = totalQuantity.Text, prescriptionStatusa = precription.Text, credita = creditNotes.Text };
                    lvSdrugs.Items.Add(row);
                    lvSdrugs2.Items.Add(row);
                    productList.Add("'" + patientName.Text + "','" + drugName.Text + "','" + price.Text + "','" + totalQuantity.Text + "','" + precription.Text + "','" + creditNotes.Text + "','"+ creditstate + "'");
                    productListQuantity.Add(totalQuantity.Text);
                    aquantisQuantity.Add(aquantis);
                    idQuantity.Add(idds.Text);
                    saleprice.Add(Convert.ToInt32(price.Text));
                }
                    numDrugsAdded.Text = lvSdrugs.Items.Count.ToString();
                    drugName.Clear();
                    patientName.Clear();
                    totalQuantity.Clear();
                    price.Clear();
                    precription.Clear();
                    creditNotes.Clear();
                    credit.IsChecked = false;
            }
        }

        private void cleardrugBox(object sender, RoutedEventArgs e)
        {
            drugName.Clear();
            patientName.Clear();
            totalQuantity.Clear();
            price.Clear();
            precription.Clear();
            creditNotes.Clear();
            //credit.IsChecked = false;
        }

        private void TotalQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            int parsedValue;
            if (!int.TryParse(totalQuantity.Text, out parsedValue))
            {
                totalQuantity.Text = "";
            }
        }

        private void removeDrug(object sender, RoutedEventArgs e)
        {
            //how to get selected items in the Listview
            //String text = lvSdrugs.SelectedItems[0].ToString();

            int sNum = lvSdrugs.SelectedIndex;

            if (lvSdrugs.SelectedItem != null)
            {
                try
                {
                    //var selected = lvSdrugs.SelectedItems.Cast<Object>().ToArray();
                    //foreach (var item in selected)
                    //{
                    //    lvSdrugs.Items.Remove(item);
                    //    lvSdrugs2.Items.Remove(item);
                    //}
                    lvSdrugs.Items.RemoveAt(sNum);
                    lvSdrugs2.Items.RemoveAt(sNum);
                    productList.RemoveAt(sNum);
                    productListQuantity.RemoveAt(sNum);
                    aquantisQuantity.RemoveAt(sNum);
                    idQuantity.RemoveAt(sNum);
                    saleprice.RemoveAt(sNum);
                    //lvSdrugs.Items.RemoveAt(lvSdrugs.Items.IndexOf(lvSdrugs.SelectedItem));
                }
                catch (Exception ex)
                {
                    precription.Text = ex.Message;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("No Product has been Selected");
            }
            numDrugsAdded.Text = lvSdrugs.Items.Count.ToString();
        }

        string itemstoInsert; bool done;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int blodNum=0;
            foreach (string avar in productList)
            {
                itemstoInsert = itemstoInsert + avar.ToString();

                DateTime now = DateTime.Now;
                string date = now.ToString();

                EchoMessage("" + Int32.Parse(aquantisQuantity[blodNum].ToString()) + "-" + Int32.Parse(productListQuantity[blodNum].ToString()) + "", 2);
                int newquantity = Int32.Parse(aquantisQuantity[blodNum].ToString()) - Int32.Parse(productListQuantity[blodNum].ToString());
                string queryUpdate;
                if (newquantity <= 0)
                {
                    queryUpdate = "UPDATE product SET Dispensed_Quantity=" + newquantity + ", Dispensed='0' WHERE id =" + idQuantity[blodNum].ToString() + "";
                }
                else
                {
                    queryUpdate = "UPDATE product SET Dispensed_Quantity=" + newquantity + " WHERE id =" + idQuantity[blodNum].ToString() + "";
                }
                //MessageBox.Show(Int32.Parse(aquantisQuantity[blodNum].ToString()) + " - " + Int32.Parse(productListQuantity[blodNum].ToString()) + " = " + newquantity + " | Id" + Int32.Parse(idQuantity[blodNum].ToString()));

                //inserts the sold product to the sales table                
                insertCommand2 = new MySqlCommand(queryUpdate, connec.connectdb);
                insertCommand = new MySqlCommand("INSERT INTO psales(Product_Id,Customer_Name,Product_Name,Price,Quantity,Prescription_Status,Credit,Credit_State,Insertion_Date,SState)values('"+ idQuantity[blodNum].ToString() + "'," + itemstoInsert + ",'" + date + "','1')", connec.connectdb);
                try
                {
                    connec.connectdb.Open();
                    int result2 = insertCommand2.ExecuteNonQuery();
                    if (result2 > 0)
                    {
                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            done = true;
                        }
                        else
                        {
                            EchoMessage("Error Unable to Sale Product(s)", 1);
                        }
                    }
                    else
                    {
                        EchoMessage("Error! moving Product, Unable to process Command", 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Sell-Product)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                    itemstoInsert = string.Empty;
                    blodNum = blodNum + 1;
                }
            }
            if(done == true)
            {
                EchoMessage("Success: Product(s) Sold", 0);
                GetAllProduc();

                productList.Clear();
                productListQuantity.Clear();
                aquantisQuantity.Clear();
                idQuantity.Clear();

                lvSdrugs.Items.Clear();
                lvSdrugs2.Items.Clear();

                numDrugsAdded.Text = "0";
            }
            else
            {
                return;
            }
        }

        //All sales
        private void GetallProductSales()
        {
            DateTime now = DateTime.Now;
            string toDate = now.Date.ToString("dd/MM/yyyy");
            try
            {
                string sql = "SELECT id,Product_Id,Product_Name,Customer_Name,Quantity,Price,Insertion_Date,Prescription_Status,Credit FROM psales WHERE Insertion_Date LIKE '%" + toDate + "%' AND SState='1' ORDER BY id DESC";
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sql, connec.connectdb);
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
        int allSalesId, makQuan, productId;
        string prodname, custName ;
        private void AllProductSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;

                if (rowSelected != null)
                {
                    allSalesId = Int32.Parse(rowSelected[0].ToString());
                    productId = Int32.Parse(rowSelected[1].ToString());
                    makQuan = Int32.Parse(rowSelected[4].ToString());
                    custName = rowSelected[3].ToString();
                    prodname = rowSelected[2].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Product-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //deletes sales and moves the sold product back to dispensary

        //select product quantity from the gotten product id
        int toAddQuantity;
        private void GetProductQuanity()
        {
            MySqlCommand ccommand;
            var cQuery = "SELECT Dispensed_Quantity FROM product WHERE id =" + productId + " ORDER BY id DESC";
            ccommand = new MySqlCommand(cQuery, connec.connectdb);
            try
            {
                connec.connectdb.Open();
                MySqlDataReader readerc = ccommand.ExecuteReader();
                while (readerc.Read())
                {
                    toAddQuantity = Int32.Parse(readerc["Dispensed_Quantity"].ToString());
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

        bool doneRemove;
        private void RemoveSale_Click(object sender, RoutedEventArgs e)
        {
            if(custName.Equals(""))
            { custName = "No Identification"; }
            var Result = MessageBox.Show("Are You sure you want to remove Product '" + prodname + "' bought by "+ custName + " from Sales?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                GetProductQuanity();

                EchoMessage("" + toAddQuantity + "+" + makQuan + "", 2);
                int returnedQuan = toAddQuantity + makQuan;

                insertCommand2 = new MySqlCommand("UPDATE product SET Dispensed_Quantity=" + returnedQuan + " WHERE id =" + productId + "", connec.connectdb);
                insertCommand = new MySqlCommand("UPDATE psales SET SState='0' WHERE id =" + allSalesId + "", connec.connectdb);
                try
                {
                    connec.connectdb.Open();
                    int result2 = insertCommand2.ExecuteNonQuery();
                    if (result2 > 0)
                    {
                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            doneRemove = true;
                        }
                        else
                        {
                            EchoMessage("Error Removing Sold Product", 1);
                        }
                    }
                    else
                    {
                        EchoMessage("Error! Removing Sold Product, Unable to process Command", 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Remove-Sale)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }

                if (doneRemove == true)
                {
                    EchoMessage("Success: Product removed from Sales", 0);
                    GetallProductSales();
                    GetAllProduc();
                }
                else
                {
                    return;
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }

        //replace product code <--
        private void ReplaceSale_Click(object sender, RoutedEventArgs e)
        {
            replaceProd.IsOpen = true;
        }
        //replace selection
        int replaceSalesId;
        private void ReplaceProduc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;

                if (rowSelected != null)
                {
                    replaceSalesId = Int32.Parse(rowSelected[0].ToString());
                    replaceName.Text = rowSelected[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Product-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        string replacedName; int replacedPrice, replacedId, replaceQuant;
        private void GetReplaceProduct()
        {
            MySqlCommand ccommand;
            var cQuery = "SELECT * FROM product WHERE id =" + replaceSalesId + " ORDER BY id DESC";
            ccommand = new MySqlCommand(cQuery, connec.connectdb);
            try
            {
                connec.connectdb.Open();
                MySqlDataReader readerc = ccommand.ExecuteReader();
                while (readerc.Read())
                {
                    replacedId = Int32.Parse(readerc["id"].ToString());
                    replacedName = readerc["Product_Name"].ToString();
                    replacedPrice = Int32.Parse(readerc["Price"].ToString());
                    replaceQuant = Int32.Parse(readerc["Dispensed_Quantity"].ToString());
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
        //replaces the product with another product
        private void RunReplace_click(object sender, RoutedEventArgs e)
        {
            GetProductQuanity();
            GetReplaceProduct();

            DateTime bnow = DateTime.Now;
            string date = bnow.ToString();

            EchoMessage("" + toAddQuantity + "+" + makQuan + "", 2);

            int areturnedQuan = toAddQuantity + makQuan;
            int breturnedQuan = replaceQuant - makQuan;

            if (replaceQuant < makQuan)
            {
                EchoMessage("The Selected Product Quantity is less than what to be replaced", 1);
                return;
            }
            else if(productId == replacedId)
            {
                EchoMessage("The Product can't be the same", 1);
                return;
            }
            else
            {
                string sQuery = "UPDATE product SET Dispensed_Quantity=" + breturnedQuan + " WHERE id =" + replaceSalesId + "";
                if (breturnedQuan == 0)
                {
                    sQuery = "UPDATE product SET Dispensed_Quantity=" + breturnedQuan + ", Dispensed='0' WHERE id =" + replaceSalesId + "";
                }
                insertCommand2 = new MySqlCommand("UPDATE product SET Dispensed_Quantity=" + areturnedQuan + " WHERE id =" + productId + "", connec.connectdb);
                insertCommand = new MySqlCommand(sQuery, connec.connectdb);
                insertCommand3 = new MySqlCommand("UPDATE psales SET Product_Id='" + replacedId + "', Product_Name='" + replacedName + "', Price='" + replacedPrice + "', Insertion_Date='" + date + "'  WHERE id =" + allSalesId + "", connec.connectdb);
                try
                {
                    connec.connectdb.Open();
                    int result2 = insertCommand2.ExecuteNonQuery();
                    if (result2 > 0)
                    {
                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            int result3 = insertCommand3.ExecuteNonQuery();
                            if (result3 > 0)
                            {
                                EchoMessage("Product has been Replaced", 0);
                                GetallProductSales();
                                GetAllProduc();
                            }
                            else
                            {
                                EchoMessage("Error Unable to Replace Product (3)", 1);
                            }
                        }
                        else
                        {
                            EchoMessage("Error Unable to Replace Product (2)", 1);
                        }
                    }
                    else
                    {
                        EchoMessage("Error Unable to Replace Product (1)", 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Replace-Sale)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }
            }
        }
        //replace product code -->

        private void MenuItem_Click(object sender, RoutedEventArgs e)
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

        private void SearchSale_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchSale.Text != "")
            {
                try
                {
                    string asearchstr = "%" + searchSale.Text + "%";
                    string sqlQuery = "SELECT id,Product_Name,Customer_Name,Quantity,Price,Insertion_Date,Prescription_Status,Credit FROM psales WHERE Insertion_Date LIKE '%" + asearchstr + "%' AND SState='1' ORDER BY id DESC";
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
    }
}
