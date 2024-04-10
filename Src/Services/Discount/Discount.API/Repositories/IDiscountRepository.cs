using Discount.API.Entities;

namespace Discount.API.Repositories;

public interface IDiscountRepository
{
    Task<Coupon> GetDiscount(string productName);
    Task<bool> CreateDiscount(Coupon coupon);
    public Task<bool> UpdateDiscount(Coupon coupon);
    public Task<bool> DeleteDiscount(string productName);
}