using System;
using System.IO;
using System.Linq;
using System.Threading;
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
        private readonly IConfigurationService _configurationService;
        private readonly Timer _timer;


        public CachingMiddleware(RequestDelegate next, IConfigurationService configurationService)
        {
            _next = next;
            _configurationService = configurationService;

            _timer = new Timer(EraseCache, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(_configurationService.LifeTime));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _timer.Change(_configurationService.LifeTime * 1000, 0);
            
            if (context.Request.GetUri().Segments.All(x => x != Constants.IMAGES))
            {
                await _next(context);
                return;
            }

            var originalBody = context.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    var filePath = $"{_configurationService.CachePath}{context.Request.GetUri().Segments[2]}.bmp";

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

                    if (context.Response.ContentType == Constants.CONTENT_TYPE_IMAGE
                        && _configurationService.CacheCapacity > Directory.GetFiles(_configurationService.CachePath).Length)
                    {
                        using (var file = new FileStream(filePath, FileMode.Create))
                        {
                            memStream.Position = 0;
                            await memStream.CopyToAsync(file);
                        }
                    }

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private void EraseCache(object state)
        {
            var dir = new DirectoryInfo(_configurationService.CachePath);
            foreach (var file in dir.GetFiles())
            {
                file.Delete();
            }
        }
    }
}