using ECommerceAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        public ProductsController(IProductWriteRepository productWriteRepository
                                 ,IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]
        public async void Get()
        {
          await  _productWriteRepository.AddRangeAsync(new()
            {
                new(){Id=Guid.NewGuid(), Name = "Pro1", Price=100, CreatedDate=DateTime.UtcNow, Stock=10},
                new(){Id=Guid.NewGuid(), Name = "Pro2", Price=200, CreatedDate=DateTime.UtcNow, Stock=100},
                new(){Id=Guid.NewGuid(), Name = "Pro3", Price=1200, CreatedDate=DateTime.UtcNow, Stock=200},
                new(){Id=Guid.NewGuid(), Name = "Pro4", Price=500, CreatedDate=DateTime.UtcNow, Stock=30},
            });
            var count =  await _productWriteRepository.SaveChanges();
        }
    }
}
