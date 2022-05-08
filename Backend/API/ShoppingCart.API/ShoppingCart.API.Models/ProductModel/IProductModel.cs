﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.API.Models
{
    /// <summary>
    /// Summary:
    ///     Defines a set of properties required to be implemented by a Product model.
    /// </summary>
    public interface IProductModel
    {
        /// <summary>
        /// Uniquely identifies each product with it's unique ID.
        /// </summary>
        /// <example>1</example>
        int ProductID { get; set; }
        /// <summary>
        /// Uniquely identifies each product category with it's unique ID.
        /// </summary>
        /// <example>1</example>
        int ProductCategoryID { get; set; }
        /// <summary>
        /// Defines the category name of this product
        /// </summary>
        /// <example>Branded Foods</example>
        string ProductCategoryName { get; set; }
        /// <summary>
        /// Defines the name of this product
        /// </summary>
        /// <example>Pepsi</example>
        string ProductName { get; set; }
        /// <summary>
        /// Defines the price of this product
        /// </summary>
        /// <example>100</example>
        int ProductPrice { get; set; }
        /// <summary>
        /// Defines the description of this product
        /// </summary>
        /// <example>A product from ABC company</example>
        string ProductDescription { get; set; }
        /// <summary>
        /// Defines the image in binary format to be saved in a server location
        /// </summary>
        /// <example>Binary object</example>
        IFormFile ProductImage { get; set; }
        /// <summary>
        /// Defines the image name of this product which is autogenerated by the server
        /// </summary>
        /// <example>1.png</example>
        string ProductImageName { get; set; } //Hide this input in the Swagger UI using private access specifier
        /// <summary>
        /// Defines the image API URL from the server
        /// </summary>
        /// <example>https://localhost:44398/images/1.png</example>
        public string ProductImageURL { get; set; }
    }
}
