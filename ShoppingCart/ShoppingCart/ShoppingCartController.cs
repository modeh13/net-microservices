using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Stores.Interfaces;

namespace ShoppingCart.ShoppingCart;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartStore _shoppingCartStore;

    public ShoppingCartController(IShoppingCartStore shoppingCartStore)
    {
        _shoppingCartStore = shoppingCartStore ?? throw new ArgumentNullException(nameof(shoppingCartStore));
    }

    [HttpGet("{userId:int}")]
    public ActionResult<ShoppingCart> GetAsync(int userId) => _shoppingCartStore.Get(userId);
}
