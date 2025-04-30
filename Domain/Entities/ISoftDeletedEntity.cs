namespace Domain.Entities;

public interface ISoftDeletedEntity
{
    bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Restore()
    {
        IsDeleted = false;
    }
}