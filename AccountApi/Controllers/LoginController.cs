using AccountApi.Common;
using AccountApi.Data;
using AccountApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly JWTTokenGenerator _tokenGenerator;

        public LoginController(ILogger<LoginController> logger, IUserRepository userRepository, JWTTokenGenerator tokenGenerator)
        {
            this._logger = logger;
            this._userRepository = userRepository;
            this._tokenGenerator = tokenGenerator;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser([FromBody]LoginDTO loginDTO)
        {
            var user = await _userRepository.GetCurrentUser(loginDTO.Username, loginDTO.Password);
            if (user != null)
            {
                var token = _tokenGenerator.GenerateToken(user.Id, user.Role);
                return Ok(new { token = token });
            }
            return BadRequest(new { message = "Username or Password correct" });
        }
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            var isUserAvailable = await _userRepository.GetUser(username: user.Username);
            if(isUserAvailable)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var result = await _userRepository.CreateUser(user);
                if (result)
                    return Ok(new { message = "User created sucessfully" });
                else
                    return BadRequest();
            }
            return BadRequest( new { message = "User already exist" });
        }
    }
}
