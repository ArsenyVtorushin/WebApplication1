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
        // Аутентификация - установление факта личности. Результат: токен
        // Авторизация - проверка доступа. Результат: одобрение или отклонение.
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
