using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ShoppingCart.API.Models
{
    /// <summary>
    /// Data model which holds the JWT token for an authenticated user.
    /// </summary>
    public class LoginResultModel
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c</example>
        [JsonPropertyName("JWT_Token")]
        public string JwtToken { get; set; }
    }
}
