using AutoMapper;
using Core.Constants;
using Core.Interfaces;
using Core.Model.Account;
using Domain.Constants;
using Domain.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Core.Services
{
    public class AccountService(IJwtTokenService jwtTokenService, UserManager<UserEntity> userManager, IMapper mapper, IImageService imageService) : IAccountService
    {
        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await jwtTokenService.CreateTokenAsync(user);
                return AuthResult.SuccessResult(token);
            }

            return AuthResult.FailureResult("Invalid email or password");
        }

        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {
            var user = mapper.Map<UserEntity>(model);
            user.Image = await imageService.SaveImageAsync(model.ImageFile!);

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User);
                var token = await jwtTokenService.CreateTokenAsync(user);
                return AuthResult.SuccessResult(token);
            }

            return AuthResult.FailureResult("Registration failed");
        }
    }
}
