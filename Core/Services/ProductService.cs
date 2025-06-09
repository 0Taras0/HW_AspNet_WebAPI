using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Model.Product;
using Core.Services;
using Domain.Data;
using Domain.Data.Entities;
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
            foreach (var ingId in model.IngredientIds!)
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

        public async Task<IEnumerable<ProductIngredientModel>> GetIngredientsAsync()
        {
            var ingredients = await context.Ingredients
                .ProjectTo<ProductIngredientModel>(mapper.ConfigurationProvider)
                .ToListAsync();
            return ingredients;
        }

        public async Task<IEnumerable<ProductSizeModel>> GetSizesAsync()
        {
            var sizes = await context.ProductSizes
                .ProjectTo<ProductSizeModel>(mapper.ConfigurationProvider)
                .ToListAsync();
            return sizes;
        }

        async Task<ProductItemModel> IProductService.Edit(ProductEditModel model)
        {
            var item = await context.Products
                .Where(x => x.Id == model.Id)
                .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            //Якщо фото немає у списку, то видаляємо його
            var imgDelete = item.ProductImages
                .Where(x => !model.ImageFiles!.Any(y => y.FileName == x.Name))
                .ToList();

            foreach (var img in imgDelete)
            {
                var productImage = await context.ProductImages
                    .Where(x => x.Id == img.Id)
                    .SingleOrDefaultAsync();
                if (productImage != null)
                {
                    await imageService.DeleteImageAsync(productImage.Name);
                    context.ProductImages.Remove(productImage);
                }
                context.SaveChanges();
            }

            short p = 0;
            //Перебираємо усі фото і їх зберігаємо або оновляємо
            foreach (var imgFile in model.ImageFiles!)
            {
                if (imgFile.ContentType == "old-image")
                {
                    var img = await context.ProductImages
                        .Where(x => x.Name == imgFile.FileName)
                        .SingleOrDefaultAsync();
                    img.Prority = p;
                    context.SaveChanges();
                }

                else
                {
                    try
                    {
                        var productImage = new ProductImageEntity
                        {
                            ProductId = item.Id,
                            Name = await imageService.SaveImageAsync(imgFile),
                            Prority = p
                        };
                        context.ProductImages.Add(productImage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Data for PRODUCT IMAGE", ex.Message);
                    }
                }

                p++;

            }


            await context.SaveChangesAsync();
            return item;
        }
    }
}
