using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Model.Account;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestModel model)
        {
            string result = await accountService.LoginByGoogle(model.Token);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest(new
                {
                    Status = 400,
                    IsValid = false,
                    Errors = new { Email = "Помилка реєстрації" }
                });
            }
            return Ok(new
            {
                Token = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            bool res = await accountService.ForgotPasswordAsync(model);
            if (res)
                return Ok();
            else
                return BadRequest(new
                {
                    Status = 400,
                    IsValid = false,
                    Errors = new { Email = "Користувача з такою поштою не існує" }
                });
        }

        [HttpGet]
        public async Task<IActionResult> ValidateResetToken([FromQuery] ValidateResetTokenModel model)
        {
            bool res = await accountService.ValidateResetTokenAsync(model);
            return Ok(new { IsValid = res });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            await accountService.ResetPasswordAsync(model);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] AccountUpdateModel model)
        {
            var result = await accountService.UpdateAsync(model);
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
