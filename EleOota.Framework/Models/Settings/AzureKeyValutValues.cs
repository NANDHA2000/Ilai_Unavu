using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Framework.Models.Settings
{
    public class AzureKeyValutValues
    {
        public string DatabaseConnectionString { get; set; }
        public string ApplicationClientId { get; set; }
        public string ApplicationClientSecret { get; set; }
        public string TenantId { get; set; }
    }
}
