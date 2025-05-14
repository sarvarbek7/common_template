namespace Application.ProcessingServices;

public interface ITranslationService
{
    TranslationResult? Translate(string? text);
}