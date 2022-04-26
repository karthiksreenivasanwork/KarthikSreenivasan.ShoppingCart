﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.API.Models
{
    /// <summary>
    /// Data model that holds information related to a cart.
    /// </summary>
    public class CartModel
    {
        /// <summary>
        /// Uniquely identifies each cart item.
        /// </summary>
        /// <example>1</example>
        [SwaggerSchema(ReadOnly = true)]
        [Required]
        public int CartID { get; set; }
        /// <summary>
        /// Uniquely identifies each cart item with a unique order id
        /// </summary>
        /// <example>1</example>
        [SwaggerSchema(ReadOnly = true)]
        [Required]
        public int OrderID { get; set; }
        /// <summary>
        /// Uniquely identifies each order with the user.
        /// </summary>
        /// <example>1</example>
        [SwaggerSchema(ReadOnly = true)]
        [Required]
        public int UserID { get; set; }
        /// <summary>
        /// Product name of the cart item
        /// </summary>
        /// <example>Branded Foods</example>
        [Required]
        public string Productname { get; set; }
        /// <summary>
        /// Defines the price of this product
        /// </summary>
        /// <example>100</example>
        [Required]
        public int ProductPrice { get; set; }
    }
}
