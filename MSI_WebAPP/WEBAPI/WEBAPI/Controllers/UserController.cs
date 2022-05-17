using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WEBAPI.Models;


namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configration;
        public UserController(IConfiguration configuration)
        {
            _configration = configuration;

        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            Console.WriteLine("????");
            string query = @"
               insert into [User] ([First_Name],[Last_Name],[User_Name]
               ,[Password],[Branch_ID],[Level]) values ( '" + user.First_Name + "' , '" + user.Last_Name + "'," +
               "'" + user.User_Name + "'," + "'" + user.Password + "'," + user.Branch_ID + "," + user.Level + ")";
            DataTable table = new DataTable();
            string sqlDataSource = _configration.GetConnectionString("UserAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                reader = command.ExecuteReader();
                table.Load(reader);
                reader.Close();
                con.Close();

            }
            Branchcs x = new Branchcs(user.Branch_ID);
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put(User user)
        {
            string query = @"
               update [User] set [First_Name]= '" + user.First_Name + "'," +
              "[Last_Name]='" + user.Last_Name + "'," +
              "[Password]='" + user.Password + "'," +
              "[User_Name]='" + user.User_Name + "'" +
              "where [Branch_ID]=" + user.Branch_ID + ";";


            DataTable table = new DataTable();
            string sqlDataSource = _configration.GetConnectionString("UserAppCon");
            SqlDataReader reader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                reader = command.ExecuteReader();
                table.Load(reader);
                reader.Close();
                con.Close();

            }
            return new JsonResult("Updated Successfully");
        }
    }
}
