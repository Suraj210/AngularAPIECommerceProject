using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Persistence.Contexts;
using F = ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<F::File>, IFileWriteRepository
    {
        public FileWriteRepository(ECommerceAPIDbContext context) : base(context)
        {
            
        }
    }
}
