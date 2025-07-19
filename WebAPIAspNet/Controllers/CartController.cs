using Core.Interfaces;
using Core.Model.Cart;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRange([FromBody] List<CartCreateUpdateModel> modelItems)
        {
            foreach (var item in modelItems)
            {
                await cartService.CreateUpdateAsync(item);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var items = await cartService.GetCartItemsAsync();
            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCartItem(long id)
        {
            await cartService.DeleteAsync(id);
            return Ok();
        }
    }
}
