using Microsoft.AspNetCore.Http;
using ShoppingCart.API.Models;

namespace ShoppingCart.API
{
    /// <summary>
    /// Summary:
    ///     Represents the form data input fields in the Swagger UI to update details for an existing product.
    /// ToDo:
    ///     Find a better approach in order to avoid model duplicaton and works as intended with the Swagger UI.
    ///         ProductSwaggerUIModel -> Hides certain input fields by using private access specifier.
    ///         ProductModel -> This model is used by the SQLDataProviders where all the properties have public access specifier.
    /// </summary>
    public class ProductSwaggerUpdateModel : ProductModel, IProductModel
    {
        private string ProductName { get; set; }
        private int ProductCategoryID { get; set; }

        private IFormFile ProductImage { get; set; }
        private string ProductImageName { get; set; }
    }
}
