using ECommerceAPI.Application.DTOs.Order;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrder_DTO createOrder_DTO);
        Task<ListOrder_DTO> GetAllOrdersAsync(int page, int size);
        Task<SingleOrder_DTO> GetOrderByIdAsync(string id);
        Task<(bool,CompletedOrder_DTO)> CompleteOrderAsync(string id);
    }
}
