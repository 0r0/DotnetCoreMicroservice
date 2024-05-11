using Asp.Versioning.Builder;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;

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
            async (ShoppingCart shoppingCart, IBasketRepository repository, DiscountGrpcService service) =>
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
        group.MapPost("/checkout/{checkOut}", async (BasketCheckout basketCheckout, IBasketRepository repository,IPublishEndpoint publish) =>
        {
            var basket = await repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
                return Results.BadRequest();
            var basketEvent = new BasketCheckoutEvent()
            {
                Country = basketCheckout.Country,
                Expiration = basketCheckout.Expiration,
                State = basketCheckout.State,
                AddressLine = basketCheckout.AddressLine,
                CardName = basketCheckout.CardName,
                UserName = basketCheckout.UserName,
                CardNumber = basketCheckout.CardName,
                EmailAddress = basketCheckout.EmailAddress,
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                PaymentMethod = basketCheckout.PaymentMethod,
                TotalPrice = basket.TotalPrice,
                ZipCode = basketCheckout.ZipCode,
                CVV = basketCheckout.CVV
            };
            // dispatch event 
            await publish.Publish(basketEvent);
            await repository.Delete(basketCheckout.UserName);
            
            return Results.Accepted();
        }).WithName("basketCheckout");
    }
}