using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VaeMobility.Application.Auth.Services.Interfaces;
using VaeMobility.Application.Generic;
using VaeMobility.Domain.Auth.Entities;

namespace VaeMobility.WebApi.Auth;

public sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public AccessTokenResult GenerateAccessToken(User user)
    {
        var signingKey = configuration["Jwt:SigningKey"];
        if (string.IsNullOrWhiteSpace(signingKey))
        {
            throw new InvalidOperationException(ApplicationMessages.JwtSigningKeyNaoConfigurado);
        }

        var minutes = configuration.GetValue("Jwt:AccessTokenExpirationMinutes", 60);
        var expiresAt = DateTime.UtcNow.AddMinutes(minutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AccessTokenResult(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
