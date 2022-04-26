using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    // Summary:
    //     Performs all CRUD(Create, Read, Update, Delete) about cart and order details.
    /// </summary>
    public class CartDataProvider
    {
        IConfiguration _configuration; //Required NuGet package - Microsoft.Extensions.Configuration.Abstractions
        DatabaseFunctions _databaseFunctions;

        public CartDataProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
            _databaseFunctions = new DatabaseFunctions();
        }

        public List<CartModel> getCartItemForUser(string usernameParam)
        {
            var cartCollection = new List<CartModel>();

            try
            {
                SqlParameter sqlUserIDParam = new SqlParameter("@UsernameParam", usernameParam);
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    sqlUserIDParam,
                };
                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_CartManagement.sp_GetCartItemsForUser", sqlParameters))
                {
                    if (sqlReader != null)
                    {
                        while (sqlReader.Read())
                            cartCollection.Add(new CartModel
                            {
                                CartID = Convert.ToInt32(sqlReader["CartID"]),
                                OrderID = Convert.ToInt32(sqlReader["OrderID"]),
                                UserID = Convert.ToInt32(sqlReader["UserID"]),
                                Productname = sqlReader["Productname"].ToString(),
                                ProductPrice = Convert.ToInt32(sqlReader["ProductPrice"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return cartCollection;
        }
    }
}
