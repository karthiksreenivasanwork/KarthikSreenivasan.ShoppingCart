namespace ShoppingCart.WebAPI.Models
{
    /// <summary>
    /// Data model that holds authentication data.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Specifies the registered username
        /// </summary>
        /// <example>Karthik</example>
        public string Username { get; set; }
        /// <summary>
        /// Specifies the password corresponding to the to perform authentication
        /// </summary>
        public string Password { get; set; }
    }
}
