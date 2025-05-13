using System.Linq.Expressions;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IRepository<T, in TId>
    where T : class, IEntity<TId>
    where TId : struct
{
    Task StartTransaction(CancellationToken cancellationToken = default);
    Task CommitTransaction(CancellationToken cancellationToken);
    Task RollbackTransaction(CancellationToken cancellationToken);

    IQueryable<T> GetAll(Expression<Func<T, bool>>? predicate = null,
        bool tracked = true,
        bool ignoreQueryFilters = false);

    Task<T?> GetById(TId id,
        bool tracked = true,
        bool ignoreQueryFilters = false,
        IEnumerable<string>? includeStrings = null,
        CancellationToken cancellationToken = default);

    Task<T?> GetFirstMatch(Expression<Func<T, bool>> predicate,
        bool tracked = true,
        bool ignoreQueryFilters = false,
        IEnumerable<string>? includeStrings = null,
        CancellationToken cancellationToken = default);

    Task Add(T entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task AddRange(IEnumerable<T> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task Update(T entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task UpdateRange(IEnumerable<T> entities,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task Remove(T entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task RemoveRange(IEnumerable<T> entity,
        bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task SaveChanges(CancellationToken cancellationToken = default);
}
