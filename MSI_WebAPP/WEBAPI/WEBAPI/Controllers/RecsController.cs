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
    public class RecsController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _configuration;

        public RecsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Freq");
            table.Columns.Add("Seas");
            table.Columns.Add("Amount");


            string Dbstr = _configuration.GetConnectionString("UserAppCon");
            string query = @"select Item,Season,Amount from Recommended_Items where Branch_ID = @ActID And (Season = 'summer' or Season = 'winter' or Season = 'ramadan' or Season = 'christmas')";
            SqlDataReader reader;

            using (SqlConnection connection = new SqlConnection(Dbstr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActID", AID);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string FrequentItem = reader[0].ToString();
                        string seas = reader[1].ToString();
                        int Amount = int.Parse(reader[2].ToString());


                        table.Rows.Add(FrequentItem, seas, Amount);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }

    }
}
