using Shopping.Web.Extensions;
using Shopping.Web.Models;

namespace Shopping.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CatalogModel> GetCatalog(string id)
    {
        return await (await _httpClient.GetAsync($"/Catalog/{id}").ConfigureAwait(false))
            .ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        return await (await _httpClient.GetAsync("/Catalog").ConfigureAwait(false))
            .ReadContentAs<IReadOnlyCollection<CatalogModel>>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        return await (await _httpClient.GetAsync($"/Catalog/getProductByCategory/{category}")
                .ConfigureAwait(false))
            .ReadContentAs<IReadOnlyCollection<CatalogModel>>();
    }

    public async Task<CatalogModel> CreateCatalog(CatalogModel model)
    {
        var response = await _httpClient.PostAsJson("/Catalog", model).ConfigureAwait(false);
        return response.IsSuccessStatusCode
            ? await response.ReadContentAs<CatalogModel>()
            : throw new Exception($"catalog with {model.Id} is not created");
    }
}