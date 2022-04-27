using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.API.StartupExtensions.SwaggerExension.Filters
{
    /// <summary>
    /// Summary:
    ///     Provide the Swagger the ability to upload files if it detects the proeprty IFormFile
    /// </summary>
    public class FileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod != HttpMethods.Post) //Filter for post methods only.
                return;


            var formParameters = context.ApiDescription.ParameterDescriptions
                .Where(paramDesc => paramDesc.HasFormFileProperty());

            if (!formParameters.Any()) //If any IFormFile properties are found, then we proceed
            {
                return;
            }

            var uploadFileMediaType = new OpenApiMediaType()
            {
                Schema = new OpenApiSchema()
                {
                    Type = "object",
                    Properties =
                        {
                            ["productimage"] = new OpenApiSchema()
                            {
                                Type = "string",
                                Format = "binary"
                            }
                        },
                    Required = new HashSet<string>() { "productimage" }
                }
            };
        }
    }

    /// <summary>
    /// Helps match the given property is of type IFormFile
    /// </summary>
    public static class ApiParameterDescriptionExtensions
    {
        /// <summary>
        /// Ditermine if a give property is of type IFormFile
        /// </summary>
        /// <param name="apiParameter">Parameter to verify</param>
        /// <returns>Returns true if IFormFile property type is found and false otherwise.</returns>
        internal static bool HasFormFileProperty(this ApiParameterDescription apiParameter)
        {
            bool isFormFilePropertyFound = false;
            if (apiParameter.ModelMetadata?.ModelType == typeof(IFormFile))
            {
                isFormFilePropertyFound = true;
            }
            return isFormFilePropertyFound;
        }
    }
}
