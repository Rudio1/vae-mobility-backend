namespace VaeMobility.Application.Auth.Services.Interfaces;

public sealed record IssuedRefreshToken(string Token, string TokenHash, DateTime ExpiresAt);

public interface IRefreshTokenIssuer
{
    IssuedRefreshToken Issue();
    string Hash(string token);
}
