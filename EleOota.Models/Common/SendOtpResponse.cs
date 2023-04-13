using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class SendOtpResponse
    {
        public int Empid { get; set; }
        public bool IsExists { get; set; }
    }
}
