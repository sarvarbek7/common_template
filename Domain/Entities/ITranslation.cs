using Domain.Enums;

namespace Domain.Entities;

public interface ITranslation<T> : ITranslation;

public interface ITranslation
{
    Language Language { get; set; }
    bool IsDefault { get; set; }
}