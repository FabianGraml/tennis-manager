using Tennis.Database.Models;
using Tennis.Model.DTOs;

namespace Tennis.Service.UserService;
public interface IUserService
{
    Task<User> Register(RegisterDTO registerDTO);
    Task<TokenDTO> Login(LoginDTO loginDTO);
    Task<string> RefreshToken();
}
