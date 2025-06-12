using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Model.Cart;
using Domain.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class CartService(AppDbContext context, IAuthService authService, IMapper mapper) : ICartService
    {
        public async Task CreateUpdateAsync(CartCreateUpdateModel model)
        {
            var userId = await authService.GetUserId();
            var entity = context.Carts.SingleOrDefault(c => c.ProductId == model.ProductId && c.UserId == userId);
            if (entity != null)
            {
                entity.Quantity = model.Quantity;
            }
            else
            {
                entity = new CartEntity
                {
                    UserId = userId,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity
                };
                context.Carts.Add(entity);
            }
            await context.SaveChangesAsync();
        }

        public async Task<List<CartItemModel>> GetCartItemsAsync()
        {
            var userId = await authService.GetUserId();

            var items = await context.Carts
                .Where(c => c.UserId == userId)
                .ProjectTo<CartItemModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            return items;
        }
    }
}
