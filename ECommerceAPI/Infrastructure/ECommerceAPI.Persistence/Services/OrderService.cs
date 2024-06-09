using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.Order;
using ECommerceAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task CreateOrderAsync(CreateOrder_DTO createOrder_DTO)
        {
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder_DTO.Address,
                Id = Guid.Parse(createOrder_DTO.BasketId),
                Description = createOrder_DTO.Description,
            });

            await _orderWriteRepository.SaveAsync();    
        }
    }
}
