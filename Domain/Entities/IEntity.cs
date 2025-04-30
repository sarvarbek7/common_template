namespace Domain.Entities;

public interface IEntity<TId>
    where TId : struct
{
    TId Id { get; init; }
}