using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using Tennis.Database.Models;
using Tennis.Model.DTOs;
using Tennis.Model.Helpers;
using Tennis.Repository.UnitOfWork;

namespace Tennis.Service.AuthService;
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    public AuthService(IUnitOfWork unitOfWork)
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
        await _unitOfWork.SaveAsync();
        return user;
    }
    public async Task<TokenDTO> Login(LoginDTO loginDTO)
    {
        User? user = await _unitOfWork.UserRepository.GetIncludingAsync(x => x.Email == loginDTO.Email, includes: q => q.Include(u => u.Role)!);
        if (user == null)
        {
            throw new ApplicationException("User cannot be found");
        }
        if (!await VerifyPasswordHash(loginDTO?.Password!, user?.PasswordHash!, user?.PasswordSalt!))
        {
            throw new ApplicationException("Wrong password");
        }
        var refreshToken = await GenerateRefreshToken();
        await SetRefreshToken(refreshToken, user!);
        TokenDTO tokenDTO = new()
        {
            JwtToken = await CreateToken(user!),
            RefreshToken = refreshToken.Token,
        };
        return tokenDTO;
    }
    public async Task<TokenDTO> RefreshToken(string refreshToken, string jwtToken)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(jwtToken);

        string userId = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        User? user = await _unitOfWork.UserRepository.GetIncludingAsync(x => x.Id == int.Parse(userId), includes: q => q.Include(u => u.Role)!);

        if (!user!.RefreshToken.Equals(refreshToken))
        {
            throw new ApplicationException("Invalid Refresh Token");
        }
        else if (user.TokenExpires < DateTime.Now)
        {
            throw new ApplicationException("Token expired");
        }
        string token = await CreateToken(user);
        var newRefreshToken = await GenerateRefreshToken();
        await SetRefreshToken(newRefreshToken, user);
        return new TokenDTO
        {
            JwtToken = token,
            RefreshToken = newRefreshToken.Token
        };
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
    private Task<string> CreateToken(User user)
    {
        List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName!),
                    new Claim(ClaimTypes.Email, user?.Email!),
                };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("MySecretKedfsadfdy"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(jwt);
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
