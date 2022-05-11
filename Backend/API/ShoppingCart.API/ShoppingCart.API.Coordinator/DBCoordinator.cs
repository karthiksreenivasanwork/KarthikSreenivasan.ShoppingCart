using Microsoft.Extensions.Configuration;
using ShoppingCart.API.DataProvider;
using ShoppingCart.API.Models;
using ShoppingCart.API.SQLDataProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.API.Coordinator
{
    public enum ProviderType
    {
        SQL = 0
    }

    public class DBCoordinator
    {
        private ProviderType _providerType;
        IConfiguration _configuration; //Required NuGet package - Microsoft.Extensions.Configuration.Abstractions

        public DBCoordinator(ProviderType providerType, IConfiguration configuration)
        {
            this._providerType = providerType;
            this._configuration = configuration;
        }

        public IUserDataProvider getUserProvider()
        {
            IUserDataProvider userDataProvider;
            switch (this._providerType)
            {
                case ProviderType.SQL:
                    userDataProvider = new UserDataProvider(this._configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME));
                    break;
                default:
                    userDataProvider = new UserDataProvider(this._configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME));
                    break;
            }
            return userDataProvider;
        }

        public IProductDataProvider getProductProvider()
        {
            IProductDataProvider productDataProvider;
            switch (this._providerType)
            {
                case ProviderType.SQL:
                    productDataProvider = new ProductDataProvider(this._configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME));
                    break;
                default:
                    productDataProvider = new ProductDataProvider(this._configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME));
                    break;
            }
            return productDataProvider;
        }

        public ICartDataProvider getCartProvider()
        {
            ICartDataProvider cartDataProvider;
            switch (this._providerType)
            {
                case ProviderType.SQL:
                    cartDataProvider = new CartDataProvider(this._configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME));
                    break;
                default:
                    cartDataProvider = new CartDataProvider(this._configuration.GetConnectionString(SqlProviderStrings.SQL_CONNECTION_KEY_NAME));
                    break;
            }
            return cartDataProvider;
        }
    }
}
