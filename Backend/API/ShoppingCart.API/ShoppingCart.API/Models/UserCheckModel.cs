namespace ShoppingCart.WebAPI.Models
{
    /// <summary>
    /// Data model that holds authentication data.
    /// </summary>
    public class UserCheckModel
    {
        /// <summary>
        /// Holds the authentication result of a user.
        /// </summary>
        /// <example>true</example>
        public bool RegisteredUser { get; set; }
        /// <summary>
        /// Holds the custom authentication message.
        /// </summary>
        ///<example>'karthik' is a registered user.</example>
        public string Message { get; set; }
    }
}
