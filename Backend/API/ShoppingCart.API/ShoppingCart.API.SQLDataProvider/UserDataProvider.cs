using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingCart.API.SQLDataProvider
{
    public class UserDataProvider
    {
        IConfiguration _configuration; //Required NuGet package - Microsoft.Extensions.Configuration.Abstractions

        public UserDataProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public bool AddNewUser(UserModel userModelToRegister)
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
    }
}
