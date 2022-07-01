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
    /// Interaction logic for adminusers.xaml
    /// </summary>
    public partial class adminusers : UserControl
    {

        dtba connec = new dtba();
        MySqlCommand insertCommand, insertCommand2;

        MySqlDataAdapter daa;
        DataSet dss;

        MySqlCommandBuilder scmbl;

        public adminusers()
        {
            InitializeComponent();
            GetAllUsers();
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

        //GETS ALL ADDED USERS
        private void GetAllUsers()
        {
            try
            {
                string sql = "SELECT * FROM USERS WHERE User_State='0' ORDER BY id DESC ";
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sql, connec.connectdb);
                daa.Fill(dss);
                UserGrid.ItemsSource = dss.Tables[0].DefaultView;
                GetBlockedUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (User-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetBlockedUsers()
        {
            try
            {
                string sql = "SELECT * FROM USERS WHERE User_State='1' ORDER BY id DESC ";
                dss = new DataSet();
                DataTable dtt = new DataTable();
                daa = new MySqlDataAdapter(sql, connec.connectdb);
                daa.Fill(dss);
                BlockedUserGrid.ItemsSource = dss.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Blocked_User-View)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //HIDES COLUMN THAT ARE NOT SUPPOSE TO BE IN THE DATAGRID
        private void UserGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "id")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "Password")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
            if (e.Column.Header.ToString() == "User_State")
            {
                e.Cancel = true;   // For not to include 
                e.Column.IsReadOnly = true; // Makes the column as read only
            }
        }

        //UserGrid data selection
        string uSERNAME;
        private void UserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;
                if (rowSelected != null)
                {
                    idds.Text = rowSelected[0].ToString();
                    uSERNAME = rowSelected[2].ToString();

                    //userid.Text = rowSelected[1].ToString();
                    //username.Text = rowSelected[2].ToString();
                    //password.Password = rowSelected[3].ToString();
                    //email.Text = rowSelected[4].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (User-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Add info to DB
        string[] eyoea;
        string akaka;
        private void UserInfoAdd(object sender, RoutedEventArgs e)
        {
            if (userid.Text == "" || username.Text == "" || password.Password == "")
            {
                EchoMessage("Fill all input Fields", 1);
            }
            else
            {
                MySqlCommand ccommand;
                var cQuery = "SELECT UserId FROM users ORDER BY id DESC";
                ccommand = new MySqlCommand(cQuery, connec.connectdb);
                try
                {
                    connec.connectdb.Open();
                    MySqlDataReader readerc = ccommand.ExecuteReader();
                    while (readerc.Read())
                    {
                        akaka += readerc["UserId"].ToString() + ",";
                        eyoea = akaka.Split(',');
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
                if (eyoea == null)
                {
                    addUsers();
                }
                else
                {
                    int pos = Array.IndexOf(eyoea, userid.Text.ToUpper());
                    if (pos > -1)
                    {
                        EchoMessage("User Already Exist", 1);
                        return;
                    }
                    else
                    {
                        addUsers();
                    }
                }
            }
        }
        //GETS ALL ADDED USERS
        private void addUsers()
        {
            DateTime now = DateTime.Now;
            string date = now.ToString();

            string insertQuery = "INSERT INTO users(UserId,Username,Password,Email)values(@dUserId,@dUsername,@dPassword,@dEmail)";
            insertCommand = new MySqlCommand(insertQuery, connec.connectdb);

            insertCommand.Parameters.AddWithValue("@dUserId", userid.Text.ToUpper());
            insertCommand.Parameters.AddWithValue("@dUsername", username.Text.ToUpper());
            insertCommand.Parameters.AddWithValue("@dPassword", password.Password.ToUpper());
            insertCommand.Parameters.AddWithValue("@dEmail", email.Text.ToUpper());
            try
            {
                connec.connectdb.Open();
                int result = insertCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    GetAllUsers();
                    EchoMessage("Success: User Added", 0);
                }
                else
                {
                    EchoMessage("Error adding User, Unable to Process Command", 1);
                }
            }
            catch (Exception ex)
            {
                EchoMessage("Error: " + ex.Message, 1);
                //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connec.connectdb.Close();
            }
            userid.Clear();
            username.Clear();
            password.Clear();
            email.Clear();
        }

        //Clear data
        private void UserInfoClear(object sender, RoutedEventArgs e)
        {
            userid.Clear();
            username.Clear();
            password.Clear();
            email.Clear();
        }

        //Save edits
        private void SaveEdit(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                try
                {
                    scmbl = new MySqlCommandBuilder(daa);
                    daa.Update(dss);
                    GetAllUsers();
                    EchoMessage("Saved", 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (User-Save)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                EchoMessage("Select User to Proceed", 1);
            }
        }

        //remove users
        private void RemoveUser(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                var Result = MessageBox.Show("Are You sure you want to Remove User '" + uSERNAME + "'?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {

                    string sQuery = "DELETE FROM `users` WHERE id='" + idds.Text + "'";
                    insertCommand = new MySqlCommand(sQuery, connec.connectdb);
                    try
                    {
                        connec.connectdb.Open();
                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            GetAllUsers();
                            EchoMessage("Success: '" + uSERNAME + "' Deleted", 0);
                        }
                        else
                        {
                            EchoMessage("Error Unable to Delete '" + uSERNAME + "'", 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " (User-Del)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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
                EchoMessage("Select User to Proceed", 1);
            }
        }
        
        //Block user
        private void BlockUser(object sender, RoutedEventArgs e)
        {
            if (idds.Text != "")
            {
                var Result = MessageBox.Show("Are You sure you want to `Block User '" + uSERNAME + "'?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    string insertQuery2 = "UPDATE users SET User_State='1' WHERE id =" + idds.Text + "";
                    insertCommand2 = new MySqlCommand(insertQuery2, connec.connectdb);

                    try
                    {
                        connec.connectdb.Open();
                        int result2 = insertCommand2.ExecuteNonQuery();
                        if (result2 > 0)
                        {
                            GetAllUsers();
                            EchoMessage("'" + uSERNAME + "' has been Blocked", 0);
                        }
                        else
                        {
                            EchoMessage("Error! Blocking User, Unable to process Command", 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " (User-Block)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                EchoMessage("Select User to Proceed", 1);
                return;
            }
        }

        //unblock user
        int unblockId; string BlockuSERNAME;
        private void BlockedUserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;
                DataRowView rowSelected = dg.SelectedItem as DataRowView;
                if (rowSelected != null)
                {
                    unblockId = Int32.Parse(rowSelected[0].ToString());
                    BlockuSERNAME = rowSelected[2].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (User-Selection)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnBlockUser(object sender, RoutedEventArgs e)
        {
            if (unblockId.ToString() != "")
            {
                var Result = MessageBox.Show("Are You sure you want to `UnBlock User '" + BlockuSERNAME + "'?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    string insertQuery2 = "UPDATE users SET User_State='0' WHERE id =" + unblockId + "";
                    insertCommand2 = new MySqlCommand(insertQuery2, connec.connectdb);
                    try
                    {
                        connec.connectdb.Open();
                        int result2 = insertCommand2.ExecuteNonQuery();
                        if (result2 > 0)
                        {
                            GetAllUsers();
                            EchoMessage("'" + BlockuSERNAME + "' has been UnBlocked", 0);
                        }
                        else
                        {
                            EchoMessage("Error! UnBlocking User, Unable to process Command", 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " (User-UnBlock)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                EchoMessage("Select Blocked User to Proceed", 1);
                return;
            }
        }

        
    }
}
