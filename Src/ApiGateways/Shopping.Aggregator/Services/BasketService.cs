using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _client;

    public BasketService(HttpClient client)
    {
        _client = client;
    }

    public async Task<BasketModel> GetBasket(string userName)
    {
       return await (await _client.GetAsync($"api/v{1}/Basket/{userName}")
            .ConfigureAwait(false)).ReadContentAs<BasketModel>();
    }
}