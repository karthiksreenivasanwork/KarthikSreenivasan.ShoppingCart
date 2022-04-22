using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.API.Controllers
{
    [Route("api/v1/Products")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Products Controller - V1")]
    public class ProductsV1Controller : ControllerBase
    {
        ProductDataProvider _productDataProvider;

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

        [HttpPost("AddProduct")]
        public IActionResult Post([FromBody] ProductModel productDataParam)
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

                _productDataProvider.addNewProduct(productModelToRegister);
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
