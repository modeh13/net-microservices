using ShoppingCart.Abstractions.Stores;

namespace ShoppingCart.Models;

public class ShoppingCart
{
    public int Id { get; private set; }
    public int UserId { get; }

    public HashSet<ShoppingCartItem> Items { get; }

    public ShoppingCart(int userId) : this(userId, new HashSet<ShoppingCartItem>()) { }

    public ShoppingCart(int id, int userId, IEnumerable<ShoppingCartItem> shoppingCartItems) : this(userId, shoppingCartItems)
    {
        Id = id;
    } 
    
    private ShoppingCart(int userId, IEnumerable<ShoppingCartItem> shoppingCartItems)
    {
        UserId = userId;
        Items = new HashSet<ShoppingCartItem>(shoppingCartItems);
    }

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
    {
        foreach (var shoppingCartItem in shoppingCartItems.Where(item => Items.Add(item)))
        {
            eventStore.Raise("ShoppingCartItemAdded", new { UserId, Item = shoppingCartItem});
        }
    }

    public void RemoveItems(int[] productCatalogIds, IEventStore eventStore)
    {
        foreach (var shoppingCartItem in Items.Where(item => productCatalogIds.Contains(item.ProductCatalogId) && Items.Remove(item)))
        {
            eventStore.Raise("ShoppingCardItemDeleted", new { UserId, Item = shoppingCartItem});
        }
    } 
}
