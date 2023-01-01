using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Social_app_3_api.Model;
using Social_app_3_api.Repositories;
using Social_app_3_api.Model.Post;
using Social_app_3_api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Social_app_3_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            var posts = await _postRepository.GetPosts();

            if (posts == null)
            {
                return BadRequest("Serviço indisponível");
            }

            return Ok(posts);
        }

        [HttpGet("userPosts/me")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Post>>> GetSelfPosts()
        {
            // !!!!!!! GAMBIARRA POR ESTAR USANDO A CHAVE EMAIL COMO
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userId = (await _userRepository.GetUser(userEmail)).Id;
            var posts = await _postRepository.GetPostsByUser(userId);

            if (posts == null)
            {
                return BadRequest("Serviço indisponível");
            }

            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            Post? post = await _postRepository.GetPost(id);

            if (post == null)
            {
                return BadRequest();
            }

            return Ok(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddPost(RegisterNewPost postContent)
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userRepository.GetUser(userEmail);

            Post newPostDate = new()
            {
                Content = postContent.Content,
                UserId = user.Id,
            };

            var result = await _postRepository.PostNewPost(newPostDate);

            if (result is null)
            {
                return NotFound("Conta não encontrada");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, RegisterNewPost updatedDataInput)
        {
            // usar path só pra só mudar o conteúdo
            // !!!!!! essa chave de verificação tem que ser trocada pelo id, ou o usuário não poderá atualizar o email sem dar mais trabalho
            string email = User.FindFirstValue(ClaimTypes.Email);
            Post? postData = await _postRepository.GetPost(id);

            if(postData is null)
            {
                return BadRequest();
            }
            if (email == postData.User.Email)
            {
                postData.Content = updatedDataInput.Content;
                await _postRepository.UpdatePost(postData);
                return Accepted(postData);
            }
            else
            {
                return Forbid("Usuário não autorizado");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);

            Post? postData = await _postRepository.GetPost(id);

            if (postData.User.Email != email)
            {
                return Unauthorized();
            }

            await _postRepository.DeletPost(postData);

            return Accepted(postData);
        }
    }
}
