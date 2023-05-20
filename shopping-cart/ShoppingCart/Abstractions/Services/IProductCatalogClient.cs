namespace ShoppingCart.Abstractions.Services;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsAsync(int[] productIds);
}
