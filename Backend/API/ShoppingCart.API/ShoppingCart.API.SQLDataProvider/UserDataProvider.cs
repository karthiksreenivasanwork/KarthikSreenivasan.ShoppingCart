using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Models;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    // Summary:
    //     Retrieves the required information in relation to user details
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

        public UserDataProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userModelToRegister">User model with the registration data.</param>
        /// <returns>Returns true if the new user registration was successful and false otherwise.</returns>
        public bool addNewUser(UserModel userModelToRegister)
        {
            bool hasUserAddedSuccessfully = false;

            try
            {
                /**
                 * Requires the NuGet package - System.Data.SqlClient.
                 */
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    using (SqlCommand command = new SqlCommand("Sch_UserManagement.sp_CreateUser"))
                    {
                        connection.Open();

                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter userNameParam = new SqlParameter("UsernameParam", userModelToRegister.Username);
                        SqlParameter passwordParam = new SqlParameter("PasswordParam", userModelToRegister.Password);
                        SqlParameter emailParam = new SqlParameter("EmailParam", userModelToRegister.Email);
                        SqlParameter phoneParam = new SqlParameter("PhoneParam", userModelToRegister.Phone);

                        command.Parameters.Add(userNameParam);
                        command.Parameters.Add(passwordParam);
                        command.Parameters.Add(emailParam);
                        command.Parameters.Add(phoneParam);

                        command.ExecuteNonQuery();

                        hasUserAddedSuccessfully = true;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
                throw sqlException;
            }
            catch (Exception exception)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(exception);
                throw exception;
            }

            return hasUserAddedSuccessfully;
        }

        /// <summary>
        /// Validate the credentials provided by the user against the database.
        /// </summary>
        /// <param name="loginModel">Login model reference.</param>
        /// <returns>Returns true if the user authentication was successful and false otherwise.</returns>
        public bool validateUser(LoginModel loginModel)
        {
            bool isValidUser = false;

            try
            {
                /**
                 * Requires the NuGet package - System.Data.SqlClient.
                 */
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    using (SqlCommand command = new SqlCommand("Sch_UserManagement.sp_ValidateUser"))
                    {
                        connection.Open();

                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", loginModel.Username);
                        SqlParameter passwordParam = new SqlParameter("@PasswordInputParam", loginModel.Password);
                        SqlParameter isValidUserParam = new SqlParameter("@UserSearchCountOutputParam", SqlDbType.Int);
                        isValidUserParam.Direction = ParameterDirection.Output;

                        command.Parameters.Add(userNameParam);
                        command.Parameters.Add(passwordParam);
                        command.Parameters.Add(isValidUserParam);

                        command.ExecuteNonQuery();
                        isValidUser = Convert.ToInt32(command.Parameters["@UserSearchCountOutputParam"].Value) == (int)UserLoginValidationResult.Exists;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
                throw sqlException;
            }
            catch (Exception exception)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(exception);
                throw exception;
            }

            return isValidUser;
        }

        /// <summary>
        ///  Verifies if a given user is a registered user in the database.
        /// </summary>
        /// <param name="username">Username to validate</param>
        /// <returns>True if the user is registered and false otherwise.</returns>
        public bool verifyUserRegistration(string username)
        {
            bool isRegisteredUser = false;

            try
            {
                /**
                 * Requires the NuGet package - System.Data.SqlClient.
                 */
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    using (SqlCommand command = new SqlCommand("Sch_UserManagement.sp_UserExists"))
                    {
                        connection.Open();

                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", username);
                        SqlParameter isValidUserParam = new SqlParameter("@UserSearchCountOutputParam", SqlDbType.Int);
                        isValidUserParam.Direction = ParameterDirection.Output;

                        command.Parameters.Add(userNameParam);
                        command.Parameters.Add(isValidUserParam);

                        command.ExecuteNonQuery();
                        isRegisteredUser = Convert.ToInt32(command.Parameters["@UserSearchCountOutputParam"].Value) == (int)UserLoginValidationResult.Exists;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
                throw sqlException;
            }
            catch (Exception exception)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(exception);
                throw exception;
            }

            return isRegisteredUser;
        }
    }
}
