using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace MSI_service
{
    public class SpecificFrequentItems
    {
        public static string conStr = "Data Source=(local);Initial Catalog=MSI;Integrated Security=SSPI; MultipleActiveResultSets=True;";
        public static Dictionary<Int32, DataRow> SortDataForSpecificTable_Seasons()
        {
            DataTable d = getAllFromFrequentItems();
            DataRow[] rows = d.Select();
            Dictionary<Int32, DataRow> data = new Dictionary<int, DataRow>();
            Dictionary<Int32, DataRow> dataAfterSortSeasons = new Dictionary<int, DataRow>();
            string[] seasons = new string[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                data[i] = rows[i];
                seasons[i] = rows[i]["Season"].ToString();
            }


            var sorted = seasons
                .Select((x, i) => new KeyValuePair<string, int>(x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<string> B = sorted.Select(x => x.Key).ToList();
            List<int> idx = sorted.Select(x => x.Value).ToList();

            foreach (var key in data)
            {

                for (int i = 0; i < idx.Count; i++)
                {
                    dataAfterSortSeasons[i] = data[idx[i]];
                }
            }
            return dataAfterSortSeasons;
        }
        public static Dictionary<Int32, DataRow> SortDataForSpecificTable_BranchIDS(string season_)
        {
            Dictionary<Int32, DataRow> data = SortDataForSpecificTable_Seasons();
            Dictionary<Int32, DataRow> winter = new Dictionary<int, DataRow>();
            Dictionary<Int32, DataRow> summer = new Dictionary<int, DataRow>();
            Dictionary<Int32, DataRow> ramadan = new Dictionary<int, DataRow>();
            Dictionary<Int32, DataRow> christmas = new Dictionary<int, DataRow>();
            Dictionary<Int32, DataRow> normal = new Dictionary<int, DataRow>();
            int w = 0, s = 0, r = 0, c = 0,n=0;
            //Divide Data into 5 Categories
            foreach (var key in data)
            {
                DataRow row = key.Value;
                string season = row["Season"].ToString();
                if (season.Equals("Winter") || season.Equals("winter"))
                {
                    winter[w] = row;
                    w++;
                }
                else if (season.Equals("Summer") || season.Equals("summer"))
                {
                    summer[s] = row;
                    s++;
                }
                else if (season.Equals("Ramadan") || season.Equals("ramadan"))
                {
                    ramadan[r] = row;
                    r++;
                }
                else if(season.Equals("Christmas")||season.Equals("christmas"))
                {
                    christmas[c] = row;
                    c++;
                }
                else
                {
                    normal[n] = row;
                    n++;
                }
            }
            #region Winter
            //============================================
            //Winter Season Sorting
            int[,] winter_Branches = new int[w, 2];
            for (int i = 0; i < winter.Count; i++)
            {
                winter_Branches[i, 0] = i;
                winter_Branches[i, 1] = int.Parse(winter[i]["Branch_ID"].ToString());
            }
            sort_Ascending(winter_Branches, 0, winter_Branches.GetLength(0) - 1);
            Dictionary<Int32, DataRow> FinalSortedwinter = new Dictionary<int, DataRow>();
            foreach (var key in winter)
            {

                for (int i = 0; i < winter_Branches.GetLength(0); i++)
                {
                    FinalSortedwinter[i] = winter[winter_Branches[i, 0]];

                }
            }



            #endregion
            #region Normal
            //============================================
            //Winter Season Sorting
            int[,] normal_Branches = new int[n, 2];
            for (int i = 0; i < normal.Count; i++)
            {
                normal_Branches[i, 0] = i;
                normal_Branches[i, 1] = int.Parse(normal[i]["Branch_ID"].ToString());
            }
            sort_Ascending(normal_Branches, 0, normal_Branches.GetLength(0) - 1);
            Dictionary<Int32, DataRow> FinalSortednormal = new Dictionary<int, DataRow>();
            foreach (var key in normal)
            {

                for (int i = 0; i < normal_Branches.GetLength(0); i++)
                {
                    FinalSortednormal[i] = normal[normal_Branches[i, 0]];

                }
            }



            #endregion
            #region Summer
            //============================================
            //Summer Season Sorting
            int[,] summer_Branches = new int[s, 2];
            for (int i = 0; i < summer.Count; i++)
            {
                summer_Branches[i, 0] = i;
                summer_Branches[i, 1] = int.Parse(summer[i]["Branch_ID"].ToString());
            }
            sort_Ascending(summer_Branches, 0, summer_Branches.GetLength(0) - 1);
            Dictionary<Int32, DataRow> FinalSortedsummer = new Dictionary<int, DataRow>();
            foreach (var key in summer)
            {

                for (int i = 0; i < summer_Branches.GetLength(0); i++)
                {
                    FinalSortedsummer[i] = summer[summer_Branches[i, 0]];

                }
            }

            #endregion
            #region Ramadan
            //============================================
            //Ramadan Season Sorting
            int[,] ramadan_Branches = new int[r, 2];
            for (int i = 0; i < ramadan.Count; i++)
            {
                ramadan_Branches[i, 0] = i;
                ramadan_Branches[i, 1] = int.Parse(ramadan[i]["Branch_ID"].ToString());
            }
            sort_Ascending(ramadan_Branches, 0, ramadan_Branches.GetLength(0) - 1);
            Dictionary<Int32, DataRow> FinalSortedramadan = new Dictionary<int, DataRow>();
            foreach (var key in ramadan)
            {

                for (int i = 0; i < ramadan_Branches.GetLength(0); i++)
                {
                    FinalSortedramadan[i] = ramadan[ramadan_Branches[i, 0]];

                }
            }


            #endregion
            #region Christmas
            //============================================
            //Christmas Season Sorting
            int[,] chris_Branches = new int[c, 2];
            for (int i = 0; i < christmas.Count; i++)
            {
                chris_Branches[i, 0] = i;
                chris_Branches[i, 1] = int.Parse(christmas[i]["Branch_ID"].ToString());
            }
            sort_Ascending(chris_Branches, 0, chris_Branches.GetLength(0) - 1);
            Dictionary<Int32, DataRow> FinalSortedchris = new Dictionary<int, DataRow>();
            foreach (var key in christmas)
            {

                for (int i = 0; i < chris_Branches.GetLength(0); i++)
                {
                    FinalSortedchris[i] = christmas[chris_Branches[i, 0]];

                }
            }



            #endregion
            if (season_.Equals("ramadan") || season_.Equals("Ramadan")) return FinalSortedramadan;
            else if (season_.Equals("winter") || season_.Equals("Winter")) return FinalSortedwinter;
            else if (season_.Equals("summer") || season_.Equals("Summer")) return FinalSortedsummer;
            else if (season_.Equals("chrismtas") || season_.Equals("Chrismtas")) return FinalSortedchris;
            else return FinalSortednormal;
        }
        public static Dictionary<Int32, DataRow> TheFinalSort(string season)
        {

            List<int> amounts = new List<int>();
            int[,] amount;
            int c = 0, w = 0;
            Dictionary<Int32, DataRow> FinalSortedAmounts = new Dictionary<int, DataRow>();
            Dictionary<Int32, DataRow> season_Data = SortDataForSpecificTable_BranchIDS(season);
            int temp = int.Parse(season_Data[0]["Branch_ID"].ToString());

            for (int i = 0; i < season_Data.Count; i++)
            {
                DataRow row = season_Data[i];
                int b_ID = int.Parse(row["Branch_ID"].ToString());
                if (temp == b_ID && i != season_Data.Count - 1)
                {
                    amounts.Add(int.Parse(row["Amount"].ToString()));

                }
                else
                {
                    if (i == season_Data.Count - 1)
                    {
                        amounts.Add(int.Parse(row["Amount"].ToString()));
                    }
                    amount = new int[amounts.Count, 2];
                    for (int x = 0; x < amounts.Count; x++)
                    {
                        amount[x, 0] = x;
                        amount[x, 1] = amounts[x];
                    }
                    sort_Descending(amount, 0, amount.GetLength(0) - 1);



                    for (int y = 0; y < amount.GetLength(0); y++)
                    {
                        FinalSortedAmounts[c] = season_Data[amount[y, 0] + w];
                        c++;
                    }

                    w = c;
                    temp = b_ID;
                    amounts = new List<int>();
                    amounts.Add(int.Parse(row["Amount"].ToString()));
                    //  break;
                }

            }
            return FinalSortedAmounts;
        }
        public static void SaveIntoDBSeasonData(string season)
        {
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            if (season.Equals("normal") || season.Equals("Normal"))
            {
                //Delete old items for each branch of season normal
                SqlCommand command = new SqlCommand(SqlQueries.DeleteFromSpecific, con);
                command.Parameters.AddWithValue("Season", season);
                command.ExecuteNonQuery();
            }
            //Get new items for each branch of specific season
            Dictionary<Int32, DataRow> seasonData = TheFinalSort(season);
            int temp = int.Parse(seasonData[0]["Branch_ID"].ToString());
            int c = 0;
            for (int i = 0; i < seasonData.Count; i++)
            {
                DataRow row = seasonData[i];
                string f_i = row["Frequent_Item"].ToString();
                int amount = int.Parse(row["Amount"].ToString());
                int b_id = int.Parse(row["Branch_ID"].ToString());
                SqlCommand command2 = new SqlCommand(SqlQueries.SaveToSpecific, con);
                command2.Parameters.AddWithValue("@Frequent_Item", f_i);
                command2.Parameters.AddWithValue("@Amount", amount);
                command2.Parameters.AddWithValue("@Season", season);
                command2.Parameters.AddWithValue("@Branch_ID", b_id);
                command2.ExecuteNonQuery();


            }

        }
        public static DataTable getAllFromFrequentItems()
        {
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Frequent_Item", typeof(string));
            dataTable.Columns.Add("Amount", typeof(long));
            dataTable.Columns.Add("Season", typeof(string));
            dataTable.Columns.Add("Branch_ID", typeof(long));
            SqlCommand command = new SqlCommand(SqlQueries.SelectAllFromFrequentItems, con);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    string s = reader["Season"].ToString();


                    dataTable.Rows.Add(reader["Frequent_Item"].ToString(), long.Parse(reader["Amount"].ToString()), s, long.Parse(reader["Branch_ID"].ToString()));

                }

            }
            return dataTable;
        }

        public static void merge_Ascending(int[,] arr, int l, int m, int r)
        {
            int n1 = m - l + 1;
            int n2 = r - m;
            int[,] L = new int[n1, 2];
            int[,] R = new int[n2, 2];
            int i, j;
            for (i = 0; i < n1; ++i)
            {
                L[i, 0] = arr[l + i, 0];
                L[i, 1] = arr[l + i, 1];
            }
            for (j = 0; j < n2; ++j)
            {
                R[j, 0] = arr[m + 1 + j, 0];
                R[j, 1] = arr[m + 1 + j, 1];
            }
            i = 0;
            j = 0;
            int k = l;
            while (i < n1 && j < n2)
            {
                if (L[i, 1] <= R[j, 1])
                {
                    arr[k, 1] = L[i, 1];
                    arr[k, 0] = L[i, 0];
                    i++;
                }
                else
                {
                    arr[k, 1] = R[j, 1];
                    arr[k, 0] = R[j, 0];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                arr[k, 1] = L[i, 1];
                arr[k, 0] = L[i, 0];
                i++;
                k++;
            }
            while (j < n2)
            {
                arr[k, 1] = R[j, 1];
                arr[k, 0] = R[j, 0];
                j++;
                k++;
            }
        }
        public static void merge_Descending(int[,] arr, int l, int m, int r)
        {
            int n1 = m - l + 1;
            int n2 = r - m;
            int[,] L = new int[n1, 2];
            int[,] R = new int[n2, 2];
            int i, j;
            for (i = 0; i < n1; ++i)
            {
                L[i, 0] = arr[l + i, 0];
                L[i, 1] = arr[l + i, 1];
            }
            for (j = 0; j < n2; ++j)
            {
                R[j, 0] = arr[m + 1 + j, 0];
                R[j, 1] = arr[m + 1 + j, 1];
            }
            i = 0;
            j = 0;
            int k = l;
            while (i < n1 && j < n2)
            {
                if (L[i, 1] >= R[j, 1])
                {
                    arr[k, 1] = L[i, 1];
                    arr[k, 0] = L[i, 0];
                    i++;
                }
                else
                {
                    arr[k, 1] = R[j, 1];
                    arr[k, 0] = R[j, 0];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                arr[k, 1] = L[i, 1];
                arr[k, 0] = L[i, 0];
                i++;
                k++;
            }
            while (j < n2)
            {
                arr[k, 1] = R[j, 1];
                arr[k, 0] = R[j, 0];
                j++;
                k++;
            }
        }
        public static void sort_Descending(int[,] arr, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;
                sort_Descending(arr, l, m);
                sort_Descending(arr, m + 1, r);
                merge_Descending(arr, l, m, r);
            }
        }
        public static void sort_Ascending(int[,] arr, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;
                sort_Ascending(arr, l, m);
                sort_Ascending(arr, m + 1, r);
                merge_Ascending(arr, l, m, r);
            }
        }

    }
}
