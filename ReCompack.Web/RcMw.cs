using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReCompack.Web
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RCMiddleware
    {
        private readonly RequestDelegate _next;

        public RCMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseRcMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RCMiddleware>();
        }
    }
}
