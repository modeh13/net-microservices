using Polly;
using ShoppingCart.Abstractions.Services;
using ShoppingCart.Abstractions.Stores;
using ShoppingCart.Services;
using SqlEventStore = ShoppingCart.Stores.EventStore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Scan(scan =>
    scan.FromCallingAssembly()
        .AddClasses()
        .AsMatchingInterface());
builder.Services.AddSingleton<IEventStore, SqlEventStore>();
builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt))));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
