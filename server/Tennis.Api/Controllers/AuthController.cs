using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tennis.Model.DTOs;
using Tennis.Service.AuthService;
namespace Tennis.Api.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        return await ExecuteAsync(_authService.Register(registerDTO));
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        return await ExecuteAsync(_authService.Login(loginDTO));
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO? refreshTokenDTO)
    {
        return await ExecuteAsync(_authService.RefreshToken(refreshTokenDTO!.RefreshToken!));
    }
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO? refreshTokenDTO)
    {
        return await ExecuteAsync(_authService.Logout(refreshTokenDTO!.RefreshToken!));
    }
}
