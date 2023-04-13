using Azure.Core.Pipeline;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EleOota.Framework.Models.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;

namespace EleOota.FrameworkExtensions
{
    public static class AzureKeyVaultExtension
    {
        public const string ConfigurationSection = "AzureKeyVaultSettings";

        public static void AddAzureKeyVault(this IConfigurationBuilder config)
        {
            var builtConfig = config.Build();
            AzureKeyValutSettings settings = new AzureKeyValutSettings();
            builtConfig.GetSection(ConfigurationSection).Bind(settings);

            var secretClient = GetSecretClient(settings);
            config.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions() 
            { 
            ReloadInterval = settings.ReloadInterval
            });
        }

        private static SecretClient GetSecretClient(AzureKeyValutSettings settings)
        {
            var uri = new Uri($"https://{settings.KeyVaultName}.vault.azure.net/");
            var clientSecret = new ClientSecretCredential(settings.TenantId, settings.ClientId, settings.ClientSecret);

            if (string.IsNullOrEmpty(settings.ProxyAddress)) 
                return new SecretClient(uri, clientSecret);

            var httpClientHandler = CreateHttpClientHandler(settings.ProxyAddress, settings.ProxyPort);
            var transport = new HttpClientTransport(httpClientHandler);
            var secretClient = new SecretClient(uri,clientSecret,new SecretClientOptions() { Transport = transport });
            return secretClient;
        }

        private static HttpClientHandler CreateHttpClientHandler(string address,int port)
        {
            return new HttpClientHandler
            {
                Proxy = new WebProxy(address, port) { Credentials = CredentialCache.DefaultCredentials }
            };
        }
    }
}
