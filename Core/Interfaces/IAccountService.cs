using Core.Constants;
using Core.Model.Account;
using Core.Model.AdminUser;
using Core.Model.Category;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        Task<string> LoginByGoogle(string token);
        Task<AuthResult> LoginAsync(LoginModel model);
        Task<AuthResult> RegisterAsync(RegisterModel model);
        Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
        Task<bool> ValidateResetTokenAsync(ValidateResetTokenModel model);
        Task ResetPasswordAsync(ResetPasswordModel model);
        Task<AuthResult> UpdateAsync(AccountUpdateModel model);
    }
}
