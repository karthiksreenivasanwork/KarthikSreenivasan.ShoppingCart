using ShoppingCart.API.Models;
using System.Collections.Generic;

namespace ShoppingCart.API.DataProvider
{
    public interface ICartDataProvider
    {
        public CartItemModel addNewItemToCart(CartItemModel cartModel);

        public CartItemModel removeProductQuantityFromCart(CartItemModel cartModel);

        public CartItemModel removeProductFromCart(CartItemModel cartModel);

        public List<CartItemCollectionModel> getCartItemForUser(string usernameParam);
    }
}
