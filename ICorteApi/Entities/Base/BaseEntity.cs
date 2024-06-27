namespace ICorteApi.Entities;

public abstract class BaseEntity : IBaseEntity, IBaseTableEntity
{
    public int Id { get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
