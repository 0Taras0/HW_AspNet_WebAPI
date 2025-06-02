namespace Core.Constants
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }

        public static AuthResult SuccessResult(string token)
        {
            return new AuthResult
            {
                Success = true,
                Token = token
            };
        }

        public static AuthResult FailureResult(string error)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = error
            };
        }
    }

}
