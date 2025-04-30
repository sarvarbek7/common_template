using System.Linq.Expressions;
using Application.Errors;
using Application.Services.Foundations;
using Application.Validators;
using Domain.Entities;
using ErrorOr;
using Infrastructure.Repositories;

namespace Infrastructure.Services.Foundations;


public abstract class BaseService<T, TId>(IRepository<T, TId> repository,
                                            IValidator<T, TId> validator) : IBaseService<T, TId>
where T : class, IEntity<TId>
where TId : struct
{
    public async Task<ErrorOr<Created>> Add(T entity,
                                            bool saveChanges = true,
                                            CancellationToken cancellationToken = default)
    {
        return await validator.ValidateOnAdd(entity).MatchAsync<ErrorOr<Created>>(async v =>
        {
            await repository.Add(entity, saveChanges, cancellationToken);

            return v;
        }, async e =>
        {
            await Task.CompletedTask;

            return e;
        });
    }

    public async Task CommitTransaction(CancellationToken cancellationToken)
    {
        await repository.CommitTransaction(cancellationToken);
    }

    public async Task<ErrorOr<Deleted>> Delete(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return await validator.ValidateOnDelete(entity).MatchAsync<ErrorOr<Deleted>>(async v =>
        {
            await repository.Remove(entity, saveChanges, cancellationToken);

            return v;
        }, async e =>
        {
            await Task.CompletedTask;

            return e;
        });
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>>? predicate = null, bool tracked = true, bool ignoreQueryFilters = false)
    {
        return repository.GetAll(predicate, tracked, ignoreQueryFilters);
    }

    public async Task<ErrorOr<T>> GetById(TId id,
        bool tracked = true,
        bool ignoreQueryFilters = false,
        IEnumerable<string>? includeStrings = null,
        CancellationToken cancellationToken = default)
    {
        if (id.Equals(default(TId)))
        {
            return ApplicationErrors.InvalidId<T>();
        }

        T? storedEntity = await repository.GetById(id,
                                                   tracked,
                                                   ignoreQueryFilters,
                                                   includeStrings,
                                                   cancellationToken);

        if (storedEntity is null)
        {
            return ApplicationErrors.EntityNotFound<T, TId>(id);
        }

        return storedEntity;
    }

    public async Task RollbackTransaction(CancellationToken cancellationToken)
    {
        await repository.RollbackTransaction(cancellationToken);
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await repository.SaveChanges(cancellationToken);
    }

    public async Task StartTransaction(CancellationToken cancellationToken = default)
    {
        await repository.StartTransaction(cancellationToken);
    }

    public async Task<ErrorOr<Updated>> Update(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return await validator.ValidateOnUpdate(entity).MatchAsync<ErrorOr<Updated>>(async v =>
        {
            await repository.Update(entity, saveChanges, cancellationToken);

            return v;
        }, async e =>
        {
            await Task.CompletedTask;

            return e;
        });
    }
}
