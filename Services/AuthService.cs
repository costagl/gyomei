using Gyomei.DTOs;
using Gyomei.Models;
using Gyomei.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Gyomei.Services;

public class AuthService
{
    private readonly GyomeiDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthService(GyomeiDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDTO?> Authenticate(LoginRequestDTO request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return null;
        }
        var userAccount = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(ua => ua.Email == request.Email);
        if (userAccount == null || !PasswordHashHandler.VerifyPassword(request.Password, userAccount.PasswordHash))
        {
            return null;
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var secretKey = _configuration["Jwt:SecretKey"];
        var tokenValidityMins = _configuration.GetValue<int>("Jwt:TokenExpirationInMinutes");
        var tokenExpiration = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userAccount.Id.ToString()),
                new Claim(ClaimTypes.Email, userAccount.Email),
                new Claim(ClaimTypes.Role, userAccount.Role.Name)
            }),
            Expires = tokenExpiration,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new LoginResponseDTO(accessToken, userAccount.Name, userAccount.Role.Name);
    }
}
