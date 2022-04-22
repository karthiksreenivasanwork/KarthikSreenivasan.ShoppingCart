using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    // Summary:
    //     Performs all CRUD (Create, Read, Update, Delete) in relation to user details.
    /// </summary>
    public class UserDataProvider
    {
        /// <summary>
        /// Enum that indicates the validation result after authenticating a user.
        /// </summary>
        private enum UserLoginValidationResult //Greatly improve the readability of the code when the corresponding values are fetched from the database.
        {
            [Description("Indicates an invalid user after authentication")]
            NotExists = 0,
            [Description("Indicates a registered user after authentication")]
            Exists = 1
        }

        IConfiguration _configuration; //Required NuGet package - Microsoft.Extensions.Configuration.Abstractions
        DatabaseFunctions<UserDataProvider> _databaseFunctions = null;

        public UserDataProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
            _databaseFunctions = new DatabaseFunctions<UserDataProvider>();
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userModelToRegister">User model with the registration data.</param>
        /// <returns>Returns true if the new user registration was successful and false otherwise.</returns>
        public bool addNewUser(UserModel userModelToRegister)
        {
            if (userModelToRegister == null)
                throw new ArgumentNullException("userModelToRegister");

            if (this.verifyUserRegistration(userModelToRegister.Username))
                throw new UserExistsException(userModelToRegister.Username);

            bool hasUserAddedSuccessfully = false;
            int commandResult = 0;
            SqlCommand commandReference = null;

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("UsernameParam", userModelToRegister.Username),
                new SqlParameter("PasswordParam", userModelToRegister.Password),
                new SqlParameter("EmailParam", userModelToRegister.Email),
                new SqlParameter("PhoneParam", userModelToRegister.Phone)
            };

            (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                "Sch_UserManagement.sp_CreateUser",
                sqlParameters);

            if (commandReference != null)
                hasUserAddedSuccessfully = commandResult > 0;

            return hasUserAddedSuccessfully;
        }

        /// <summary>
        /// Validate the credentials provided by the user against the database.
        /// </summary>
        /// <param name="username">Username to validate</param>
        /// <returns>Returns the hashed password for a registered user.</returns>
        public string returnHashedPassword(string username)
        {
            if (username == null)
                throw new ArgumentNullException("username");

            string hashedPassword = String.Empty;
            int commandResult = 0;
            SqlCommand commandReference = null;

            SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", username);
            SqlParameter hashedOutputPasswordParam = new SqlParameter("@HashedPasswordOutputParam", SqlDbType.VarChar, 200);
            hashedOutputPasswordParam.Direction = ParameterDirection.Output;

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                userNameParam,
                hashedOutputPasswordParam
            };

            (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                "Sch_UserManagement.sp_ReturnHashedPasswordOfRegisteredUser",
                sqlParameters);

            if (commandReference != null)
                hashedPassword = commandReference.Parameters["@HashedPasswordOutputParam"].Value.ToString();

            return hashedPassword;
        }

        /// <summary>
        ///  Verifies if a given user is a registered user in the database.
        /// </summary>
        /// <param name="username">Username to validate</param>
        /// <returns>True if the user is registered and false otherwise.</returns>
        public bool verifyUserRegistration(string username)
        {
            if (username == null)
                throw new ArgumentNullException("username");

            bool isRegisteredUser = false;
            int commandResult = 0;
            SqlCommand commandReference = null;

            SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", username);
            SqlParameter isValidUserParam = new SqlParameter("@UserSearchCountOutputParam", SqlDbType.Int);
            isValidUserParam.Direction = ParameterDirection.Output;

            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                userNameParam,
                isValidUserParam
            };

            (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                "Sch_UserManagement.sp_UserExists",
                sqlParameters);

            if (commandReference != null)
                isRegisteredUser = Convert.ToInt32(commandReference.Parameters["@UserSearchCountOutputParam"].Value) == (int)UserLoginValidationResult.Exists;

            return isRegisteredUser;
        }
    }
}
