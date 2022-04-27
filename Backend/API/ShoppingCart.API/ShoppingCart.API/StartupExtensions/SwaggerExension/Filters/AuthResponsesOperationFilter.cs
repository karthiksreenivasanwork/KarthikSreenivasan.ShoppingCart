using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.API.StartupExtensions.SwaggerExension.Filters
{
    /// <summary>
    /// Summary:
    ///     Represents the configuration of each endpoint in the SwaggerUI that needs JWT token authorization represented by a padlock icon.
    /// </summary>
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Implement the interface method IOperationFilter which applies a padlock to the endpoint of the defined custom attribute.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>(); //Custom attribute applied to the the endpoints that need authorization.

            if (authAttributes.Any())
            {
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    //Initializing the dictionary
                    {
                        new OpenApiSecurityScheme //Dictionary Key
                        {
                            Reference = new OpenApiReference
                            {
                                Id = SwaggerServiceExtensions.AUTHORIZATION_SCHEME_NAME, //This name has to match the name provided in the AddSecurityDefinition method for the popup to work in the said method.
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>() //Dictionary Value
                    }
                };
                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
                /*
                 * Appears as a possible response if we have applied  the following custom attribute to our endpoints.
                 * Microsoft.AspNetCore.Authorization.AuthorizeAttribute
                 */
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Unauthorized. Invalid token was found"
                }
                );
            }
        }
    }
}
