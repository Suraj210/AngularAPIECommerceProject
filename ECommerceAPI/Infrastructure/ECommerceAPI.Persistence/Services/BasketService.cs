﻿using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.ViewModels.Baskets;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Application.Repositories;

namespace ECommerceAPI.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _contextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;

        public BasketService(IHttpContextAccessor contextAccessor,
                             UserManager<AppUser> userManager,
                             IOrderReadRepository orderReadRepository,
                             IBasketWriteRepository basketWriteRepository,
                             IBasketItemWriteRepository basketItemWriteRepository,
                             IBasketItemReadRepository basketItemReadRepository,
                             IBasketReadRepository basketReadRepository)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var username = _contextAccessor?.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
                AppUser? user = await _userManager.Users
                       .Include(u => u.Baskets)
                       .FirstOrDefaultAsync(u => u.UserName == username);

                var _basket = from basket in user.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order
                              };


                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))
                {
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }

                await _basketWriteRepository.SaveAsync();
                return targetBasket;
            }

            throw new Exception("Something went wrong!");
        }
        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
                BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));


                if (_basketItem != null)
                {
                    _basketItem.Quantity++;
                }
                else
                {
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity,
                    });
                }

                await _basketItemWriteRepository.SaveAsync();

            }

        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
                Basket? result = await _basketReadRepository.Table
                        .Include(b => b.BasketItems)
                        .ThenInclude(bi => bi.Product)
                        .FirstOrDefaultAsync(b => b.Id == basket.Id);

                return result.BasketItems.ToList();
            }

            throw new Exception("Basket is null!");
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if (_basketItem != null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public Basket? GetUserActiveBasket
        {
            get
            {
                Basket? basket = ContextUser().Result;
                return basket;
            }
        }
    }
}
