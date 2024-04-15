using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly ILogger<DiscountService> _logger;
    private readonly IDiscountRepository _repository;

    public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName);
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound,
                $"discount with product name ={request.ProductName} "));
        return new CouponModel()
        {
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount,
            Id = coupon.Id
        };
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        await _repository.CreateDiscount(new Coupon()
        {
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount,
            Id = request.Coupon.Id
        }).ConfigureAwait(false);
        _logger.LogInformation("Discount is successfully created . ProductName : {ProductName}",
            request.Coupon.ProductName);
        return request.Coupon;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
        ServerCallContext context)
    {
        var deleted = await _repository.DeleteDiscount(request.ProductName);

        return new DeleteDiscountResponse()
        {
            Success = deleted
        };
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var success = await _repository.UpdateDiscount(new Coupon()
        {
            Id = request.Coupon.Id,
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount
        }).ConfigureAwait(false);
        if (!success)
            throw new InvalidOperationException("update discount is not happen");
        return request.Coupon;
    }
}