using Asp.Versioning.Builder;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;

namespace Basket.API;

public static class Endpoints
{
    public static void BasketRoutes(this WebApplication application,
        ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application
            .MapGroup("api/v{version:apiVersion}/")
            .WithApiVersionSet(apiVersionSet);

        group.MapGet("/Basket/{userName}",
            async (string userName, IBasketRepository repository) =>
            {
                var basket = await repository.GetBasket(userName);
                return Results.Ok(basket ?? new ShoppingCart(userName));
            }
        );


        group.MapPost("/Basket/",
            async (ShoppingCart shoppingCart, IBasketRepository repository,DiscountGrpcService service) =>
            {
                //get discount coupon amount and subtract from shopping cards item price
                foreach (var item in shoppingCart.Items)
                {
                    var coupon = await service.GetDiscount(item.ProductName);
                    item.Price -= coupon.Amount;
                }
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