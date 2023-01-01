using Social_app_3_api.Model.Post;

namespace Social_app_3_api.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>?> GetPosts();
        Task<Post?> GetPost(int postId);
        Task<List<Post>?> GetPostsByUser(int id);
        Task<Post?> PostNewPost(Post postData);
        //Task<Post?> PutPost(Post postData);
        //Task<Post?> PatchPost(Post postData);
        Task<Post?> UpdatePost(Post postData);
        Task<Post?> DeletPost(Post post);
    }
}
