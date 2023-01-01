using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Social_app_3_api.Model;
using Social_app_3_api.Model.User;
using Social_app_3_api.Repositories;
using Social_app_3_api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Social_app_3_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthService _authService = new AuthService();
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userRepository.GetUsers();

            if (users == null)
            {
                return BadRequest("Serviço indisponível");
            }
            
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User? user = await _userRepository.GetUser(id);

            if (user == null)
            {
                return BadRequest();
            }

            user.Password = "";

            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, Register updatedDataInput)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            User? userData = await _userRepository.GetUser(id);

            if (email == updatedDataInput.Email && !(userData is null) && userData.Id == id)
            {
                User userUpdatedData = new()
                {
                    FirstName = updatedDataInput.FirstName,
                    LastName = updatedDataInput.LastName,
                    Email = updatedDataInput.Email,
                    Password = updatedDataInput.Password,
                    UserName = updatedDataInput.UserName,
                    Id = id
                };

                await _userRepository.UpdateUser(userUpdatedData);
                return Accepted(userUpdatedData);

            }
            else
            {
                return Forbid("Usuário não autorizado");
            }
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest();
            }

            User? userData = await _userRepository.GetUser(id);

            if (userData.Email != email)
            {
                return Unauthorized();
            }

            await _userRepository.DeletUser(userData);

            return Accepted(userData);
        }
    }
}
