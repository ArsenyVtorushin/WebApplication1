using Microsoft.AspNetCore.Identity.Data;

namespace WebApplication1.Service
{
    public interface IAuthService
    {
        Task<string?> Authenticate(LoginRequest request);
        Task<bool> Register(LoginRequest request);
    }
    public class AuthService : IAuthService
    {
        // Авторизация - уста
        public async Task<string?> Authenticate(LoginRequest request)
        {
            return "";
        }

        public async Task<bool> Register(LoginRequest request)
        {
            return true;
        }
    }
}
