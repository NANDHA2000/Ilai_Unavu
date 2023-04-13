using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class User
    {
        public string Email { get; set; }
        public int UserId { get; set; }
        public string UserType { get; set; } = "Empoyee";
        public string RedireURL { get; set; } = "/employeemain";
        public string StatusName { get; set; }
        public int UserStaus { get; set; }

    }
  
}
