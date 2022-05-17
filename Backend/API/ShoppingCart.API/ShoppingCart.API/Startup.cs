using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ShoppingCart.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Default Implementation: This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerDocumentation();
            services.AddCorsConfiguration();
            services.AddJWTServices();
        }

        /// <summary>
        /// Default Implementation: This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
                app.UseCorsConfigurationForLocal();
            }
            if (env.IsProduction())
            {
                app.UseCorsConfigurationForProduction();
            }
            /**
             * Make the images in the server accessable via API URL
             * /
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")), //Relative path of the images in the server
                RequestPath = new PathString("/images") //API Path configuration
            });
            /**
             * Required for viewing image files via URL from the server
             * Example: 
             * API URL Path: https://localhost:44398/images/0.png
             * Relative Directory Path: \API\ShoppingCart.API\ShoppingCart.API\wwwroot\images\0.png
             */
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseJWTServices();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Support for logging using the NuGet package - Serilog.Extensions.Logging.File to log to a file.
            loggerFactory.AddFile($"{Configuration.GetValue<string>(ApiStrings.LOG_PATH)}\\ShoppingCart.API_{DateTime.Today.Date.ToString("MMddyyyy")}.txt");
        }
    }
}
