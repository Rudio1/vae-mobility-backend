using VaeMobility.Domain.Auth.Entities;

namespace VaeMobility.Domain.Auth.Services.Interfaces;

public interface IAuthDomainService
{
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RefreshToken> IssueRefreshTokenAsync(Guid userId, string tokenHash, DateTime expiresAt, CancellationToken cancellationToken = default);
    Task<RefreshToken?> FindRefreshTokenAsync(string tokenHash, CancellationToken cancellationToken = default);
}
