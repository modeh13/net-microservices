using System.Net.Http.Headers;
using System.Text.Json;
using ShoppingCart.Services.Interfaces;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart.Services;

public class ProductCatalogClient : IProductCatalogClient
{
    private readonly HttpClient _httpClient;
    private const string ProductCatalogBaseUri = "https://git.io/JeJiE";
    private const string GetProductPathTemplate = "?productIds=[{0}]";

    public ProductCatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(ProductCatalogBaseUri);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private sealed record ProductCatalogProduct(int ProductId, string ProductName, string ProductDescription, Money Price);

    public Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsAsync(int[] productIds)
    { 
        // TODO: GR - Comment this out while ProductCatalog is implemented.
        //using var httpResponseMessage = await RequestProductFromProductCatalogAsync(productIds);
        //return await ConvertToShoppingCartItems(httpResponseMessage);

        var shoppingCartItems = new List<ShoppingCartItem>
        {
            new(1, "Product 1", "Desc P1", new Money("USD", 45.50M)),
            new(2, "Product 2", "Desc P2", new Money("USD", 5.90M)),
            new(3, "Product 3", "Desc P3", new Money("USD", 25.90M))
        };
        
        return Task.FromResult<IEnumerable<ShoppingCartItem>>(shoppingCartItems);
    }

    private async Task<HttpResponseMessage> RequestProductFromProductCatalogAsync(IEnumerable<int> productCatalogIds)
    {
        var productsResources = string.Format(GetProductPathTemplate, string.Join(",", productCatalogIds));

        return await _httpClient.GetAsync(productsResources);
    }

    private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage httpResponseMessage)
    {
        httpResponseMessage.EnsureSuccessStatusCode();

        var streamResponse = await httpResponseMessage.Content.ReadAsStreamAsync();
        var products = await JsonSerializer.DeserializeAsync<List<ProductCatalogProduct>>(streamResponse,
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true}) ?? new List<ProductCatalogProduct>();

        return products.Select(product => new ShoppingCartItem(product.ProductId, product.ProductName, product.ProductDescription, product.Price));
    }
}
