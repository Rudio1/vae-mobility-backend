using Microsoft.EntityFrameworkCore;
using VaeMobility.Application.Generic.Services.Interfaces;
using VaeMobility.Domain.Generic.Entities;
using VaeMobility.Domain.Generic.Repositories.Interfaces;
using VaeMobility.Infra.Data.Context;

namespace VaeMobility.Infra.Data.Generic.Repositories;

public class Repository<TEntity>(AppDbContext context, ICurrentUserContext currentUserContext) : IRepository<TEntity>
    where TEntity : Entity
{
    protected AppDbContext Context => context;

    public virtual Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Update(TEntity entity)
    {
        var entry = context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            context.Attach(entity);
            entry.State = EntityState.Modified;
            return;
        }

        if (entry.State == EntityState.Unchanged)
        {
            entry.State = EntityState.Modified;
        }
    }

    public void Remove(TEntity entity)
    {
        entity.Delete(currentUserContext.UserId);
        var entry = context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            context.Attach(entity);
        }
    }
}
