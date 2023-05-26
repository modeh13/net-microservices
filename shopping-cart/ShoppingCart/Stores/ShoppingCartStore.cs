using System.Data.SqlClient;
using Dapper;
using ShoppingCart.Abstractions.Stores;

namespace ShoppingCart.Stores;

public class ShoppingCartStore : IShoppingCartStore
{
    private readonly IConfiguration _configuration;

    private const string InsertShoppingCartSql =
        @"INSERT INTO dbo.ShoppingCart (UserId) OUTPUT INSERTED.Id VALUES (@UserId)";

    private const string SelectShoppingCartSql = @"SELECT SC.Id ShoppingCartId, SI.ProductCatalogId, SI.ProductName,
        SI.ProductDescription, SI.Amount, SI.Currency
        FROM ShoppingCartItem SI
        INNER JOIN ShoppingCart SC ON SI.ShoppingCartId = SC.Id
        WHERE SC.UserId = @UserId";

    private const string DeleteAllForShoppingCartSql = @"DELETE SI FROM dbo.ShoppingCartItem SI 
        INNER JOIN dbo.ShoppingCart SC ON SI.ShoppingCartId = SC.Id AND SC.UserId = @UserId";

    private const string InsertShoppingCartItemsSql =
        @"INSERT INTO dbo.ShoppingCartItem (ShoppingCartId, ProductCatalogId, ProductName, ProductDescription, Amount, Currency)
                                  VALUES (@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)";
    
    public ShoppingCartStore(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private string? GetConnectionString()
    {
        return _configuration.GetConnectionString("ShoppingCartDb");
    }

    public async Task<ShoppingCartModel?> GetAsync(int userId)
    {
        await using var sqlConnection = new SqlConnection(GetConnectionString());
        var queryResult = (await sqlConnection.QueryAsync(SelectShoppingCartSql, new { UserId = userId })).ToArray();

        if (queryResult.Length <= 0)
        {
            return default;
        }

        var shoppingCartItems = queryResult
            .Select(shoppingCartItem => new ShoppingCartItem(Convert.ToInt32(shoppingCartItem.ProductCatalogId), 
                shoppingCartItem.ProductName, 
                shoppingCartItem.ProductDescription, 
                new Money(shoppingCartItem.Currency, Convert.ToDecimal(shoppingCartItem.Amount))))
            .OrderBy(shoppingCartItem => shoppingCartItem.ProductCatalogId);
        
        return new ShoppingCartModel(queryResult.First().ShoppingCartId, userId, shoppingCartItems);
    }

    public async Task SaveAsync(ShoppingCartModel shoppingCart)
    {
        await using var sqlConnection = new SqlConnection(GetConnectionString());
        await sqlConnection.OpenAsync();

        await using var dbTransaction = sqlConnection.BeginTransaction();
        var shoppingCartId = shoppingCart.Id > 0
            ? shoppingCart.Id
            : await sqlConnection.QuerySingleAsync<int>(InsertShoppingCartSql, new { shoppingCart.UserId },
                dbTransaction);

        await sqlConnection.ExecuteAsync(DeleteAllForShoppingCartSql, new { shoppingCart.UserId }, dbTransaction);
        await sqlConnection.ExecuteAsync(InsertShoppingCartItemsSql, shoppingCart.Items.Select(item => new
        {
            ShoppingCartId = shoppingCartId,
            item.ProductCatalogId,
            item.ProductName,
            ProductDescription = item.Description,
            item.Money.Amount,
            item.Money.Currency
        }), dbTransaction);
        await dbTransaction.CommitAsync();
    }
}
