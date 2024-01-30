using Catalog.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API;

[Route("api/")]
public static class Endpoints
{
    public static void CatalogRoutes(this WebApplication application)
    {
        application.MapGet("/api/v1/Catalog", () => new List<object>());
        application.MapGet("/api/v1/catalog/{id}", (string id) => new object());
        application.MapGet("/api/v1/catalog/getProductByCategory/{category}", (string category) => new object());
        application.MapPost("/api/v1/catalog", (Product product) => { });
        application.MapPut("/api/v1/catalog/{id}", (string id) => { });
        application.MapDelete("/api/v1/catalog/{id}", (string id) => { });
    }
}