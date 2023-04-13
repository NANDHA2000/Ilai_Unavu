using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Framework.Models.Settings
{ 
    public class DatabaseAdvancedSettingsOptions
    {
        public TimeSpan CommandTimeout { get; set; } = new TimeSpan(0, 0, 30);
        public string DatabaseConnectionString { get; set; }
    }
}
