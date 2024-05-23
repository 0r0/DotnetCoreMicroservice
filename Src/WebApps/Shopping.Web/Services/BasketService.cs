using Shopping.Web.Models;

namespace Shopping.Web.Services;

public class BasketService : IBasketService
{
    public Task<BasketModel> GetBasket(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<BasketModel> UpdateBasket(BasketModel model)
    {
        throw new NotImplementedException();
    }

    public Task CheckoutBasket(BasketCheckoutModel model)
    {
        throw new NotImplementedException();
    }
}