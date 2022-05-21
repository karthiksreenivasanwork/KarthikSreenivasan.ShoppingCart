using Microsoft.Extensions.Configuration;
using ShoppingCart.API.Coordinator;
using ShoppingCart.API.DataProvider;

namespace ShoppingCart.API.BusinessLogic
{
    public class User
    {
        DBCoordinator _dBCoordinator;

        public User(ProviderType providerType, IConfiguration configuration)
        {
            _dBCoordinator = new DBCoordinator(providerType, configuration);
        }

        public IUserDataProvider getUserProvider()
        {
            return _dBCoordinator.getUserProvider();
        }
    }
}
