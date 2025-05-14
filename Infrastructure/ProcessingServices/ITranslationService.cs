using System.Text;
using Application.ProcessingServices;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ProcessingServices;

internal class TranslationService(ILogger<TranslationService> logger) : ITranslationService
{
    private static readonly Dictionary<string, string> cyrToLat = new()
        {
            {"а", "a"}, {"А", "A"},
            {"б", "b"}, {"Б", "B"},
            {"в", "v"}, {"В", "V"},
            {"г", "g"}, {"Г", "G"},
            {"д", "d"}, {"Д", "D"},
            {"ж", "j"}, {"Ж", "J"},
            {"з", "z"}, {"З", "Z"},
            {"и", "i"}, {"И", "I"},
            {"й", "y"}, {"Й", "Y"},
            {"к", "k"}, {"К", "K"},
            {"л", "l"}, {"Л", "L"},
            {"м", "m"}, {"М", "M"},
            {"н", "n"}, {"Н", "N"},
            {"о", "o"}, {"О", "O"},
            {"п", "p"}, {"П", "P"},
            {"р", "r"}, {"Р", "R"},
            {"с", "s"}, {"С", "S"},
            {"т", "t"}, {"Т", "T"},
            {"у", "u"}, {"У", "U"},
            {"ф", "f"}, {"Ф", "F"},
            {"х", "x"}, {"Х", "X"},
            {"ц", "ts"}, {"Ц", "Ts"},
            {"ч", "ch"}, {"Ч", "Ch"},
            {"ш", "sh"}, {"Ш", "Sh"},
            {"щ", "sh"}, {"Щ", "Sh"},
            {"ю", "yu"}, {"Ю", "Yu"},
            {"я", "ya"}, {"Я", "Ya"},
            {"ё", "yo"}, {"Ё", "Yo"},
            {"ў", "o'"}, {"Ў", "O'"},
            {"ғ", "g'"}, {"Ғ", "G'"},
            {"ъ", ""}, {"Ъ", ""},
            {"ы", "i"}, {"Ы", "I"},
            {"ь", ""}, {"Ь", ""},
            {"э", "e"}, {"Э", "E"},
            {"қ", "q"}, {"Қ", "Q"},
            {"ҳ", "h"}, {"Ҳ", "H"}
        };

    private static readonly Dictionary<string, string> latToCyr = new()
        {
            { "a", "а" }, { "A", "А" },
            { "b", "б" }, { "B", "Б" },
            { "v", "в" }, { "V", "В" },
            { "g", "г" }, { "G", "Г" },
            { "d", "д" }, { "D", "Д" },
            { "j", "ж" }, { "J", "Ж" },
            { "z", "з" }, { "Z", "З" },
            { "i", "и" }, { "I", "И" },
            { "y", "й" }, { "Y", "Й" },
            { "k", "к" }, { "K", "К" },
            { "l", "л" }, { "L", "Л" },
            { "m", "м" }, { "M", "М" },
            { "n", "н" }, { "N", "Н" },
            { "o", "о" }, { "O", "О" },
            { "p", "п" }, { "P", "П" },
            { "r", "р" }, { "R", "Р" },
            { "s", "с" }, { "S", "С" },
            { "t", "т" }, { "T", "Т" },
            { "u", "у" }, { "U", "У" },
            { "f", "ф" }, { "F", "Ф" },
            { "x", "х" }, { "X", "Х" },
            { "e", "е" }, { "E", "Е" },
            { "q", "қ" }, { "Q", "Қ" },
            { "h", "ҳ" }, { "H", "Ҳ" }
        };

    private static readonly Dictionary<string, string> specialCases = new Dictionary<string, string>
            {
                { "ch", "ч" }, { "Ch", "Ч" },
                { "sh", "ш" }, { "Sh", "Ш" },
                { "yu", "ю" }, { "Yu", "Ю" },
                { "ye", "е" }, { "Ye", "E" },
                { "ya", "я" }, { "Ya", "Я" },
                { "yo", "ё" }, { "Yo", "Ё" },
                { "o'", "ў" }, { "O'", "Ў" },
                { "g'", "ғ" }, { "G'", "Ғ" },
            };

    private static string CyrillicToLatinWithContext(string cyrillic)
    {
        var result = new StringBuilder();
        var vowels = new HashSet<char> { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я',
                                             'А', 'Е', 'Ё', 'И', 'О', 'У', 'Ы', 'Э', 'Ю', 'Я' };
        var softSigns = new HashSet<char> { 'ь', 'Ь', 'ъ', 'Ъ' };

        for (int i = 0; i < cyrillic.Length; i++)
        {
            char current = cyrillic[i];

            // Handle "е" and "Е" with contextual rules
            if (current == 'е' || current == 'Е')
            {
                bool isStartOfWord = i == 0 || !cyrToLat.Keys.Contains(cyrillic[i - 1].ToString());
                bool followsVowelOrSoftSign = i > 0 && (vowels.Contains(cyrillic[i - 1]) || softSigns.Contains(cyrillic[i - 1]));

                if (isStartOfWord || followsVowelOrSoftSign)
                {
                    result.Append(current == 'е' ? "ye" : "Ye");
                }
                else
                {
                    result.Append(current == 'е' ? "e" : "E");
                }
            }
            else
            {
                // Use dictionary lookup for other characters
                if (cyrToLat.GetValueOrDefault(current.ToString()) is { } transliteration)
                {
                    result.Append(transliteration);
                }
                else
                {
                    result.Append(current); // Keep the original character if no match
                }
            }
        }

        return result.ToString();
    }

    private static string LatinToCyrillic(string latin)
    {
        // Replace special cases first
        foreach (var specialCase in specialCases)
        {
            latin = latin.Replace(specialCase.Key, specialCase.Value);
        }

        var result = new StringBuilder();
        var words = latin.Split(' ', StringSplitOptions.RemoveEmptyEntries); // Split text into words

        foreach (var word in words)
        {
            if (word.Length > 0 && word.StartsWith("e", StringComparison.OrdinalIgnoreCase))
            {
                // Convert starting "e" to "э" or "Э" depending on case
                var firstLetter = word[0] == 'E' ? "Э" : "э";
                result.Append(firstLetter + word.Substring(1));
            }
            else
            {
                result.Append(word);
            }

            result.Append(' '); // Preserve spaces
        }

        var translatedText = result.ToString().Trim(); // Remove trailing space

        // Replace individual Latin letters with Cyrillic counterparts
        foreach (var token in latToCyr)
        {
            translatedText = translatedText.Replace(token.Key, token.Value);
        }

        return translatedText;
    }

    public TranslationResult? Translate(string? text)
    {
        if (text is null)
        {
            return null;
        }

        try
        {
            string latinText = CyrillicToLatinWithContext(text);
            string cyrillicText = LatinToCyrillic(text);

            return new TranslationResult(latinText, cyrillicText);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occuring to convert text.\r\nText: {text}", text);

            return new TranslationResult(text, text);
        }
    }
}