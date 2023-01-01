using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Social_app_3_api.Model.User;
using Social_app_3_api.Repositories;
using Social_app_3_api.Services;
using System.Net;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Social_app_3_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService = new AuthService();
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;    
        }

        [HttpPost("register")]
        [AllowAnonymous]
        //public async Task<ActionResult> Register(User user)
        public async Task<ActionResult> Register(Register userInput)
        {
            User newUserData = new()
            {
                FirstName = userInput.FirstName,
                LastName = userInput.LastName,
                Email = userInput.Email,
                Password = userInput.Password,
                UserName = userInput.UserName
            };

            string hashedPassword = _authService.GenerateHash(newUserData, newUserData.Password!);
            newUserData.Password = hashedPassword;

            User? user = await _userRepository.PostNewUser(newUserData);

            if (user == null)
            {
                return BadRequest("Dados inválidos");
            }

            user.Password = "";

            var token = _authService.GenerateToken(user);

            return CreatedAtAction("Register", new
            {
                user,
                token
            });
        }


    }
}
