using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Social_app_3_api.Data;
using Social_app_3_api.Model.User;

namespace Social_app_3_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>?> GetUsers()
        {
            try
            {
                return await _context.Users
                    .Include(c => c.Posts)
                    .Select(e => new User { Password = "", Email = e.Email, FirstName = e.FirstName, Id=e.Id, LastName = e.LastName, Role = e.Role, UserName = e.UserName, Posts = e.Posts })
                    .ToListAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<User?> GetUser(int userId)
        {
            try
            {
                return await _context.Users
                    .FirstAsync(c => c.Id == userId);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<User?> GetUser(string userEmail)
        {
            try
            {
                return await _context.Users
                    .FirstAsync(c => c.Email == userEmail);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<User?> PostNewUser(User userData)
        {
            try
            {
                await _context.Users
                    .AddAsync(userData);
                await _context.SaveChangesAsync();
                return userData;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<User?> UpdateUser(User userData)
        {
            try
            {
                int updates = await Task.Run(() =>
                {
                    _context.Users.Update(userData);
                    return _context.SaveChanges();
                });

                if (updates < 1){
                    return null;
                }

                return userData;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }
        public async Task<User?> DeletUser(User user)
        {
            try {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            } catch(Exception e) {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
