using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Domain.Generic.Repositories.Interfaces;

namespace VaeMobility.Domain.Catalog.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken = default);
    Task<bool> HasProductsAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
