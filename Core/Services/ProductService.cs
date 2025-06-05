using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Model.Product;
using Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class ProductService(IMapper mapper, AppDbContext context) : IProductService
    {
        public async Task<ProductItemModel> GetById(int id)
        {
            try
            {
                var model = await context.Products
                    .Where(p => p.Id == id)
                    .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }

        }

        public async Task<List<ProductItemModel>> GetBySlug(string slug)
        {
            try
            {
                var model = await context.Products
                    .Where(p => p.Slug == slug)
                    .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<ProductItemModel>();
            }
        }

        public async Task<List<ProductItemModel>> List()
        {
            try
            {
                return await context.Products
                    .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<ProductItemModel>();
            }
        }
    }
}
