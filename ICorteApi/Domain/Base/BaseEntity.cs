using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public abstract class BaseEntity : BaseCrudEntity, IBaseEntity
{
    public int Id { get; set;}
}
