using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new NullReferenceException();
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        if (productName == null)
            throw new NullReferenceException("Product name can not be null");
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = (await connection.QueryFirstOrDefaultAsync<Coupon>("select * from Coupon where ProductName = @ProductName",
            param: new {ProductName = productName}).ConfigureAwait(false)) ;
        return coupon ?? new Coupon() {ProductName = "No Discount", Amount = 0, Description = "No discount"};
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection
            (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync
            ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount}).ConfigureAwait(false);

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection =
            new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await connection.ExecuteAsync
        ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
            new
            {
                ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount,
                Id = coupon.Id
            }).ConfigureAwait(false);

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
         await using var connection =
            new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
            new {ProductName = productName});

        if (affected == 0)
            return false;

        return true;
    }
}