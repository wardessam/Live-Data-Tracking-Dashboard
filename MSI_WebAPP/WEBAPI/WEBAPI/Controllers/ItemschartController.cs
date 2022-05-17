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
    public class ItemschartController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _Configuration;

        public ItemschartController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable graphs = new DataTable();
            graphs.Columns.Add("Y");
            graphs.Columns.Add("X");

            List<int> Y = new List<int>();
            List<string> X = new List<string>();

            string query = @"select Top (10) [Amount],[Frequent_Item] from Frequent_Items where Branch_ID = " + AID + " order by Amount DESC";
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
                        Y.Add(Convert.ToInt32(reader[0]));
                        X.Add(Convert.ToString(reader[1]));
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
