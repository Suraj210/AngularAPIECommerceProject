using ECommerceAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrder_DTO createOrder_DTO);
        Task<ListOrder_DTO> GetAllOrdersAsync(int page, int size);
    }
}
