using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Application.Models;
using Ordering.Domain.Entity;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandlers : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandlers> _logger;
    private readonly IOrderRepository _orderRepository;

    public CheckoutOrderCommandHandlers(IOrderRepository orderRepository, IEmailService emailService,
        ILogger<CheckoutOrderCommandHandlers> logger)
    {
        _orderRepository = orderRepository;
        _emailService = emailService;
        _logger = logger;
    }


    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = request.Map();
        var order = await _orderRepository.AddAsync(orderEntity).ConfigureAwait(false);

        _logger.LogInformation("order with id: {orderId} has been created", order.Id);

        await SendEmail(order);

        return order.Id;
    }


    #region helper

    private async Task SendEmail(Order order)
    {
        try
        {
            await _emailService.SendEmail(new Email()
            {
                Body = order.ToString(),
                Subject = default,
                To = default
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError("order with id: {orderId} has been failed due to an error with the mail service",
                order.Id);
        }
    }

    #endregion
}