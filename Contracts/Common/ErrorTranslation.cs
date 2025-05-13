namespace Contracts.Common;

public record ErrorTranslation(string Code, string Uz, string? En, string? Ru, string? Kril)
{
    private readonly static List<ErrorTranslation> list = [];

    public static void AddTranslation(ErrorTranslation error)
    {
        list.Add(error);
    }

    public static IEnumerable<ErrorTranslation> List => list;
}