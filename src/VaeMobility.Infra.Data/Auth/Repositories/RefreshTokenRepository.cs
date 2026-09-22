using Microsoft.EntityFrameworkCore;
using VaeMobility.Domain.Auth.Entities;
using VaeMobility.Domain.Auth.Repositories.Interfaces;
using VaeMobility.Infra.Data.Context;

namespace VaeMobility.Infra.Data.Auth.Repositories;

public sealed class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
        => await context.Set<RefreshToken>().AddAsync(token, cancellationToken);

    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        => context.Set<RefreshToken>().FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);
}
