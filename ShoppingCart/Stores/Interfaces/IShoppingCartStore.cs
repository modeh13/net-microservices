namespace ShoppingCart.Stores.Interfaces;

public interface IShoppingCartStore
{
    ShoppingCart.ShoppingCart Get(int userId);
    void Save(ShoppingCart.ShoppingCart shoppingCart);
}