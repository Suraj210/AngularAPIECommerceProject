using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Contexts;
using F= ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Persistence.Repositories
{
    public class ProductImageFileWriteRepository : WriteRepository<F::ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
        }

        
    }
}
