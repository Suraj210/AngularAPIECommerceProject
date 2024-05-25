using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Infrastructure.Enums;
using ECommerceAPI.Infrastructure.Services.Storage;
using ECommerceAPI.Infrastructure.Services.Storage.Azure;
using ECommerceAPI.Infrastructure.Services.Storage.Local;
using ECommerceAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        }
        public static void AddStorage<T>(this IServiceCollection serviveCollection) where T : Storage, IStorage
        {

            serviveCollection.AddScoped<IStorage, T>();

        }

        public static void AddStorage(this IServiceCollection serviveCollection, StorageType storageType)
        {

            switch (storageType)
            {
                case StorageType.Local:
                    serviveCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviveCollection.AddScoped<IStorage, AzureStorage>();

                    break;
                case StorageType.AWS:
                    break;
                default:
                    serviveCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }

        }
    }
}
