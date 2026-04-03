/*
                                  JWT (JSON Web Token)

    стандарт компактного и самодостаточного 
    способа безопасной передачи информации между сторонами в 
    виде JSON-объекта

    Состоит из 3 частей: Header.Payload.Signature разделенных точками
     - Header - метаданные (тип токена, алгоритм подписки)
     - Payload - полезная нагрузка. 
        Содержит утверждения (Claims) о пользователе, 
        т.е. хранятся сами данные. (логин, роль пользователя, время жизни токена)
     - Signature - подпись. Гарантирует целостность токена и создаётся путём подписания 
        кодированных Header и Payload секретным ключем.

    1. JWT не зашифрован, а закодирован Base64Url. Любой человек может декодировать. 
    Не хранить в JWT пароли, номера карт и т.д.

    2. Неизменяем. Blacklist vs Whitelist. Whitelist хранит валидные токены, у которых большое
    время жизни. Если пользователь сменит пароль - удалим из whitelist'а

    3. Есть время жизни. 

    Постоянный токен (Refresh) - большое время жизни, хранится в БД.
    Временный токен (Access) - 15 минут. Нужен для аутентификации.

    Отзывать токены нужно:
    1) Для удаления пользователя
    2) Для защиты аккаунта (если токен попал в плохие руки)

    localstorage cookies. HttpOnly cookies. SCRM-атаки

*/
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Domain;

namespace WebApplication1.Service
{
    public interface IJWTService
    {
        string GenerateToken(User user);
        int? ValidateToken(string token); 
    }

    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;
        public string GenerateToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); // возвращается id
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
