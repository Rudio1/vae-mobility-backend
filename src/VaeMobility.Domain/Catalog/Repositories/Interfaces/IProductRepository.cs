using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Domain.Generic.Repositories.Interfaces;

namespace VaeMobility.Domain.Catalog.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ListAsync(string? status, Guid? categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ListActiveAsync(string? categorySlug, bool? featured, CancellationToken cancellationToken = default);
    Task<bool> AllExistAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default);
}
