using Social_app_3_api.Model.Post;
using Social_app_3_api.Model.User;

namespace Social_app_3_api.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>?> GetUsers();
        Task<User?> GetUser(int userId);
        Task<User?> GetUser(string email);
        Task<User?> PostNewUser(User userData);
        //Task<User?> PutUser(User userData);
        //Task<User?> PatchUser(User userData);
        Task<User?> UpdateUser(User userData);
        Task<User?> DeletUser(User user);
    }
}
