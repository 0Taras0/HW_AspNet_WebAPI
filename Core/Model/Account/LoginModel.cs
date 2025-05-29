namespace Core.Model.Account
{
    public class LoginModel
    {
        /// <summary>
        /// Електронна адреса користувача для входу в систему.
        /// </summary>
        /// <example>johndoe@example.com</example>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Пароль користувача для входу в систему.
        /// </summary>
        /// <example>Password123!</example>
        public string Password { get; set; } = string.Empty;
    }
}
