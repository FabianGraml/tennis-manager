using Microsoft.AspNetCore.Mvc;
using Tennis.Model.DTOs;
using Tennis.Model.Models;
using Tennis.Model.Results;
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
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        return await ExecuteAsync(_authService.Register(registerDTO));
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        return await ExecuteAsync(_authService.Login(loginDTO));
    }
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO? refreshTokenDTO)
    {
        return await ExecuteAsync(_authService.RefreshToken(refreshTokenDTO!.RefreshToken!));
    }
}
