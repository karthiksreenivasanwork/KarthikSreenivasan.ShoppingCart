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
        DatabaseFunctions<ProductDataProvider> _databaseFunctions;

        public ProductDataProvider(IConfiguration configuration)
        {
            this._configuration = configuration;
            _databaseFunctions = new DatabaseFunctions<ProductDataProvider>();
        }

        public List<ProductCategoryModel> getAllProductCategories()
        {
            var productCategoryCollection = new List<ProductCategoryModel>();

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
            return productCategoryCollection;
        }

        public List<ProductModel> getAllProducts()
        {
            var productCollection = new List<ProductModel>();

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
                            ProductImage = sqlReader["ProductImage"].ToString(),
                        });
                    }
                }
            }
            return productCollection;
        }
    }
}
