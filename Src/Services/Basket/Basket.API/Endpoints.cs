using Asp.Versioning.Builder;
using Basket.API.Entities;
using Basket.API.Repositories;

namespace Basket.API;

public static class Endpoints
{
    public static void BasketRoutes(this WebApplication application,
        ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application
            .MapGroup("api/v{version:apiVersion}/{basket}")
            .WithApiVersionSet(apiVersionSet);

        group.MapGet("/Basket/{userName}",
            async (string userName, IBasketRepository repository) =>
            {
                var basket = await repository.GetBasket(userName);
                return Results.Ok(basket ?? new ShoppingCart(userName));
            }
        );


        group.MapPost("/Basket/",
            async (ShoppingCart shoppingCart, IBasketRepository repository) =>
            {
                await repository.UpdateBasket(shoppingCart);
                return Results.Ok(shoppingCart);
            });
     


        group.MapDelete("/basket/{userName}",
            async (string userName, IBasketRepository repository) =>
            {
                await repository.Delete(userName);
                return Results.Ok();
            });
    }
}