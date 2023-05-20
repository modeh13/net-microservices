namespace ShoppingCart.Models;

public record ShoppingCartItem(int ProductCatalogId, string ProductName, string Description, Money Money)
{
    public virtual bool Equals(ShoppingCartItem? obj) => obj != null && ProductCatalogId == obj.ProductCatalogId;
    public override int GetHashCode() => ProductCatalogId.GetHashCode();
}
