using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.API;

public static class EndPoints
{
    public static void OrderEndPoints(this WebApplication application, ApiVersionSet apiVersionSet)
    {
        RouteGroupBuilder group = application.MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);
        group.MapGet("/Order/{userName}", async (IMediator mediator,string userName) =>
        {
            var query = new GetOrdersListQuery(userName);
            var order = await mediator.Send(query);
            return Results.Ok(order);
        }).WithName("GetOrder");

        group.MapPost("/Order", async (IMediator mediator,CheckoutOrderCommand command) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("CheckOutOrder");
        group.MapPut("/Order", async (IMediator mediator,UpdateOrderCommand command) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateOrder");
        group.MapDelete("/Order", async (IMediator mediator,[FromBody]DeleteOrderCommand command) =>
        {
            await mediator.Send(command);
            Results.NoContent();
        }).WithName("DeleteOrder");
        
        group.MapDelete("/Order/{id}", async ( IMediator mediator,int id) =>
        {
            await mediator.Send(new DeleteOrderCommand(id)).ConfigureAwait(false);
            Results.NoContent();
        
        }).WithName("DeleteOrderById");
    }
}