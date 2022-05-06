using Microsoft.AspNetCore.Http;
using ShoppingCart.API.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace ShoppingCart.API
{
    /// <summary>
    /// Summary:
    ///     Represents the form data input fields in the Swagger UI to add a new cart item.
    /// ToDo:
    ///     Find a better approach in order to avoid model duplicaton and works as intended with the Swagger UI.
    /// </summary>
    public class CartSwaggerAddModel : CartModel
    {
        private int CartID { get; set; }
        private int UserID { get; set; }
        private string Username { get; set; }
        private int OrderID { get; set; }
        private string Productname { get; set; }
        private int ProductPrice { get; set; }
    }
}
