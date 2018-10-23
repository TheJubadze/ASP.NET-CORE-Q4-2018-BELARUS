﻿using System.IO;
using System.Linq;
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
            if (!context.Request.GetUri().Segments.Any(x => x.Contains(Constants.IMAGES)))
            {
                await _next(context);
                return;
            }

            var originalBody = context.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    var filePath = $"{configurationService.CachePath}{context.Request.GetUri().Segments[2]}.bmp";

                    if (File.Exists(filePath))
                    {
                        using (var file = new FileStream(filePath, FileMode.Open))
                        {
                            await file.CopyToAsync(originalBody);
                        }

                        return;
                    }

                    context.Response.Body = memStream;

                    await _next(context);

                    if (context.Response.ContentType == Constants.CONTENT_TYPE_IMAGE)
                    {
                        using (var file = new FileStream(filePath, FileMode.Create))
                        {
                            memStream.Position = 0;
                            await memStream.CopyToAsync(file);

                            memStream.Position = 0;
                            await memStream.CopyToAsync(originalBody);
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