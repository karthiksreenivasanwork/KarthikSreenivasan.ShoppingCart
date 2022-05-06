using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        public async Task<int> AddNewItemToCart(CartModel cartModel)
        {
            if (cartModel == null)
                throw new ArgumentNullException("cartModel");

            int commandResult = 0;
            SqlCommand commandReference = null;
            int cartIDOutputData = 0;

            try
            {
                SqlParameter productIDOutputSQLParam = new SqlParameter("CartIDOutputParam", SqlDbType.Int);
                productIDOutputSQLParam.Direction = ParameterDirection.Output;

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("UserNameParam", cartModel.Username),
                    new SqlParameter("ProductIDParam", cartModel.ProductID),
                    productIDOutputSQLParam
                };

                (commandResult, commandReference) = await _databaseFunctions.executeNonQueryAsync(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_CartManagement.sp_CreateCartItems",
                    sqlParameters);

                if (commandReference != null)
                    if (commandResult > 0) //Record successfully inserted into the database.
                        cartIDOutputData = Convert.ToInt32(productIDOutputSQLParam.Value);
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return cartIDOutputData;
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
