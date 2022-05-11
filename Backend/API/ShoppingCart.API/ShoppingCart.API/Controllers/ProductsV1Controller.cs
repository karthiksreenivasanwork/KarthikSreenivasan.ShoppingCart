using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShoppingCart.API.BusinessLogic;
using ShoppingCart.API.DataProvider;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        Product _product;
        IProductDataProvider _productDataProvider;

        /// <summary>
        /// Initialize controller
        /// </summary>
        /// <param name="configuration">Dependency injected parameter to get application configuration</param>
        public ProductsV1Controller(IConfiguration configuration)
        {
            this._product = new Product(Coordinator.ProviderType.SQL, configuration);
            this._productDataProvider = this._product.getProductProvider();
        }

        /// <summary>
        /// Returns all the types of product categories
        /// </summary>
        /// <returns>Returns ShoppingCart.API.Models.ProductCategoryModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductCategoryModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the product categories.")]
        [HttpGet("categories")]
        public IActionResult GetAllCategories()
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
        /// <returns>Returns a collection of ShoppingCart.API.Models.ProductModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the list of all the products available.")]
        [HttpGet("products")]
        public IActionResult GetAllProducts()
        {
            try
            {
                List<ProductModel> productCollection = _productDataProvider.getAllProducts();
                foreach (var product in productCollection)
                    product.ProductImageURL = ProductImageManager.GetImageApiURL(this.Request.Host.Value, product.ProductImageName); //Update the image URL for each product.
                return Ok(productCollection);
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to retrieve all the products.");
            }
        }

        /// <summary>
        /// Returns the list of all the products based on it's category ID
        /// </summary>
        /// <returns>Returns a collection of ShoppingCart.API.Models.ProductModel</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to retrieve the list of all the products available.")]
        [HttpGet("products/{categoryID}")]
        public IActionResult GetProductsByCategory(int categoryID)
        {
            try
            {
                List<ProductModel> productCollection = _productDataProvider.getProductsByCategory(categoryID);
                foreach (var product in productCollection)
                    product.ProductImageURL = ProductImageManager.GetImageApiURL(this.Request.Host.Value, product.ProductImageName);
                return Ok(productCollection);
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to retrieve all the products.");
            }
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <returns>Returns prodct name</returns>
        [HttpPost("add"), CustomAuthorize(Role.Admin)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Product `{0}` already exists")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to add a new product.")]
        public async Task<IActionResult> PostProduct([FromForm] ProductSwaggerAddModel productDataParam)
        {
            ProductModel productModelToRegister = new ProductModel();
            try
            {
                if (this._productDataProvider.checkForExistingProduct(productDataParam.ProductName))
                    return Conflict(string.Format("Product `{0}` already exists", productDataParam.ProductName));

                productModelToRegister = new ProductModel()
                {
                    ProductCategoryName = productDataParam.ProductCategoryName, //The database will get the category ID based on the category name.
                    ProductName = productDataParam.ProductName,
                    ProductDescription = productDataParam.ProductDescription,
                    ProductPrice = productDataParam.ProductPrice,
                    /*
                     * ToDo - This is a temporary solution to assign image name dynamically handled by the server
                     * but will need to implement a better solution.
                     */
                    ProductImageName = string.Concat(_productDataProvider.getAllProducts().Count.ToString(), ".",
                                       productDataParam.ProductImage.FileName.Split('.')[1])
                };
                //Async-Await: It uploads the image while also creating the database record for the product details which improves performance.
                ProductImageManager.uploadProductImage(productDataParam.ProductImage, productModelToRegister.ProductImageName);
                // This task is independent which doesn't need the result of uploadProductImage method.
                productModelToRegister.ProductID = await _productDataProvider.AddNewProduct(productModelToRegister);
            }
            catch (ProductException pex)
            {
                return Conflict(string.Format(pex.Message, productDataParam.ProductName));
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to add a new product.");
            }
            //CreatedAtAction returns status code 201 response.
            return CreatedAtAction("PostProduct", new { id = productModelToRegister.ProductID }, productModelToRegister);
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <returns>Status code 204 on successful update</returns>
        [HttpPut("update/{id}"), CustomAuthorize(Role.Admin)]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Product ID `{0}` does not match the ID `{1}` of the product model.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product ID `{0}` does not exists")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to update product - `{0}`.")]
        public IActionResult PutProduct(int id, [FromForm] ProductSwaggerUpdateModel productUpdateModelParam)
        {
            if (id != productUpdateModelParam.ProductID)
                return BadRequest(string.Format("Product ID `{0}` does not match the ID `{1}` of the product model.", id, productUpdateModelParam.ProductID));

            ProductModel productModelToUpdate = null;
            ProductModel productResultAfterUpdate = new ProductModel();

            try
            {
                if (!this._productDataProvider.checkForExistingProduct(id))
                    return NotFound("Product ID `{0}` does not exists");

                productModelToUpdate = new ProductModel()
                {
                    ProductID = id,
                    ProductName = productUpdateModelParam.ProductName,
                    ProductCategoryName = productUpdateModelParam.ProductCategoryName, //The database will get the category ID based on the category name.
                    ProductDescription = productUpdateModelParam.ProductDescription,
                    ProductPrice = productUpdateModelParam.ProductPrice
                };

                productResultAfterUpdate = _productDataProvider.UpdateProduct(productModelToUpdate);
            }
            catch (ProductException pex)
            {
                return Conflict(string.Format("Product ID `{0}` does not exists", id));
            }
            catch (Exception ex)
            {
                return Problem(detail: string.Format("Something went wrong. Unable to update product - `{0}`.", productUpdateModelParam.ProductName));
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product to remove with this Product ID</param>
        /// <returns>Returns ShoppingCart.API.Models.ProductModel if product was removed successfully</returns>
        [HttpDelete("delete/{id}"), CustomAuthorize(Role.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductModel[]))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product ID `{0}` does not exists")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to delete product ID - `{0}`.")]
        public IActionResult DeleteProduct(int id)
        {
            ProductModel deletedProductModel = null;

            try
            {
                if (!this._productDataProvider.checkForExistingProduct(id))
                    return NotFound(string.Format("Product ID `{0}` does not exists", id));

                deletedProductModel = _productDataProvider.deleteProduct(id);
                if (deletedProductModel == null)
                    return NotFound(string.Format("Product ID `{0}` does not exists", id));
            }
            catch (ProductException pex)
            {
                return Conflict(string.Format("Product ID `{0}` does not exists", id));
            }
            catch (Exception ex)
            {
                return Problem(detail: string.Format("Something went wrong. Unable to delete product ID - `{0}`.", id));
            }
            return Ok(deletedProductModel);
        }

        /// <summary>
        /// Delete multiple products
        /// </summary>
        /// <param name="ids">Collection of Product ID for deletion</param>
        /// <returns>Collection of ShoppingCart.API.Models.ProductModel if products were removed successfully and Status Code 404 otherwise</returns>
        [HttpPost("DeleteMultiple"), CustomAuthorize(Role.Admin)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductModel[]))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Product ID `{0}` does not exist. Hence, deleting multiple products has been canceled.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something went wrong. Unable to delete the products")]
        public IActionResult DeleteMultiple([FromQuery] int[] ids)
        {
            List<ProductModel> productsToDelete = new List<ProductModel>();
            try
            {
                List<ProductModel> allProducts = _productDataProvider.getAllProducts();
                for (int i = 0; i < ids.Length; i++)
                {
                    ProductModel foundProduct = allProducts.Find(product => product.ProductID == ids[i]);
                    if (foundProduct == null) //Even if one product in the request does not exist, we cancel the delete operation.
                    {
                        if (productsToDelete.Count > 0)
                            productsToDelete.Clear();
                        return NotFound(string.Format("Product ID `{0}` does not exist. Hence, deleting multiple products has been canceled.", ids[i]));
                    }
                    productsToDelete.Add(foundProduct);
                }

                foreach (ProductModel deleteProduct in productsToDelete)
                    _productDataProvider.deleteProduct(deleteProduct.ProductID);
            }
            catch (Exception ex)
            {
                return Problem(detail: "Something went wrong. Unable to delete the products");
            }
            return Ok(productsToDelete);
        }
    }
}
