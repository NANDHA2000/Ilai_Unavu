using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Framework.Models.Settings
{
    public class AzureKeyValutSettings
    {
        public TimeSpan ReloadInterval { get; set; } = new TimeSpan(0,1,0);
        public string KeyVaultName { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }
    }
}
