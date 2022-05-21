using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingCart.API
{
    /// <summary>
    /// Summary:
    ///    Represents the configuration definitions and usage of CORS (Cross-Origin Resource Sharing)
    /// </summary>
    public static class CorsServiceExtensions
    {
        /// <summary>
        /// Extension method that creates CORS (Cross-Origin Resource Sharing) policy to allow website from another domain to call this API.
        /// </summary>
        /// <param name="services">Service reference</param>
        /// <returns>Updated configuration of the service reference</returns>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors();
            return services;
        }

        /// <summary>
        /// Allow the Agnular application hosted locally to connect to this API during development
        /// </summary>
        /// <param name="app">Application builder reference</param>
        /// <returns>Updated configuration of the Application builder reference</returns>
        public static IApplicationBuilder UseCorsConfigurationForLocal(this IApplicationBuilder app)
        {
            //Allow the Agnular application hosted locally to connect to this API during development
            app.UseCors(
                options =>
                {
                    options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                    options.WithOrigins("http://localhost:4201").AllowAnyMethod().AllowAnyHeader();
                    options.WithOrigins("http://localhost:4202").AllowAnyMethod().AllowAnyHeader();
                });
            return app;
        }

        /// <summary>
        /// Extension method that applies CORS (Cross-Origin Resource Sharing) policy to the specific production domains.
        /// </summary>
        /// <param name="app">Application builder reference</param>
        /// <returns>Updated configuration of the Application builder reference</returns>
        public static IApplicationBuilder UseCorsConfigurationForProduction(this IApplicationBuilder app)
        {
            /*
             * Need to provide the webiste URL that is hosted with a domain.
             * Additionally need to mention the header configurations.
             * NOTE: https://www.shoppingcart.karthik.com/ is not a real website. It is just a dummy value at this time.
             */
            string websiteURLProduction = "https://www.shoppingcart.karthik.com/";
            app.UseCors(
                options => options.WithOrigins(websiteURLProduction).AllowAnyMethod().AllowAnyHeader()
                ); ;
            return app;
        }
    }
}
