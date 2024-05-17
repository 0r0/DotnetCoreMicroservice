using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _client;

    public OrderService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
    {
       return await (await _client.GetAsync($"api/v{1}/Order/{userName}").ConfigureAwait(false))
            .ReadContentAs<IReadOnlyCollection<OrderResponseModel>>();
    }
}