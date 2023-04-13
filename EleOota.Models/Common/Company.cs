using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyMail { get; set; }
        public int CompanyStatus { get; set; }
        //public int WorkingDaysCount { get; set; }
    }
}
