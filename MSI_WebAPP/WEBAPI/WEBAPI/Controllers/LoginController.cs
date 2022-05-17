using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBAPI.Models;
using System.Data.SqlClient;
using System.Data;


namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public Respons employeeLogin(Login login)
        {
            int log = 0;
            int value = -1;
            string Dbstr = _configuration.GetConnectionString("UserAppCon");
            string query = @"select Branch_ID from [dbo].[User] where [User_Name] = @nam and [Password] = @pass";


            using (SqlConnection connection = new SqlConnection(Dbstr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nam", login.Email);
                    command.Parameters.AddWithValue("@pass", login.Password);
                    value = Convert.ToInt32(command.ExecuteScalar());

                    if (value != 0 && value != -1)
                    {
                        Branchcs x = new Branchcs(value);
                        log = 1;
                    }


                    connection.Close();
                }
            }

            if (log == 0)
            {
                return new Respons { Status = "Invalid", Message = "Invalid User." };
            }
            else
                return new Respons { Status = "Success", Message = "Login Successfully" };
        }

    }
}
