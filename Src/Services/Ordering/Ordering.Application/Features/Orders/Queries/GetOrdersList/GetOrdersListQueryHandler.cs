using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery,List<OrdersVm>>
{
    public Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}