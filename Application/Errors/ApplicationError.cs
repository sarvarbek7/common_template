using Application.Common;
using Domain.Entities;
using ErrorOr;

namespace Application.Errors;

public static class ApplicationErrors
{
    private const string errorPrefix = "Common";

    public static Error InvalidId<T>()
    {
        return ErrorBuilder.Build(code: $"{errorPrefix}.InvalidId",
                                  type: ErrorType.Validation,
                                  args: nameof(T));
    }

    public static Error EntityNotFoundForGivenId<T, TId>(TId id)
        where TId : struct
        where T : class, IEntity<TId>
    {
        string entityName = typeof(T).Name;

        return ErrorBuilder.Build($"{errorPrefix}.EntityNotFoundForGivenId",
                                  ErrorType.NotFound,
                                  entityName,
                                  id);
    }

    public static Error EntityNotFound<T>()
        where T : class, IEntity
    {
        string entityName = typeof(T).Name;

        return ErrorBuilder.Build($"{errorPrefix}.EntityNotFound",
                                  ErrorType.NotFound,
                                  entityName);
    }

    public static Error DefaultLanguageCanNotBeMoreThanOne => Error.Validation($"{errorPrefix}.DefaultLanguageCanNotBeMoreThanOne");
    public static Error AtLeastOneDefaultLanguageMustSet => Error.Validation($"{errorPrefix}.AtLeastOneDefaultLanguageMustSet");
}