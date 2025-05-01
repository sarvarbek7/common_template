namespace Domain.Entities;

public interface ICreationAuditableEntity<TAudit, TAuditId>
    where TAudit : class, IEntity<TAuditId>
    where TAuditId : struct
{
    DateTime CreatedAt { get; set; }
    TAudit? CreatedBy { get; set; }
    TAuditId? CreatedById { get; set; }
}