using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tennis.Model.DTOs;
using Tennis.Service.AuthService;

namespace Tennis.Api.Controllers
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
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            return Ok(await _authService.Register(registerDTO));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            return Ok(await _authService.Login(loginDTO));
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO? tokenDTO)
        {
            return Ok(await _authService.RefreshToken(tokenDTO!.RefreshToken!, tokenDTO.JwtToken!));
        }
    }
}
