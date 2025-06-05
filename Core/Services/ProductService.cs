using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Model.Product;
using Domain.Data;
using Domain.Entities;
using MailKit;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class ProductService(IMapper mapper, AppDbContext context, IImageService imageService) : IProductService
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

        public async Task<ProductEntity> Create(ProductCreateModel model)
        {
            var entity = mapper.Map<ProductEntity>(model);
            context.Products.Add(entity);
            await context.SaveChangesAsync();
            foreach (var ingId in model.ProductIngredientsId!)
            {
                var productIngredient = new ProductIngredientEntity
                {
                    ProductId = entity.Id,
                    IngredientId = ingId
                };
                context.ProductIngredients.Add(productIngredient);
            }
            await context.SaveChangesAsync();


            for (short i = 0; i < model.ImageFiles!.Count; i++)
            {
                try
                {
                    var productImage = new ProductImageEntity
                    {
                        ProductId = entity.Id,
                        Name = await imageService.SaveImageAsync(model.ImageFiles[i]),
                        Prority = i
                    };
                    context.ProductImages.Add(productImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data for PRODUCT IMAGE", ex.Message);
                }
            }
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
