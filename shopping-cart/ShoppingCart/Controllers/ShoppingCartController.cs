using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Abstractions.Services;
using ShoppingCart.Abstractions.Stores;

namespace ShoppingCart.Controllers;

[ApiController]
[Route("api/[controller]/{userId:int}")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartStore _shoppingCartStore;
    private readonly IEventStore _eventStore;
    private readonly IProductCatalogClient _productCatalogClient;

    public ShoppingCartController(IShoppingCartStore shoppingCartStore,
        IEventStore eventStore,
        IProductCatalogClient productCatalogClient)
    {
        _shoppingCartStore = shoppingCartStore ?? throw new ArgumentNullException(nameof(shoppingCartStore));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _productCatalogClient = productCatalogClient ?? throw new ArgumentNullException(nameof(productCatalogClient));
    }

    [HttpGet("")]
    public async Task<ActionResult<ShoppingCartModel>> GetAsync(int userId)
    {
        var shoppingCart = await _shoppingCartStore.GetAsync(userId);
        if (shoppingCart is null)
        {
            return NotFound();
        }

        return Ok(shoppingCart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<ShoppingCartModel>> PostAsync(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart = await _shoppingCartStore.GetAsync(userId) ?? new ShoppingCartModel(userId);

        var shoppingCartItems = await _productCatalogClient.GetShoppingCartItemsAsync(productIds);
        await shoppingCart.AddItemsAsync(shoppingCartItems, _eventStore);
        await _shoppingCartStore.SaveAsync(shoppingCart);

        return Ok(shoppingCart);
    }

    [HttpDelete("items")]
    public async Task<IActionResult> DeleteAsync(int userId, [FromBody] int[] productIds)
    {
        var shoppingCart = await _shoppingCartStore.GetAsync(userId);
        if (shoppingCart is null)
        {
            return NotFound();
        }

        await shoppingCart.RemoveItemsAsync(productIds, _eventStore);
        await _shoppingCartStore.SaveAsync(shoppingCart);

        return Ok();
    }
}
