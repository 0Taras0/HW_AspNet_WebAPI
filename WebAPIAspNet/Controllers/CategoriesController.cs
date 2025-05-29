using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Constants;
using Domain.Data;
using Domain.Data.Entities;
using Core.Interfaces;
using Core.Model.Category;

namespace Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(AppDbContext context, IMapper mapper, IImageService imageService, IValidator<CategoryCreateModel> validator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await mapper.ProjectTo<CategoryItemModel>(context.Categories).ToListAsync();

            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateModel model)
        {
            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.ImageFile!);
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await context.Categories.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await imageService.DeleteImageAsync(entity.Image);
            context.Categories.Remove(entity);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var model = await mapper
                .ProjectTo<CategoryItemModel>(context.Categories.Where(x => x.Id == id))
                .SingleOrDefaultAsync();
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateModel model)
        {
            var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existing == null)
            {
                return NotFound();
            }

            existing = mapper.Map(model, existing);

            if (model.ImageFile != null)
            {
                await imageService.DeleteImageAsync(existing.Image);
                existing.Image = await imageService.SaveImageAsync(model.ImageFile);
            }
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
