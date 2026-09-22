using VaeMobility.Domain.Auth.Entities;

namespace VaeMobility.Application.Auth.Services.Interfaces;

public sealed record AccessTokenResult(string AccessToken, DateTime ExpiresAt);

public interface IJwtTokenService
{
    AccessTokenResult GenerateAccessToken(User user);
}
