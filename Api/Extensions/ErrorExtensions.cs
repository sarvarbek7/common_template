using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions;

public static class ErrorExtensions
{
    private static readonly Dictionary<string, string> _defaultErrorMessages = [];

    public static void AddErrorDefaultDescription(string language, string defaultDescription)
    {
        _defaultErrorMessages.Add(language, defaultDescription);
    }

    public static void AddErrorDefaultDescriptions(Dictionary<string, string> defaultMessages)
    {
        foreach (var message in defaultMessages)
        {
            _defaultErrorMessages.Add(message.Key, message.Value);
        }
    }

    public static ProblemDetails ToProblemDetails(this Error error, string language)
    {
        string desription;

        if (Translation.TryGetValue(error.Code, language, out var value))
        {
            desription = value;
        }
        else
        {
            if (_defaultErrorMessages.TryGetValue(language, out var defaultDescription))
            {
                desription = defaultDescription;
            }
            else
            {
                desription = error.Description;
            }
        }

        (int statusCode, string title) = error.Type switch
        {
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Bad Request"),
            ErrorType.Unauthorized => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            ErrorType.Forbidden => (StatusCodes.Status403Forbidden, "Forbidden"),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "NotFound"),
            ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error"),
        };

        Dictionary<string, object?> extensions = new()
        {
            {"ErrorCode", error.Code}
        };

        return new ProblemDetails()
        {
            Title = title,
            Status = statusCode,
            Detail = desription,
            Extensions = extensions,
            Type = null
        };
    }
}