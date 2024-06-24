namespace ICorteApi.Entities;

public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
