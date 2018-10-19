using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;

namespace WebApp.Middleware
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;

        public CachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tt = context.Request.GetUri().Segments[1];
            await _next(context);
            tt = context.Response.ContentType;
        }
    }
}
