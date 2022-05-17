using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MSI_service
{
    class sqlserver
    {
        public void GetData(string Server_Name, string Database_Name, string UserID, string password, string query)
        {
            SqlConnection con;
            string connectionString = "Server = " + Server_Name + "; Database = " + Database_Name + "; User Id = " + UserID + "; Password = " + password + ";";
            con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.CommandType = CommandType.Text;
            SqlDataReader reader = com.ExecuteReader();
            DataTable Transactions = new DataTable();
            Transactions.Load(reader);
            reader.Close();
            con.Close();
            con.Dispose();
            DataFilterization DF = new DataFilterization();
            DF.Filterization(Transactions);
        }
    }
}
