using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _catalogContext.Products.Find(p => true).ToListAsync();
    }

    public async Task<Product> GetProductById(string id)
    {
        return await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string productName)
    {
        return await _catalogContext.Products.Find(p => p.Name == productName).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        return await _catalogContext.Products.Find(p => p.Category == categoryName).ToListAsync();
    }

    public async Task CreateProduct(Product product)
    {
        await _catalogContext.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _catalogContext.Products.ReplaceOneAsync(a => a.Id == product.Id, product);
        return updateResult.IsAcknowledged && updateResult.MatchedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var deleteResult = await _catalogContext.Products.DeleteOneAsync(a => a.Id == id).ConfigureAwait(false);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}