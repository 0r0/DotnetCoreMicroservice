using Asp.Versioning.Builder;
using Discount.API.Entities;
using Discount.API.Repositories;

namespace Discount.API;

public static class EndPoints
{
    public static void DiscountRoutes(this WebApplication application,ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application.MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
        group.MapGet("/Discount/{productName}", async (string productName,IDiscountRepository repository)=> await repository.GetDiscount(productName
        ));
        group.MapPost("/Discount", async (Coupon coupon, IDiscountRepository repository) =>
        {
            await repository.CreateDiscount(coupon);
            return Results.Created("id", coupon.Id);
        });

        group.MapPut("/Discount/{id}", async (int id, Coupon coupon, IDiscountRepository repository) 
            => Results.Ok((object?) await repository.UpdateDiscount(coupon: coupon)));

        group.MapDelete("/Discount/{ProductName}", async (string productName, IDiscountRepository repository)
            => Results.Ok((object?) await repository.DeleteDiscount(productName)));
    }
}