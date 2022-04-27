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

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="configuration">Dependency injected parameter to get application configuration</param>
        public CartV1Controller(IConfiguration configuration)
        {
            _cartDataProvider = new CartDataProvider(configuration);
        }

        /// <summary>
        ///  Returns the cart item details for a specific authorized user token
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.CartModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CartModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the cart items.")]
        [HttpGet("Items"), CustomAuthorize(Role.User)]
        public IActionResult GetCartItems()
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity; //Get the username from JWT payload
                return Ok(_cartDataProvider.getCartItemForUser(identity.Name));
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to get all cart items.");
            }
        }
    }
}
