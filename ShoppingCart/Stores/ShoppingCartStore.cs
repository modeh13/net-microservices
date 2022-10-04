using ShoppingCart.Stores.Interfaces;

namespace ShoppingCart.Stores;

public class ShoppingCartStore : IShoppingCartStore
{
    private static readonly IDictionary<int, ShoppingCart.ShoppingCart> Database = new Dictionary<int, ShoppingCart.ShoppingCart>();

    public ShoppingCart.ShoppingCart Get(int userId)
    {
        return Database.ContainsKey(userId) ? Database[userId] : new ShoppingCart.ShoppingCart(userId);
    }

    public void Save(ShoppingCart.ShoppingCart shoppingCart)
    {
        Database[shoppingCart.UserId] = shoppingCart;
    }
}