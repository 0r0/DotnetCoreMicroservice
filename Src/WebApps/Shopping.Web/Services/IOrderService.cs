using Shopping.Web.Models;

namespace Shopping.Web.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
}