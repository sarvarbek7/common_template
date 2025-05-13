namespace Domain.Entities;

public interface IEntity<TId> : IEntity
    where TId : struct
{
    TId Id { get; init; }
}

public interface IEntity;