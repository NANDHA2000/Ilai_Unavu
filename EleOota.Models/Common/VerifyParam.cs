using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class VerifyParam
    {
        public string Mail { get; set; }
        public bool IsSuccess { get; set; }
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string UserType { get; set; }
        public string RedireURL { get; set; } 
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string CompanyName { get; set; }
    }
  
}
