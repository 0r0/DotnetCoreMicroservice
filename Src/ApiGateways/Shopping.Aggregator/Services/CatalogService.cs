using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _client;

    public CatalogService(HttpClient client)
    {
        _client = client;
    }

    public async Task<CatalogModel> GetCatalog(string id)
    {
        return await (await _client.GetAsync($"/api/v1/Catalog/{id}").ConfigureAwait(false))
            .ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        return await (await _client.GetAsync("api/v1/Catalog").ConfigureAwait(false))
            .ReadContentAs<IReadOnlyCollection<CatalogModel>>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        return  await (await _client.GetAsync($"api/v{1}/Catalog/getProductByCategory/{category}")
            .ConfigureAwait(false))
            .ReadContentAs<IReadOnlyCollection<CatalogModel>>();
    }
}