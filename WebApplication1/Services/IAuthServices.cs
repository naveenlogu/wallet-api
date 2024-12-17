namespace WebApplication1.Services
{
    public interface IAuthServices
    {
        Task<bool> ValidateUser(string username, string password);
        string GenerateJwtToken(string username);
    }
}
