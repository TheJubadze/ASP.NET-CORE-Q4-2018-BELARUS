using AutoMapper;
using Core;
using DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApp.Logger;

namespace WebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(connStr));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILogger, FileLogger>(_ => new FileLogger("log.txt"));
            services.AddAutoMapper();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            ILogger logger)
        {
            logger.LogInformation("Application start");
            logger.LogInformation($"Application location: {env.ContentRootPath}");
            logger.LogInformation($"Connection string: {_configuration.GetConnectionString("DefaultConnection")}");
            logger.LogInformation($"Products list count: {_configuration.GetSection("RepositorySettings").GetSection("ProductsCountMax").Value}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");
            app.ConfigureExceptionHandler(logger);
            app.UseMvcWithDefaultRoute();
        }
    }
}
