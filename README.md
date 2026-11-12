# InventoryHub — Full-Stack Integration (Blazor WebAssembly + Minimal API)

Capstone project for Microsoft's *Full-Stack Integration* course, built and optimized with Microsoft Copilot.

## Architecture & Implemented Features
1. **Minimal API Back-End (`ServerProgram.cs`)**:
   - Exposes `GET /api/productlist` (plus `POST /api/productlist` and `DELETE /api/productlist/{id}`) returning strongly-typed JSON with nested `Category` objects (`{ id, name, price, stock, category: { id, name } }`).
   - Configures CORS policy (`AllowBlazorClient`) so Blazor WebAssembly can communicate seamlessly with the API.
   - Implements server-side `IMemoryCache` caching (60-second sliding expiration) to optimize throughput and eliminate redundant database/list hits.
2. **Blazor WebAssembly Front-End (`FetchProducts.razor`, `ClientProgram.cs`, `ProductModels.cs`)**:
   - Consumes `/api/productlist` asynchronously via `HttpClient` + `System.Text.Json`.
   - Implements client-side in-memory product caching and cancellation/timeout error handling (`try/catch` for `HttpRequestException`, `TaskCanceledException`, `JsonException`) with user-friendly fallback states.
   - Renders responsive product & category cards with `@key` optimization.
3. **Copilot Integration & Reflection**:
   - Microsoft Copilot assisted in generating API integration code, debugging CORS & JSON property casing mismatches (`PropertyNameCaseInsensitive = true`), structuring nested JSON models, and implementing caching optimizations.

## Running Locally
```bash
dotnet run --project ServerApp.csproj
dotnet run --project ClientApp.csproj
```
