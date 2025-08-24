using AutoMapper;
using Core.Interfaces;
using Core.Model.Category;
using Core.Services;
using Domain.Constants;
using Domain.Data;
using Domain.Data.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await categoryService.ListAsync();

            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateModel model)
        {
            var entity = await categoryService.CreateAsync(model);
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await categoryService.DeleteAsync(id);
            return NoContent();
        }

        //[Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var model = await categoryService.GetItemByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateModel model)
        {
            var entity = await categoryService.UpdateAsync(model);

            return Ok(entity);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] CategorySearchModel searchModel)
        {
            try
            {
                var model = await categoryService.ListAsync(searchModel);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    invalid = ex.Message
                });
            }
        }

        [HttpGet("category-name/{id}")]
        public async Task<IActionResult> GetCategoryNameById(int id)
        {
            var categoryName = await categoryService.GetCategoryNameById(id);
            return Ok(categoryName);
        }
    }
}
