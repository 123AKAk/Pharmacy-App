using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UIproj.myclass;

namespace UIproj
{
    class Watchdog
    {
        public static dtba connec = new dtba();
        public static MySqlCommand insertCommand;

        public static void fight(string username, string userevent)
        {
            string insertQuery = "INSERT INTO usersevents(User_Name,User_Event)values(@dUser_Name,@dUser_Event)";
            insertCommand = new MySqlCommand(insertQuery, connec.connectdb);

            insertCommand.Parameters.AddWithValue("@dUser_Name", username);
            insertCommand.Parameters.AddWithValue("@dUser_Event", userevent);

            try
            {
                connec.connectdb.Open();
                int result = insertCommand.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("User Event Captured");
                }
                else
                {
                    MessageBox.Show("Unable to Capture User Event");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"Error on Watch Dog");
            }
            finally
            {
                connec.connectdb.Close();
            }
        }
    }
}
