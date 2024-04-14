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
        return await base.CreateDiscount(request, context);
    }
}