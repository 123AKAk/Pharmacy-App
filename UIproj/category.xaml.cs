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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIproj.myclass;

namespace UIproj
{
    /// <summary>
    /// Interaction logic for category.xaml
    /// </summary>
    public partial class category : UserControl
    {
        dtba connec = new dtba();
        MySqlCommand insertCommand;

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;
        public category()
        {
            InitializeComponent();
            GetAllCatego();
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
                    MessageBox.Show("Category Name Already Exist");
                    return;
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
            try
            {
                scmbl = new MySqlCommandBuilder(daa);
                daa.Update(dss);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Cat-Save)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            MessageBox.Show("Unable to Delete Category '" + catame.Text + "'", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Select Sudent to Process Command", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
