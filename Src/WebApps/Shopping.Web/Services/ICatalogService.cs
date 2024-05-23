using Shopping.Web.Models;

namespace Shopping.Web.Services;

public interface ICatalogService
{
    public Task<CatalogModel> GetCatalog(string id);
    public Task<IEnumerable<CatalogModel>> GetCatalog();
    public Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
    public Task<CatalogModel> CreateCatalog(CatalogModel model);
}