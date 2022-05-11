using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Coordinator;
using ShoppingCart.API.DataProvider;

namespace ShoppingCart.API.BusinessLogic
{
    public class Product
    {
        DBCoordinator _dBCoordinator;

        public Product(ProviderType providerType, IConfiguration configuration)
        {
            _dBCoordinator = new DBCoordinator(providerType, configuration);
        }

        public IProductDataProvider getProductProvider()
        {
            return _dBCoordinator.getProductProvider();
        }
    }
}
