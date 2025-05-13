using System.Data.Common;
using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories;


public abstract class Repository<TContext, T, TId>(TContext context) : IRepository<T, TId>
    where TContext : DbContext
    where T : class, IEntity<TId>
    where TId : struct
{
    protected TContext Context { get; } = context;

    public async Task StartTransaction(CancellationToken cancellationToken = default)
    {
        await Context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransaction(CancellationToken cancellationToken)
    {
        await Context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransaction(CancellationToken cancellationToken)
    {
        await Context.Database.BeginTransactionAsync(cancellationToken);
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>>? predicate = null, bool tracked = true,
        bool ignoreQueryFilters = false)
    {
        IQueryable<T> query = Context.Set<T>();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }

    public async Task<T?> GetById(TId id,
        bool tracked = true,
        bool ignoreQueryFilters = false,
        IEnumerable<string>? includeStrings = null,
        CancellationToken cancellationToken = default)
    {
        var query = GetAll(x => x.Id.Equals(id), tracked, ignoreQueryFilters);

        if (includeStrings is not null)
        {
            foreach (var includeString in includeStrings)
            {
                query = query.Include(includeString);
            }
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetFirstMatch(Expression<Func<T, bool>> predicate,
        bool tracked = true,
        bool ignoreQueryFilters = false,
        IEnumerable<string>? includeStrings = null,
        CancellationToken cancellationToken = default)
    {
        var query = GetAll(predicate, tracked, ignoreQueryFilters);

        if (includeStrings is not null)
        {
            foreach (var includeString in includeStrings)
            {
                query = query.Include(includeString);
            }
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Add(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await Context.AddAsync(entity, cancellationToken);

        if (saveChanges)
        {
            await SaveChanges(cancellationToken);
        }
    }

    public async Task AddRange(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        await Context.AddRangeAsync(entities, cancellationToken);

        if (saveChanges)
        {
            await SaveChanges(cancellationToken);
        }
    }

    public async Task Update(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        Context.Update(entity);

        if (saveChanges)
        {
            await SaveChanges(cancellationToken);
        }
    }

    public async Task UpdateRange(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        Context.UpdateRange(entities);

        if (saveChanges)
        {
            await SaveChanges(cancellationToken);
        }
    }

    public async Task Remove(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        Context.Remove(entity);

        if (saveChanges)
        {
            await SaveChanges(cancellationToken);
        }
    }

    public async Task RemoveRange(IEnumerable<T> entity, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        Context.RemoveRange(entity);

        if (saveChanges)
        {
            await SaveChanges(cancellationToken);
        }
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}