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
                entity = mapper.Map<CartEntity>(model);
                entity.UserId = userId;

                context.Carts.Add(entity);
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var userId = await authService.GetUserId();
            var item = await context.Carts
                .SingleOrDefaultAsync(x => x.UserId == userId && x.ProductId == id);
            if (item != null)
            {
                context.Carts.Remove(item);
                await context.SaveChangesAsync();
            }
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
