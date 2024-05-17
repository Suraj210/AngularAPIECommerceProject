using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Contexts;
using F= ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Persistence.Repositories.ProductImageFile
{
    public class ProductImageFileReadRepository : ReadRepository<F::ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
