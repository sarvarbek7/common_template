using Domain.Entities;
using ErrorOr;

namespace Application.Validators;


public abstract class BaseValidator<T, TId, TAudit, TAuditId> : IValidator<T, TId>
    where T : class, IEntity<TId>
    where TId : struct
    where TAudit : class, IEntity<TAuditId>
    where TAuditId : struct
{
    public ErrorOr<Created> ValidateOnAdd(T model)
    {
        if (model is ICreationAuditableEntity<TAudit, TAuditId> auditable)
        {
            if (auditable.CreatedById is null ||
                !auditable.CreatedById.Equals(default(TAuditId))
               )
            {
                throw new MissingAuditIdException();
            }

            if (auditable.CreatedAt == default)
            {
                throw new MissingAuditTimeException();
            }
        }

        return new Created();
    }

    public ErrorOr<Deleted> ValidateOnDelete(T model)
    {
        if (model is IDeletionAuditableEntity<TAudit, TAuditId> auditable)
        {
            if (auditable.DeletedById is null ||
                !auditable.DeletedById.Equals(default(TAuditId))
               )
            {
                throw new MissingAuditIdException();
            }

            if (auditable.DeletedAt == default)
            {
                throw new MissingAuditTimeException();
            }
        }

        return new Deleted();
    }

    public ErrorOr<Updated> ValidateOnUpdate(T model)
    {
        if (model is IUpdateAuditableEntity<TAudit, TAuditId> auditable)
        {
            if (auditable.UpdatedById is null ||
                !auditable.UpdatedById.Equals(default(TAuditId))
               )
            {
                throw new MissingAuditIdException();
            }

            if (auditable.UpdatedAt == default)
            {
                throw new MissingAuditTimeException();
            }
        }

        return new Updated();
    }
}