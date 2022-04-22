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
        /// Register a new product.
        /// </summary>
        /// <param name="newProductToRegister">A new product with the specifications.</param>
        /// <returns>Returns true if the new product was added successfully and false otherwise.</returns>
        public bool addNewProduct(ProductModel newProductToRegister)
        {
            if (newProductToRegister == null)
                throw new ArgumentNullException("newProductToRegister");

            if (this.checkForExistingProduct(newProductToRegister.ProductName))
                throw new ProductExistsException(newProductToRegister.ProductName);

            bool hasProductAddedSuccessfully = false;
            int commandResult = 0;
            SqlCommand commandReference = null;

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("ProductCategoryNameParam", newProductToRegister.ProductCategoryName),
                    new SqlParameter("ProductNameParam", newProductToRegister.ProductName),
                    new SqlParameter("ProductPriceParam", newProductToRegister.ProductPrice),
                    new SqlParameter("ProductDescriptionParam", newProductToRegister.ProductDescription),
                    new SqlParameter("ProductImageNameParam", newProductToRegister.ProductImageName)
                };

                (commandResult, commandReference) = _databaseFunctions.executeNonQuery(
                    _configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME),
                    "Sch_ProductManagement.sp_CreateProduct",
                    sqlParameters);

                if (commandReference != null)
                    hasProductAddedSuccessfully = commandResult > 0;
            }
            catch (Exception ex)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
            return hasProductAddedSuccessfully;
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
                    "Sch_ProductManagement.sp_ProductExists",
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
    }
}
