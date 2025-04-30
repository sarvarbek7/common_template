using ErrorOr;

namespace Application.Common;

public static class ErrorBuilder
{
    public static Error Build(string code, ErrorType type, params object[] args)
    {
        Dictionary<string, object> metadata = new()
        {
            {AppConstants.ErrorMetadateKey, args}
        };

        return Error.Custom((int)type, code, description: string.Empty, metadata: metadata);
    }
}
