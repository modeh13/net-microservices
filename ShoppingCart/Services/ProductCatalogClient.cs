using System.Net.Http.Headers;
using System.Text.Json;
using ShoppingCart.Services.Interfaces;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart.Services;

public class ProductCatalogClient : IProductCatalogClient
{
    private readonly HttpClient _httpClient;
    private static string productCatalogBaseUri = "https://git.io/JeJiE";
    private static string getProductPathTemplate = "?productIds=[{0}]";
    
    public ProductCatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(productCatalogBaseUri);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private sealed record ProductCatalogProduct(int ProductId, string ProductName, string ProductDescription, Money Price);

    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItemsAsync(int[] productIds)
    {
        using var httpResponseMessage = await RequestProductFromProductCatalogAsync(productIds);
        
        return await ConvertToShoppingCartItems(httpResponseMessage);
    }

    private async Task<HttpResponseMessage> RequestProductFromProductCatalogAsync(int[] productCatalogIds)
    {
        var productsResources = string.Format(getProductPathTemplate, string.Join(",", productCatalogIds));

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
