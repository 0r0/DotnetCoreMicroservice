using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API;

public static class Endpoints
{
    public static void CatalogRoutes(this WebApplication application)
    {
        application.MapGet("/api/v1/Catalog",
            async (IProductRepository repository) => await repository.GetProducts());
        application.MapGet("/api/v1/catalog/{id}",
            async (string id, IProductRepository repository) => await repository.GetProductById(id));
        application.MapGet("/api/v1/catalog/getProductByCategory/{category}",
            async (string category, IProductRepository repository) =>
                await repository.GetProductByCategory(category));
        application.MapPost("/api/v1/catalog", async (Product product, IProductRepository repository) =>
        {
            await repository.CreateProduct(product);
            return Results.Created("id", product.Id);
        });
        application.MapPut("/api/v1/catalog/{id}", async (string id, Product product, IProductRepository repository) =>
        {
            await repository.UpdateProduct(product);
            return Results.NoContent();
        });
        application.MapDelete("/api/v1/catalog/{id}", async (string id, IProductRepository repository) =>
        {
            await repository.DeleteProduct(id);
            return Results.NoContent();
        });
      
    }
}