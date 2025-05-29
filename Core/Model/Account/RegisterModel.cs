using Microsoft.AspNetCore.Http;

namespace Core.Model.Account
{
    public class RegisterModel
    {
        /// <summary>
        /// Ім'я користувача
        /// </summary>
        /// <example></example>
        public string FirstName { get; set; } = String.Empty;

        /// <summary>
        /// Прізвище користувача
        /// </summary>
        /// <example></example>
        public string LastName { get; set; } = String.Empty;

        /// <summary>
        /// Електронна пошта користувача
        /// </summary>
        /// <example></example>
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// Пароль пошта користувача
        /// </summary>
        /// <example></example>
        public string Password { get; set; } = String.Empty;
        public IFormFile? ImageFile { get; set; } = null;
    }
}
