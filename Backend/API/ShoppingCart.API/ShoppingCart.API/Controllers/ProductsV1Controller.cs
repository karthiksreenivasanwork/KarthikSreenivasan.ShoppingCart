using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;

namespace ShoppingCart.API.Controllers
{
    /// <summary>
    /// Summary:
    ///     Purpose of this controller is to manage product related data.
    /// </summary>
    [Route("api/v1/Products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Products Controller - V1")]
    public class ProductsV1Controller : ControllerBase
    {
        /*
         * ToDo - Move this to business logic using a interface to coordinate with the data provider.
         */
        ProductDataProvider _productDataProvider;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="configuration">Dependency injected parameter to get application configuration</param>
        public ProductsV1Controller(IConfiguration configuration)
        {
            _productDataProvider = new ProductDataProvider(configuration);
        }

        /// <summary>
        /// Returns all the types of product categories
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.ProductCategoryModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductCategoryModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the product categories.")]
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllProductCategories()
        {
            try
            {
                return Ok(_productDataProvider.getAllProductCategories());
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to get all product categories.");
            }
        }

        /// <summary>
        /// Returns the list of all the products available
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.ProductModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the list of all the products available.")]
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                return Ok(_productDataProvider.getAllProducts());
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to retrieve all the products.");
            }
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.ProductModel</returns>
        [HttpPost("AddProduct"), Authorize]
        public IActionResult Post([FromForm] ProductModel productDataParam)
        {
            try
            {
                ProductModel productModelToRegister = new ProductModel()
                {
                    ProductCategoryName = productDataParam.ProductCategoryName, //The database will get the category ID based on the category name.
                    ProductName = productDataParam.ProductName,
                    ProductDescription = productDataParam.ProductDescription,
                    ProductPrice = productDataParam.ProductPrice,
                    ProductImageName = productDataParam.ProductImageName
                };

                if (ProductImageManager.uploadProductImage(productDataParam.ProductImage))
                    _productDataProvider.addNewProduct(productModelToRegister);
                else
                {
                    //ToDo - Log this information.
                    System.Diagnostics.Debug.WriteLine(String.Format("Error occured while uploading the image {0}", productDataParam.ProductImage));
                    return Problem(detail: "Something went wrong. Unable to add a new product.");
                }
            }
            catch (ProductExistsException pex)
            {
                return Conflict(string.Format(pex.Message, productDataParam.ProductName));
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to add a new product.");
            }
            //CreatedAtAction returns status code 201 response.
            return CreatedAtAction("Post", string.Format("Product - '{0}' added successfully", productDataParam.ProductName));
        }
    }
}
