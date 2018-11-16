using System;
using AutoMapper;
using Core;
using DataAccess.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs;
using Swashbuckle.AspNetCore.Swagger;
using WebApp.Filters;
using WebApp.Logger;
using WebApp.Middleware;
using WebApp.Services;

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
            
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ILogger, FileLogger>(_ => new FileLogger("log.txt"));
            services.AddAutoMapper();
            
            Action<MvcOptions> configMvcAction = x => { };

            var isLoggingEnabled = _configuration.GetSection("Logging").GetValue<bool>("ActionLoggingEnabled");
            if(isLoggingEnabled) 
                configMvcAction = options => options.Filters.Add(typeof(LoggingFilterAttribute));
            
            services.AddMvc(configMvcAction);

            services.UseBreadcrumbs(GetType().Assembly);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMiddleware<CachingMiddleware>();
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");
            app.ConfigureExceptionHandler(logger);
            app.UseMvc(BuildRoutes);
        }

        private static void BuildRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("images", "images/{id?}", defaults: new {controller = "Category", action = "Image"});
            routeBuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
