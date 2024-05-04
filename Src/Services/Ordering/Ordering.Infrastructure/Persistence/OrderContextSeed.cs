using Microsoft.Extensions.Logging;
using Ordering.Domain.Entity;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            await orderContext.Orders.AddRangeAsync(PreConfiguredOrder());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("seed database associated by {dbContext}",nameof(OrderContext));
        }
    }

    private static Order[] PreConfiguredOrder()
    {
        return new[]
        {
            new Order()
            {
                Country = "Iran",
                Expiration = "Unknown",
                State = "Tehran",
                AddressLine = "Tehran.AmirAbad",
                CardName = "Mehdi",
                CardNumber = "1234",
                FirstName = "Mehdi",
                EmailAddress = "Mehdi.goharinezhad@gmail.com",
                LastName = "Goharinezhad",
                ZipCode = "1234",
                PaymentMethod = 1,
                TotalPrice = 1000000,
                UserName = "swn",
                CVV = "313"
            }
        };
    }
}