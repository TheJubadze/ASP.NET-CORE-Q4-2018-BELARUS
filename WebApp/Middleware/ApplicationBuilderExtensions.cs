using System.IO;
using Microsoft.Extensions.FileProviders;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string root)
        {
            const string nodeModules = "node_modules";
            var path = Path.Combine(root, nodeModules);
            var options = new StaticFileOptions
            {
                RequestPath = $"/{nodeModules}",
                FileProvider = new PhysicalFileProvider(path)
            };

            app.UseStaticFiles(options);

            return app;
        }
    }
}
