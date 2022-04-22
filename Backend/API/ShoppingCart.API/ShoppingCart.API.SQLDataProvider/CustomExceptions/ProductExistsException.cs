using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    // Summary:
    //     Represents the error that occurs if the product name has already been taken.
    /// </summary>
    public class ProductExistsException : Exception
    {
        public ProductExistsException()
        {
        }

        public ProductExistsException(string productName)
            : base(string.Format("Product `{0}` already exists", productName))
        {
        }

    }
}
