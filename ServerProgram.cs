using Microsoft.Extensions.Caching.Memory;
using InventoryHub.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("AllowBlazorClient");

var seedProducts = new List<ProductDto>
{
    new() { Id = 1, Name = "Laptop", Price = 1200.50m, Stock = 25, Category = new CategoryDto { Id = 101, Name = "Electronics" } },
    new() { Id = 2, Name = "Headphones", Price = 50.00m, Stock = 100, Category = new CategoryDto { Id = 102, Name = "Accessories" } },
    new() { Id = 3, Name = "Mechanical Keyboard", Price = 149.99m, Stock = 45, Category = new CategoryDto { Id = 102, Name = "Accessories" } },
    new() { Id = 4, Name = "4K Monitor", Price = 399.00m, Stock = 18, Category = new CategoryDto { Id = 101, Name = "Electronics" } }
};

app.MapGet("/api/productlist", (IMemoryCache cache) =>
{
    const string cacheKey = "inventory_productlist";
    if (!cache.TryGetValue(cacheKey, out List<ProductDto>? cached))
    {
        cached = seedProducts;
        cache.Set(cacheKey, cached, TimeSpan.FromSeconds(60));
    }
    return Results.Ok(cached);
});

app.MapPost("/api/productlist", (ProductDto item, IMemoryCache cache) =>
{
    item.Id = seedProducts.Count > 0 ? seedProducts.Max(p => p.Id) + 1 : 1;
    seedProducts.Add(item);
    cache.Remove("inventory_productlist");
    return Results.Created($"/api/productlist/{item.Id}", item);
});

app.Run();
