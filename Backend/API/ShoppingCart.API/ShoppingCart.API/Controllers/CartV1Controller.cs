using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

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
        /*
         * ToDo - Move this to business logic using a interface to coordinate with the data provider.
         */
        CartDataProvider _cartDataProvider;
        ProductDataProvider _productDataProvider;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="configuration">Dependency injected parameter to get application configuration</param>
        public CartV1Controller(IConfiguration configuration)
        {
            _cartDataProvider = new CartDataProvider(configuration);
            _productDataProvider = new ProductDataProvider(configuration);
        }

        /// <summary>
        ///  Returns the cart item details for a specific authorized user token
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.CartModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CartModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the cart items.")]
        [HttpGet("items"), CustomAuthorize(Role.User)]
        public IActionResult GetCartItems()
        {
            try
            {
                return Ok(_cartDataProvider.getCartItemForUser(((ClaimsIdentity)User.Identity).Name));
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to get all cart items.");
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
                return Ok(_cartDataProvider.getCartItemForUser(((ClaimsIdentity)User.Identity).Name).Count);
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to get all cart items.");
            }
        }

        /// <summary>
        /// Add a new cart item
        /// </summary>
        /// <returns></returns>
        [HttpPost("add"), CustomAuthorize(Role.User)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product ID `{0}` does not exists")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to add a new item to the cart.")]
        public async Task<IActionResult> PostNewCartItem([FromForm] CartSwaggerAddModel cartItemDetails)
        {
            CartModel newCartItem = new CartModel();

            if (!this._productDataProvider.checkForExistingProduct(cartItemDetails.ProductID))
                return NotFound(string.Format("Product ID `{0}` does not exists", cartItemDetails.ProductID));
            try
            {
                newCartItem = new CartModel()
                {
                    Username = ((ClaimsIdentity)User.Identity).Name,
                    ProductID = cartItemDetails.ProductID
                };
                newCartItem.CartID = await _cartDataProvider.AddNewItemToCart(newCartItem);
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to add a new item to the cart.");
            }
            //CreatedAtAction returns status code 201 response.
            return CreatedAtAction("PostNewCartItem", new { id = newCartItem.CartID }, newCartItem);
        }
    }
}
