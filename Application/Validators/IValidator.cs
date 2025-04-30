using Domain.Entities;
using ErrorOr;

namespace Application.Validators;

public interface IValidator<T, TId>
    where T : class, IEntity<TId>
    where TId : struct
{
    ErrorOr<Created> ValidateOnAdd(T model);
    ErrorOr<Updated> ValidateOnUpdate(T model);
    ErrorOr<Deleted> ValidateOnDelete(T model);
}