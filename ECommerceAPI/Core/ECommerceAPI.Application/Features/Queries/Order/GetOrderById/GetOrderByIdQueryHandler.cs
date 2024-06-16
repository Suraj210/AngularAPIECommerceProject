using ECommerceAPI.Application.Abstractions.Services;
using MediatR;

namespace ECommerceAPI.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
           var data = await _orderService.GetOrderByIdAsync(request.Id);

            return new GetOrderByIdQueryResponse()
            {
                Id = data.Id,
                OrderCode = data.OrderCode,
                Address = data.Address,
                Description = data.Description,
                CreatedTime = data.CreatedTime,
                BasketItems = data.BasketItems,
            };
        }
    }
}
