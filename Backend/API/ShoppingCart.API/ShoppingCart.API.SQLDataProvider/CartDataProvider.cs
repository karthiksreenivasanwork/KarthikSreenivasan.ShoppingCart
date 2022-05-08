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

        public CartItemModel addNewItemToCart(CartItemModel cartModel)
        {
            if (cartModel == null)
                throw new ArgumentNullException("cartModel");

            int commandResult = 0;
            SqlCommand commandReference = null;
            CartItemModel cartModelAdded = new CartItemModel();

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("UserNameParam", cartModel.Username),
                    new SqlParameter("ProductIDParam", cartModel.ProductID)
                };

                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_CartManagement.sp_CreateCartItems",
                    sqlParameters))
                {
                    if (sqlReader != null && sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        cartModelAdded = new CartItemModel
                        {
                            CartID = Convert.ToInt32(sqlReader["CartID"]),
                            OrderID = Convert.ToInt32(sqlReader["OrderID"]),
                            UserID = Convert.ToInt32(sqlReader["UserID"]),
                            Username = sqlReader["Username"].ToString(),
                            ProductID = Convert.ToInt32(sqlReader["ProductID"]),
                            Productname = sqlReader["ProductName"].ToString(),
                            ProductPrice = Convert.ToInt32(sqlReader["ProductPrice"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return cartModelAdded;
        }

        public CartItemModel removeProductQuantityFromCart(CartItemModel cartModel)
        {
            if (cartModel == null)
                throw new ArgumentException("cartModel");

            CartItemModel cartModelAfterDelete = null;

            try
            {
                SqlParameter orderIDParam = new SqlParameter("OrderIDParam", cartModel.OrderID);
                SqlParameter productIDParam = new SqlParameter("ProductIDParam", cartModel.ProductID);

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    orderIDParam,
                    productIDParam
                };

                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_CartManagement.spRemoveProductQtyFromCart",
                    sqlParameters))
                {
                    if (sqlReader != null && sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        cartModelAfterDelete = new CartItemModel
                        {
                            CartID = Convert.ToInt32(sqlReader["CartID"]),
                            OrderID = Convert.ToInt32(sqlReader["OrderID"]),
                            ProductID = Convert.ToInt32(sqlReader["ProductID"]),
                            ProductPrice = Convert.ToInt32(sqlReader["ProductPrice"]),
                            Productname = sqlReader["Productname"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return cartModelAfterDelete;
        }

        public CartItemModel removeProductFromCart(CartItemModel cartModel)
        {
            if (cartModel == null)
                throw new ArgumentException("cartModel");

            CartItemModel cartModelAfterDelete = null;

            try
            {
                SqlParameter orderIDParam = new SqlParameter("OrderIDParam", cartModel.OrderID);
                SqlParameter productIDParam = new SqlParameter("ProductIDParam", cartModel.ProductID);

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    orderIDParam,
                    productIDParam
                };

                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_CartManagement.spRemoveProductFromCart",
                    sqlParameters))
                {
                    if (sqlReader != null && sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        cartModelAfterDelete = new CartItemModel
                        {
                            CartID = Convert.ToInt32(sqlReader["CartID"]),
                            OrderID = Convert.ToInt32(sqlReader["OrderID"]),
                            ProductID = Convert.ToInt32(sqlReader["ProductID"]),
                            ProductPrice = Convert.ToInt32(sqlReader["ProductPrice"]),
                            Productname = sqlReader["Productname"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return cartModelAfterDelete;
        }

        public List<CartItemCollectionModel> getCartItemForUser(string usernameParam)
        {
            var cartCollection = new List<CartItemCollectionModel>();

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
                            cartCollection.Add(new CartItemCollectionModel
                            {
                                OrderID = Convert.ToInt32(sqlReader["OrderID"]),
                                ProductID = Convert.ToInt32(sqlReader["ProductID"]),
                                Productname = sqlReader["Productname"].ToString(),
                                Quantity = Convert.ToInt32(sqlReader["Quantity"]),
                                TotalAmount = Convert.ToInt64(sqlReader["TotalAmount"]),
                                ProductImageName = sqlReader["ProductImageName"].ToString(),
                                UserID = Convert.ToInt32(sqlReader["UserID"]),
                                Username = sqlReader["Username"].ToString(),
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
