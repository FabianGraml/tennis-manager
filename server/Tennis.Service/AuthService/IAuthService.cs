using Tennis.Model.DTOs;
using Tennis.Model.Models;
using Tennis.Model.Results;

namespace Tennis.Service.AuthService;
public interface IAuthService
{
    Task<Result<ResponseModel, ResponseModel>> Register(RegisterDTO registerDTO);
    Task<Result<TokenDTO, ResponseModel>> Login(LoginDTO loginDTO);
    Task<Result<TokenDTO, ResponseModel>> RefreshToken(string refreshToken);
    Task<Result<ResponseModel, ResponseModel>> Logout(string refreshToken);
}
