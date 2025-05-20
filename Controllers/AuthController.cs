using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EshoppingZoneAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO dto)
        {
            var token = await _authService.Register(dto);
            if (token == null) return BadRequest("User already exists.");
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            var token = await _authService.Login(dto);
            if (token == null) return Unauthorized("Invalid credentials.");
            return Ok(new { token });
        }
    }
}
