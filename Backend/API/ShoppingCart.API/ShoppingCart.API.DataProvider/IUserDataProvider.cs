using ShoppingCart.API.Models;

namespace ShoppingCart.API.DataProvider
{
    public interface IUserDataProvider
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userModelToRegister">User model with the registration data.</param>
        /// <returns>Returns true if the new user registration was successful and false otherwise.</returns>
        public int addNewUser(UserModel userModelToRegister);

        /// <summary>
        /// Validate the credentials provided by the user against the database.
        /// </summary>
        /// <param name="username">Username to validate</param>
        /// <returns>Returns the hashed password for a registered user.</returns>
        public string returnHashedPassword(string username);
        /// <summary>
        ///  Verifies if a given user is a registered user in the database.
        /// </summary>
        /// <param name="username">Username to validate</param>
        /// <returns>True if the user is registered and false otherwise.</returns>
        public bool verifyUserRegistration(string username);
    }
}
