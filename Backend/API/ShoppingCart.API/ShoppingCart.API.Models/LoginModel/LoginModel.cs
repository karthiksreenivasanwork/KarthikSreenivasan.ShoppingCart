using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoppingCart.API.Models
{
    /// <summary>
    /// Data model that holds authentication data.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Specifies the registered username
        /// </summary>
        /// <example>karthik</example>
        [Required]
        [JsonPropertyName("Username")]
        public string Username { get; set; }
        /// <summary>
        /// Specifies the password corresponding to the to perform authentication
        /// </summary>
        /// <example>p@ssw0rd123!</example>
        [Required]
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
