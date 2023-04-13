using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class EmailConfig
    {
        public string ToEmail { get; set; }
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
    }
}
