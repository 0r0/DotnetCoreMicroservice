using Asp.Versioning.Builder;
using Discount.API.Entities;
using Discount.API.Repositories;

namespace Discount.API;

public static class EndPoints
{
    public static void DiscountRoutes(this WebApplication application, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application.MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
        group.MapGet("/Discount/{productName}", async (string productName, IDiscountRepository repository) =>
            await repository.GetDiscount(productName
            )).WithName("GetDiscount");
        group.MapPost("/Discount", async (Coupon coupon, IDiscountRepository repository) =>
        {
            await repository.CreateDiscount(coupon);
            return Results.CreatedAtRoute("GetDiscount", new {productName = coupon.ProductName});
        });

        group.MapPut("/Discount/{id}", async (int id, Coupon coupon, IDiscountRepository repository)
            => Results.Ok(await repository.UpdateDiscount(coupon: coupon)));

        group.MapDelete("/Discount/{ProductName}", async (string productName, IDiscountRepository repository)
            => Results.Ok(await repository.DeleteDiscount(productName)));
    }
}