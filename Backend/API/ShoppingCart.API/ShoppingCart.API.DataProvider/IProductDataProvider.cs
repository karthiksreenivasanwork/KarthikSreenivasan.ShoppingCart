using ShoppingCart.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.API.DataProvider
{
    public interface IProductDataProvider
    {
        public List<ProductCategoryModel> getAllProductCategories();

        public List<ProductModel> getAllProducts();

        public List<ProductModel> getProductsByName(string productSearchText);

        /// <summary>
        /// Returns the product collection based on it's category id.
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<ProductModel> getProductsByCategory(int categoryID);

        /// <summary>
        /// Register a new product.
        /// </summary>
        /// <param name="newProductToRegister">A new product with the specifications.</param>
        /// <returns>Returns the product ID if the new product was added successfully or 0 otherwise.</returns>
        /// <exception cref="ArgumentNullException">Input parameter cannot be null</exception>
        /// <exception cref="ProductException">Is thrown if this an existing product</exception>
        public Task<int> AddNewProduct(ProductModel newProductToRegister);
        /// <summary>
        /// Update product details
        /// </summary>
        /// <param name="productDataToUpdate">Update model type ShoppingCart.API.Models.ProductModel</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Input parameter cannot be null</exception>
        /// <exception cref="ProductException">Is thrown if no product exists</exception>
        public ProductModel UpdateProduct(ProductModel productDataToUpdate);

        /// <summary>
        ///  Verifies if a given product already exists in the database.
        /// </summary>
        /// <param name="productName">Product name to validate</param>
        /// <returns>True if the product already exists and false otherwise.</returns>
        public bool checkForExistingProduct(string productName);

        /// <summary>
        ///  Verifies if a given product already exists in the database.
        /// </summary>
        /// <param name="productID">Product id to validate</param>
        /// <returns>True if the product already exists and false otherwise.</returns>
        public bool checkForExistingProduct(int productID);

        /// <summary>
        ///  Remove a product
        /// </summary>
        /// <param name="productID">Product id to remove</param>
        /// <returns>Returns the delete product details if it has been removed successfully and null reference otherwise.</returns>
        public ProductModel deleteProduct(int productID);
    }
}
