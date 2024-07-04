using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Base;

public abstract class BaseEntity : BaseSoftCrudEntity, IBaseEntity
{
    public int Id { get; set;}
}
