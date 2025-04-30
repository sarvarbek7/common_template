using Application.Common;
using Domain.Entities;
using ErrorOr;

namespace Application.Errors;

public static class ApplicationErrors
{
    public static Error InvalidId<T>()
    {
        return ErrorBuilder.Build(code: "Common.InvalidId",
                                  type: ErrorType.Validation,
                                  args: nameof(T));
    }

    public static Error EntityNotFound<T, TId>(TId id)
        where TId : struct
        where T : class, IEntity<TId>
    {
        string entityName = typeof(T).Name;

        return ErrorBuilder.Build($"Common.EntityNotFound",
                                  ErrorType.NotFound,
                                  entityName,
                                  id);
    }
}