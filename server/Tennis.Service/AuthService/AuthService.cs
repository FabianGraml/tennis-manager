using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Tennis.Database.Models;
using Tennis.Model.DTOs;
using Tennis.Model.Helpers;
using Tennis.Model.Models;
using Tennis.Model.Results;
using Tennis.Repository.UnitOfWork;
using Tennis.Service.AppSettingsService;
using Tennis.Service.AppSettingsService.AppSettings;

namespace Tennis.Service.AuthService;
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppSettingsService<AppSettingsConfig> _settingsService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthService(IUnitOfWork unitOfWork, IAppSettingsService<AppSettingsConfig> settingsService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _settingsService = settingsService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Result<ResponseModel, ResponseModel>> Register(RegisterDTO registerDTO)
    {
        User? existingUser = await _unitOfWork.UserRepository.GetAsync(x => x.Email == registerDTO.Email);
        if (existingUser != null)
        {
            return Result<ResponseModel, ResponseModel>.FromFailure(
           new ResponseModel("User with given Email already exist"), 400);
        }
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
        return Result<ResponseModel, ResponseModel>.FromSuccess(
               new ResponseModel("User registered successfully"), 200);
    }
    public async Task<Result<TokenDTO, ResponseModel>> Login(LoginDTO loginDTO)
    {
        User? user = await _unitOfWork.UserRepository.GetIncludingAsync(x => x.Email == loginDTO.Email, includes: q => q.Include(u => u.Role)!);
        if (user == null)
        {
            return Result<TokenDTO, ResponseModel>.FromFailure(
                   new ResponseModel("User cannot be found"), 400);
        }
        if (!await VerifyPasswordHash(loginDTO?.Password!, user?.PasswordHash!, user?.PasswordSalt!))
        {
            return Result<TokenDTO, ResponseModel>.FromFailure(
                   new ResponseModel("Wrong password"), 400);
        }
        var refreshToken = await GenerateRefreshToken();
        await SetRefreshToken(refreshToken, user!);

        return Result<TokenDTO, ResponseModel>.FromSuccess(new TokenDTO
        {
            JwtToken = await CreateToken(user!),
            RefreshToken = refreshToken.Token,
        }, 200);
    }
    public async Task<Result<TokenDTO, ResponseModel>> RefreshToken(string refreshToken)
    {
        User? user = await _unitOfWork.UserRepository.GetIncludingAsync(x => x.RefreshToken == refreshToken, includes: q => q.Include(u => u.Role)!);

        if (user == null || user!.RefreshToken.Equals(refreshToken) == false)
        {
            return Result<TokenDTO, ResponseModel>.FromFailure(
                   new ResponseModel("Invalid Refresh Token"), 400);
        }
        else if (user.TokenExpires < DateTime.Now)
        {
            return Result<TokenDTO, ResponseModel>.FromFailure(
                   new ResponseModel("Token expired"), 400);
        }
        string token = await CreateToken(user);
        var newRefreshToken = await GenerateRefreshToken();
        await SetRefreshToken(newRefreshToken, user);
        return Result<TokenDTO, ResponseModel>.FromSuccess(new TokenDTO
        {
            JwtToken = token,
            RefreshToken = newRefreshToken.Token,
        }, 200);
    }
    public async Task<Result<ResponseModel, ResponseModel>> Logout(string refreshToken)
    {
        string? userId = await GetClaimValueByToken("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        User? user = await _unitOfWork.UserRepository.GetAsync(x => x.RefreshToken == refreshToken && x.Id == int.Parse(userId));
        if(user == null)
        {
            return Result<ResponseModel, ResponseModel>.FromFailure(
                   new ResponseModel("Invalid Jwt Token or Username"), 400);
        }
        user.TokenExpires = DateTime.Now;
        await _unitOfWork.SaveAsync();
        return Result<ResponseModel, ResponseModel>.FromFailure(
                   new ResponseModel("Logout was successful"), 200);
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

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_settingsService.GetSettings().JwtSecretKey!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(_settingsService.GetSettings().JwtValid!)),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(jwt);
    }
    private Task<RefreshToken> GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddMinutes(int.Parse(_settingsService.GetSettings().RefreshTokenValid!)),
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
    public Task<string> GetClaimValueByToken(string claimType)
    {
        HttpRequest? request = _httpContextAccessor.HttpContext!.Request;
        string jwt = string.Empty;
        string authHeader = request.Headers["Authorization"].ToString();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
            jwt = authHeader[7..];
        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken token = tokenHandler.ReadJwtToken(jwt);
        string claimValue = token.Claims.First(claim => claim.Type == claimType).Value;
        return Task.FromResult(claimValue);
    }
}
