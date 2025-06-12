using Application.Errors;
using Domain.Entities;
using ErrorOr;

namespace Application.Common.Extensions;

public static class TranslatableExtensions
{
    public static ErrorOr<Success> ValidateTranslations<T, TTranslation>(this ITranslatable<T, TTranslation> translatable)
        where TTranslation : ITranslation<T>
        where T : ITranslatable
    {
        List<Error> errors = [];

        int defaultLanguageCount = translatable.Translations.Count(x => x.IsDefault);

        if (defaultLanguageCount > 1)
        {
            errors.Add(ApplicationErrors.DefaultLanguageCanNotBeMoreThanOne);
        }
        else if (defaultLanguageCount == 0)
        {
            errors.Add(ApplicationErrors.AtLeastOneDefaultLanguageMustSet);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Success();
    }
}