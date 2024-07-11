using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public abstract class BaseCrudEntity : IBaseCrudEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
