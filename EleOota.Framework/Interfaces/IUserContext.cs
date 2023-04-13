using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.FrameworkInterfaces
{
    public interface IUserContext
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string EmployeeId { get; set; }
    }
}
