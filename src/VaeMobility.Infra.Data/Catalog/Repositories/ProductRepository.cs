using Microsoft.EntityFrameworkCore;
using VaeMobility.Application.Generic.Services.Interfaces;
using VaeMobility.Domain.Catalog.Entities;
using VaeMobility.Domain.Catalog.Enums;
using VaeMobility.Domain.Catalog.Repositories.Interfaces;
using VaeMobility.Infra.Data.Context;
using VaeMobility.Infra.Data.Generic.Repositories;

namespace VaeMobility.Infra.Data.Catalog.Repositories;

public sealed class ProductRepository(AppDbContext context, ICurrentUserContext currentUserContext)
    : Repository<Product>(context, currentUserContext), IProductRepository
{
    public override Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Query().FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

    public Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => Query().FirstOrDefaultAsync(product => product.Slug == slug, cancellationToken);

    public Task<bool> SlugExistsAsync(string slug, Guid? excludingId, CancellationToken cancellationToken = default)
        => Context.Set<Product>().AnyAsync(
            product => product.Slug == slug && (!excludingId.HasValue || product.Id != excludingId),
            cancellationToken);

    public async Task<IReadOnlyList<Product>> ListAsync(string? status, Guid? categoryId, CancellationToken cancellationToken = default)
    {
        var query = Query();
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(product => product.Status == status);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(product => product.CategoryId == categoryId);
        }

        return await query
            .OrderByDescending(product => product.Featured)
            .ThenBy(product => product.SortOrder)
            .ThenBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> ListActiveAsync(string? categorySlug, bool? featured, CancellationToken cancellationToken = default)
    {
        var query = Query().Where(product => product.Status == ProductStatuses.Active);

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var categoryIds = Context.Set<Category>()
                .Where(category => category.Slug == categorySlug)
                .Select(category => category.Id);
            query = query.Where(product => categoryIds.Contains(product.CategoryId));
        }

        if (featured is true)
        {
            query = query.Where(product => product.Featured);
        }

        return await query
            .OrderByDescending(product => product.Featured)
            .ThenBy(product => product.SortOrder)
            .ThenBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AllExistAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
        {
            return true;
        }

        var count = await Context.Set<Product>().CountAsync(product => ids.Contains(product.Id), cancellationToken);
        return count == ids.Distinct().Count();
    }

    private IQueryable<Product> Query()
        => Context.Set<Product>()
            .Include(product => product.Images)
            .Include(product => product.Highlights)
            .Include(product => product.Specs)
            .Include(product => product.Variants)
            .Include(product => product.Badges)
            .Include(product => product.Related);
}
