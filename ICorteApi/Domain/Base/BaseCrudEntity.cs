using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public abstract class BaseCrudEntity : IBaseCrudEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
