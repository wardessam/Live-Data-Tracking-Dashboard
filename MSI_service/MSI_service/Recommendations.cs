using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace MSI_service
{
    public class Recommendations
    {
        public static string conStr = "Data Source=(local);Initial Catalog=MSI;Integrated Security=SSPI; MultipleActiveResultSets=True;";
        public static void Normal_Recommendations()
        {

            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            //Delete Old Normal Normal Recommendations
            SqlCommand command222 = new SqlCommand(SqlQueries.DeletePreviousRecommendations, con);
            command222.Parameters.AddWithValue("Season", "normal");
            command222.ExecuteNonQuery();
            //
            string Today = DateTime.Now.ToShortDateString();
            Dictionary<Int64, Dictionary<string, Int64>> FrequentItems = new Dictionary<Int64, Dictionary<string, Int64>>();
           
            SqlCommand command = new SqlCommand(SqlQueries.SelectAllFromSpecific, con);
            command.Parameters.AddWithValue("Season", "normal");
            SqlDataReader reader = command.ExecuteReader();
            
            
            //Step 2 - Get all the data of Frequent items
            // Frequent Items Key:- Branch ID
            //Frequent Items Value:- Dictionary that has multiple strings 'Frequent Items' as keys
            //Dictionary Value that has multiple ints 'Amount'
            //Sample
            // Key: '1' :-- Value (Dictionary) : [("coffee","milk"),20]  
            // Key: '1' :-- Value (Dictionary) : [("Paper","pen"),70] 
            // Key: '3' :-- Value (Dictionary) : [("Laptop","Airpods"),120] 
            // Same key can have multiple dictionaries
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                  if (!FrequentItems.ContainsKey(Int64.Parse(reader["Branch_ID"].ToString())))
                     {
                      FrequentItems.Add(Int64.Parse(reader["Branch_ID"].ToString()), new Dictionary<string, Int64>());
                     }
                    FrequentItems[Int64.Parse(reader["Branch_ID"].ToString())].Add(reader["Frequent_Item"].ToString(), Int32.Parse(reader["Amount"].ToString()));
                }

            }
            else
            {
                Console.WriteLine("No rows found.");
            }

           



            foreach (var x in FrequentItems)
            {
                Dictionary<string, long> dd = x.Value;
                int Recommendations_Count = 0;

                foreach (KeyValuePair<string, long> entry in dd)
                {

                    if (Recommendations_Count < 10)
                    {
                        // entry.Value or entry.Key
                        SqlCommand command2 = new SqlCommand(SqlQueries.SaveNormalRecommdations, con);
                        command2.Parameters.AddWithValue("@Item", entry.Key);
                        command2.Parameters.AddWithValue("@Season", "Normal");
                        command2.Parameters.AddWithValue("@Amount", entry.Value);
                        command2.Parameters.AddWithValue("@Branch_ID", x.Key);
                        command2.ExecuteNonQuery();
                        Recommendations_Count++;
                    }
                }



            }

        }
        public static void Seasons_Recommendations(string season)
        {
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            //Delete all season data before recommend new items

            SqlCommand command222 = new SqlCommand(SqlQueries.DeletePreviousRecommendations, con);
            command222.Parameters.AddWithValue("Season", season);
            command222.ExecuteNonQuery();


            //Add New seasons Recommendations
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Branch_ID", typeof(Int64));
            dataTable.Columns.Add("Frequent_Item", typeof(string));
            dataTable.Columns.Add("Amount", typeof(Int64));
            
            SqlCommand command = new SqlCommand(SqlQueries.SelectAllFromSpecific, con);
            command.Parameters.AddWithValue("Season", season);
            SqlDataReader reader4 = command.ExecuteReader();
            
           
            //Step 1 - Get all the data of Frequent items

            if (reader4.HasRows)
            {
                while (reader4.Read())
                {
                 dataTable.Rows.Add(long.Parse(reader4["Branch_ID"].ToString()), reader4["Frequent_Item"].ToString(), long.Parse(reader4["Amount"].ToString()));
                }

            }
            else
            {
                Console.WriteLine("No rows found.");
            }


            int sizeofTable = dataTable.Rows.Count;
            Dictionary<string, List<long>> Items = new Dictionary<string, List<long>>();
            for (int i = 0; i < sizeofTable; i++)
            {
                string s = dataTable.Rows[i]["Branch_ID"] + " " + dataTable.Rows[i]["Frequent_Item"];
                if (!Items.ContainsKey(s))
                {
                    Items.Add(s, new List<long>());
                }
                Items[s].Add((long)dataTable.Rows[i]["Amount"]);

            }

            int Recommendations_Count = 0,counter=0;
            //Get First branch ID So we could save the only 10 recommendations
            string idx = dataTable.Rows[0]["Branch_ID"] + " " + dataTable.Rows[0]["Frequent_Item"];
            string f = Items[idx].ToString();
            string[] result_ = f.Split(' ');
            string branch_ = result_[0];
            //
            foreach (var x in Items)
            {

                string data = x.Key;
                string[] result = data.Split(' ');
                string branch = result[0]; string item = result[1];
                if (branch.Equals(branch_))
                {
                    if (Recommendations_Count < 10)
                    {
                        List<long> list = new List<long>(x.Value);
                        long amount = 0;
                        for (int t = 0; t < list.Count; t++)
                        {
                            amount += list[t];
                        }
                        amount /= list.Count;
                        SqlCommand command2 = new SqlCommand(SqlQueries.SaveSpecificRecommdations, con);
                        command2.Parameters.AddWithValue("@Item", item);
                        command2.Parameters.AddWithValue("@Season", season);
                        command2.Parameters.AddWithValue("@Amount", amount);
                        command2.Parameters.AddWithValue("@Branch_ID", branch);
                        command2.ExecuteNonQuery();
                    }
                    Recommendations_Count++;
                }
                else
                {
                    Recommendations_Count=0;
                    branch_ = branch;
                    //
                    List<long> list = new List<long>(x.Value);
                    long amount = 0;
                    for (int t = 0; t < list.Count; t++)
                    {
                        amount += list[t];
                    }
                    amount /= list.Count;
                    SqlCommand command2 = new SqlCommand(SqlQueries.SaveSpecificRecommdations, con);
                    command2.Parameters.AddWithValue("@Item", item);
                    command2.Parameters.AddWithValue("@Season", season);
                    command2.Parameters.AddWithValue("@Amount", amount);
                    command2.Parameters.AddWithValue("@Branch_ID", branch);
                    command2.ExecuteNonQuery();
                    Recommendations_Count++;
                }
                


            }
        }
        public static void Reports()
        {
            List<int> branch_IDs = new List<int>();
            DateTime now = DateTime.Now;
            DataTable sales = new DataTable();
            sales.Columns.Add("cost", typeof(decimal));
            sales.Columns.Add("amount", typeof(int));
            sales.Columns.Add("Date", typeof(DateTime));
            sales.Columns.Add("branchID", typeof(int));
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            SqlCommand command = new SqlCommand("select Branch_ID from [dbo].[Store_Branch]", con);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    branch_IDs.Add(int.Parse(reader[0].ToString()));
                }

            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            con.Close();
            con.Open();
            foreach (int x in branch_IDs)
            {
                command = new SqlCommand("select SUM(Cost),count(T_ID) from [dbo].[Store_Transaction] where Branch_ID=@ID and Date=@date", con);
                command.Parameters.AddWithValue("@ID", x.ToString());
                command.Parameters.AddWithValue("@date", now.ToShortDateString());
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sales.Rows.Add(reader[0].ToString(), reader[1].ToString(), now.ToString(), x.ToString());
                    }

                }
            }
            reader.Close();
            con.Close();
            con.Open();
            foreach (DataRow row in sales.Rows)
            {
                SqlCommand cmd;
                string insert_FI = "insert into Total_Sales_Branch(Cost,Amount,Date,Branch_ID) values(@C , @am, @d, @bid);";
                cmd = new SqlCommand(insert_FI, con);
                cmd.Parameters.AddWithValue("@C", decimal.Parse(row[0].ToString()));
                cmd.Parameters.AddWithValue("@am", int.Parse(row[1].ToString()));
                cmd.Parameters.AddWithValue("@d", DateTime.Parse(row[2].ToString()));
                cmd.Parameters.AddWithValue("@bid", int.Parse(row[3].ToString()));
                int ret = cmd.ExecuteNonQuery();
            }
            con.Close();
        }


    }
}
