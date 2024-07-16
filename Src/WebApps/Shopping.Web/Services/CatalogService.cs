using Shopping.Web.Extensions;
using Shopping.Web.Models;

namespace Shopping.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(HttpClient httpClient, ILogger<CatalogService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CatalogModel> GetCatalog(string id)
    {
        _logger.LogInformation("getting catalog products from url:{url}",_httpClient.BaseAddress);
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