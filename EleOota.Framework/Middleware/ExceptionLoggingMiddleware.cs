using EleOota.FrameworkExceptions.ApiExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EleOota.FrameworkMiddleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionLoggingMiddleware> logger;
        public ExceptionLoggingMiddleware(RequestDelegate next,ILogger<ExceptionLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (ApiException ex)
            {
                logger.LogError(ex, $"{ex.Message}");
                throw;
            }
            
        }
    }
}
