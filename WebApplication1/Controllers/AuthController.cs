using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            //if(_authServices.ValidateUser(loginRequest.Username, loginRequest.Password))
            //{
            //    var token = _authServices.GenerateJwtToken(loginRequest.Username);
            //    return Ok(new { Token = token });
            //}
            var isValidUser = await _authServices.ValidateUser(loginRequest.Username, loginRequest.Password);

            if (isValidUser)
            {
                var token = _authServices.GenerateJwtToken(loginRequest.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized(new {Message = "Invalid username or password" });
        }
    }
}
