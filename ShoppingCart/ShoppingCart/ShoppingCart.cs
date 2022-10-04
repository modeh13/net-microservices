namespace ShoppingCart.ShoppingCart;

public class ShoppingCart
{
    private readonly HashSet<ShoppingCartItem> _items = new();

    public int UserId { get; }
    public IEnumerable<ShoppingCartItem> Items => _items;

    public ShoppingCart(int userId) => UserId = userId;

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems)
    {
        foreach (var shoppingCartItem in shoppingCartItems)
        {
            _items.Add(shoppingCartItem);
        }
    }

    public void RemoveItems(int[] productCatalogIds) => _items.RemoveWhere(item => productCatalogIds.Contains(item.ProductCatalogId));
}

public record ShoppingCartItem(int ProductCatalogId, string ProductName, string Description, Money money)
{
    public virtual bool Equals(ShoppingCartItem? obj) => obj != null && ProductCatalogId == obj.ProductCatalogId;
    public override int GetHashCode() => ProductCatalogId.GetHashCode();
}

public record Money(string Currency, decimal Amount);
