using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Interfaces;
using Core.Model.Product;
using Core.Model.Product.Ingredient;
using Core.Model.Search;
using Core.Model.Search.Params;
using Domain.Data;
using Domain.Entities;
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
                    .Where(p => !p.IsDeleted)
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

        public async Task<ProductItemModel> Edit(ProductEditModel model)
        {
            var entity = await context.Products
                .Where(x => x.Id == model.Id)
                .SingleOrDefaultAsync();

            mapper.Map(model, entity);

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

            var existingIngredients = context.ProductIngredients
                .Where(pi => pi.ProductId == item.Id);

            context.ProductIngredients.RemoveRange(existingIngredients);

            if (model.IngredientIds != null)
            {
                foreach (var ingredientId in model.IngredientIds.Distinct())
                {
                    var newIngredient = new ProductIngredientEntity
                    {
                        ProductId = item.Id,
                        IngredientId = ingredientId
                    };
                    context.ProductIngredients.Add(newIngredient);
                }
            }


            await context.SaveChangesAsync();
            return item;
        }

        public async Task<ProductIngredientModel> UploadIngredient(CreateIngredientModel model)
        {
            var entity = mapper.Map<IngredientEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.ImageFile!);
            context.Ingredients.Add(entity);
            await context.SaveChangesAsync();

            return mapper.Map<ProductIngredientModel>(entity);
        }

        public async Task Delete(long id)
        {
            var product = await context.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            product.IsDeleted = true;

            await context.SaveChangesAsync();
        }

        async Task<ProductSearchResult> IProductService.SearchProductsAsync(ProductSearchModel model)
        {
            var query = context.Products.AsQueryable();

            query = query.Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(p => p.Name.Contains(model.Name));
            }

            if (!string.IsNullOrEmpty(model.Slug))
            {
                query = query.Where(p => p.Slug == model.Slug);
            }

            if (model.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == model.CategoryId.Value);
            }

            if (model.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= model.MinPrice.Value);
            }

            if (model.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= model.MaxPrice.Value);
            }

            var totalCount = await query.CountAsync();

            var safeItemsPerPage = model.ItemPerPage < 1 ? 10 : model.ItemPerPage;
            var totalPages = (int)Math.Ceiling(totalCount / (double)safeItemsPerPage);
            var safePage = Math.Min(Math.Max(1, model.Page), Math.Max(1, totalPages));

            int skip = (safePage - 1) * safeItemsPerPage;

            var result = await query
                .Skip(skip)
                .Take(safeItemsPerPage)
                .ProjectTo<ProductItemModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            var allProductsQuery = context.Products.Where(p => !p.IsDeleted);

            decimal minPrice = await allProductsQuery.AnyAsync() ? await allProductsQuery.MinAsync(p => p.Price) : 0;
            decimal maxPrice = await allProductsQuery.AnyAsync() ? await allProductsQuery.MaxAsync(p => p.Price) : 0;

            return new ProductSearchResult
            {
                Items = result,
                Pagination = new PaginationModel
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    ItemsPerPage = safeItemsPerPage,
                    CurrentPage = safePage
                },
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };
        }
    }
}
