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
    public class ReportController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _configuration;
       
        public ReportController(IConfiguration configuration)
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
            string query = @"select Cost,Amount,Date from dbo.Total_Sales_Branch where Date = @DateofToday and Branch_ID = @ActID";
            SqlDataReader reader;

            using (SqlConnection connection = new SqlConnection(Dbstr))
            {
                connection.Open();
                using(SqlCommand command = new SqlCommand(query , connection))
                {
                    command.Parameters.AddWithValue("@DateofToday", DateTime.Today.ToShortDateString());
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
