using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DotnetCore.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            logFactory.AddFile("Logs/Exception.txt");
            _logger = logFactory.CreateLogger("Exception");
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(exception.Message);
        }
    }
    public static class CustomExceptionExtensions
    {
        public static void UseExceptionHandleMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}
