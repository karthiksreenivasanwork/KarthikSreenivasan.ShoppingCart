using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    // Summary:
    //     Represents the error that occurs if the product name has already been taken.
    /// </summary>
    public class ProductException : Exception
    {
        public ProductException()
        {
        }

        public ProductException(string errorMessage)
            : base(errorMessage)
        {
        }

    }
}
