using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain;

namespace WebApplication1.Repository
{
    public interface IUserRepo
    {
        Task<User> GetUser(string login);
        Task<User> AddUser(string login, string password);
    }
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _db;
        public UserRepo (AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> GetUser(string login)
        {
            try
            {
                var user = await _db.Users
                    .FirstOrDefaultAsync(u => u.Login == login);
                return user;
            }
            catch
            {
                return null;
            }
        }
        public async Task<User> AddUser(string login, string password)
        {
            try
            {
                _db.Users.AddAsync(new User { Login = login, Password = password });
                await _db.SaveChangesAsync();
                return await GetUser(login);
            }
            catch
            {
                return null;
            }
        }
    }
}
