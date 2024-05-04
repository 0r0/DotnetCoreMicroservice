using Asp.Versioning.Builder;
using MediatR;
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
        group.MapGet("/Order/{userName}", async (string userName, IMediator mediator) =>
        {
            var query = new GetOrdersListQuery(userName);
            var order = await mediator.Send(query);
            return Results.Ok(order);
        }).WithName("GetOrder");

        group.MapPost("/Order", async (CheckoutOrderCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).WithName("CheckOutOrder");
        group.MapPut("/Order", async (UpdateOrderCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateOrder");
        group.MapDelete("/Order", async (DeleteOrderCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            Results.NoContent();
        }).WithName("DeleteOrder");

        group.MapDelete("/Order/{id}", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteOrderCommand(id)).ConfigureAwait(false);
            Results.NoContent();

        }).WithName("DeleteOrder");
    }
}