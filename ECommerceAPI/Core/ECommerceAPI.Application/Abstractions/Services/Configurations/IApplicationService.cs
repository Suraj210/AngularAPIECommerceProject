using ECommerceAPI.Application.DTOs.Configurations;

namespace ECommerceAPI.Application.Abstractions.Services.Configurations
{
    public interface IApplicationService
    {
        List<Menu> GetAuthorizeDefinitionEndpoints(Type assemblyType);
    }
}
