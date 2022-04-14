using Swashbuckle.AspNetCore.Annotations;

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

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public long Phone { get; set; }
    }
}
