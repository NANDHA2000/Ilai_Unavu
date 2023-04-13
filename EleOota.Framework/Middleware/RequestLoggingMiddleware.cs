using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EleOota.FrameworkMiddleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                logger.LogInformation($"Http Request Start : ({httpContext.Request?.Method}){httpContext.Request?.Scheme}//{httpContext.Request?.Host}{httpContext.Request?.Path}" +
                    $"{httpContext.Request?.QueryString}");
                await next(httpContext);
            }
            finally
            {
                logger.LogInformation($"Http Request Start : ({httpContext.Request?.Method}){httpContext.Request?.Scheme}//{httpContext.Request?.Host}{httpContext.Request?.Path}" +
                   $"{httpContext.Request?.QueryString} =>{ httpContext.Response?.StatusCode}");
            }

        }
    }
}
