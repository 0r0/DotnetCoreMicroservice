using Shopping.Web.Extensions;
using Shopping.Web.Models;

namespace Shopping.Web.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
    {
        var response = await _httpClient.GetAsync($"/Order/{userName}").ConfigureAwait(false);
        return response.IsSuccessStatusCode
            ? await response.ReadContentAs<IEnumerable<OrderResponseModel>>()
            : throw new Exception("sth get wrong in calling api");
    }
}