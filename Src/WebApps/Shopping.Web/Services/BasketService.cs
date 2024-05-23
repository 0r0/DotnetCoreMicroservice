using Shopping.Web.Extensions;
using Shopping.Web.Models;

namespace Shopping.Web.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _client;

    public BasketService(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<BasketModel> GetBasket(string userName)
    {
        var response = await _client.GetAsync($"/Basket/{userName}")
            .ConfigureAwait(false);
        return response.IsSuccessStatusCode
            ? await response.ReadContentAs<BasketModel>()
            : throw new Exception("sth get wrong in calling api");
    }

    public async Task<BasketModel> UpdateBasket(BasketModel model)
    {
        var response = await _client.PutAsJson("/Basket", model);

        return response.IsSuccessStatusCode
            ? await response.ReadContentAs<BasketModel>()
            : throw new Exception("Something went wrong when calling api.");
    }

    public async Task CheckoutBasket(BasketCheckoutModel model)
    {
        var response = await _client.PostAsJson("/Basket/Checkout", model);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling api.");
        }
    }
}