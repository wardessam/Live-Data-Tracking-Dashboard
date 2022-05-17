using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBAPI.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName ="varchar(50)")]
        public string First_Name { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Last_Name { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string User_Name { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }
        
        public int Branch_ID { get; set; }
        
        public  int Level { get; set; }
        public User()
        {

        }
    }
}
