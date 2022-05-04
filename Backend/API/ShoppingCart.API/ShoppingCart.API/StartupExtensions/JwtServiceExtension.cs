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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

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
                    /*
                     * Whenever I had implemented the `Authorized` attribute, I wanted to send a meaningful JSON response to the user. 
                     * It was achieved using  JwtBearerEvents events using the OnChallenge Func delegate.
                     * Additionally, I have implemented OnAuthenticationFailed delegate as well which returns an appropriate header 
                     * error response for two important Jwt token validation exceptions.
                     */
                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse(); //Skip the default logic for this challenge.
                            var payload = new JObject();

                            if (context.Error == null) //No JWT Token is empty.
                            {
                                payload = new JObject
                                {
                                    ["error"] = "Unauthorized",
                                    ["error_description"] = "No token found.",
                                    ["error_uri"] = context.ErrorUri
                                };

                            }
                            else
                            {
                                payload = new JObject //When the JWT token is invalid.
                                {
                                    ["error"] = context.Error,
                                    ["error_description"] = context.ErrorDescription == string.Empty ? "Invalid token found" : context.ErrorDescription,
                                    ["error_uri"] = context.ErrorUri
                                };
                            }
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 401;

                            return context.Response.WriteAsync(payload.ToString());
                        },
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenValidationException))
                            {
                                context.Response.Headers.Add("Invalid Token", "true");
                            }

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
            //Order of middleware call is important. Call UseAuthentication first before calling UseAuthorization
            app.UseAuthentication(); //For JWT authentication 
            app.UseAuthorization();
            return app;
        }
    }
}
