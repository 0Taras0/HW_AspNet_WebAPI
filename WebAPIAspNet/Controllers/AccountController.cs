using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Model.Account;

namespace Domain.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await accountService.LoginAsync(model);
            if (result.Success)
                return Ok(new { Token = result.Token });

            return Unauthorized(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var result = await accountService.RegisterAsync(model);
            if (result.Success)
                return Ok(new { Token = result.Token });

            return BadRequest(new
            {
                status = 400,
                isValid = false,
                errors = result.ErrorMessage
            });
        }
    }
}
