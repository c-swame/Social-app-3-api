using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Social_app_3_api.Data;
using Social_app_3_api.Model.Post;
using Social_app_3_api.Model.User;

namespace Social_app_3_api.Repositories
{
    /// <summary>
    /// Classe para fazer intermédio e aplicar regras de negócio entre o banco de dados (model) e os controllers
    /// model base: Post
    /// </summary>
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// método responsável por buscar todos os posts
        /// </summary>
        /// <returns>Lista com todos os posts (apenas o id do usuário responsável é enviado nessa requisição)</returns>
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

        /// <summary>
        /// método responsável por buscar posts através do id do post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns>objeto do tipo Post incluindo o ojeto que representa o usuário dono do post</returns>
        public async Task<Post?> GetPost(int postId)
        {
            try
            {
                //!!!!! Remover senha do usuário retorno. -> criar classe que contenha apenas os dados do usuário que devem ser retornados (superclasse? derivada?)
                //!!!!!? Remover o usuário do retorno? !+
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

        /// <summary>
        /// Retorna todos os posts de um único usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna os posts do usuário dono do id passado como argumento</returns>
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

        /// <summary>
        /// Cria um novo post. É esperado que todos os dados do novo post, incluindo o id do usuário sejam passados // ?????!!!!! Tornar o campo Usuário um campo virtual? perco performace nesse caso usando um capo virtual?????
        /// </summary>
        /// <param name="postData"></param>
        /// <returns>
        /// sucesso: Representação do objeto criado
        /// falha: null
        /// </returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <returns>
        /// sucesso: Representação do objeto atualizado
        /// falha: nullo
        /// </returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <returns>
        /// sucesso: Representação do elemento deletado;
        /// falha: null
        /// </returns>
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
