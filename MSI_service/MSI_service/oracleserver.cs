using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data;


namespace MSI_service
{
    class oracleserver
    {
        public void GetData(string Server_Name, string Database_Name, string UserID, string password, string query)
        {
            string connectionString = "@Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)" +
                "(HOST = " + Server_Name + ")(PORT =  1521  ))" +
                "(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = " + Database_Name + "))); Password = " + password + "; User ID = " + UserID;
            OracleConnection con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "select ActorID from Actors";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable Transaction = new DataTable();
            Transaction.Load(dr);
            dr.Close();
            con.Close();
            con.Dispose();
            DataFilterization DF = new DataFilterization();
            DF.Filterization(Transaction);
        }
    }
}
