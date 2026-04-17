// Аутентификация - установление факта личности. Результат: токен
// Авторизация - проверка доступа. Результат: одобрение или отклонение.
using WebApplication1.Domain;
using WebApplication1.Repository;

namespace WebApplication1.Service
{
    public interface IAuthService
    {
        Task<string?> Authenticate(LoginRequest request);
        Task<bool> Register(LoginRequest request);
    }
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;
        private readonly IJWTService _jWTService;

        public AuthService (IUserRepo userRepo, IJWTService jWTService)
        {
            _userRepo = userRepo;
            _jWTService = jWTService;
        }

        public async Task<string?> Authenticate(LoginRequest request)
        {
            try
            {
                var user = await _userRepo.GetUser(request.Login);
                if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.Password))
                    return null;
                return _jWTService.GenerateToken(user);

            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Register(LoginRequest request)
        {
            try
            {
                var hashedPassword = PasswordHasher.HashPassword(request.Password);
                var user = await _userRepo.AddUser(request.Login, hashedPassword);
                if (user == null)
                {
                    return false;
                }
                else return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
