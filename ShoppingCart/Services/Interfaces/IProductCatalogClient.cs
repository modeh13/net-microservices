using ShoppingCart.ShoppingCart;

namespace ShoppingCart.Services.Interfaces;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsAsync(int[] productIds);
}
