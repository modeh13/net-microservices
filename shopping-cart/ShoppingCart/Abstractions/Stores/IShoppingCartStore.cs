namespace ShoppingCart.Abstractions.Stores;

public interface IShoppingCartStore
{
    Task<ShoppingCartModel?> GetAsync(int userId);
    Task SaveAsync(ShoppingCartModel shoppingCart);
}
