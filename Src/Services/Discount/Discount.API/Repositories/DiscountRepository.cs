using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new NullReferenceException();
        using var connection = new NpgsqlConnection(configuration.GetValue<string>("ConnectionStrings"));

    }

    public Task<Coupon> GetDiscount(string productName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Create(Coupon coupon)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Coupon coupon)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(string productName)
    {
        throw new NotImplementedException();
    }
}