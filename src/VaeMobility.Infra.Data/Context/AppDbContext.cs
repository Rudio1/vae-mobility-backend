using Microsoft.EntityFrameworkCore;
using VaeMobility.Application.Generic.Services.Interfaces;
using VaeMobility.Domain.Generic.Entities;
using VaeMobility.Domain.Generic.Repositories.Interfaces;
using VaeMobility.Infra.Data.Extensions;

namespace VaeMobility.Infra.Data.Context;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    ICurrentUserContext currentUserContext)
    : DbContext(options), IUnitOfWork
{
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await MarkNewEntitiesAsAddedAsync(cancellationToken);
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplySoftDeleteFilters();
    }

    private void ApplyAuditInfo()
    {
        var userId = currentUserContext.UserId;

        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreatedAudit(userId);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetUpdatedAudit(userId);
                    break;
            }
        }
    }

    private async Task MarkNewEntitiesAsAddedAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries().Where(item => item.State == EntityState.Modified).ToList())
        {
            var stored = await entry.GetDatabaseValuesAsync(cancellationToken);
            if (stored is null)
            {
                entry.State = EntityState.Added;
            }
        }
    }
}
