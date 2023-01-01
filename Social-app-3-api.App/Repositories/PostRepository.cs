using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Social_app_3_api.Data;
using Social_app_3_api.Model.Post;
using Social_app_3_api.Model.User;

namespace Social_app_3_api.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Post>?> GetPosts()
        {
            try
            {
                //.Select(e => new User { Password = "", Email = e.Email, FirstName = e.FirstName, Id=e.Id, LastName = e.LastName, Role = e.Role, UserName = e.UserName, Posts = e.Posts })
                return await _context.Posts
                    .ToListAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<Post?> GetPost(int postId)
        {
            try
            {
                return await _context.Posts
                    .Include(c => c.User)
                    .FirstAsync(c => c.PostId == postId);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<List<Post>?> GetPostsByUser(int id)
        {
            try
            {
                return await _context.Posts
                    .Where<Post>(c => c.UserId == id)
                    .Select(e => new Post { UserId = e.UserId, PostId = e.PostId, Content = e.Content })
                    .ToListAsync();
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<Post?> PostNewPost(Post postData)
        {
            try
            {
                await _context.Posts
                    .AddAsync(postData);
                await _context.SaveChangesAsync();

                return postData;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        public async Task<Post?> UpdatePost(Post postData)
        {
            try
            {
                int updates = await Task.Run(() =>
                {
                    _context.Posts.Update(postData);
                    return _context.SaveChanges();
                });

                if (updates < 1){
                    return null;
                }

                return postData;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return null;
            }
        }

        //public async Task<Post?> PatchPost(Post postData)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Post?> DeletPost(Post post)
        {
            try {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return post;
            } catch(Exception e) {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
