using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.API.SQLDataProvider
{
    internal class SqlProviderStrings
    {
        /// <summary>
        /// Defined in the appsettings.json of the API project which we can access using dependency injection of the reference to the configuration object.
        /// </summary>
        public const string SQL_CONNECTION_KEY_NAME = "SQLConnection";
    }
}
