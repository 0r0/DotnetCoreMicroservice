using Shopping.Web.Models;

namespace Shopping.Web.Services;

public class OrderService : IOrderService
{
    public Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
    {
        throw new NotImplementedException();
    }
}