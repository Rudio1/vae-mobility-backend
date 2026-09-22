using VaeMobility.Domain.Auth.Entities;
using VaeMobility.Domain.Auth.Repositories.Interfaces;
using VaeMobility.Domain.Auth.Services.Interfaces;

namespace VaeMobility.Domain.Auth.Services;

public sealed class AuthDomainService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository) : IAuthDomainService
{
    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        => userRepository.GetByEmailAsync(email, cancellationToken);

    public Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => userRepository.GetByIdAsync(id, cancellationToken);

    public async Task<RefreshToken> IssueRefreshTokenAsync(
        Guid userId,
        string tokenHash,
        DateTime expiresAt,
        CancellationToken cancellationToken = default)
    {
        var token = RefreshToken.Issue(userId, tokenHash, expiresAt);
        await refreshTokenRepository.AddAsync(token, cancellationToken);
        return token;
    }

    public Task<RefreshToken?> FindRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default)
        => refreshTokenRepository.GetByHashAsync(tokenHash, cancellationToken);
}
