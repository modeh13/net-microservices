using ShoppingCart.EventFeed.Interfaces;

namespace ShoppingCart.ShoppingCart;

public class ShoppingCart
{
    private readonly HashSet<ShoppingCartItem> _items = new();

    public int UserId { get; }
    public IEnumerable<ShoppingCartItem> Items => _items;

    public ShoppingCart(int userId) => UserId = userId;

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
    {
        foreach (var shoppingCartItem in shoppingCartItems.Where(item => _items.Add(item)))
        {
            eventStore.Raise("ShoppingCartItemAdded", new { UserId, Item = shoppingCartItem});
        }
    }

    public void RemoveItems(int[] productCatalogIds, IEventStore eventStore)
    {
        foreach (var shoppingCartItem in _items.Where(item => productCatalogIds.Contains(item.ProductCatalogId) && _items.Remove(item)))
        {
            eventStore.Raise("ShoppingCardItemDeleted", new { UserId, Item = shoppingCartItem});
        }
    } 
}

public record ShoppingCartItem(int ProductCatalogId, string ProductName, string Description, Money money)
{
    public virtual bool Equals(ShoppingCartItem? obj) => obj != null && ProductCatalogId == obj.ProductCatalogId;
    public override int GetHashCode() => ProductCatalogId.GetHashCode();
}

public record Money(string Currency, decimal Amount);
