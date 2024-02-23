using Asp.Versioning.Builder;
using Catalog.API.Entities;
using Catalog.API.Repositories;

namespace Catalog.API;

public static class Endpoints
{
    public static void CatalogRoutes(this WebApplication application,
        ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        group.MapGet("/Catalog",
            async (IProductRepository repository) => await repository.GetProducts());

        group.MapGet("/catalog/{id}",
            async (string id, IProductRepository repository) => await repository.GetProductById(id));

        group.MapGet("/catalog/getProductByCategory/{category}",
            async (string category, IProductRepository repository) =>
                await repository.GetProductByCategory(category));
        
        group.MapPost("/catalog",
            async (Product product, IProductRepository repository) =>
            {
                await repository.CreateProduct(product);
                return Results.Created("id", product.Id);
            });

        group.MapPut("/catalog/{id}",
            async (string id, Product product, IProductRepository repository) =>
            {
                await repository.UpdateProduct(product);
                return Results.NoContent();
            });
        
        
        group.MapDelete("/catalog/{id}",
            async (string id, IProductRepository repository) =>
            {
                await repository.DeleteProduct(id);
                return Results.NoContent();
            });
    }
}