namespace Domain.Entities;

public interface IUpdateAuditableEntity<TAudit, TAuditId>
    where TAudit : class, IEntity<TAuditId>
    where TAuditId : struct
{
    DateTime? UpdatedAt { get; set; }
    TAudit? UpdatedBy { get; set; }
    TAuditId? UpdatedById { get; set; }
}