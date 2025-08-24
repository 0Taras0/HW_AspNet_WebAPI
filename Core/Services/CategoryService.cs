using AutoMapper;
using Core.Interfaces;
using Core.Model.Category;
using Core.Model.Pagination;
using Domain.Data;
using Domain.Data.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class CategoryService(AppDbContext context, IMapper mapper, IImageService imageService) : ICategoryService
    {
        public async Task<CategoryItemModel> CreateAsync(CategoryCreateModel model)
        {
            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.ImageFile!);
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();
            var createdModel = mapper.Map<CategoryItemModel>(entity);
            return createdModel;
        }

        public async Task<CategoryItemModel> DeleteAsync(long id)
        {
            var entity = await context.Categories.FindAsync(id);

            entity!.IsDeleted = true;

            await context.SaveChangesAsync();
            var deletedModel = mapper.Map<CategoryItemModel>(entity);
            return deletedModel;
        }

        public async Task<string> GetCategoryNameById(int id)
        {
            var category = await context.Categories
                .Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
            return category;
        }

        public async Task<CategoryItemModel?> GetItemByIdAsync(int id)
        {
            var model = await mapper
                .ProjectTo<CategoryItemModel>(context.Categories.Where(x => x.Id == id && !x.IsDeleted))
                .SingleOrDefaultAsync();
            return model;
        }

        public async Task<List<CategoryItemModel>> ListAsync()
        {
            var model = await mapper.ProjectTo<CategoryItemModel>(context.Categories.Where(c => !c.IsDeleted)).ToListAsync();
            return model;
        }

        public async Task<PaginationModel<CategoryItemModel>> ListAsync(CategorySearchModel searchModel)
        {
            int page = searchModel.CurrentPage;
            int pageSize = searchModel.PageSize;
            var query = context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchModel.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchModel.CategorySlug))
            {
                query = query.Where(x => x.Slug.ToLower().Contains(searchModel.CategorySlug.ToLower()));
            }
            var model = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => mapper.Map<CategoryItemModel>(x))
                .ToListAsync();
            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);

            var paginationModel = new PaginationModel<CategoryItemModel>
            {
                Items = model,
                TotalPages = pages,
                CurrentPage = page,
                PageSize = searchModel.PageSize
            };

            return paginationModel;
        }

        public async Task<CategoryItemModel> UpdateAsync(CategoryUpdateModel model)
        {
            var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

            existing = mapper.Map(model, existing);

            if (model.ImageFile != null)
            {
                await imageService.DeleteImageAsync(existing.Image);
                existing.Image = await imageService.SaveImageAsync(model.ImageFile);
            }
            await context.SaveChangesAsync();
            var updateModel = mapper.Map<CategoryItemModel>(existing);
            return updateModel;
        }
    }
}
