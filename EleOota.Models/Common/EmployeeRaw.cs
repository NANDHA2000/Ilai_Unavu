using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class EmployeeRaw
    {
        public int Empid { get; set; }
        public string EmpName { get; set; }
        public string EmpMail { get; set; }
        public string EmpPhone { get; set; }
        public string EmpCardNo { get; set; }
        public int CompanyId { get; set; }
        public int StatusId { get; set; }
    }
}
