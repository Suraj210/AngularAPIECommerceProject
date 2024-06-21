using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Order
{
    public class CompletedOrder_DTO
    {
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
    }
}
