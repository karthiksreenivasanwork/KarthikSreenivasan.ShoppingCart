using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.DataProvider;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Security.Claims;

namespace ShoppingCart.API.Controllers
{
    /// <summary>
    /// Summary:
    ///     Purpose of this controller is to manage cart related data.
    /// </summary>
    [Route("api/v1/Cart")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Cart Controller - V1")]
    public class CartV1Controller : ControllerBase
    {
        ILogger _logger;

        Cart _cart;
        Product _product;

        ICartDataProvider _cartProvider;
        IProductDataProvider _productDataProvider;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="configuration">Dependency injected parameter to get application configuration</param>
        /// <param name="logger">Dependency injected parameter to get logging reference</param>
        public CartV1Controller(IConfiguration configuration, ILogger<CartV1Controller> logger)
        {
            this._cart = new Cart(Coordinator.ProviderType.SQL, configuration);
            this._cartProvider = this._cart.getCartProvider();

            this._product = new Product(Coordinator.ProviderType.SQL, configuration);
            this._productDataProvider = this._product.getProductProvider();
            this._logger = logger;
        }

        /// <summary>
        ///  Returns the cart item details for a specific authorized user token
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.CartItemCollectionModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CartItemCollectionModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the cart items.")]
        [HttpGet("items"), CustomAuthorize(Role.User)]
        public IActionResult GetCartItems()
        {
            try
            {
                var cartItemCollection = this._cartProvider.getCartItemForUser(((ClaimsIdentity)User.Identity).Name);
                foreach (var product in cartItemCollection)
                    product.ProductImageURL = ProductImageManager.GetImageApiURL(this.Request.Host.Value, product.ProductImageName);
                return Ok(cartItemCollection);
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrong. Unable to get all cart items.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: errorMessage);
            }
        }

        /// <summary>
        ///  Returns the total product count added to the cart.
        /// </summary>
        /// <returns>Total items in the cart</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(int))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the cart items.")]
        [HttpGet("count"), CustomAuthorize(Role.User)]
        public IActionResult GetCartItemsCount()
        {
            try
            {
                /*
                 * Todo:
                 * In case of performance issues, we need to create a direct call to the database.
                 */
                return Ok(this._cartProvider.getCartItemForUser(((ClaimsIdentity)User.Identity).Name).Count);
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrong. Unable to get cart count.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: errorMessage);
            }
        }

        /// <summary>
        /// Add a new cart item
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.CartItemModel</returns>
        [HttpPost("add"), CustomAuthorize(Role.User)]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CartItemModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product ID `{0}` does not exists")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to add a new item to the cart.")]
        public IActionResult PostNewCartItem([FromForm] CartItemSwaggerAddModel cartItemDetails)
        {
            CartItemModel newCartItemParam = new CartItemModel();
            CartItemModel newCartItemResult = new CartItemModel();

            if (!this._productDataProvider.checkForExistingProduct(cartItemDetails.ProductID))
                return NotFound(string.Format("Product ID `{0}` does not exists", cartItemDetails.ProductID));
            try
            {
                newCartItemParam = new CartItemModel()
                {
                    Username = ((ClaimsIdentity)User.Identity).Name,
                    ProductID = cartItemDetails.ProductID
                };

                newCartItemResult = this._cartProvider.addNewItemToCart(newCartItemParam);
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrong.Unable to add a new item to the cart.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: errorMessage);
            }
            //CreatedAtAction returns status code 201 response.
            return CreatedAtAction("PostNewCartItem", new { id = newCartItemResult.CartID }, newCartItemResult);
        }

        /// <summary>
        /// Remove product quantity from cart.
        /// </summary>
        /// <param name="cartModelToDelete">Order ID and Product ID are required</param>
        /// <returns>Returns ShoppingCart.API.Models.CartItemModel if product from the cart was removed successfully</returns>
        [HttpPost("removeprodqty"), CustomAuthorize(Role.User)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CartItemModel[]))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product ID `{0}` does not exists")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Unable to find product id {0} for the order {1}.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to delete cart item.")]
        public IActionResult RemoveProductQuantity([FromForm] CartItemSwaggerDeleteModel cartModelToDelete)
        {
            CartItemModel deletedCartItemModel = null;

            try
            {
                if (!this._productDataProvider.checkForExistingProduct(cartModelToDelete.ProductID))
                    return NotFound(string.Format("Product ID `{0}` does not exists", cartModelToDelete.ProductID));

                CartItemModel cartItemToDelete = new CartItemModel
                {
                    OrderID = Convert.ToInt32(cartModelToDelete.OrderID),
                    ProductID = Convert.ToInt32(cartModelToDelete.ProductID)
                };

                deletedCartItemModel = this._cartProvider.removeProductQuantityFromCart(cartItemToDelete);
                if (deletedCartItemModel == null)
                    return NotFound(string.Format("Unable to find product id {0} for the order {1}.", cartItemToDelete.OrderID, cartItemToDelete.ProductID));
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrong. Unable to delete cart item.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: errorMessage);
            }
            return Ok(deletedCartItemModel);
        }

        /// <summary>
        /// Remove the entire product along with it's quantity from the cart.
        /// </summary>
        /// <param name="cartModelToDelete">Order ID and Product ID are required</param>
        /// <returns>Returns ShoppingCart.API.Models.CartItemModel if product from the cart was removed successfully</returns>
        [HttpPost("removeproduct"), CustomAuthorize(Role.User)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CartItemModel[]))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product ID `{0}` does not exists")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Unable to find product id {0} for the order {1}.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to delete product from cart.")]
        public IActionResult RemoveProduct([FromForm] CartItemSwaggerDeleteModel cartModelToDelete)
        {
            CartItemModel deletedCartItemModel = null;

            try
            {
                if (!this._productDataProvider.checkForExistingProduct(cartModelToDelete.ProductID))
                    return NotFound(string.Format("Product ID `{0}` does not exists", cartModelToDelete.ProductID));

                CartItemModel cartItemToDelete = new CartItemModel
                {
                    OrderID = Convert.ToInt32(cartModelToDelete.OrderID),
                    ProductID = Convert.ToInt32(cartModelToDelete.ProductID)
                };

                deletedCartItemModel = this._cartProvider.removeProductFromCart(cartItemToDelete);
                if (deletedCartItemModel == null)
                    return NotFound(string.Format("Unable to find product id {0} for the order {1}.", cartItemToDelete.OrderID, cartItemToDelete.ProductID));
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrong. Unable to delete product from cart.";
                this._logger.LogError($"{errorMessage} Exception Details: \n {ex}");
                return Problem(detail: errorMessage);
            }
            return Ok(deletedCartItemModel);
        }
    }
}
