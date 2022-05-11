using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Coordinator;
using ShoppingCart.API.DataProvider;

namespace ShoppingCart.API.BusinessLogic
{
    public class Cart
    {
        DBCoordinator _dBCoordinator;

        public Cart(ProviderType providerType, IConfiguration configuration)
        {
            _dBCoordinator = new DBCoordinator(providerType, configuration);
        }

        public ICartDataProvider getCartProvider()
        {
            return _dBCoordinator.getCartProvider();
        }
    }
}
