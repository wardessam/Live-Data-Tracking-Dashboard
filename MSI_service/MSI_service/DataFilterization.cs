using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MSI_service
{
    class DataFilterization
    {
        SqlConnection con;
        SqlCommand cmd;
        DataTable transactions = new DataTable();
        public DataFilterization()
        {
            string connectionString = "Data Source=(local);Initial Catalog=MSI;Integrated Security=SSPI";
            con = new SqlConnection(connectionString);
            transactions.Columns.Add("Items", typeof(string));
            transactions.Columns.Add("Date", typeof(DateTime));
            transactions.Columns.Add("Cost", typeof(decimal));
            transactions.Columns.Add("BranchID", typeof(int));
        }

        public void Filterization(DataTable Store_Transaction_Data)
        {
            con.Open();


            foreach (DataRow row in Store_Transaction_Data.Rows)
            {
                try
                {
                    int T_ID = Convert.ToInt32(row["T_ID"]);
                    string T_Data = row["T_Data"].ToString();
                    DateTime Date = Convert.ToDateTime(row["Date"]);
                    decimal Cost = Convert.ToDecimal(row["Cost"]);
                    int Branch_ID = Convert.ToInt32(row["Branch_ID"]);


                    if (T_Data == null || T_Data.Equals(""))
                        continue;
                    if (Date == null || !Date.ToShortDateString().Equals(DateTime.Today.ToShortDateString()))
                        continue;
                    if (Cost == null || Cost < 0.0m)
                        continue;



                    string ADD_DATA_To_Store_Transaction_TABLE = SqlQueries.ADD_DATA_To_Store_Transaction_TABLE;
                    cmd = new SqlCommand(ADD_DATA_To_Store_Transaction_TABLE, con);
                    cmd.Parameters.AddWithValue("@T_ID", T_ID);
                    cmd.Parameters.AddWithValue("@T_Data", T_Data);
                    cmd.Parameters.AddWithValue("@Date", Date.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Cost", Cost);
                    cmd.Parameters.AddWithValue("@Branch_ID", Branch_ID);
                    int ret = cmd.ExecuteNonQuery();
                    transactions.Rows.Add(T_Data, Date, Cost, Branch_ID);
                }
                catch (Exception e)
                {
                    continue;
                }



            }

            con.Close();
            lossyCountingAlgo LCA = new lossyCountingAlgo();
            LCA.bucketes_creator(transactions);
        }
    }
}
