using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entity;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if(order is null)
        {

            throw new NotFoundException(nameof(Order), request.Id);
            _logger.LogError("order does not exist in database");
        }
        
        await _orderRepository.UpdateAsync(request.Map());
        
        _logger.LogInformation("order with {id} is updated successfully",order.Id);



    }
}