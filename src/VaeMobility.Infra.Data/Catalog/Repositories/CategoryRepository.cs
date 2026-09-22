using Microsoft.EntityFrameworkCore;
using VaeMobility.Application.Generic.Services.Interfaces;
using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Domain.Catalog.Repositories.Interfaces;
using VaeMobility.Infra.Data.Context;
using VaeMobility.Infra.Data.Generic.Repositories;

namespace VaeMobility.Infra.Data.Catalog.Repositories;

public sealed class CategoryRepository(AppDbContext context, ICurrentUserContext currentUserContext)
    : Repository<Category>(context, currentUserContext), ICategoryRepository
{
    public override Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Query().FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

    public Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => Query().FirstOrDefaultAsync(category => category.Slug == slug, cancellationToken);

    public Task<bool> SlugExistsAsync(string slug, Guid? excludingId, CancellationToken cancellationToken = default)
        => Context.Set<Category>().AnyAsync(
            category => category.Slug == slug && (!excludingId.HasValue || category.Id != excludingId),
            cancellationToken);

    public async Task<IReadOnlyList<Category>> ListAsync(CancellationToken cancellationToken = default)
        => await Query()
            .OrderBy(category => category.SortOrder)
            .ThenBy(category => category.Name)
            .ToListAsync(cancellationToken);

    public Task<bool> HasProductsAsync(Guid categoryId, CancellationToken cancellationToken = default)
        => Context.Set<Product>().AnyAsync(product => product.CategoryId == categoryId, cancellationToken);

    private IQueryable<Category> Query()
        => Context.Set<Category>().Include(category => category.Images);
}
