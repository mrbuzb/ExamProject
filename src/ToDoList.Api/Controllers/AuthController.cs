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

        /// <summary>
        /// Foydalanuvchini ro‘yxatdan o‘tkazadi
        /// </summary>
        /// <remarks>
        /// Namuna so‘rov:
        ///
        ///     POST /api/auth/signUp
        ///     {
        ///         "firstName": "Ali",
        ///         "lastName": "Valiyev",
        ///         "userName": "alivaliev",
        ///         "password": "Password123!",
        ///         "phoneNumber": "+998901234567",
        ///         "email": "ali@example.com",
        ///         "roleId": 2
        ///     }
        /// </remarks>
        /// <response code="200">Yaratilgan foydalanuvchi ID'si qaytariladi</response>
        [HttpPost("signUp")]
        public async Task<ActionResult<long>> SignUp([FromBody] UserCreateDto dto)
        {
            var userId = await _authService.SignUpUserAsync(dto);
            return Ok(userId);
        }

        /// <summary>
        /// Foydalanuvchini tizimga kirgizadi (login)
        /// </summary>
        /// <remarks>
        /// Namuna so‘rov:
        ///
        ///     POST /api/auth/login
        ///     {
        ///         "userName": "alivaliev",
        ///         "password": "Password123!"
        ///     }
        /// </remarks>
        /// <response code="200">Access va refresh tokenlar qaytariladi</response>
        /// <response code="401">Login yoki parol noto‘g‘ri</response>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UserLoginDto dto)
        {
            var result = await _authService.LoginUserAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Refresh token orqali yangi access token oladi
        /// </summary>
        /// <remarks>
        /// Namuna so‘rov:
        ///
        ///     POST /api/auth/refreshToken
        ///     {
        ///         "refreshToken": "eyJhbGciOiJIUzI1NiIs..."
        ///     }
        /// </remarks>
        /// <response code="200">Yangi access va refresh token qaytariladi</response>
        /// <response code="401">Refresh token noto‘g‘ri yoki eskirgan</response>
        [HttpPost("refreshToken")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Foydalanuvchini tizimdan chiqaradi (logout)
        /// </summary>
        /// <remarks>
        /// Namuna so‘rov:
        ///
        ///     DELETE /api/auth/logOut?token=eyJhbGciOiJIUzI1NiIs...
        /// </remarks>
        /// <response code="204">Muvaffaqiyatli logOut qilindi</response>
        [HttpDelete("logOut")]
        public async Task<IActionResult> LogOut([FromQuery] string token)
        {
            await _authService.LogOutAsync(token);
            return NoContent();

        }
    }
}
