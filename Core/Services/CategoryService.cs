using AutoMapper;
using Core.Interfaces;
using Core.Model.Category;
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

            await imageService.DeleteImageAsync(entity.Image);
            context.Categories.Remove(entity);
            await context.SaveChangesAsync();
            var deletedModel = mapper.Map<CategoryItemModel>(entity);
            return deletedModel;
        }

        public async Task<CategoryItemModel?> GetItemByIdAsync(int id)
        {
            var model = await mapper
                .ProjectTo<CategoryItemModel>(context.Categories.Where(x => x.Id == id))
                .SingleOrDefaultAsync();
            return model;
        }

        public async Task<List<CategoryItemModel>> ListAsync()
        {
            var model = await mapper.ProjectTo<CategoryItemModel>(context.Categories).ToListAsync();
            return model;
        }

        public async Task<CategoryItemModel> UpdateAsync(CategoryUpdateModel model)
        {
            var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);

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
