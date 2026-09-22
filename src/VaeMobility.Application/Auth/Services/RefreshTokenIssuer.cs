using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using VaeMobility.Application.Auth.Services.Interfaces;

namespace VaeMobility.Application.Auth.Services;

public sealed class RefreshTokenIssuer(IConfiguration configuration) : IRefreshTokenIssuer
{
    public IssuedRefreshToken Issue()
    {
        var days = int.TryParse(configuration["Jwt:RefreshTokenExpirationDays"], out var parsed) ? parsed : 7;
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return new IssuedRefreshToken(token, Hash(token), DateTime.UtcNow.AddDays(days));
    }

    public string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}
