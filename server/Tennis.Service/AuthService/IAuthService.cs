using Tennis.Database.Models;
using Tennis.Model.DTOs;

namespace Tennis.Service.AuthService;
public interface IAuthService
{
    Task<User> Register(RegisterDTO registerDTO);
    Task<TokenDTO> Login(LoginDTO loginDTO);
    Task<TokenDTO> RefreshToken(string refreshToken, string jwtToken);
}
