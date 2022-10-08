using ShoppingCart.Services;
using ShoppingCart.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Scan(scan =>
    scan.FromCallingAssembly()
        .AddClasses()
        .AsMatchingInterface());
builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapGet("/", () => "Hello World!");

app.Run();
