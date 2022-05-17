using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MSI_service
{
    class mysql
    {
        public void GetData(string Server_Name,string Database_Name,string UserID, string password , string query) {
            MySqlConnection con = new MySqlConnection();
            string connectionString = "Server = "+Server_Name+"; Database = "+Database_Name+"; Uid = "+UserID+"; Pwd = "+password+";";
            con.ConnectionString = connectionString;
            con.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            MySqlDataAdapter adapt = new MySqlDataAdapter();
            adapt.SelectCommand = cmd;
            DataTable Transaction = new DataTable();
            adapt.Fill(Transaction);
            adapt.Dispose();
            con.Close();
            con.Dispose();
            DataFilterization DF = new DataFilterization();
            DF.Filterization(Transaction);
        }

    }
}
