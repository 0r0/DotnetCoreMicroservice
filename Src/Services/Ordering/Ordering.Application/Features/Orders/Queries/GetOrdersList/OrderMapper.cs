using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entity;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public static class OrderMapper
{
    public static IReadOnlyCollection<OrdersVm> Map(this IEnumerable<Order> orders)
    {
        return orders.Select(a => new OrdersVm()
        {
            UserName = a.UserName,
            Country = a.Country,
            Expiration = a.Expiration,
            Id = a.Id,
            State = a.State,
            AddressLine = a.AddressLine,
            CardName = a.CardName,
            CardNumber = a.CardName,
            EmailAddress = a.EmailAddress,
            FirstName = a.FirstName,
            LastName = a.LastName,
            PaymentMethod = a.PaymentMethod,
            TotalPrice = a.TotalPrice,
            ZipCode = a.ZipCode,
            CVV = a.CVV
        }).ToList().AsReadOnly();
    }

    public static Order Map(this CheckoutOrderCommand command)
    {
        return new Order()
        {
            UserName = command.UserName,
            Country = command.Country,
            Expiration = command.Expiration,
            State = command.State,
            AddressLine = command.AddressLine,
            CardName = command.CardName,
            CardNumber = command.CardName,
            EmailAddress = command.EmailAddress,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PaymentMethod = command.PaymentMethod,
            TotalPrice = command.TotalPrice,
            ZipCode = command.ZipCode,
            CVV = command.CVV
        };
    }

    public static Order Map(this UpdateOrderCommand command)
    {
        return new Order()
        {
            UserName = command.UserName,
            Country = command.Country,
            Expiration = command.Expiration,
            State = command.State,
            AddressLine = command.AddressLine,
            CardName = command.CardName,
            CardNumber = command.CardName,
            EmailAddress = command.EmailAddress,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PaymentMethod = command.PaymentMethod,
            TotalPrice = command.TotalPrice,
            ZipCode = command.ZipCode,
            CVV = command.CVV
        };
    }
}