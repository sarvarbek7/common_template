namespace Domain.Entities;

public interface ITranslatable<T, TTranslation> : ITranslatable
    where TTranslation : ITranslation<T>
    where T : ITranslatable
{
    List<TTranslation> Translations { get; set; }
}

public interface ITranslatable;