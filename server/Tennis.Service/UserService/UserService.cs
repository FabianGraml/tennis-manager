using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Tennis.Database.Models;
using Tennis.Model.DTOs;
using Tennis.Model.Helpers;
using Tennis.Repository.UnitOfWork;

namespace Tennis.Service.UserService;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User> Register(RegisterDTO registerDTO)
    {
        CreatePasswordHash(registerDTO?.Password!, out byte[] passwordHash, out byte[] passwordSalt);
        User user = new()
        {
            Firstname = registerDTO?.Firstname,
            Lastname = registerDTO?.Lastname,
            Email = registerDTO?.Email,
            CreatedAt = DateTime.Now,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            RoleId = 2,
        };
        await _unitOfWork.UserRepository.AddAsync(user);
        return user;
    }
    public async Task<TokenDTO> Login(LoginDTO loginDTO)
    {
        User? user = await _unitOfWork.UserRepository.GetAsync(x => x.Email == loginDTO.Email);
        if (user == null)
        {
            throw new ApplicationException("User cannot be found");
        }
        if (!await VerifyPasswordHash(loginDTO?.Password!, user?.PasswordHash!, user?.PasswordSalt!))
        {
            throw new ApplicationException("Wrong password");
        }
        TokenDTO tokenDTO = new()
        {
            JwtToken = await CreateToken(user!)
        };

        return tokenDTO;
    }
    public Task<string> RefreshToken()
    {
        throw new NotImplementedException();
    }
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

    }
    private static Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        var hmac = new HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Task.FromResult(computedHash.SequenceEqual(passwordHash));

    }
    private async Task<string> CreateToken(User user)
    {
        List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName!),
                    new Claim(ClaimTypes.Email, user?.Email!),
                };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("testete"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        RefreshToken refreshToken = await GenerateRefreshToken();
        await SetRefreshToken(refreshToken, user!);
        return jwt;
    }
    private static Task<RefreshToken> GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };
        return Task.FromResult(refreshToken);
    }

    private async Task SetRefreshToken(RefreshToken? newRefreshToken, User user)
    {
        user.RefreshToken = newRefreshToken!.Token!;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }
}
