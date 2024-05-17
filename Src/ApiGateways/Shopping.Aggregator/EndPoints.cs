using System.Collections.Immutable;
using Asp.Versioning.Builder;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator;

public static class EndPoints
{
    public static void ShoppingAggregatorRoutes(this WebApplication application, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application.MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
        group.MapGet("/shopping/{userName}", async (string userName, ICatalogService catalogService,
                IBasketService basketService, IOrderService orderService) =>
            {
                var basket = await basketService.GetBasket(userName);
                foreach (var basketItem in basket.Items)
                {
                    var product = await catalogService.GetCatalog(basketItem.ProductId);
                    basketItem.ProductName = product.Name;
                    basketItem.Category = product.Category;
                    basketItem.Description = product.Description;
                    basketItem.Summary = product.Summary;
                    basketItem.ImageFile = product.ImageFile;

                }

                var orders = await orderService.GetOrdersByUserName(userName);

                 var shoppingModel = new ShoppingModel()
                {
                    Orders = orders.ToImmutableList(),
                    UserName = userName,
                    BasketWithProducts = basket

                };
                return Results.Ok(shoppingModel);
            }
        );
    }
}