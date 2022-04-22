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
    //     Performs all CRUD (Create, Read, Update, Delete) in relation to product details.
    /// </summary>
    //public class ProductDataProvider
    //{
    //    /// <summary>
    //    /// Enum that indicates the validation result after authenticating a user.
    //    /// </summary>
    //    private enum ProductExistsResult //Greatly improve the readability of the code when the corresponding values are fetched from the database.
    //    {
    //        [Description("Indicates a new product")]
    //        ProductNotExists = 0,
    //        [Description("Indicates an existing product")]
    //        ProductExists = 1
    //    }

    //    /// <summary>
    //    /// Defined in the appsettings.json of the API project which we can access using dependency injection of the reference to the configuration object.
    //    /// </summary>
    //    const string SQL_CONNECTION_KEY_NAME = "SQLConnection";

    //    IConfiguration _configuration; //Required NuGet package - Microsoft.Extensions.Configuration.Abstractions

    //    public ProductDataProvider(IConfiguration configuration)
    //    {
    //        this._configuration = configuration;
    //    }

    //    /// <summary>
    //    /// Register a new user.
    //    /// </summary>
    //    /// <param name="userModelToRegister">User model with the registration data.</param>
    //    /// <returns>Returns true if the new user registration was successful and false otherwise.</returns>
    //    public bool addNewUser(UserModel userModelToRegister)
    //    {
    //        bool hasUserAddedSuccessfully = false;

    //        try
    //        {
    //            if (this.verifyUserRegistration(userModelToRegister.Username))
    //                throw new UserExistsException();
    //            /**
    //             * Requires the NuGet package - System.Data.SqlClient.
    //             */
    //            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString(this._configuration.getconn)))
    //            {
    //                using (SqlCommand command = new SqlCommand("Sch_UserManagement.sp_CreateUser"))
    //                {
    //                    connection.Open();

    //                    command.Connection = connection;
    //                    command.CommandType = CommandType.StoredProcedure;

    //                    SqlParameter userNameParam = new SqlParameter("UsernameParam", userModelToRegister.Username);
    //                    SqlParameter passwordParam = new SqlParameter("PasswordParam", userModelToRegister.Password);
    //                    SqlParameter emailParam = new SqlParameter("EmailParam", userModelToRegister.Email);
    //                    SqlParameter phoneParam = new SqlParameter("PhoneParam", userModelToRegister.Phone);

    //                    command.Parameters.Add(userNameParam);
    //                    command.Parameters.Add(passwordParam);
    //                    command.Parameters.Add(emailParam);
    //                    command.Parameters.Add(phoneParam);

    //                    command.ExecuteNonQuery();

    //                    hasUserAddedSuccessfully = true;
    //                }
    //            }
    //        }
    //        catch (SqlException sqlException)
    //        {
    //            //ToDo - Log this information
    //            System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
    //            throw sqlException;
    //        }
    //        catch (Exception exception)
    //        {
    //            //ToDo - Log this information
    //            System.Diagnostics.Debug.WriteLine(exception);
    //            throw exception;
    //        }

    //        return hasUserAddedSuccessfully;
    //    }

    //    /// <summary>
    //    /// Validate the credentials provided by the user against the database.
    //    /// </summary>
    //    /// <param name="username">Username to validate</param>
    //    /// <returns>Returns the hashed password for a registered user.</returns>
    //    public string returnHashedPassword(string username)
    //    {
    //        string hashedPassword = String.Empty;

    //        try
    //        {
    //            /**
    //             * Requires the NuGet package - System.Data.SqlClient.
    //             */
    //            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString(SQL_CONNECTION_KEY_NAME)))
    //            {
    //                using (SqlCommand command = new SqlCommand("Sch_UserManagement.sp_ReturnHashedPasswordOfRegisteredUser"))
    //                {
    //                    connection.Open();

    //                    command.Connection = connection;
    //                    command.CommandType = CommandType.StoredProcedure;

    //                    SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", username);
    //                    SqlParameter hashedOutputPasswordParam = new SqlParameter("@HashedPasswordOutputParam", SqlDbType.VarChar, 200);
    //                    hashedOutputPasswordParam.Direction = ParameterDirection.Output;

    //                    command.Parameters.Add(userNameParam);
    //                    command.Parameters.Add(hashedOutputPasswordParam);

    //                    command.ExecuteNonQuery();
    //                    hashedPassword = command.Parameters["@HashedPasswordOutputParam"].Value.ToString();
    //                }
    //            }
    //        }
    //        catch (SqlException sqlException)
    //        {
    //            //ToDo - Log this information
    //            System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
    //            throw sqlException;
    //        }
    //        catch (Exception exception)
    //        {
    //            //ToDo - Log this information
    //            System.Diagnostics.Debug.WriteLine(exception);
    //            throw exception;
    //        }

    //        return hashedPassword;
    //    }

    //    /// <summary>
    //    ///  Verifies if a given user is a registered user in the database.
    //    /// </summary>
    //    /// <param name="username">Username to validate</param>
    //    /// <returns>True if the user is registered and false otherwise.</returns>
    //    public bool verifyUserRegistration(string username)
    //    {
    //        bool isRegisteredUser = false;

    //        try
    //        {
    //            /**
    //             * Requires the NuGet package - System.Data.SqlClient.
    //             */
    //            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString(SQL_CONNECTION_KEY_NAME)))
    //            {
    //                using (SqlCommand command = new SqlCommand("Sch_UserManagement.sp_UserExists"))
    //                {
    //                    connection.Open();

    //                    command.Connection = connection;
    //                    command.CommandType = CommandType.StoredProcedure;

    //                    SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", username);
    //                    SqlParameter isValidUserParam = new SqlParameter("@UserSearchCountOutputParam", SqlDbType.Int);
    //                    isValidUserParam.Direction = ParameterDirection.Output;

    //                    command.Parameters.Add(userNameParam);
    //                    command.Parameters.Add(isValidUserParam);

    //                    command.ExecuteNonQuery();
    //                    isRegisteredUser = Convert.ToInt32(command.Parameters["@UserSearchCountOutputParam"].Value) == (int)UserLoginValidationResult.Exists;
    //                }
    //            }
    //        }
    //        catch (SqlException sqlException)
    //        {
    //            //ToDo - Log this information
    //            System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
    //            throw sqlException;
    //        }
    //        catch (Exception exception)
    //        {
    //            //ToDo - Log this information
    //            System.Diagnostics.Debug.WriteLine(exception);
    //            throw exception;
    //        }

    //        return isRegisteredUser;
    //    }
    //}
}
