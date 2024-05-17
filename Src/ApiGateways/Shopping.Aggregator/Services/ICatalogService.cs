using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface ICatalogService
{
    public Task<CatalogModel> GetCatalog(string id);
    public Task<IEnumerable<CatalogModel>> GetCatalog();
    public Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
}