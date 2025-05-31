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

        [Authorize(Roles = $"{Roles.Admin}")]
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
    }
}
