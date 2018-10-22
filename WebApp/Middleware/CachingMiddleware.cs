using System.IO;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using WebApp.Common;
using WebApp.Services;

namespace WebApp.Middleware
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;

        public CachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfigurationService configurationService)
        {
            if (context.Request.GetUri().Segments.Length < 2)
            {
                await _next(context);
                return;
            }

            Stream originalBody = context.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await _next(context);

                    if (context.Response.ContentType == Constants.CONTENT_TYPE_IMAGE)
                    {
                        using (var file = new FileStream($"{configurationService.CachePath}{context.Request.GetUri().Segments[2]}.bmp", FileMode.Create))
                        {
                            memStream.Position = 0;
                            await memStream.CopyToAsync(file);
                        }
                    }
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}