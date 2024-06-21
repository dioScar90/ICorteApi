namespace ICorteApi.Entities;

public abstract class BaseEntity
{
    public virtual int Id { get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
