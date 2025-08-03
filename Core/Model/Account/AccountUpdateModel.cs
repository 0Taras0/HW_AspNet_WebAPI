using Microsoft.AspNetCore.Http;

namespace Core.Model.Account
{
    public class AccountUpdateModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; } = null;
        public string Password { get; set; } = string.Empty;
    }
}
