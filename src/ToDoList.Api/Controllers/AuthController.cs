using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;

namespace ToDoList.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signUp")]
        public async Task<ActionResult<long>> SignUp([FromBody] UserCreateDto dto)
        {
            var userId = await _authService.SignUpUserAsync(dto);
            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UserLoginDto dto)
        {
            var result = await _authService.LoginUserAsync(dto);
            return Ok(result);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            return Ok(result);
        }

        [HttpDelete("logOut")]
        public async Task<IActionResult> LogOut([FromQuery] string token)
        {
            await _authService.LogOutAsync(token);
            return NoContent();
        }
    }
}