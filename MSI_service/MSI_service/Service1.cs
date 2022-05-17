using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Data;
using System.Data.SqlClient;



namespace MSI_service
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 43200000; //every 12 hours = 43200000 milliseconds
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
           
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            SqlConnection con;
            string connectionString = "Data Source=(local);Initial Catalog=MSI;Integrated Security=SSPI";
            con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand com = new SqlCommand("select * from Store_Branch", con);
            com.CommandType = CommandType.Text;
            SqlDataReader reader = com.ExecuteReader();
            DataTable Branches = new DataTable();
            Branches.Load(reader);
            con.Close();
            con.Dispose();
            for (int i = 0; i < Branches.Rows.Count; i++)
            {
                string codeType = Branches.Rows[i][4].ToString();
                string server_name = Branches.Rows[i][5].ToString();
                string Database_name = Branches.Rows[i][6].ToString();
                string UserID = Branches.Rows[i][7].ToString();
                string password = Branches.Rows[i][8].ToString();
                string query = Branches.Rows[i][3].ToString();

                if (codeType.Equals("mysql"))
                {
                    mysql ms = new mysql();
                    ms.GetData(server_name, Database_name, UserID, password, query);
                }
                else if (codeType.Equals("sqlserver"))
                {
                    sqlserver ss = new sqlserver();
                    ss.GetData(server_name, Database_name, UserID, password, query);
                }
                else if (codeType.Equals("oracle"))
                {
                    oracleserver os = new oracleserver();
                    os.GetData(server_name, Database_name, UserID, password, query);
                }
            }
            DateTime today = DateTime.Now;
            DateTime Next_season = today.AddDays(15);
            string season = "";
            lossyCountingAlgo LCA = new lossyCountingAlgo();
            int hijiri_month =LCA.ConvertDateCalendar(Next_season);
            if (Next_season.Month >= 6 && Next_season.Month <= 8)
            {
                season = "summer";
                SpecificFrequentItems.SaveIntoDBSeasonData(season);
                Recommendations.Seasons_Recommendations(season);
            }
            else if(Next_season.Month == 12)
            {
                season = "christmas";
                SpecificFrequentItems.SaveIntoDBSeasonData(season);
                Recommendations.Seasons_Recommendations(season);
            }
            else if(Next_season.Month>=1 && Next_season.Month <= 2)
            {
                season = "winter";
                SpecificFrequentItems.SaveIntoDBSeasonData(season);
                Recommendations.Seasons_Recommendations(season);
            }
            else if(hijiri_month == 9)
            {
                season = "ramadan";
                SpecificFrequentItems.SaveIntoDBSeasonData(season);
                Recommendations.Seasons_Recommendations(season);
            }
            else
            {
                season = "normal";
                SpecificFrequentItems.SaveIntoDBSeasonData(season);
                Recommendations.Normal_Recommendations();
            }
            Recommendations.Reports();
        }

    }
}
