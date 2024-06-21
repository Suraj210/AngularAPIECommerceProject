using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using MediatR;

namespace ECommerceAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        readonly IOrderService _orderService;
        readonly IMailService _mailService;
        public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            (bool succedded, CompletedOrder_DTO dto) result = await _orderService.CompleteOrderAsync(request.Id);
            if (result.succedded)
            {
                await _mailService.SendCompletedOrderMailAsync(result.dto.Email, result.dto.OrderCode,
                    result.dto.OrderDate, result.dto.UserSurname);
            }
            return new CompleteOrderCommandResponse();
        }
    }
}
