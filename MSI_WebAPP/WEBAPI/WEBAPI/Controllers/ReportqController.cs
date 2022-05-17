using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportqController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _configuration;

        public ReportqController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Cost");
            table.Columns.Add("Amount");
            table.Columns.Add("Date");


            string Dbstr = _configuration.GetConnectionString("UserAppCon");
            string query = "select Cost,Amount,Date from Total_Sales_Branch where YEAR(Date) = @curYear and MONTH(Date) BETWEEN @quarter_start and @quarter_end and Branch_ID = @ActID";
            SqlDataReader reader;

            using (SqlConnection connection = new SqlConnection(Dbstr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    string curYear = DateTime.Today.Year.ToString();
                    int curMonth = DateTime.Today.Month;
                    int quarter_start = 0, quarter_end = 0;
                    if (curMonth >= 1 && curMonth <= 3)
                    {
                        quarter_start = 1; quarter_end = 3;
                    }
                    if (curMonth >= 4 && curMonth <= 6)
                    {
                        quarter_start = 4; quarter_end = 6;
                    }
                    if (curMonth >= 7 && curMonth <= 9)
                    {
                        quarter_start = 7; quarter_end = 9;
                    }
                    if (curMonth >= 10 && curMonth <= 12)
                    {
                        quarter_start = 10; quarter_end = 12;
                    }

                    command.Parameters.AddWithValue("@curYear", curYear);
                    command.Parameters.AddWithValue("@quarter_start", quarter_start.ToString());
                    command.Parameters.AddWithValue("@quarter_end", quarter_end.ToString());
                    command.Parameters.AddWithValue("@ActID", AID);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        decimal Cost = decimal.Parse(reader[0].ToString());
                        int Amount = int.Parse(reader[1].ToString());
                        DateTime retrived_Date_as_DateTime = DateTime.Parse(reader[2].ToString());
                        string Date = retrived_Date_as_DateTime.ToShortDateString();

                        table.Rows.Add(Cost, Amount, Date);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
