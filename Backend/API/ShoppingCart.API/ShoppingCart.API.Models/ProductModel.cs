using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.API.Models
{
    /// <summary>
    /// Data model that holds information related to a product.
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// Uniquely identifies each product with it's unique ID.
        /// </summary>
        /// <example>1</example>
        [SwaggerSchema(ReadOnly = true), FromForm]
        public int ProductID { get; set; }
        /// <summary>
        /// Uniquely identifies each product category with it's unique ID.
        /// </summary>
        /// <example>1</example>
        [SwaggerSchema(ReadOnly = true), FromForm]
        public int ProductCategoryID { get; set; }
        /// <summary>
        /// Defines the category name of this product
        /// </summary>
        /// <example>Branded Foods</example>
        [Required]
        public string ProductCategoryName { get; set; }
        /// <summary>
        /// Defines the name of this product
        /// </summary>
        /// <example>Pepsi</example>
        [Required]
        public string ProductName { get; set; }
        /// <summary>
        /// Defines the price of this product
        /// </summary>
        /// <example>100</example>
        [Required]
        public int ProductPrice { get; set; }
        /// <summary>
        /// Defines the description of this product
        /// </summary>
        /// <example>A product from ABC company</example>
        [Required]
        public string ProductDescription { get; set; }
        /// <summary>
        /// Defines the image in binary format to be saved in a server location
        /// </summary>
        /// <example>Binary object</example>
        //[Required]
        //public IFormFile ProductImage { get; set; }
        /// <summary>
        /// Defines the image name of this product
        /// </summary>
        /// <example>pepsi_image.png</example>
        [Required]
        public string ProductImageName { get; set; }
    }
}
