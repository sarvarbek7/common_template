namespace Domain.Entities;

public interface IDeletionAuditableEntity<TAudit, TAuditId> : ISoftDeletedEntity
    where TAudit : class, IEntity<TAuditId>
    where TAuditId : struct
{
    DateTime? DeletedAt { get; set; }
    TAudit? DeletedBy { get; set; }
    TAuditId? DeletedById { get; set; }
}