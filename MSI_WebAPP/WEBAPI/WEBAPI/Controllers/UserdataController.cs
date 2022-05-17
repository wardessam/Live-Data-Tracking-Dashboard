using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserdataController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _Configuration;

        public UserdataController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable table = new DataTable();
            table.Columns.Add("First");
            table.Columns.Add("Last");
            table.Columns.Add("UserName");
            table.Columns.Add("Pass");
            table.Columns.Add("BID");


            string Dbstr = _Configuration.GetConnectionString("UserAppCon");
            string query = @"select First_Name,Last_Name,User_Name,Password from [User] where Branch_ID = @ActID";
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
                        string First = reader[0].ToString();
                        string Second = reader[1].ToString();
                        string User = reader[2].ToString();
                        string Pass = reader[3].ToString();

                        table.Rows.Add(First, Second, User, Pass, AID);
                    }
                    reader.Close();
                    connection.Close();
                }
            }



            return new JsonResult(table);
        }
    }
}