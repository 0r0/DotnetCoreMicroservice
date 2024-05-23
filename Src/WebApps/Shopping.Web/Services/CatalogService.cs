using Shopping.Web.Models;

namespace Shopping.Web.Services;

public class CatalogService : ICatalogService
{
    public Task<CatalogModel> GetCatalog(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        throw new NotImplementedException();
    }

    public Task<CatalogModel> CreateCatalog(CatalogModel model)
    {
        throw new NotImplementedException();
    }
}