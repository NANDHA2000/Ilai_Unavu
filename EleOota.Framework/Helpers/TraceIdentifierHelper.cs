using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace EleOota.FrameworkHelpers
{
    public static class TraceIdentifierHelper
    {
        public static string GetIdentifier(HttpContext httpContext)
        {
            return Activity.Current?.Id ?? httpContext.TraceIdentifier;
        }
    }
}
