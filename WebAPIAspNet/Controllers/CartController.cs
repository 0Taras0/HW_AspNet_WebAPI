using Core.Interfaces;
using Core.Model.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIAspNet.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUpdate([FromBody] CartCreateUpdateModel model)
        {
            var email = User.Claims.First().Value;

            await cartService.CreateUpdateAsync(model);

            return Ok(new { message = "Cart updated" });
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await cartService.GetCartItemsAsync();
            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await cartService.DeleteAsync(id);
            return Ok();
        }
    }
}
