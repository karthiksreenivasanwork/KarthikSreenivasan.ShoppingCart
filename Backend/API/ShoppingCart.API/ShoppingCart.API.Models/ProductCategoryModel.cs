﻿using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.API.Models
{
    /// <summary>
    /// Data model that holds information related to a product category
    /// </summary>
    public class ProductCategoryModel
    {
        /// <summary>
        /// Uniquely identifies each product category with it's unique ID.
        /// </summary>
        /// <example>1</example>
        [SwaggerSchema(ReadOnly = true)]
        public int ProductCategoryID { get; set; }
        /// <summary>
        /// Defines the category a product belongs to
        /// </summary>
        /// <example>Branded Foods</example>
        [Required]
        public string ProductCategoryName { get; set; }
    }
}