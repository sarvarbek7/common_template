using System.Linq.Expressions;
using Domain.Entities;
using ErrorOr;

namespace Application.Services.Foundations;

public interface IBaseService<T, in TId> : IBaseService
    where T : class, IEntity<TId>
    where TId : struct
{
    // Task<ListResult<T>> List(Expression<Func<T, bool>>? expression = null,
    //     string? orderString = null,
    //     CancellationToken cancellationToken = default);
    IQueryable<T> GetAll(Expression<Func<T, bool>>? predicate = null,
        bool tracked = true,
        bool ignoreQueryFilters = false);
    Task<ErrorOr<T>> GetById(TId id, bool tracked = true, bool ignoreQueryFilters = false, IEnumerable<string>? includeStrings = null, CancellationToken cancellationToken = default);
    Task<ErrorOr<Created>> Add(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);
    Task<ErrorOr<Updated>> Update(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> Delete(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);
}

public interface IBaseService
{
    Task StartTransaction(CancellationToken cancellationToken = default);
    Task CommitTransaction(CancellationToken cancellationToken = default);
    Task RollbackTransaction(CancellationToken cancellationToken = default);
    Task SaveChanges(CancellationToken cancellationToken = default);
}