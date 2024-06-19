using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace ExportApp.Middlewares
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        //public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        //{
        //    _next = next;
        //    _logger = logger;
        //}

        public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    _logger.LogInformation($"Received request: {context.Request.Path}");
        //    await _next(context);
        //}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation($"Received request: {context.Request.Path}");
            await next(context);
        }
    }
}
