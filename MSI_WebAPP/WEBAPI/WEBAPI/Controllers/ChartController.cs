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
    public class ChartController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _Configuration;

        public ChartController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            DataTable graphs = new DataTable();
            graphs.Columns.Add("Y");
            graphs.Columns.Add("X");

            List<decimal> Y = new List<decimal>();
            List<string> X = new List<string>();

            string query = @"select Top (7) [Date] from Total_Sales_Branch where Branch_ID = " + AID + " order by [Date] DESC";
            string sqlDataSource = _Configuration.GetConnectionString("UserAppCon");
            SqlDataReader reader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    reader = myCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        X.Add(Convert.ToDateTime(reader[0]).ToShortDateString());
                    }
                    reader.Close();
                    mycon.Close();
                }
            }

            query = @"select Top (7) cost from Total_Sales_Branch where Branch_ID = " + AID + " order by [Date] DESC";
            sqlDataSource = _Configuration.GetConnectionString("UserAppCon");
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    reader = myCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Y.Add(Convert.ToDecimal(reader[0]));
                    }
                    reader.Close();
                    mycon.Close();
                }
            }

            for (int i = 0; i < X.Count; i++)
            {
                graphs.Rows.Add(Y[i], X[i]);
            }

            return new JsonResult(graphs);
        }
    }
}
