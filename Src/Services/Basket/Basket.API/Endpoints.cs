using Asp.Versioning.Builder;

namespace Basket.API;

public static class Endpoints
{
    public static void CatalogRoutes(this WebApplication application,
        ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application
            .MapGroup("api/v{version:apiVersion}/{basket}")
            .WithApiVersionSet(apiVersionSet);

        group.MapGet("/Basket",
            async (IBasketRepository repository) => await repository.GetBaskets());

        // group.MapGet("/catalog/{id}",
        //     async (string id, IBasketRepository repository) => await repository.GetBasketById(id));
        //
        // group.MapGet("/catalog/getBasketByCategory/{category}",
        //     async (string category, IBasketRepository repository) =>
        //         await repository.GetBasketByCategory(category));

        group.MapPost("/Basket",
            async (Basket basket, IBasketRepository repository) =>
            {
                await repository.CreateBasket(basket);
                return Results.Created("id", basket.Id);
            });
        group.MapPost("/Basket/Checkkout",
            async (Basket basket, IBasketRepository repository) =>
            {
                await repository.CreateBasket(basket);
                return Results.Created("id", basket.Id);
            });

        // group.MapPut("/catalog/{id}/{basket}",
        //     async (string id, Basket basket, IBasketRepository repository) =>
        //     {
        //         await repository.UpdateBasket(basket);
        //         return Results.NoContent();
        //     });


        group.MapDelete("/basket/{id}",
            async (string id, IBasketRepository repository) =>
            {
                await repository.DeleteBasket(id);
                return Results.NoContent();
            });
    }
}

public class Basket
{
    public object? Id { get; set; }
}

public interface IBasketRepository
{
    Task<object> GetBaskets();
    Task GetBasketById(string id);
    Task CreateBasket(Basket basket);
    Task UpdateBasket(Basket basket);
    Task DeleteBasket(string id);
    Task GetBasketByCategory(string category);
}