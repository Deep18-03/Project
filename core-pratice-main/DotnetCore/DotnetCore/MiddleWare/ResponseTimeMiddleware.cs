using DotnetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCore
{
    // Custom Middleware for log the response time
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ResponseTimeMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            logFactory.AddFile("Logs/ResponseTime.txt");
            _logger = logFactory.CreateLogger("ResponseTime");
        }
        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() => {
                watch.Stop();
                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds; 
                _logger.LogInformation("{duration}ms", responseTimeForCompleteRequest);
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}
public static class MyMiddlewareExtensions
{
    public static void UseResponseTimeMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ResponseTimeMiddleware>();
    }
}
