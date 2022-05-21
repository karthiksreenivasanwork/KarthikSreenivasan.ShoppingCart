using ShoppingCart.API.Models;

namespace ShoppingCart.API
{
    /// <summary>
    /// Summary:
    ///     Represents the form data input fields in the Swagger UI to delete an item from the cart.
    /// ToDo:
    ///     Find a better approach in order to avoid model duplicaton and works as intended with the Swagger UI.
    /// </summary>
    public class CartItemSwaggerDeleteModel : CartItemModel
    {
        private int CartID { get; set; }
        private int UserID { get; set; }
        private string Username { get; set; }
        private string Productname { get; set; }
        private int ProductPrice { get; set; }
    }
}
