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
    public class FreqmController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _configuration;

        public FreqmController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Freq");
            table.Columns.Add("Amount");
            table.Columns.Add("Date");


            string Dbstr = _configuration.GetConnectionString("UserAppCon");
            string query = "select Frequent_Item,Amount,Date from Frequent_Items where MONTH(Date) = @curMonth and YEAR(Date) = @curYear and Branch_ID = @ActID";
            SqlDataReader reader;

            using (SqlConnection connection = new SqlConnection(Dbstr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    string curMonth = DateTime.Today.Month.ToString();
                    string curYear = DateTime.Today.Year.ToString();

                    command.Parameters.AddWithValue("@curMonth", curMonth);
                    command.Parameters.AddWithValue("@curYear", curYear);
                    command.Parameters.AddWithValue("@ActID", AID);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string FrequentItem = reader[0].ToString();
                        int Amount = int.Parse(reader[1].ToString());
                        DateTime retrived_Date_as_DateTime = DateTime.Parse(reader[2].ToString());
                        string Date = retrived_Date_as_DateTime.ToShortDateString();

                        table.Rows.Add(FrequentItem, Amount, Date);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
