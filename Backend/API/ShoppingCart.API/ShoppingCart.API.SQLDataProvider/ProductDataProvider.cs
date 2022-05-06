using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    // Summary:
    //     Performs all CRUD(Create, Read, Update, Delete) in relation to product details.
    /// </summary>
    public class ProductDataProvider
    {
        IConfiguration _configuration; //Required NuGet package - Microsoft.Extensions.Configuration.Abstractions
        DatabaseFunctions _databaseFunctions;

        public ProductDataProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
            _databaseFunctions = new DatabaseFunctions();
        }

        public List<ProductCategoryModel> getAllProductCategories()
        {
            var productCategoryCollection = new List<ProductCategoryModel>();

            try
            {
                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_GetAllProductCategories"))
                {
                    if (sqlReader != null)
                    {
                        while (sqlReader.Read())
                            productCategoryCollection.Add(new ProductCategoryModel
                            {
                                ProductCategoryID = Convert.ToInt32(sqlReader["ProductCategoryID"]),
                                ProductCategoryName = sqlReader["ProductCategoryName"].ToString()
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
            return productCategoryCollection;
        }

        public List<ProductModel> getAllProducts()
        {
            var productCollection = new List<ProductModel>();

            try
            {
                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_GetAllProducts"))
                {
                    if (sqlReader != null)
                    {
                        while (sqlReader.Read())
                        {
                            productCollection.Add(new ProductModel
                            {
                                ProductID = Convert.ToInt32(sqlReader["ProductID"]),
                                ProductCategoryID = Convert.ToInt32(sqlReader["ProductCategoryID"]),
                                ProductCategoryName = sqlReader["ProductCategoryName"].ToString(),
                                ProductName = sqlReader["ProductName"].ToString(),
                                ProductPrice = Convert.ToInt32(sqlReader["ProductPrice"].ToString()),
                                ProductDescription = sqlReader["ProductDescription"].ToString(),
                                ProductImageName = sqlReader["ProductImageName"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return productCollection;
        }

        /// <summary>
        /// Returns the product collection based on it's category id.
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<ProductModel> getProductsByCategory(int categoryID)
        {
            var productCollection = new List<ProductModel>();

            try
            {
                SqlParameter productCategoryIDParam = new SqlParameter("ProductCategoryIDParam", SqlDbType.Int);
                productCategoryIDParam.Value = categoryID;
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    productCategoryIDParam
                };

                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_GetProductsByCategory", sqlParameters))
                {
                    if (sqlReader != null)
                    {
                        while (sqlReader.Read())
                        {
                            productCollection.Add(new ProductModel
                            {
                                ProductID = Convert.ToInt32(sqlReader["ProductID"]),
                                ProductCategoryID = Convert.ToInt32(sqlReader["ProductCategoryID"]),
                                ProductCategoryName = sqlReader["ProductCategoryName"].ToString(),
                                ProductName = sqlReader["ProductName"].ToString(),
                                ProductPrice = Convert.ToInt32(sqlReader["ProductPrice"].ToString()),
                                ProductDescription = sqlReader["ProductDescription"].ToString(),
                                ProductImageName = sqlReader["ProductImageName"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return productCollection;
        }

        /// <summary>
        /// Register a new product.
        /// </summary>
        /// <param name="newProductToRegister">A new product with the specifications.</param>
        /// <returns>Returns the product ID if the new product was added successfully or 0 otherwise.</returns>
        /// <exception cref="ArgumentNullException">Input parameter cannot be null</exception>
        /// <exception cref="ProductException">Is thrown if this an existing product</exception>
        public async Task<int> AddNewProduct(ProductModel newProductToRegister)
        {
            if (newProductToRegister == null)
                throw new ArgumentNullException("newProductToRegister");

            if (this.checkForExistingProduct(newProductToRegister.ProductName))
                throw new ProductException(string.Format("Product `{0}` already exists", newProductToRegister.ProductName));

            int commandResult = 0;
            SqlCommand commandReference = null;
            int productIDOutputData = 0;

            try
            {
                SqlParameter productIDOutputSQLParam = new SqlParameter("ProductIDOutputParam", SqlDbType.Int);
                productIDOutputSQLParam.Direction = ParameterDirection.Output;

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("ProductCategoryNameParam", newProductToRegister.ProductCategoryName),
                    new SqlParameter("ProductNameParam", newProductToRegister.ProductName),
                    new SqlParameter("ProductPriceParam", newProductToRegister.ProductPrice),
                    new SqlParameter("ProductDescriptionParam", newProductToRegister.ProductDescription),
                    new SqlParameter("ProductImageNameParam", newProductToRegister.ProductImageName),
                    productIDOutputSQLParam
                };

                (commandResult, commandReference) = await _databaseFunctions.executeNonQueryAsync(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_CreateProduct",
                    sqlParameters);

                if (commandReference != null)
                    if (commandResult > 0) //Record successfully inserted into the database.
                        productIDOutputData = Convert.ToInt32(productIDOutputSQLParam.Value);
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return productIDOutputData;
        }


        /// <summary>
        /// Update product details
        /// </summary>
        /// <param name="productDataToUpdate">Update model type ShoppingCart.API.Models.ProductModel</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Input parameter cannot be null</exception>
        /// <exception cref="ProductException">Is thrown if no product exists</exception>
        public ProductModel UpdateProduct(ProductModel productDataToUpdate)
        {
            if (productDataToUpdate == null)
                throw new ArgumentNullException("productToUpdate");

            if (!this.checkForExistingProduct(productDataToUpdate.ProductID))
                throw new ProductException(string.Format("Product ID `{0}` does not exists", productDataToUpdate.ProductID));

            ProductModel updatedProductModel = productDataToUpdate;

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("ProductIDParam", productDataToUpdate.ProductID),
                    new SqlParameter("ProductCategoryNameParam", productDataToUpdate.ProductCategoryName),
                    new SqlParameter("ProductPriceParam", productDataToUpdate.ProductPrice),
                    new SqlParameter("ProductDescriptionParam", productDataToUpdate.ProductDescription)
                };

                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_UpdateProduct",
                    sqlParameters))
                {
                    if (sqlReader != null && sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        updatedProductModel = new ProductModel
                        {
                            ProductCategoryID = Convert.ToInt32(sqlReader["ProductCategoryIDUpdated"]),
                            ProductPrice = Convert.ToInt32(sqlReader["ProductPriceUpdated"]),
                            ProductDescription = sqlReader.GetString("ProductDescriptionUpdated")
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
            return updatedProductModel;
        }

        /// <summary>
        ///  Verifies if a given product already exists in the database.
        /// </summary>
        /// <param name="productName">Product name to validate</param>
        /// <returns>True if the product already exists and false otherwise.</returns>
        public bool checkForExistingProduct(string productName)
        {
            if (productName == null)
                throw new ArgumentNullException("productName");

            bool isExistingProduct = true; //By default, we shall add any new products
            int commandResult = 0;
            SqlCommand commandReference = null;

            try
            {
                SqlParameter productnameParam = new SqlParameter("ProductNameInputParam", productName);
                SqlParameter isExistingProductParam = new SqlParameter("ProductSearchCountOutputParam", SqlDbType.Int);
                isExistingProductParam.Direction = ParameterDirection.Output;

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    productnameParam,
                    isExistingProductParam
                };

                (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_ProductExistsByName",
                    sqlParameters);

                if (commandReference != null)
                    isExistingProduct = Convert.ToInt32(commandReference.Parameters["ProductSearchCountOutputParam"].Value) == (int)DatabaseSearchResult.RecordExists;
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return isExistingProduct;
        }

        /// <summary>
        ///  Verifies if a given product already exists in the database.
        /// </summary>
        /// <param name="productID">Product id to validate</param>
        /// <returns>True if the product already exists and false otherwise.</returns>
        public bool checkForExistingProduct(int productID)
        {
            if (productID == 0)
                throw new ArgumentException("productID");

            bool isExistingProduct = true; //By default, we shall add any new products
            int commandResult = 0;
            SqlCommand commandReference = null;

            try
            {
                SqlParameter productnameParam = new SqlParameter("ProductIDParam", productID);
                SqlParameter isExistingProductParam = new SqlParameter("ProductSearchCountOutputParam", SqlDbType.Int);
                isExistingProductParam.Direction = ParameterDirection.Output;

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    productnameParam,
                    isExistingProductParam
                };

                (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_ProductExistsByID",
                    sqlParameters);

                if (commandReference != null)
                    isExistingProduct = Convert.ToInt32(commandReference.Parameters["ProductSearchCountOutputParam"].Value) == (int)DatabaseSearchResult.RecordExists;
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return isExistingProduct;
        }

        /// <summary>
        ///  Remove a product
        /// </summary>
        /// <param name="productID">Product id to remove</param>
        /// <returns>Returns the delete product details if it has been removed successfully and null reference otherwise.</returns>
        public ProductModel deleteProduct(int productID)
        {
            if (productID == 0)
                throw new ArgumentException("productID");

            ProductModel productModelAfterDelete = null;

            try
            {
                SqlParameter productnameParam = new SqlParameter("ProductIDParam", productID);

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    productnameParam,
                };

                using (SqlDataReader sqlReader = _databaseFunctions.executeReader(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_DeleteProduct",
                    sqlParameters))
                {
                    if (sqlReader != null && sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        productModelAfterDelete = new ProductModel
                        {
                            ProductID = Convert.ToInt32(sqlReader["ProductID"]),
                            ProductCategoryID = Convert.ToInt32(sqlReader["ProductCategoryID"]),
                            ProductName = sqlReader["ProductName"].ToString(),
                            ProductPrice = Convert.ToInt32(sqlReader["ProductPrice"].ToString()),
                            ProductDescription = sqlReader["ProductDescription"].ToString(),
                            ProductImageName = sqlReader["ProductImageName"].ToString(),
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
            return productModelAfterDelete;
        }

    }
}
