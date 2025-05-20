using System.Collections.Frozen;

namespace Shared;

public static class Translation
{
    private static string _fallbackLanguage = string.Empty;
    private static readonly Dictionary<string, Dictionary<string, string>> _translations = [];

    public static void SetFallbackLanguae(string fallbackLanguage)
    {
        _fallbackLanguage = fallbackLanguage;
    }

    /// <summary>
    /// Adds translations. First level key is translation code, second level key is language code
    /// </summary>
    public static void AddTranslations(Dictionary<string, Dictionary<string, string>> translations)
    {
        foreach (var translation in translations)
        {
            _translations.Add(translation.Key, translation.Value);
        }
    }

    /// <summary>
    ///  Adds a translation. First level key is translation code, second level key is language code
    /// </summary>
    public static void AddTranslation(KeyValuePair<string, Dictionary<string, string>> translation)
    {
        _translations.Add(translation.Key, translation.Value);
    }

    public static bool TryGetValue(string translationCode, string languageCode, out string value)
    {
        value = string.Empty;

        if (string.IsNullOrWhiteSpace(translationCode))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return false;
        }

        if (_translations.TryGetValue(translationCode, out var translation))
        {
            if (translation.TryGetValue(languageCode, out var translationValue))
            {
                value = translationValue;

                return true;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_fallbackLanguage))
                {
                    if (translation.TryGetValue(_fallbackLanguage, out var fallbackTranslationValue))
                    {
                        value = fallbackTranslationValue;

                        return true;
                    }
                }

                return false;
            }
        }

        return false;
    }
}
