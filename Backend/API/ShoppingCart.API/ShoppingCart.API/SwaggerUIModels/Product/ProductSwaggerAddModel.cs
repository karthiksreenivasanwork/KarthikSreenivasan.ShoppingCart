using Microsoft.AspNetCore.Http;
using ShoppingCart.API.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace ShoppingCart.API
{
    /// <summary>
    /// Summary:
    ///     Represents the form data input fields in the Swagger UI to register a new product.
    /// ToDo:
    ///     Find a better approach in order to avoid model duplicaton and works as intended with the Swagger UI.
    ///         ProductSwaggerUIModel -> Hides certain input fields by using private access specifier.
    ///         ProductModel -> This model is used by the SQLDataProviders where all the properties have public access specifier.
    /// </summary>
    public class ProductSwaggerAddModel : ProductModel, IProductModel //IProductModel - Making the implementation of the members via ProductModel explicit.
    {
        private int ProductID { get; set; } //Hide this input in the Swagger UI using private access specifier
        private int ProductCategoryID { get; set; }
        private string ProductImageName { get; set; } 
    }
}
