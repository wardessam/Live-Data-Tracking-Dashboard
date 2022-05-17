using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace MSI_service
{
    //Call From Filterization
    class lossyCountingAlgo
    {
        DataTable FI = new DataTable();
        DataTable new_FI = new DataTable();
        
        public void bucketes_creator(DataTable items) 
        {
            double min_freq = items.Rows.Count*0.4;
            double e = 0.01;
            double bucket_size = Math.Ceiling(1 / e);
            int beta_bucket_size = 2;
            DataTable bucket = new DataTable();

            bucket.Columns.Add("Items", typeof(string));
            bucket.Columns.Add("Date", typeof(DateTime));
            bucket.Columns.Add("BranchID", typeof(int));

            FI.Columns.Add("Items",typeof(string));
            FI.Columns.Add("Frequency",typeof(int));
            FI.Columns.Add("Date",typeof(DateTime));
            FI.Columns.Add("Season",typeof(string));
            FI.Columns.Add("BranchID",typeof(int));
            FI.Columns.Add("Delta", typeof(int));

            new_FI.Columns.Add("Items", typeof(string));
            new_FI.Columns.Add("Frequency", typeof(int));
            new_FI.Columns.Add("Date", typeof(DateTime));
            new_FI.Columns.Add("Season", typeof(string));
            new_FI.Columns.Add("BranchID", typeof(int));
            new_FI.Columns.Add("Delta", typeof(int));

            int counter = 0;
            int total_count = 0;
            int bucket_num = 1;

            items.Columns.Remove(items.Columns[2].ColumnName);
            foreach (DataRow row in items.Rows)
            {
                if (counter < bucket_size) 
                {
                    bucket.Rows.Add(row.ItemArray);
                    counter++;
                    total_count++;
                }
                if (total_count == items.Rows.Count && counter != bucket_size)
                {
                    Lossy_Counting(bucket, min_freq, e, bucket_num);
                    counter = 0;
                    bucket_num++;
                    bucket.Clear();
                }
                if (counter == bucket_size) 
                {
                    Lossy_Counting(bucket,min_freq,e,bucket_num);
                    counter = 0;
                    bucket_num++;
                    bucket.Clear();
                    if (total_count != items.Rows.Count)
                    {
                        bucket.Rows.Add(row.ItemArray);
                        counter++;
                        total_count++;
                    }
                }
            }

            //insert FI list in the database
            SqlConnection con;
            string connectionString = "Data Source=(local);Initial Catalog=MSI;Integrated Security=SSPI";
            con = new SqlConnection(connectionString);
            con.Open();
            foreach (DataRow row in FI.Rows)
            {
                SqlCommand cmd;
                string insert_FI = "insert into Frequent_Items(Frequet_Item,Amount,Date,Season,Branch_ID) values(@f_i , @am, @d, @s, @bid);";
                cmd = new SqlCommand(insert_FI, con);
                cmd.Parameters.AddWithValue("@f_i", row[0].ToString());
                cmd.Parameters.AddWithValue("@am", int.Parse(row[1].ToString()));
                cmd.Parameters.AddWithValue("@d", DateTime.Parse(row[2].ToString()));
                cmd.Parameters.AddWithValue("@s", row[3].ToString());
                cmd.Parameters.AddWithValue("@bid", int.Parse(row[4].ToString()));
                int ret = cmd.ExecuteNonQuery();
            }
            con.Close();

            counter = 0;
            bucket_num = 1;
            total_count = 0;
            foreach (DataRow row in items.Rows)
            {
                if (counter < (bucket_size * beta_bucket_size))
                {
                    bucket.Rows.Add(row.ItemArray);
                    counter++;
                    total_count++;
                }
                if (total_count == items.Rows.Count && counter != (bucket_size*beta_bucket_size))
                {
                    lossy_counting_itemsets(bucket, min_freq, e, bucket_num, beta_bucket_size);
                    counter = 0;
                    bucket_num++;
                    bucket.Clear();
                }
                if (counter == (bucket_size*beta_bucket_size))
                {
                    lossy_counting_itemsets(bucket, min_freq, e, bucket_num, beta_bucket_size);
                    counter = 0;
                    bucket_num++;
                    bucket.Clear();
                    if(total_count != items.Rows.Count)
                    {
                        bucket.Rows.Add(row.ItemArray);
                        counter++;
                        total_count++;
                    }

                }
            }
            con.Open();
            foreach (DataRow row in new_FI.Rows)
            {
                SqlCommand cmd;
                string insert_FI = "insert into Frequent_Items(Frequet_Item,Amount,Date,Season,Branch_ID) values(@f_i , @am, @d, @s, @bid);";
                cmd = new SqlCommand(insert_FI, con);
                cmd.Parameters.AddWithValue("@f_i", row[0].ToString());
                cmd.Parameters.AddWithValue("@am", int.Parse(row[1].ToString()));
                cmd.Parameters.AddWithValue("@d", DateTime.Parse(row[2].ToString()));
                cmd.Parameters.AddWithValue("@s", row[3].ToString());
                cmd.Parameters.AddWithValue("@bid", int.Parse(row[4].ToString()));
                int ret = cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        public int ConvertDateCalendar(DateTime DateConv)
        {
            DateTimeFormatInfo DTFormat;
            string DateLangCulture = "ar-sa";
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;
            DTFormat.Calendar = new System.Globalization.HijriCalendar();

            DTFormat.ShortDatePattern = "yyyy-MM-dd HH:mm tt";
            string hijri_date = DateConv.Date.ToString("f", DTFormat);
            int l = hijri_date.Length - 10;
            hijri_date = hijri_date.Remove(10, l);
            return (int.Parse(hijri_date.Substring(3, 2)));
        }
    
        public void Lossy_Counting(DataTable bucket, double min_freq, double e, int bucket_num) 
        {
            foreach(DataRow row in bucket.Rows)
            {
                string item = row[0].ToString();
                int branch_ID = int.Parse(row[2].ToString());
                string[] transaction_items = item.Split(',');
                for (int i = 0; i < transaction_items.Count(); i++) 
                {
                    bool exists = FI.Select().ToList().Exists(Row => Row["Items"].ToString().Equals(transaction_items[i]) && Row["BranchID"].ToString().Equals(branch_ID.ToString()));
                    if (exists) 
                    {
                        DataRow dr = FI.Select("Items = '"+transaction_items[i]+"' and BranchID = "+branch_ID.ToString()).FirstOrDefault();
                        if (dr != null)
                        {
                            dr["Frequency"] = int.Parse(dr["Frequency"].ToString())+1;
                        }
                    }
                    else 
                    {
                        string season="";
                        DateTime date = DateTime.Parse(row[1].ToString());
                        int hijiri_month = ConvertDateCalendar(date);
                        int branchID = int.Parse(row[2].ToString());

                        if(date.Month >=6 && date.Month <=8)
                        {
                            season = "summer";
                        }
                        else if (date.Month == 12 || date.Month == 1 || date.Month == 2) 
                        {
                            if (date.Month == 12) 
                            {
                                season = "christmas";
                            }
                            else 
                            {
                                season = "winter";
                            }
                        }
                        else if (hijiri_month == 9)
                        {
                            season = "ramadan";
                        }
                        else 
                        {
                            season = "normal";
                        }
                        int delta = bucket_num - 1;
                        FI.Rows.Add(transaction_items[i],1,date,season,branchID,delta);
                    }
                }
            }
            List<DataRow> delete_rows = new List<DataRow>();
            foreach(DataRow row in FI.Rows)
            {
                int freq = int.Parse(row[1].ToString());
                int delta = int.Parse(row[5].ToString());
                if ((freq + delta) <= bucket_num) 
                {
                    delete_rows.Add(row);
                }
            }
            foreach (DataRow r in delete_rows)
            {
                FI.Rows.Remove(r);
            }

        }

        public IEnumerable<IEnumerable<T>>
        GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public void lossy_counting_itemsets(DataTable bucket, double min_freq, double e, int bucket_num, int beta_bucket_size)
        {
            //0 = items
            //1 = date
            //2 = branch id
            int delta = Math.Abs(bucket_num - beta_bucket_size);
            DataTable subsets_date_branchID = new DataTable();
            subsets_date_branchID.Columns.Add("Items", typeof(string));
            subsets_date_branchID.Columns.Add("Date", typeof(DateTime));
            subsets_date_branchID.Columns.Add("BranchID", typeof(int));

            foreach (DataRow row in bucket.Rows)
            {
                string[] items = row[0].ToString().Split(',');
                string one_item = "";
                if (items.Count() == 1)
                    continue;
                IEnumerable<IEnumerable<string>> combinations = GetKCombs<string>(items, 2);
                foreach (var x in combinations)
                {
                    List<string> a = x.ToList<string>();
                    for (int i = 0; i < a.Count; i++)
                    {
                        one_item += a[i];
                        one_item += ',';
                    }
                    one_item = one_item.Remove(one_item.Length - 1, 1);
                    subsets_date_branchID.Rows.Add(one_item,row[1],row[2]);
                    one_item = "";
                }
            }
            List<DataRow> deleted = new List<DataRow>();
            foreach (DataRow row in subsets_date_branchID.Rows)
            {
                int frequency = 0;
                string itemset = row[0].ToString();
                string[] two_items = itemset.Split(',');
                string swap_items = two_items[1] + "," + two_items[0];
                DateTime date = DateTime.Parse(row[1].ToString());
                int branchID = int.Parse(row[2].ToString());

                if(deleted.Contains(row))
                {
                    continue;
                }

                DataRow[] dr = subsets_date_branchID.Select("(Items = '" + itemset + "' or Items = '"+ swap_items +"') and BranchID = " + branchID.ToString());

                if (dr != null)
                {
                    frequency=dr.Count();
                    foreach (DataRow row2 in dr)
                    {
                        deleted.Add(row2);
                    }
                }
                bool exists = new_FI.Select().ToList().Exists(Row => (Row["Items"].ToString().Equals(itemset) || Row["Items"].ToString().Equals(swap_items)) && Row["BranchID"].ToString().Equals(branchID.ToString()));
                if (exists)
                {
                    DataRow row3 = new_FI.Select("(Items = '" + itemset + "' or Items = '" + swap_items + "') and BranchID = " + branchID.ToString()).FirstOrDefault();
                    if (frequency+delta<=bucket_num)
                    {
                        new_FI.Rows.Remove(row3);
                    }
                    else
                    {
                         row3["Frequency"] = int.Parse(row3["Frequency"].ToString()) + frequency;
                    }
                }
                else
                {
                    if (frequency >= beta_bucket_size)
                    {
                        string season = "";
                        int hijiri_month = ConvertDateCalendar(date);
      
                        if (date.Month >= 6 && date.Month <= 8)
                        {
                            season = "summer";
                        }
                        else if (date.Month == 12 || date.Month == 1 || date.Month == 2)
                        {
                            if (date.Month == 12)
                            {
                                season = "christmas";
                            }
                            else
                            {
                                season = "winter";
                            }
                        }
                        else if (hijiri_month == 9)
                        {
                            season = "ramadan";
                        }
                        else
                        {
                            season = "normal";
                        }
                        new_FI.Rows.Add(itemset,frequency,date,season,branchID,delta);
                    }
                }

            }
        }
    }
}
