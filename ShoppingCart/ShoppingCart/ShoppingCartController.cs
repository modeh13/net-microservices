using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services.Interfaces;
using ShoppingCart.Stores.Interfaces;

namespace ShoppingCart.ShoppingCart;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartStore _shoppingCartStore;
    private readonly IProductCatalogClient _productCatalogClient;

    public ShoppingCartController(IShoppingCartStore shoppingCartStore, IProductCatalogClient productCatalogClient)
    {
        _shoppingCartStore = shoppingCartStore ?? throw new ArgumentNullException(nameof(shoppingCartStore));
        _productCatalogClient = productCatalogClient ?? throw new ArgumentNullException(nameof(productCatalogClient));
    }

    [HttpGet("{userId:int}")]
    public ActionResult<ShoppingCart> GetAsync(int userId) => _shoppingCartStore.Get(userId);

    [HttpPost("{userId:int}/items")]
    public async Task<ActionResult<ShoppingCart>> PostAsync(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart = _shoppingCartStore.Get(userId);
        var shoppingCartItems = await _productCatalogClient.GetShoppingCartItemsAsync(productIds);
        
        shoppingCart.AddItems(shoppingCartItems);
        _shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
    }

    [HttpDelete("{userId:int}/items")]
    public ActionResult<ShoppingCart> DeleteAsync(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart = _shoppingCartStore.Get(userId);
        
        shoppingCart.RemoveItems(productIds);
        _shoppingCartStore.Save(shoppingCart);

        return shoppingCart;
    }
}
