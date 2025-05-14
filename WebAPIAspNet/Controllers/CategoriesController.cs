using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAspNet.Data;
using WebAPIAspNet.Data.Entities;
using WebAPIAspNet.Interfaces;
using WebAPIAspNet.Model.Category;

namespace WebAPIAspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(AppDbContext context, IMapper mapper, IImageService imageService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await mapper.ProjectTo<CategoryItemModel>(context.Categories).ToListAsync();

            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = mapper.Map<CategoryEntity>(model);
            entity.Image = await imageService.SaveImageAsync(model.Image);
            context.Categories.Add(entity);
            await context.SaveChangesAsync();
            return Ok(new { entity.Id, entity.Name, entity.Slug, entity.Image});
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
    }
}
