using AccountApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AccountApi.Data
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User user);
        Task<bool> GetUser(int userId = 0, string username = "");
        Task<User> GetCurrentUser(string username, string password);
    }
    public class SQLUserRepository : IUserRepository
    {
        private readonly ILogger<IUserRepository> _logger;
        private readonly AppDbContext _context;

        public SQLUserRepository(ILogger<IUserRepository> logger, AppDbContext context)
        {
            this._logger = logger;
            this._context = context;
        }
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                var newUser = await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create User {ex}");
                return false;
            }
            return true;
        }
        public async Task<bool> GetUser(int userId = 0, string username = "")
        {
            if(userId > 0)
            {
                return await _context.Users.AnyAsync(u => u.Id == userId);
            }
            else if (!username.IsNullOrEmpty())
            {
                var user = await _context.Users.AnyAsync(u => u.Username == username);
                return user;
            }
            return false;
        }

        public async Task<User> GetCurrentUser(string username, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var isValid = BCrypt.Net.BCrypt.Verify(username, user.Password);
            if (isValid)
                return user;
            else
                return null;
        }
    }
}
