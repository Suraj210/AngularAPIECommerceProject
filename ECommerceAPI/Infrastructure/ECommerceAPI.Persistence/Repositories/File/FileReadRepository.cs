using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Contexts;
using F = ECommerceAPI.Domain.Entities;


namespace ECommerceAPI.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<F::File>, IFileReadRepository
    {
        public FileReadRepository(ECommerceAPIDbContext context) : base(context)
        {
        }
    }
}
