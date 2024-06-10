using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Order
{
    public class ListOrder_DTO
    {
        public int  TotalOrderCount { get; set; }
        public object Orders { get; set; }
    }
}
