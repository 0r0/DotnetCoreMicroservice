using Discount.API.Entities;

namespace Discount.API.Repositories;

public interface IDiscountRepository
{
    Task<Coupon> GetDiscount(string productName);
    Task<bool> Create(Coupon coupon);
    public Task<bool> Update(Coupon coupon);
    public Task<bool> Delete(string productName);
}