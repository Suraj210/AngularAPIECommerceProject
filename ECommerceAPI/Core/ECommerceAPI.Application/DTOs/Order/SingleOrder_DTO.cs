using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Order
{
    public class SingleOrder_DTO
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public DateTime CreatedTime { get; set; }
        public object BasketItems { get; set; }
    }
}
