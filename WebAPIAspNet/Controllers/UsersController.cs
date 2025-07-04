﻿using Core.Interfaces;
using Core.Model.Search.Params;
using Core.Model.Seeder;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIAspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : Controller
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            var model = await userService.GetAllUsersAsync();

            return Ok(model);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] UserSearchModel model)
        {
            var result = await userService.SearchUsersAsync(model);
            return Ok(result);
        }

        [HttpGet("seed")]
        public async Task<IActionResult> SeedUsers([FromQuery] SeedItemsModel model)
        {
            var result = await userService.SeedUsersAsync(model);
            return Ok(result);
        }
    }
}
