namespace WebAPIAspNet.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> CreateTokenAsync();
    }
}
