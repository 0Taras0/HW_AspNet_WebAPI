using Core.Constants;
using Core.Model.Account;
using Core.Model.Category;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        Task<AuthResult> LoginAsync(LoginModel model);
        Task<AuthResult> RegisterAsync(RegisterModel model);
    }
}
