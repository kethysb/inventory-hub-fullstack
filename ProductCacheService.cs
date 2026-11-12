using System.Text.Json;
using InventoryHub.Shared.Models;

namespace InventoryHub.Client.Services;

public class ProductCacheService
{
    private readonly HttpClient _http;
    private ProductDto[]? _cachedProducts;
    private DateTime _lastFetchedUtc = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(2);

    public ProductCacheService(HttpClient http) => _http = http;

    public async Task<ProductDto[]> GetProductsAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && _cachedProducts is not null && (DateTime.UtcNow - _lastFetchedUtc) < _cacheDuration)
        {
            return _cachedProducts;
        }

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var json = await _http.GetStringAsync("api/productlist", cts.Token);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _cachedProducts = JsonSerializer.Deserialize<ProductDto[]>(json, options) ?? Array.Empty<ProductDto>();
        _lastFetchedUtc = DateTime.UtcNow;
        return _cachedProducts;
    }
}
