using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class EmployeeUploadExcel
    {
        public string EmpName { get; set; }
        public string EmpMail { get; set; }
        public int CompanyId { get; set; }
        public string Error { get; set; }
    }
    public class EmployeeUpload
    {
        public IEnumerable<EmployeeUploadExcel> FailedEmployeeRecord { get; set; }
        public string UploadError { get; set; }
        public string ErrorFileName { get; set; }
        public int UploadedRecordCount { get; set; }
        public int FailedRecordCount { get; set; }
    }
}
