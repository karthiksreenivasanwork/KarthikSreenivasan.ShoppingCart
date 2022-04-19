using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.API
{
    /// <summary>
    /// Summary:
    ///    Represents the configuration definitions and usage of 'JSON Web Token' (JWT) token.
    /// </summary>
    public static class JwtServiceExtensions
    {
        /// <summary>
        /// Configure JWT validation for the end points with the [Authorize] attribute.
        /// </summary>
        /// <param name="services">Service reference</param>
        /// <returns>Updated configuration of the service reference</returns>
        public static IServiceCollection AddJWTServices(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //JwtBearerDefaults - Requires NuGet package - Microsoft.AspNetCore.Authentication.JwtBearer - Version 5.0.16
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = JwtTokenManager.getTokenValidationParameters();

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            return services;
        }

        /// <summary>
        ///  Extension method that applies the JWT configuration.
        /// </summary>
        /// <param name="app">Application builder reference</param>
        /// <returns>Updated configuration of the Application builder reference</returns>
        public static IApplicationBuilder UseJWTServices(this IApplicationBuilder app)
        {
            app.UseAuthentication(); //For JWT authentication
            app.UseAuthorization();
            return app;
        }
    }
}
