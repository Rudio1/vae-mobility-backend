using Microsoft.EntityFrameworkCore;
using VaeMobility.Application.Generic.Services.Interfaces;
using VaeMobility.Domain.Auth.Entities;
using VaeMobility.Domain.Auth.Repositories.Interfaces;
using VaeMobility.Infra.Data.Context;
using VaeMobility.Infra.Data.Generic.Repositories;

namespace VaeMobility.Infra.Data.Auth.Repositories;

public sealed class UserRepository(AppDbContext context, ICurrentUserContext currentUserContext)
    : Repository<User>(context, currentUserContext), IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return Context.Set<User>().FirstOrDefaultAsync(user => user.Email == normalized, cancellationToken);
    }
}
