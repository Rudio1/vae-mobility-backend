using VaeMobility.Domain.Auth.Entities;
using VaeMobility.Domain.Generic.Repositories.Interfaces;

namespace VaeMobility.Domain.Auth.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
