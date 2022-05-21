using Microsoft.Extensions.Configuration;
using ShoppingCart.API.DataProvider;
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
    public class UserDataProvider : IUserDataProvider
    {
        string _sqlConnectionString;
        DatabaseFunctions _databaseFunctions = null;

        public UserDataProvider(string sqlConnectionString)
        {
            this._sqlConnectionString = sqlConnectionString;
            this._databaseFunctions = new DatabaseFunctions();
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="userModelToRegister">User model with the registration data.</param>
        /// <returns>Returns the new User ID if the new user registration was successful.</returns>
        public int addNewUser(UserModel userModelToRegister)
        {
            if (userModelToRegister == null)
                throw new ArgumentNullException("userModelToRegister");

            if (this.verifyUserRegistration(userModelToRegister.Username))
                throw new UserExistsException(userModelToRegister.Username);

            int commandResult = 0;
            SqlCommand commandReference = null;
            int userIDOutputData = 0;

            SqlParameter userIDOutPutSQLParam = new SqlParameter("UserIDOutputParam", SqlDbType.Int);
            userIDOutPutSQLParam.Direction = ParameterDirection.Output;

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("UsernameParam", userModelToRegister.Username),
                    new SqlParameter("PasswordParam", userModelToRegister.Password),
                    new SqlParameter("EmailParam", userModelToRegister.Email),
                    new SqlParameter("PhoneParam", userModelToRegister.Phone),
                    userIDOutPutSQLParam
                };

                (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                    this._sqlConnectionString,
                    "Sch_UserManagement.sp_CreateUser",
                    sqlParameters);

                if (commandReference != null && commandResult > 0)
                    userIDOutputData = Convert.ToInt32(userIDOutPutSQLParam.Value);
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return userIDOutputData;
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

            try
            {
                SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", username);
                SqlParameter hashedOutputPasswordParam = new SqlParameter("@HashedPasswordOutputParam", SqlDbType.VarChar, 200);
                hashedOutputPasswordParam.Direction = ParameterDirection.Output;

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    userNameParam,
                    hashedOutputPasswordParam
                };

                (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                    this._sqlConnectionString,
                    "Sch_UserManagement.sp_ReturnHashedPasswordOfRegisteredUser",
                    sqlParameters);

                if (commandReference != null)
                    hashedPassword = commandReference.Parameters["@HashedPasswordOutputParam"].Value.ToString();
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
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

            try
            {
                SqlParameter userNameParam = new SqlParameter("@UsernameInputParam", username);
                SqlParameter isValidUserParam = new SqlParameter("@UserSearchCountOutputParam", SqlDbType.Int);
                isValidUserParam.Direction = ParameterDirection.Output;

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    userNameParam,
                    isValidUserParam
                };

                (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                    this._sqlConnectionString,
                    "Sch_UserManagement.sp_UserExists",
                    sqlParameters);

                if (commandReference != null)
                    isRegisteredUser = Convert.ToInt32(commandReference.Parameters["@UserSearchCountOutputParam"].Value) == (int)DatabaseSearchResult.RecordExists;
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return isRegisteredUser;
        }
    }
}
