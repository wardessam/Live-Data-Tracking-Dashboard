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
    public class DashBordController : ControllerBase
    {
        int AID = Branchcs.AID;
        private readonly IConfiguration _Configuration;

        public DashBordController (IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable data = new DataTable();
            data.Columns.Add("ID");
            data.Columns.Add("Value");

            List<decimal> cardval = new List<decimal>();
            decimal value = 0;
            string query = @"select sum(Cost) from Total_Sales_Branch where Branch_ID = " + AID;
            string sqlDataSource = _Configuration.GetConnectionString("UserAppCon");

            using(SqlConnection mycon = new SqlConnection (sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon)) 
                {
                    value = Convert.ToDecimal(myCommand.ExecuteScalar());
                    mycon.Close();
                }
            }
            value = Math.Round(value, 2);
            cardval.Add(value);

            query = @"select Top (365) cost from Total_Sales_Branch where Branch_ID = "+ AID +" order by [Date] DESC";
            sqlDataSource = _Configuration.GetConnectionString("UserAppCon");
            decimal sum = 0;
            SqlDataReader reader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    reader = myCommand.ExecuteReader();
                    while(reader.Read())
                    {
                        sum += Convert.ToDecimal(reader[0]);
                       
                    }
                    reader.Close();
                    mycon.Close();
                }
            }
            value = sum / 365;
            value = Math.Round(value, 2);
            cardval.Add(value);

            query = @"select Top (30) cost from Total_Sales_Branch where Branch_ID = " + AID + " order by [Date] DESC";
            sqlDataSource = _Configuration.GetConnectionString("UserAppCon");
            sum = 0;
            
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    reader = myCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        sum += Convert.ToDecimal(reader[0]);
                    }
                    reader.Close();
                    mycon.Close();
                }
            }
            value = sum / 30;
            value = Math.Round(value, 2);
            cardval.Add(value);

            query = @"select Top (1) cost from Total_Sales_Branch where Branch_ID = " + AID + " order by [Date] DESC";
            sqlDataSource = _Configuration.GetConnectionString("UserAppCon");
            sum = 0;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    reader = myCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        sum += Convert.ToDecimal(reader[0]);
                    }
                    reader.Close();
                    mycon.Close();
                }
            }
            value = sum / 24;
            value = Math.Round(value, 2);
            cardval.Add(value);

            query = @"select sum (Cost) , Branch_ID from Total_Sales_Branch group by Branch_ID order by SUM(cost) Desc";
            sqlDataSource = _Configuration.GetConnectionString("UserAppCon");
            int rank = 1;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    reader = myCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        if (AID == Convert.ToInt32(reader[1]))
                        {
                            break;
                        }
                        rank++;
                    }
                    reader.Close();
                    mycon.Close();
                }
            }
            value = rank;
            cardval.Add(value);

            for(int i = 0; i<cardval.Count; i++)
            {
                data.Rows.Add(i, cardval[i]);
            }

            return new JsonResult(data);
        }
    }
}
