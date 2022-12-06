using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenManagerUI
{
    internal class DatabaseConnection
    {
        MySqlConnection conn;
        public DatabaseConnection()
        {
            // Open a MySql Connection
            string server = "localhost";
            string database = "token_manager";
            string username = "tokenManagerUser";
            string password = "password";
            string constring = "SERVER=" + server + ";Database=" + database + ";UID=" + username + ";PASSWORD=" + password + ";";
            this.conn = new MySqlConnection(constring);
            conn.Open();
        }
        // Method to return the database connection
        public MySqlConnection connect()
        {
            return this.conn;
        }
        // Method to return the data reader
        public MySqlDataReader Query(string q)
        {
            string query = q;
            MySqlCommand cmd = new MySqlCommand(query, this.conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        // Method to close the connection
        public void Close()
        {
            this.conn.Close();
        }
    }
}
