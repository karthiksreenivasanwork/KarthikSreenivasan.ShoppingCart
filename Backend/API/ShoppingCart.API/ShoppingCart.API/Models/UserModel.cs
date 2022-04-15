﻿using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.WebAPI.Models
{
    /// <summary>
    /// Data model that holds new user registration data.
    /// </summary>
    public class UserModel
    {
        /**
        * How to hide a property just in post request description of swagger using swashbuckle?
        * Step 1: Install NuGet package: Swashbuckle.AspNetCore.Annotations
        * Step 2: Call EnableAnnotations method detailed below
        * Step 3: Use the attribute 'SwaggerSchema to ReadOnly' in the property that needs to be hidden.
        */
        [SwaggerSchema(ReadOnly = true)]
        public int ID { get; set; }
        /// <summary>
        /// Username of the user.
        /// </summary>
        /// <example>karthik</example>
        [Required] // Failure to provide Username in the request causes model validation to fail (400 -  "The Username field is required.")
        public string Username { get; set; }
        /// <summary>
        /// Password of the user.
        /// </summary>
        /// <example>p@ssw0rd123!</example>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Email address of the user.
        /// </summary>
        /// <example>karthik@gmail.com</example>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Phone numer of the user.
        /// </summary>
        /// <example>9940568874</example>
        [Required]
        public long Phone { get; set; }
    }
}
