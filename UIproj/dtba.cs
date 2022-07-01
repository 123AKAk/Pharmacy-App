using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIproj.myclass
{
    class dtba
    {
        public MySqlConnection connectdb;

        //public static bool connectiontest;
        public dtba()
        {
            string connetionString = "server=localhost;database=eyopharm;uid=root;pwd=;SSL Mode=None";
            connectdb = new MySqlConnection(connetionString);
        }
    }
}
