using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly ILogger<BasketCheckoutConsumer> _logger;
    private readonly IMediator _mediator;

    public BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var checkoutCommand =CheckoutOrderCommandMap(context.Message);
       var result =await  _mediator.Send(checkoutCommand);
       _logger.LogInformation("Basket CheckoutEvent Consumed Successfully, created order Id {id}",result);
    }

    private static CheckoutOrderCommand CheckoutOrderCommandMap(BasketCheckoutEvent basketCheckoutEvent)
    {
        return new CheckoutOrderCommand()
        {
            Country = basketCheckoutEvent.Country,
            Expiration = basketCheckoutEvent.Expiration,
            State = basketCheckoutEvent.State,
            AddressLine = basketCheckoutEvent.AddressLine,
            CardName = basketCheckoutEvent.CardName,
            CardNumber = basketCheckoutEvent.CardNumber,
            EmailAddress = basketCheckoutEvent.EmailAddress,
            FirstName = basketCheckoutEvent.FirstName,
            LastName = basketCheckoutEvent.LastName,
            PaymentMethod = basketCheckoutEvent.PaymentMethod,
            TotalPrice = basketCheckoutEvent.TotalPrice,
            UserName = basketCheckoutEvent.UserName,
            ZipCode = basketCheckoutEvent.ZipCode,
            CVV = basketCheckoutEvent.CVV
        };
    }
}