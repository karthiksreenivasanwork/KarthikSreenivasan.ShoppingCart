using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingCart Web API", Version = "v1" });
                /**
                 * Helps swagger identify the controller based on versioning without conflicts.
                 * Error Details: Actions require a unique method/path combination for Swagger/OpenAPI 3.0
                 * Solution: Call ResolveConflictingActions.
                 */
                c.ResolveConflictingActions(apiDescription => apiDescription.First());

                /*
                 * Add custom name to controllers for displaying in the Swagger documentation.
                 * Group name provided in the ApiExplorerSettings attribute will be mapped.
                 */
                c.TagActionsBy(apiDescriptionRef => new[] { apiDescriptionRef.GroupName });
                c.DocInclusionPredicate((groupName, apiDescriptionRef) => true);

                /**
                 * How to hide a property just in post request description of swagger using swashbuckle?
                 * Step 1: Install NuGet package: Swashbuckle.AspNetCore.Annotations
                 * Step 2: Call EnableAnnotations method detailed below
                 * Step 3: Use the attribute 'SwaggerSchema to ReadOnly' in the property that needs to be hidden.
                 */
                c.EnableAnnotations();
            });

            /*
             * Apply versioning for each controller.
             */
            services.AddApiVersioning(apiVersioningServiceRef =>
            {
                apiVersioningServiceRef.DefaultApiVersion = new ApiVersion(1, 0);
                apiVersioningServiceRef.AssumeDefaultVersionWhenUnspecified = true;
                apiVersioningServiceRef.ReportApiVersions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCart.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Match controller to each API version of the controller.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
