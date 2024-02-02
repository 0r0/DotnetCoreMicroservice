using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration
            .GetValue<string>("DatabaseSettings:ConnectionStrings"));
        var database = client.GetDatabase(configuration
            .GetValue<string>("DatabaseSettings:DatabaseName"));
        Products = database.GetCollection<Product>(configuration
            .GetValue<string>("DatabaseSettings:CollectionName"));
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}