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
    /// Interaction logic for admincategory.xaml
    /// </summary>
    public partial class admincategory : UserControl
    {
        dtba connec = new dtba();
        MySqlCommand insertCommand;

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;
        public admincategory()
        {
            InitializeComponent();
            GetAllCatego();
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


        //ADD CATEGORY
        string[] richiea;
        string madda;
        private void addCategory(object sender, RoutedEventArgs e)
        {
            if (catName.Text != "")
            {
                MySqlCommand ccommand;
                var cQuery = "SELECT Category_Name FROM catego ORDER BY id DESC";
                ccommand = new MySqlCommand(cQuery, connec.connectdb);
                try
                {
                    connec.connectdb.Open();
                    MySqlDataReader readerc = ccommand.ExecuteReader();
                    while (readerc.Read())
                    {
                        madda += readerc["Category_Name"].ToString() + ",";

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
                int pos = Array.IndexOf(richiea, catName.Text.ToUpper());
                if (pos > -1)
                {
                    MessageBox.Show("Category Name Already Exit");
                }
                else
                {
                    DateTime now = DateTime.Now;
                    string date = now.ToString();

                    string insertQuery = "INSERT INTO catego(Category_Name,Date_Added)values(@dCatname,@dDate)";
                    insertCommand = new MySqlCommand(insertQuery, connec.connectdb);
                    insertCommand.Parameters.AddWithValue("@dCatname", catName.Text.ToUpper());
                    insertCommand.Parameters.AddWithValue("@dDate", date);
                    try
                    {
                        connec.connectdb.Open();
                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Category Added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            GetAllCatego();
                        }
                        else
                        {
                            MessageBox.Show("Unable to process Command", "Error! Adding Category", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                }
            }
            else
            {
                MessageBox.Show("Input Field cannot be Empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catName.Clear();
        }

        //GETS ALL ADDED CATEGORY
        private void GetAllCatego()
        {            
            try
            {
                string sql = "SELECT * FROM catego ORDER BY id DESC";                
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sql, connec.connectdb);
                daa.Fill(dss);
                allCatego.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Cat-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        //HIDES COLUMN THAT ARE NOT SUPPOSE TO BE IN THE DATAGRID
        private void allCatego_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Date_Added")
            {                
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        //allCatego datagrid selection
        private void allCatego_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;
                if (rowSelected != null)
                {
                    idds.Text = rowSelected[0].ToString();
                    catame.Text = rowSelected[1].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Cat-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    GetAllCatego();
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

        //deletes category
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                var Result = MessageBox.Show("Are You sure you want to Delete Category '" + catame.Text + "'?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {

                    string sQuery = "DELETE FROM `catego` WHERE id='" + idds.Text + "'";
                    insertCommand = new MySqlCommand(sQuery, connec.connectdb);
                    try
                    {
                        connec.connectdb.Open();
                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Category '" + catame.Text + "' Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            GetAllCatego();
                        }
                        else
                        {
                            MessageBox.Show("Unable to Delete Category '" + catame.Text + "'", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " (Cat-Del)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Select Category to Process Command", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //category search
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchstr = "%" + cateSearch.Text + "%";

                string sqlQuery = "SELECT * FROM catego WHERE Category_Name LIKE '" + searchstr + "' ";

                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sqlQuery, connec.connectdb);
                daa.Fill(dss);
                allCatego.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }       
    }
}
